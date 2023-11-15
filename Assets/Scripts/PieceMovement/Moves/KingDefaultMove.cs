using UnityEngine;

namespace Chess2D.PieceMovement
{
    public class KingDefaultMove : PieceMoveAlgorithm
    {
        protected override Vector3Int MoveVector => Vector3Int.one;
    }
}