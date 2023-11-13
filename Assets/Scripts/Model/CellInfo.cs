using Chess2D.UI;
using System;
using UnityEngine;

namespace Chess2D.Model
{
    [Serializable]
    public class CellInfo
    {
        public Vector2Int coord;
        public PieceType pieceType;
    }
}