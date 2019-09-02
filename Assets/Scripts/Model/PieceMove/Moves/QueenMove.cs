using Chess2D.Model.PieceMove;
using Chess2D.UI;
using UnityEngine;

namespace Lesstergy.Chess2D {

    public class QueenMove : PieceMoveAlgorithm {

        public QueenMove() {
            moveVector = new Vector3Int(Board.FinishIndex, Board.FinishIndex, Board.FinishIndex);
        }
    }

}
