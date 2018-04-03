using UnityEngine;
using UnityEngine.UI;

namespace Lesstergy.Chess2D {

    public class GameInstaller : MonoBehaviour {

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
        [Header("Arrangment")]
        public ArrangmentOfPieces arrangment;

        [Space(10)]
        [Header("GameObjects Group Parent")]
        public GameObject pieceGroupParent;
        public GameObject cellGroupParent;

        private void Awake() {
            Inject();
            Initialize();
            StartGame();
        }

        private void Inject() {
            boardController.Inject(chessboard, cellPrefab, cellGroupParent);
            pieceController.Inject(boardController, arrangment, piecePrefabBuilder, pieceGroupParent);
            pieceMoveController.Inject(boardController, pieceController);
            teamController.Inject(pieceController, pieceMoveController, boardController, pawnPromotionController);
            pawnPromotionController.Inject(pieceChooseView, boardController, pieceController, pieceMoveController, piecePrefabBuilder);

            restartController.Inject(restartButton);
        }

        private void Initialize() {
            boardController.Initialize();
            pieceMoveController.Initialize();
            teamController.Initialize();

            pieceController.Initialize();
            pawnPromotionController.Initialize();

            restartController.Initialize();
        }

        private void StartGame() {
            teamController.StartGame();
        }
    }

}
