using System.Collections.Generic;
using UnityEngine;

namespace Lesstergy.Chess2D {

    public class PawnKillMove : PieceMoveAlgorithm {

        public PawnKillMove() {
            moveVector = new Vector3Int(0, 0, 1);
        }

        public override List<MoveInfo> GetAvailableMoves(Piece movingPiece, IBoardContoller boardController) {
            List<MoveInfo> moves = new List<MoveInfo>();

            Vector3Int pMoveVector = InvertVectorMoveByTeam(moveVector, movingPiece.teamType);

            Vector2Int coord = movingPiece.cellCoord;

            FillKillMove(moves, boardController, movingPiece, coord.x - pMoveVector.z, coord.y + pMoveVector.z);
            FillKillMove(moves, boardController, movingPiece, coord.x + pMoveVector.z, coord.y + pMoveVector.z);

            return moves;
        }

    }
}
