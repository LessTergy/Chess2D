using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesstergy.Chess2D {

    public class GameInstaller : MonoBehaviour {

        [Header("Controllers")]
        public BoardController boardController;
        public PieceController pieceController;

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
        [Header("Team colors")]
        public Color whiteTeamColor;
        public Color blackTeamColor;

        [Space(10)]
        [Header("GameObjects Group Parent")]
        public GameObject pieceGroupParent;
        public GameObject cellGroupParent;

        private void Awake() {
            Inject();
            Initialize();
        }

        private void Inject() {
            boardController.Inject(chessboard, cellPrefab, cellGroupParent);
            pieceController.Inject(boardController, arrangment, piecePrefabBuilder, pieceGroupParent, whiteTeamColor, blackTeamColor);
        }

        private void Initialize() {
            boardController.Initialize();
            pieceController.Initialize();
        }
    }

}
