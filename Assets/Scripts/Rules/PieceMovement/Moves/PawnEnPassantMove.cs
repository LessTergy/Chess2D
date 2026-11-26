using System.Collections.Generic;
using Chess2D.Model;

namespace Chess2D.PieceMovement
{
    internal sealed class PawnEnPassantMove : PieceMoveAlgorithm
    {
        public override List<MoveData> GetAvailableMoves(PieceState piece, GameModel game)
        {
            var moves = new List<MoveData>();
            var opposingPawn = game.LastDoubleStepPawn;
            if (opposingPawn == null || opposingPawn.PlayerType == piece.PlayerType)
            {
                return moves;
            }

            var from = piece.Square;
            var neighbor = opposingPawn.Square;
            var offset = neighbor.X - from.X;
            if (neighbor.Y == from.Y && (offset == -1 || offset == 1))
            {
                var target = from.Offset(offset, Forward(piece.PlayerType));
                if (game.GetCellStateForMove(target, piece) == CellState.Free)
                {
                    AddCapture(moves, game, piece, target, opposingPawn);
                }
            }

            return moves;
        }
    }
}
