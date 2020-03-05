using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Model.PieceMove
{
    public class BishopMove : PieceMoveAlgorithm
    {
        protected override Vector3Int GetMoveVector()
        {
            return new Vector3Int(0, 0, Board.FinishIndex);
        }
    }
}