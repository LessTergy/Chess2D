using UnityEngine;

namespace Chess2D.Model.PieceMove
{
    public class QueenMove : PieceMoveAlgorithm
    {
        protected override Vector3Int MoveVector => new(GameConstants.FinishIndex, GameConstants.FinishIndex, GameConstants.FinishIndex);
    }
}