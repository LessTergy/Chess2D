using System.Collections;
using Chess2D.Controller;
using Chess2D.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Chess2D.UI
{
    public sealed class GameStatusView : MonoBehaviour
    {
        [Header("Draw Actions")]
        [SerializeField] private Button _offer;
        [SerializeField] private Text _offerLabel;
        [SerializeField] private Button _claim;
        [SerializeField] private Text _claimLabel;

        [Header("Result")]
        [SerializeField] private CanvasGroup _resultBanner;
        [SerializeField] private Text _result;
        [SerializeField] private CanvasGroup _mateHighlightPrefab;
        [SerializeField] private RectTransform _whiteFlagPrefab;
        [SerializeField, Min(0f)] private float _revealDuration = 0.55f;

        private GameModel _game;
        private BoardController _board;
        private PieceController _pieces;
        private PieceMoveController _moves;

        public void Initialize(GameModel game, BoardController board, PieceController pieces, PieceMoveController moves)
        {
            _game = game;
            _board = board;
            _pieces = pieces;
            _moves = moves;
            _offer.onClick.AddListener(OnOffer);
            _claim.onClick.AddListener(OnClaim);
            _game.Changed += Refresh;
        }

        private void OnDestroy()
        {
            if (_game == null)
            {
                return;
            }

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

        public void Refresh()
        {
            if (_game.Result != GameResult.Ongoing)
            {
                _moves.CancelDrag();
                _board.SetInteractive(false);
                _offer.gameObject.SetActive(false);
                _claim.gameObject.SetActive(false);
                _result.text = ResultText();
                _resultBanner.gameObject.SetActive(true);
                StartCoroutine(RevealResult());
                return;
            }

            _resultBanner.gameObject.SetActive(false);
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
            CanvasGroup highlight = null;
            RectTransform flag = null;
            if (_game.Result == GameResult.Checkmate)
            {
                var king = _game.CurrentKing;
                highlight = Instantiate(_mateHighlightPrefab, _board.GetCell(king.Square).RectT, false);
                flag = Instantiate(_whiteFlagPrefab, _pieces.GetView(king).transform, false);
            }

            var flagScale = flag != null ? flag.localScale : Vector3.one;
            var duration = Mathf.Max(0.01f, _revealDuration);
            for (var elapsed = 0f; ; elapsed += Time.unscaledDeltaTime)
            {
                var progress = Mathf.SmoothStep(0f, 1f, elapsed / duration);
                _resultBanner.alpha = progress;
                if (highlight != null)
                {
                    highlight.alpha = progress;
                    flag.localScale = flagScale * progress;
                }

                if (elapsed >= duration)
                {
                    yield break;
                }

                yield return null;
            }
        }
    }
}
