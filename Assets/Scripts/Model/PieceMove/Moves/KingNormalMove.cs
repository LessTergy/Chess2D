using UnityEngine;

namespace Chess2D.Model.PieceMove
{
    public class KingNormalMove : PieceMoveAlgorithm
    {
        protected override Vector3Int GetMoveVector()
        {
            return Vector3Int.one;
        }
    }
}