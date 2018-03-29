using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Lesster.Chess2D {

    [Serializable]
    public class CellInfo {
        public Vector2Int coord;
        public Piece.Type pieceType;
    }

}
