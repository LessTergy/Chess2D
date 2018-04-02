using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesstergy.Chess2D {

    public abstract class IBoardContoller : MonoBehaviour {

        public abstract Cell GetCell(int x, int y);

        public Cell GetCell(Vector2Int coord) {
            return GetCell(coord.x, coord.y);
        }

        public abstract Cell.State GetCellStateForPiece(int x, int y, Piece piece);

        public abstract void ReplacePiece(Vector2Int startPosition, Vector2Int endPosition);

        public abstract void HidePiece(Cell cell, Piece piece);
        public abstract void ShowPiece(Cell cell, Piece piece);
    }
}
