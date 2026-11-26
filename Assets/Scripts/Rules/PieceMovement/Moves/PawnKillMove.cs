using System.Collections.Generic;
using Chess2D.Model;

namespace Chess2D.PieceMovement
{
    internal sealed class PawnKillMove : PieceMoveAlgorithm
    {
        public override List<MoveData> GetAvailableMoves(PieceState piece, GameModel game)
        {
            var moves = new List<MoveData>();
            AddDiagonalCapture(moves, game, piece, -1);
            AddDiagonalCapture(moves, game, piece, 1);
            return moves;
        }

        private static void AddDiagonalCapture(List<MoveData> moves, GameModel game, PieceState piece, int fileOffset)
        {
            var target = piece.Square.Offset(fileOffset, Forward(piece.PlayerType));
            if (game.GetCellStateForMove(target, piece) == CellState.Opponent)
            {
                AddCapture(moves, game, piece, target, game.GetPieceAt(target));
            }
        }
    }
}
