﻿using Chess2D.Model.PieceMove;
using Chess2D.UI;
using UnityEngine;

namespace Lesstergy.Chess2D {

    public class BishopMove : PieceMoveAlgorithm {

        public BishopMove() {
            moveVector = new Vector3Int(0, 0, Board.FinishIndex);
        }

    }

}
