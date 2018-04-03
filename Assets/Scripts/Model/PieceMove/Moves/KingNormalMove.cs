using UnityEngine;

namespace Lesstergy.Chess2D {

    public class KingNormalMove : PieceMoveAlgorithm {

        public KingNormalMove() {
            moveVector = new Vector3Int(1, 1, 1);
        }
    }

}
