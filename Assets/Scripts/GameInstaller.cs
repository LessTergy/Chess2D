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

        private BoardController _boardController;
        private PieceController _pieceController;
        private PieceMoveController _pieceMoveController;
        private PawnPromotionController _pawnPromotionController;
        private GameCycleController _gameCycleController;
        private RestartController _restartController;

        private void Awake()
        {
            Install();
            Initialize();
            StartGame();
        }

        private void Install()
        {
            _boardController = new BoardController(_boardView, _cellPrefab, _cellGroupParent);
            _pieceMoveController = new PieceMoveController(_boardController);
            _pieceController = new PieceController(_boardController,_pieceMoveController, _defaultArrangement, _pieceFactory, _pieceGroupParent);
            
            var gameBuilder = new GameBuilder(_boardController, _pieceController);
            gameBuilder.Build();
            GameModel gameModel = gameBuilder.GameModel;
            
            _pawnPromotionPopup.Construct(_pieceConfig, GameConstants.PromotionPieces);
            _pawnPromotionController = new PawnPromotionController(gameModel, _pawnPromotionPopup, _boardController, _pieceController, _pieceMoveController);
            
            _gameCycleController = new GameCycleController(gameModel, _pieceController, _pieceMoveController, _pawnPromotionController);
            _restartController = new RestartController(_restartButton);
        }

        private void Initialize()
        {
            _pieceMoveController.Initialize();
            _gameCycleController.Initialize();
            
            _pawnPromotionController.Initialize();
            _restartController.Initialize();
        }

        private void StartGame()
        {
            _gameCycleController.StartGame();
        }
    }
}