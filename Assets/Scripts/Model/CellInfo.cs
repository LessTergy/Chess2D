using System;
using UnityEngine;

namespace Lesstergy.Chess2D {

    [Serializable]
    public class CellInfo {
        public Vector2Int coord;
        public Piece.Type pieceType;
    }

}
