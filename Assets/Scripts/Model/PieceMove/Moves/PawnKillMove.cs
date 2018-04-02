using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesstergy.Chess2D {

    public class PawnKillMove : PieceMoveAlgorithm {

        public PawnKillMove() {
            moveVector = new Vector3Int(0, 0, 1);
        }

        public override List<MoveInfo> GetAvailableMoves(Piece piece, IBoardContoller boardController) {
            List<MoveInfo> moves = new List<MoveInfo>();

            Vector3Int pMoveVector = InvertVectorMoveByTeam(moveVector, piece.teamType);

            FillKillMove(moves, boardController, piece, piece.coord.x - pMoveVector.z, piece.coord.y + pMoveVector.z);
            FillKillMove(moves, boardController, piece, piece.coord.x + pMoveVector.z, piece.coord.y + pMoveVector.z);

            return moves;
        }

    }
}
