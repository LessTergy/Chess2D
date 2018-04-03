using System.Collections.Generic;
using UnityEngine;

namespace Lesstergy.Chess2D {

    public class KnightMove : PieceMoveAlgorithm {

        public KnightMove() {
            moveVector = new Vector3Int(2, 2, 0);
        }

        public override List<MoveInfo> GetAvailableMoves(Piece movingPiece, IBoardContoller boardController) {
            List<MoveInfo> moves = new List<MoveInfo>();

            Vector2Int coord = movingPiece.coord;

            //Up left
            FillCellMove(moves, boardController, movingPiece, coord.x - 1, coord.y + moveVector.y);
            //Up right
            FillCellMove(moves, boardController, movingPiece, coord.x + 1, coord.y + moveVector.y);

            //Down left
            FillCellMove(moves, boardController, movingPiece, coord.x - 1, coord.y - moveVector.y);
            //Down right
            FillCellMove(moves, boardController, movingPiece, coord.x + 1, coord.y - moveVector.y);

            //Left left
            FillCellMove(moves, boardController, movingPiece, coord.x - moveVector.x, coord.y - 1);
            //Left right
            FillCellMove(moves, boardController, movingPiece, coord.x - moveVector.x, coord.y + 1);

            //Right left
            FillCellMove(moves, boardController, movingPiece, coord.x + moveVector.x, coord.y - 1);
            //Right right
            FillCellMove(moves, boardController, movingPiece, coord.x + moveVector.x, coord.y + 1);
            
            return moves;
        }
    }

}
