﻿using UnityEngine;

namespace Chess2D.PieceMovement
{
    public class RookMove : PieceMoveAlgorithm
    {
        protected override Vector3Int MoveVector => new(GameConstants.FinishIndex, GameConstants.FinishIndex, 0);
    }
}