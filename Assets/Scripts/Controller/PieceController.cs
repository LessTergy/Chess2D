using UnityEngine;
using System;
using System.Collections.Generic;

namespace Lesster.Chess2D {

    public class PieceController : MonoBehaviour, IController {

        public PiecePrefabContainer piecePrefabContainer;
        public ArrangmentOfPieces arrangmentOfPieces;

        public IBoardContoller boardController;
        public GameObject piecesParent;

        public Color whiteSideColor;
        public Color blackSideColor;

        public void Initialize() {
            piecePrefabContainer.Init();

            CreatePieces(arrangmentOfPieces.whitePieceCells, ChessSide.Type.White, whiteSideColor);
            CreatePieces(arrangmentOfPieces.blackPieceCells, ChessSide.Type.Black, blackSideColor);
        }

        private void CreatePieces(List<CellInfo> cellInfoList, ChessSide.Type sideType, Color sideColor) {
            foreach (var cellInfo in cellInfoList) {
                Cell actualCell = boardController.GetCell(cellInfo.coord.x, cellInfo.coord.y);

                Piece piece = piecePrefabContainer.CreatePiece(cellInfo.pieceType);
                piece.name = sideType.ToString() + " " + cellInfo.pieceType.ToString();
                piece.Initialize(sideType, sideColor);

                RectTransform pieceRect = piece.transform as RectTransform;
                pieceRect.sizeDelta = actualCell.rectT.sizeDelta;
                piece.transform.SetParent(piecesParent.transform);
                piece.transform.position = actualCell.transform.position;
                piece.transform.localScale = Vector3.one;
            }
        }
    }
}
