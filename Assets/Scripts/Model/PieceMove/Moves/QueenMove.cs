using UnityEngine;

namespace Lesstergy.Chess2D {

    public class QueenMove : PieceMoveAlgorithm {

        public QueenMove() {
            moveVector = new Vector3Int(7, 7, 7);
        }
    }

}
