using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Model.PieceMove
{
    public class QueenMove : PieceMoveAlgorithm
    {
        protected override Vector3Int GetMoveVector()
        {
            return new Vector3Int(GameConstants.FinishIndex, GameConstants.FinishIndex, GameConstants.FinishIndex);
        }
    }
}