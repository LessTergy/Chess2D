using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesstergy.Chess2D {

    public class BishopMove : PieceMoveAlgorithm {

        public BishopMove() {
            moveVector = new Vector3Int(0, 0, 7);
        }

    }

}
