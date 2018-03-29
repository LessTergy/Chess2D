using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangeInt = Lesster.Math.RangeInt;

namespace Lesster.Chess2D {

    public static class ChessInfo {

        public const int CELL_COUNT = 8;

        public static Dictionary<ChessPiece.Type, RangeInt> chessCountDict = new Dictionary<ChessPiece.Type, RangeInt> {
            { ChessPiece.Type.King, new RangeInt(1, 1) },
            { ChessPiece.Type.Queen, new RangeInt(0, 1) },
            { ChessPiece.Type.Knight, new RangeInt(0, 2) },
            { ChessPiece.Type.Bishop, new RangeInt(0, 2) },
            { ChessPiece.Type.Rook, new RangeInt(0, 2) },
            { ChessPiece.Type.Pawn, new RangeInt(0, 8) }
        };
    }

}
