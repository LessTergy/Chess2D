﻿using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Model.PieceMove
{
    public class RookMove : PieceMoveAlgorithm
    {
        protected override Vector3Int GetMoveVector()
        {
            return new Vector3Int(Board.FinishIndex, Board.FinishIndex, 0);
        }
    }
}