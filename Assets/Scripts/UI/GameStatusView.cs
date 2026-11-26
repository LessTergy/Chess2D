using System.Collections;
using Chess2D.Controller;
using Chess2D.Model;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Chess2D.UI
{
    public sealed class GameStatusView : MonoBehaviour
    {
        private static readonly Color PanelColor = new(0.13f, 0.10f, 0.08f, 0.96f);
        private static readonly Color MateColor = new(0.86f, 0.12f, 0.10f, 0.88f);

        private GameModel _game;
        private RectTransform _footer;
        private RectTransform _restart;
        private BoardController _board;
        private PieceController _pieces;
        private PieceMoveController _moves;
        private Button _offer;
        private Button _claim;
        private Text _offerLabel;
        private Text _claimLabel;
        private Text _result;
        private Font _font;

        public static void Create(Button restart, GameModel game, BoardController board,
            PieceController pieces, PieceMoveController moves)
        {
            var canvas = restart.GetComponentInParent<Canvas>().rootCanvas;
            var overlay = CreateRect("Game status", canvas.transform, Vector2.zero, Vector2.one);
            var view = overlay.gameObject.AddComponent<GameStatusView>();
            view._footer = CreateRect("Footer", overlay, Vector2.zero, Vector2.zero);
            view._restart = (RectTransform)restart.transform;
            view._game = game;
            view._board = board;
            view._pieces = pieces;
            view._moves = moves;
            view.Build();
            game.Changed += view.Refresh;
            view.Refresh();
        }

        private void Build()
        {
            _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            _offer = CreateButton("Draw offer", OnOffer, out _offerLabel);
            _claim = CreateButton("Draw claim", OnClaim, out _claimLabel);
            var banner = CreateRect("Result", _footer, Vector2.zero, Vector2.one);
            AddPanel(banner);
            _result = CreateLabel(banner);
            banner.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _game.Changed -= Refresh;
            _offer.onClick.RemoveListener(OnOffer);
            _claim.onClick.RemoveListener(OnClaim);
        }

        private void OnOffer()
        {
            if (_game.DrawOfferBy.HasValue)
            {
                _game.TryAcceptDraw();
            }
            else
            {
                _game.TryOfferDraw();
            }
        }

        private void OnClaim() => _game.TryClaimDraw();

        private void Refresh()
        {
            LayoutFooter();
            if (_game.Result != GameResult.Ongoing)
            {
                _moves.CancelDrag();
                _board.SetInteractive(false);
                _offer.gameObject.SetActive(false);
                _claim.gameObject.SetActive(false);
                _result.transform.parent.gameObject.SetActive(true);
                _result.text = ResultText();
                StartCoroutine(RevealResult());
                return;
            }

            var ownOffer = _game.DrawOfferBy == _game.CurrentPlayer;
            var showOffer = _game.CanOfferDraw || (_game.CanPlay && _game.DrawOfferBy.HasValue);
            var showClaim = _game.CanClaimDraw || _game.CanClaimWithMove || (_game.CanPlay && _game.ClaimOnNextMove);
            _offer.gameObject.SetActive(showOffer);
            _offer.interactable = !ownOffer;
            _offerLabel.text = ownOffer ? "Ничья предложена · сделайте ход" :
                _game.DrawOfferBy.HasValue ? "Принять ничью" : "Предложить ничью";
            _claim.gameObject.SetActive(showClaim);
            _claim.interactable = !_game.ClaimOnNextMove;
            _claimLabel.text = _game.ClaimOnNextMove ? "Сделайте заявленный ход" :
                _game.CanClaimDraw ? "Заявить ничью" : "Заявить ничью с ходом";
            ((RectTransform)_offer.transform).anchorMax = new Vector2(showClaim ? 0.49f : 1f, 1f);
            ((RectTransform)_claim.transform).anchorMin = new Vector2(showOffer ? 0.51f : 0f, 0f);
        }

        private void Start() => LayoutFooter();

        private void OnRectTransformDimensionsChange()
        {
            if (_restart != null)
            {
                LayoutFooter();
            }
        }

        private void LayoutFooter()
        {
            if (_board?.BoardRect == null)
            {
                return;
            }

            var canvas = (RectTransform)_footer.parent;
            var rect = canvas.rect;
            var height = Mathf.Min(rect.width * 0.085f, rect.height * 0.067f);
            var margin = height * 0.25f;
            var bottom = rect.yMin + margin;
            var restart = RectTransformUtility.CalculateRelativeRectTransformBounds(canvas, _restart);
            var full = new Vector2(rect.xMin + margin, rect.xMax - margin);
            var span = FreeSpan(full, restart, bottom, height, margin);
            if (_game.Result == GameResult.Ongoing)
            {
                var board = RectTransformUtility.CalculateRelativeRectTransformBounds(canvas, _board.BoardRect);
                span = FreeSpan(span, board, bottom, height, margin);
                if (span.y - span.x < height * 5f)
                {
                    var top = rect.yMax - margin - height;
                    var upper = FreeSpan(FreeSpan(full, restart, top, height, margin), board, top, height, margin);
                    if (upper.y - upper.x > span.y - span.x)
                    {
                        span = upper;
                        bottom = top;
                    }
                }
            }

            if (span.y <= span.x)
            {
                span = FreeSpan(full, restart, bottom, height, margin);
            }

            var width = Mathf.Min(span.y - span.x, height * 11f);
            _footer.pivot = new Vector2(0.5f, 0f);
            _footer.sizeDelta = new Vector2(width, height);
            _footer.anchoredPosition = new Vector2((span.x + span.y) / 2f - rect.xMin, bottom - rect.yMin);
        }

        private static Vector2 FreeSpan(Vector2 span, Bounds obstacle, float bottom, float height, float margin)
        {
            if (obstacle.min.y >= bottom + height + margin || obstacle.max.y <= bottom - margin ||
                obstacle.min.x >= span.y || obstacle.max.x <= span.x)
            {
                return span;
            }

            return obstacle.min.x - span.x > span.y - obstacle.max.x
                ? new Vector2(span.x, Mathf.Max(span.x, obstacle.min.x - margin))
                : new Vector2(Mathf.Min(span.y, obstacle.max.x + margin), span.y);
        }

        private string ResultText() => _game.Result switch
        {
            GameResult.Checkmate => _game.CurrentPlayer == PlayerType.White ? "Мат · Чёрные победили" : "Мат · Белые победили",
            GameResult.Stalemate => "Ничья · Пат",
            GameResult.InsufficientMaterial => "Ничья · Недостаточно материала для мата",
            GameResult.ThreefoldRepetition => "Ничья · Троекратное повторение позиции",
            GameResult.FivefoldRepetition => "Ничья · Пятикратное повторение позиции",
            GameResult.FiftyMoveRule => "Ничья · Правило 50 ходов",
            GameResult.SeventyFiveMoveRule => "Ничья · Правило 75 ходов",
            GameResult.DrawByAgreement => "Ничья · По соглашению игроков",
            _ => throw new System.InvalidOperationException()
        };

        private IEnumerator RevealResult()
        {
            var banner = _result.transform.parent.gameObject.AddComponent<CanvasGroup>();
            Image square = null;
            RectTransform flag = null;
            if (_game.Result == GameResult.Checkmate)
            {
                var king = _game.CurrentKing;
                var cell = _board.GetCell(king.Square);
                square = AddPanel(CreateRect("Mate highlight", cell.RectT, Vector2.zero, Vector2.one));
                flag = CreateFlag(_pieces.GetView(king).transform);
            }

            const float duration = 0.55f;
            for (var elapsed = 0f; ; elapsed += Time.unscaledDeltaTime)
            {
                var amount = Mathf.SmoothStep(0f, 1f, elapsed / duration);
                banner.alpha = amount;
                if (flag != null)
                {
                    square.color = new Color(MateColor.r, MateColor.g, MateColor.b, MateColor.a * amount);
                    flag.localScale = Vector3.one * amount;
                }

                if (elapsed >= duration)
                {
                    yield break;
                }
                yield return null;
            }
        }

        private static RectTransform CreateFlag(Transform king)
        {
            var flag = CreateRect("White flag", king, new Vector2(0.67f, 0.48f), new Vector2(0.98f, 0.96f));
            flag.pivot = Vector2.zero;
            var pole = CreateRect("Pole", flag, Vector2.zero, new Vector2(0.10f, 1f));
            AddPanel(pole).color = Color.white;
            var cloth = CreateRect("Cloth", flag, new Vector2(0.08f, 0.52f), Vector2.one);
            AddPanel(cloth).color = Color.white;
            cloth.gameObject.AddComponent<Shadow>();
            return flag;
        }

        private Button CreateButton(string title, UnityAction action, out Text label)
        {
            var rect = CreateRect(title, _footer, Vector2.zero, Vector2.one);
            var panel = AddPanel(rect);
            panel.raycastTarget = true;
            var button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = panel;
            button.onClick.AddListener(action);
            label = CreateLabel(rect);
            return button;
        }

        private Text CreateLabel(RectTransform parent)
        {
            var label = CreateRect("Label", parent, new Vector2(0.03f, 0.10f), new Vector2(0.97f, 0.90f))
                .gameObject.AddComponent<Text>();
            label.font = _font;
            label.color = Color.white;
            label.fontStyle = FontStyle.Bold;
            label.alignment = TextAnchor.MiddleCenter;
            label.resizeTextForBestFit = true;
            label.resizeTextMinSize = 8;
            label.resizeTextMaxSize = 48;
            label.raycastTarget = false;
            return label;
        }

        private static Image AddPanel(RectTransform rect)
        {
            var image = rect.gameObject.AddComponent<Image>();
            image.color = PanelColor;
            image.raycastTarget = false;
            return image;
        }

        private static RectTransform CreateRect(string title, Transform parent, Vector2 min, Vector2 max)
        {
            var rect = (RectTransform)new GameObject(title, typeof(RectTransform)).transform;
            rect.SetParent(parent, false);
            rect.anchorMin = min;
            rect.anchorMax = max;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
            return rect;
        }
    }
}
