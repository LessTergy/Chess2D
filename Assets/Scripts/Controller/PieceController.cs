using UnityEngine;
using System;
using System.Collections.Generic;
using Lesstergy.UI;

namespace Lesstergy.Chess2D {

    public class PieceController : IPieceController, IController {

        #region Injections
        private IBoardContoller boardController;

        private ArrangmentOfPieces arrangmentOfPieces;
        
        private PiecePrefabBuilder piecePrefabBuilder;
        private GameObject piecesParent;

        //Colors
        private Color whiteTeamColor;
        private Color blackTeamColor;
        #endregion

        public void Inject(IBoardContoller bc, ArrangmentOfPieces aop, PiecePrefabBuilder ppb, GameObject pieceParent, Color whiteTeam, Color blackTeam) {
            boardController = bc;
            arrangmentOfPieces = aop;
            piecePrefabBuilder = ppb;
            piecesParent = pieceParent;

            whiteTeamColor = whiteTeam;
            blackTeamColor = blackTeam;
        }

        public void Initialize() {
            piecePrefabBuilder.Init();

            CreateTeamPieces(arrangmentOfPieces.whitePieceCells, ChessTeam.Type.White, whiteTeamColor);
            CreateTeamPieces(arrangmentOfPieces.blackPieceCells, ChessTeam.Type.Black, blackTeamColor);
        }

        private void CreateTeamPieces(List<CellInfo> cellInfoList, ChessTeam.Type teamType, Color teamColor) {
            foreach (var cellInfo in cellInfoList) {
                Cell actualCell = boardController.GetCell(cellInfo.coord.x, cellInfo.coord.y);

                //Init
                Piece piece = piecePrefabBuilder.CreatePiece(cellInfo.pieceType);
                piece.name = teamType.ToString() + " " + cellInfo.pieceType.ToString();
                piece.InitChessTeam(teamType, teamColor);

                //Size and position
                RectTransform pieceRect = piece.transform as RectTransform;
                pieceRect.sizeDelta = actualCell.rectT.sizeDelta;
                piece.transform.SetParent(piecesParent.transform);
                piece.transform.position = actualCell.transform.position;
                piece.transform.localScale = Vector3.one;

                //Events
                piece.interactive.OnTouchDown += delegate { Piece_OnTouchDown(piece); };
                piece.interactive.OnTouchUp += delegate { Piece_OnTouchDown(piece); };
                piece.interactive.OnMove += delegate (InteractiveEventArgs args) { Piece_OnMove(args, piece); };
            }
        }


        //Piece Move events
        private void Piece_OnTouchDown(Piece piece) {

        }

        private void Piece_OnTouchUp(Piece piece) {

        }

        private void Piece_OnMove(InteractiveEventArgs eventArgs, Piece piece) {
            eventArgs.sender.transform.position += (Vector3) eventArgs.data.delta;
        }
    }
}
