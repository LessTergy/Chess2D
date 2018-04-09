using System;
using System.Collections.Generic;
using UnityEngine;

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
                CreatePiece(cellInfo.pieceType, teamType, cellInfo.coord);
            }
        }

        public void CreatePiece(Piece.Type type, ChessTeam.Type teamType, Vector2Int cellCoord) {
            Cell actualCell = boardController.GetCell(cellCoord);

            //Init
            Piece piece = piecePrefabBuilder.CreatePiece(type, teamType);
            piece.name = teamType.ToString() + " " + type.ToString();
            piece.cellCoord = cellCoord;

            actualCell.SetPiece(piece);

            //Size and position
            RectTransform pieceRect = piece.transform as RectTransform;
            pieceRect.sizeDelta = actualCell.rectT.sizeDelta;
            piece.transform.SetParent(piecesParent.transform);
            piece.transform.position = actualCell.transform.position;
            piece.transform.localScale = Vector3.one;

            OnPieceCreated(piece);
        }

        public void UpdatePieceTargetByEnenmy(Piece friendlyPiece, Piece enemyPiece) {
            if (!enemyPiece.isEnable) {
                return;
            }

            foreach (PieceMoveAlgorithm moveAlgorithm in enemyPiece.moves) {
                List<MoveInfo> moves = moveAlgorithm.GetAvailableMoves(enemyPiece, boardController);

                foreach (MoveInfo moveInfo in moves) {
                    if (moveInfo.cell.coord == friendlyPiece.cellCoord) {
                        friendlyPiece.isTarget = true;
                        return;
                    }
                }
            }
        }

    }
}
