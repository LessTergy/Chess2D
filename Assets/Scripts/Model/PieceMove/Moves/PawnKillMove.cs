using System.Collections.Generic;
using Chess2D.Controller;
using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Model.PieceMove
{
    public class PawnKillMove : PieceMoveAlgorithm
    {
        protected override Vector3Int MoveVector => new(0, 0, 1);

        public override List<MoveData> GetAvailableMoves(PieceView movingPiece, BoardController boardController)
        {
            var moves = new List<MoveData>();

            Vector3Int pMoveVector = InvertMoveVector(MoveVector, movingPiece.PlayerType);

            Vector2Int coord = movingPiece.cellCoord;

            FillKillMove(moves, boardController, movingPiece, coord.x - pMoveVector.z, coord.y + pMoveVector.z);
            FillKillMove(moves, boardController, movingPiece, coord.x + pMoveVector.z, coord.y + pMoveVector.z);

            return moves;
        }
    }
}