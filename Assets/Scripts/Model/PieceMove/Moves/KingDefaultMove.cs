using UnityEngine;

namespace Chess2D.Model.PieceMove
{
    public class KingDefaultMove : PieceMoveAlgorithm
    {
        protected override Vector3Int MoveVector => Vector3Int.one;
    }
}