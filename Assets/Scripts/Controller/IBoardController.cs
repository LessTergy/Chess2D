using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesstergy.Chess2D {

    public abstract class IBoardContoller : MonoBehaviour {

        public abstract Board GetBoard();

        public abstract Cell GetCell(int x, int y);

        public Cell GetCell(Vector2Int coord) {
            return GetCell(coord.x, coord.y);
        }

        public abstract Cell.State GetCellStateForPiece(int x, int y, Piece piece);

        public abstract void ReplacePiece(Vector2Int startPosition, Vector2Int endPosition);

        public abstract void HidePiece(Piece piece);
        public abstract void ShowPiece(Piece piece);
    }
}
