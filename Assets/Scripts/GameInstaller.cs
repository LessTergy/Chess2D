using Chess2D.Controller;
using Chess2D.Model;
using Chess2D.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Chess2D
{
    public class GameInstaller : MonoBehaviour
    {
        [Space(10)]
        [Header("UI Elements")]
        [SerializeField] private BoardView _boardView;
        [SerializeField] private PawnPromotionPopup _pawnPromotionPopup;
        [SerializeField] private Button _restartButton;

        [Space(10)]
        [Header("Prefabs")]
        [SerializeField] private CellView _cellPrefab;

        [Space(10)]
        [Header("Configs")]
        [SerializeField] private ArrangementConfig _defaultArrangement;
        [SerializeField] private PieceConfig _pieceConfig;
        [SerializeField] private PieceFactory _pieceFactory;

        [Space(10)]
        [Header("GameObjects Group Parent")]
        [SerializeField] private GameObject _pieceGroupParent;
        [SerializeField] private GameObject _cellGroupParent;

        private BoardController _board;
        private GameModel _game;
        private PieceMoveController _moves;
        private PieceController _pieces;
        private PawnPromotionController _promotion;
        private RestartController _restart;

        private void Awake()
        {
            Install();
            Initialize();
            StartGame();
        }

        private void Install()
        {
            _game = GameBuilder.Build(_defaultArrangement);
            _board = new BoardController(_boardView, _cellPrefab, _cellGroupParent);
            _moves = new PieceMoveController(_board, _game);
            _pieces = new PieceController(_game, _board, _moves, _pieceFactory, _pieceGroupParent.transform);
            _promotion = new PawnPromotionController(_game, _pawnPromotionPopup, _board, _pieces, _moves);
            _restart = new RestartController(_restartButton);
        }

        private void Initialize()
        {
            _moves.Initialize();
            _pawnPromotionPopup.Construct(_pieceConfig, GameConstants.PromotionPieces);
            _pieces.Initialize();
            _promotion.Initialize();
            _restart.Initialize();
        }

        private void StartGame()
        {
            _board.CreateCells();
            _pieces.CreatePlayerPieces();
            GameStatusView.Create(_restartButton, _game, _board, _pieces, _moves);
        }

        private void OnDestroy()
        {
            _promotion?.Dispose();
            _pieces?.Dispose();
            _restart?.Dispose();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                _moves?.CancelDrag();
            }
        }

        private void OnApplicationPause(bool isPaused)
        {
            if (isPaused)
            {
                _moves?.CancelDrag();
            }
        }
    }
}
