using System.Collections.Generic;
using Chess2D.Controller;
using Chess2D.UI;
using UnityEngine;

namespace Chess2D.PieceMovement
{
    public class KnightMove : PieceMoveAlgorithm
    {
        protected override Vector3Int MoveVector => new(2, 2, 0);

        public override List<MoveData> GetAvailableMoves(PieceView movingPiece, BoardController boardController)
        {
            var moves = new List<MoveData>();

            Vector2Int coord = movingPiece.cellCoord;

            //Up left
            FillCellMove(moves, boardController, movingPiece, coord.x - 1, coord.y + MoveVector.y);
            //Up right
            FillCellMove(moves, boardController, movingPiece, coord.x + 1, coord.y + MoveVector.y);

            //Down left
            FillCellMove(moves, boardController, movingPiece, coord.x - 1, coord.y - MoveVector.y);
            //Down right
            FillCellMove(moves, boardController, movingPiece, coord.x + 1, coord.y - MoveVector.y);

            //Left left
            FillCellMove(moves, boardController, movingPiece, coord.x - MoveVector.x, coord.y - 1);
            //Left right
            FillCellMove(moves, boardController, movingPiece, coord.x - MoveVector.x, coord.y + 1);

            //Right left
            FillCellMove(moves, boardController, movingPiece, coord.x + MoveVector.x, coord.y - 1);
            //Right right
            FillCellMove(moves, boardController, movingPiece, coord.x + MoveVector.x, coord.y + 1);

            return moves;
        }
    }

}