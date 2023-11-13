using Chess2D.Controller;
using Chess2D.Model;
using Chess2D.UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Chess2D
{
    public class GameInstaller : MonoBehaviour
    {
        [Header("Controllers")]
        public BoardController boardController;
        public PieceController pieceController;
        public PieceMoveController pieceMoveController;
        public TeamController teamController;
        public PawnPromotionController pawnPromotionController;
        public RestartController restartController;

        [Space(10)]
        [Header("Objects")]
        [SerializeField] private BoardView _boardView;
        [SerializeField] private PawnPromotionPopup _pawnPromotionPopup;
        public Button restartButton;

        [Space(10)]
        [Header("Prefabs")]
        public CellView cellPrefab;
        public PieceConfig pieceConfig;

        [Space(10)]
        [Header("Arrangement")]
        public ArrangementConfig arrangement;

        [Space(10)]
        [Header("GameObjects Group Parent")]
        public GameObject pieceGroupParent;
        public GameObject cellGroupParent;

        private void Awake()
        {
            Install();
            Initialize();
            StartGame();
        }

        private void Install()
        {
            boardController.Construct(_boardView, cellPrefab, cellGroupParent);
            pieceController.Construct(boardController, arrangement, pieceConfig, pieceGroupParent);
            pieceMoveController.Construct(boardController, pieceController);
            teamController.Construct(pieceController, pieceMoveController, pawnPromotionController);
            pawnPromotionController.Construct(_pawnPromotionPopup, boardController, pieceController, pieceMoveController);
            _pawnPromotionPopup.Construct(pieceConfig, GameConstants.PromotionPieces);

            restartController.Construct(restartButton);
        }

        private void Initialize()
        {
            boardController.Initialize();
            pieceMoveController.Initialize();
            teamController.Initialize();

            pieceController.Initialize();
            pawnPromotionController.Initialize();

            restartController.Initialize();
        }

        private void StartGame()
        {
            teamController.StartGame();
        }
    }

}