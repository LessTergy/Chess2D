using System;
using System.Collections.Generic;
using Chess2D.Model;

namespace Chess2D
{
    public static class GameConstants
    {
        public const int CellCount = 8;
        public const int StartIndex = 0;
        public const int FinishIndex = CellCount - 1;

        public static readonly IReadOnlyList<PieceType> PromotionPieces = Array.AsReadOnly(new[]
        {
            PieceType.Queen, PieceType.Rook, PieceType.Knight, PieceType.Bishop
        });
    }
}
