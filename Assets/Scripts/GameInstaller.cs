using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesstergy.Chess2D {

    public class GameInstaller : MonoBehaviour {

        [Header("Controllers")]
        public BoardController boardController;
        public PieceController pieceController;
        public PieceMoveController pieceMoveController;
        public TeamController teamController;

        [Space(10)]
        [Header("Objects")]
        public Board chessboard;

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
            teamController.Inject(pieceController, pieceMoveController, boardController);
        }

        private void Initialize() {
            boardController.Initialize();
            pieceMoveController.Initialize();
            teamController.Initialize();

            pieceController.Initialize();
        }

        private void StartGame() {
            teamController.StartGame();
        }
    }

}
