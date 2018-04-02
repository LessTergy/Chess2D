using System.Collections.Generic;
using UnityEngine;
using System;

namespace Lesstergy.Chess2D {

    public class PieceController : MonoBehaviour, IController {

        public event Action<Piece> OnPieceCreated = delegate { };

        #region Injections
        private IBoardContoller boardController;

        private ArrangmentOfPieces arrangmentOfPieces;
        
        private PiecePrefabBuilder piecePrefabBuilder;
        private GameObject piecesParent;
        #endregion

        public void Inject(IBoardContoller bc, ArrangmentOfPieces aop, PiecePrefabBuilder ppb, GameObject pieceParent) {
            boardController = bc;
            arrangmentOfPieces = aop;
            piecePrefabBuilder = ppb;
            piecesParent = pieceParent;
        }

        public void Initialize() {
            piecePrefabBuilder.Init();

            CreateTeamPieces(arrangmentOfPieces.whitePieceCells, ChessTeam.Type.White);
            CreateTeamPieces(arrangmentOfPieces.blackPieceCells, ChessTeam.Type.Black);
        }

        private void CreateTeamPieces(List<CellInfo> cellInfoList, ChessTeam.Type teamType) {
            foreach (var cellInfo in cellInfoList) {
                Cell actualCell = boardController.GetCell(cellInfo.coord);

                //Init
                Piece piece = piecePrefabBuilder.CreatePiece(cellInfo.pieceType, teamType);
                piece.name = teamType.ToString() + " " + cellInfo.pieceType.ToString();
                piece.coord = cellInfo.coord;

                actualCell.SetPiece(piece);

                //Size and position
                RectTransform pieceRect = piece.transform as RectTransform;
                pieceRect.sizeDelta = actualCell.rectT.sizeDelta;
                piece.transform.SetParent(piecesParent.transform);
                piece.transform.position = actualCell.transform.position;
                piece.transform.localScale = Vector3.one;

                OnPieceCreated(piece);
            }
        }
        
    }
}
