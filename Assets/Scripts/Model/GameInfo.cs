using Chess2D.UI;
using System.Collections.Generic;
using RangeInteger = Lesstergy.Math.RangeInt;

namespace Chess2D.Model
{
    public static class GameInfo
    {
        public static readonly Dictionary<Piece.Type, RangeInteger> PieceCountDict = new Dictionary<Piece.Type, RangeInteger>
        {
            { Piece.Type.King, new RangeInteger(1, 1) },
            { Piece.Type.Queen, new RangeInteger(0, 1) },
            { Piece.Type.Knight, new RangeInteger(0, 2) },
            { Piece.Type.Bishop, new RangeInteger(0, 2) },
            { Piece.Type.Rook, new RangeInteger(0, 2) },
            { Piece.Type.Pawn, new RangeInteger(0, 8) }
        };
    }
}