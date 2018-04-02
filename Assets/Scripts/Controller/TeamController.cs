using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesstergy.Chess2D {

    public class TeamController : MonoBehaviour, IController {

        private ChessTeam whiteTeam;
        private ChessTeam blackTeam;

        private PieceController pieceController;
        private PieceMoveController pieceMoveController;

        public void Inject() {

        }

        public void Initialize() {
            pieceController.OnPieceCreated += PieceController_OnPieceCreated;
            pieceMoveController.OnFinishMove += PieceMoveController_OnFinishMove;
        }

        private void PieceController_OnPieceCreated(Piece piece) {
            ChessTeam team = GetTeam(piece.teamType);
            team.pieces.Add(piece);

            if (piece.type == Piece.Type.King) {
                team.SetKing(piece);
            }
        }

        private void PieceMoveController_OnFinishMove(Piece piece) {

        }

        private ChessTeam GetTeam(ChessTeam.Type teamType) {
            return (teamType == ChessTeam.Type.White) ? whiteTeam : blackTeam;
        }
    }

}
