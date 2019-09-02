using System;
using Chess2D.UI;
using Lesstergy.Chess2D;
using UnityEngine;

namespace Chess2D.Model
{

    [Serializable]
    public class CellInfo
    {
        public Vector2Int coord;
        public Piece.Type pieceType;
    }

}