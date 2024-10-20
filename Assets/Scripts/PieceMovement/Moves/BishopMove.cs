﻿using UnityEngine;

namespace Chess2D.PieceMovement
{
    public class BishopMove : PieceMoveAlgorithm
    {
        protected override Vector3Int MoveVector => new(0, 0, GameConstants.FinishIndex);
    }
}