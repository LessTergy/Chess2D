using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesstergy.Chess2D {

    public abstract class IBoardContoller : MonoBehaviour {

        public abstract Cell GetCell(int x, int y);
        public abstract Cell.State GetCellStateForPiece(int x, int y, Piece piece);

    }
}
