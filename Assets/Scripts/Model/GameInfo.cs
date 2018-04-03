using System.Collections.Generic;
using RangeInt = Lesstergy.Math.RangeInt;

namespace Lesstergy.Chess2D {

    public static class GameInfo {
        
        public static Dictionary<Piece.Type, RangeInt> pieceCountDict = new Dictionary<Piece.Type, RangeInt> {
            { Piece.Type.King, new RangeInt(1, 1) },
            { Piece.Type.Queen, new RangeInt(0, 1) },
            { Piece.Type.Knight, new RangeInt(0, 2) },
            { Piece.Type.Bishop, new RangeInt(0, 2) },
            { Piece.Type.Rook, new RangeInt(0, 2) },
            { Piece.Type.Pawn, new RangeInt(0, 8) }
        };
    }

}
