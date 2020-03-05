using Chess2D.Controller;
using Chess2D.Model;
using Chess2D.UI;
using UnityEngine;
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
        public Board chessboard;
        public PieceChooseView pieceChooseView;
        public Button restartButton;

        [Space(10)]
        [Header("Prefabs")]
        public Cell cellPrefab;
        public PiecePrefabBuilder piecePrefabBuilder;

        [Space(10)]
        [Header("Arrangement")]
        public ArrangementOfPieces arrangement;

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
            boardController.Construct(chessboard, cellPrefab, cellGroupParent);
            pieceController.Construct(boardController, arrangement, piecePrefabBuilder, pieceGroupParent);
            pieceMoveController.Construct(boardController, pieceController);
            teamController.Construct(pieceController, pieceMoveController, pawnPromotionController);
            pawnPromotionController.Construct(pieceChooseView, boardController, pieceController, pieceMoveController, piecePrefabBuilder);

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