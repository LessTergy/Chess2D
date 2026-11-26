using System.Collections.Generic;
using Chess2D.Model;

namespace Chess2D.PieceMovement
{
    internal sealed class KnightMove : PieceMoveAlgorithm
    {
        private static readonly (int X, int Y)[] Offsets =
        {
            (-1, 2), (1, 2), (-1, -2), (1, -2),
            (-2, -1), (-2, 1), (2, -1), (2, 1)
        };

        public override List<MoveData> GetAvailableMoves(PieceState piece, GameModel game)
        {
            var moves = new List<MoveData>();
            foreach (var offset in Offsets)
            {
                AddStep(moves, game, piece, piece.Square.Offset(offset.X, offset.Y));
            }

            return moves;
        }
    }
}
