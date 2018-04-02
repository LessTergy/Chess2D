using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesstergy.Chess2D {

    public class RookMove : PieceMoveAlgorithm {

        public RookMove() {
            moveVector = new Vector3Int(7, 7, 0);
        }
    }

}
