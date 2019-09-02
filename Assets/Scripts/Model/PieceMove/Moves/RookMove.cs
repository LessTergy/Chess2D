using Chess2D.Model.PieceMove;
using Chess2D.UI;
using UnityEngine;

namespace Lesstergy.Chess2D {

    public class RookMove : PieceMoveAlgorithm {

        public RookMove() {
            moveVector = new Vector3Int(Board.FinishIndex, Board.FinishIndex, 0);
        }
    }

}
