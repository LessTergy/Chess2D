using System.Collections.Generic;
using Chess2D.Commands;
using Chess2D.Model;

namespace Chess2D.PieceMovement
{
    internal abstract class PieceMoveAlgorithm
    {
        protected virtual (int Horizontal, int Vertical, int Diagonal) Range => (0, 0, 0);

        public virtual List<MoveData> GetAvailableMoves(PieceState piece, GameModel game)
        {
            var moves = new List<MoveData>();
            AddRay(moves, game, piece, 1, 0, Range.Horizontal);
            AddRay(moves, game, piece, -1, 0, Range.Horizontal);
            AddRay(moves, game, piece, 0, 1, Range.Vertical);
            AddRay(moves, game, piece, 0, -1, Range.Vertical);
            AddRay(moves, game, piece, 1, 1, Range.Diagonal);
            AddRay(moves, game, piece, -1, -1, Range.Diagonal);
            AddRay(moves, game, piece, 1, -1, Range.Diagonal);
            AddRay(moves, game, piece, -1, 1, Range.Diagonal);
            return moves;
        }

        private static void AddRay(List<MoveData> moves, GameModel game, PieceState piece, int x, int y, int distance)
        {
            for (var step = 1; step <= distance; step++)
            {
                if (!AddStep(moves, game, piece, piece.Square.Offset(x * step, y * step)))
                {
                    break;
                }
            }
        }

        protected static bool AddStep(List<MoveData> moves, GameModel game, PieceState piece, Square target)
        {
            switch (game.GetCellStateForMove(target, piece))
            {
                case CellState.Free:
                    moves.Add(new MoveData(target, new PieceMoveCommand(game, piece, target)));
                    return true;
                case CellState.Opponent:
                    AddCapture(moves, game, piece, target, game.GetPieceAt(target));
                    break;
            }

            return false;
        }

        protected static void AddCapture(List<MoveData> moves, GameModel game, PieceState piece, Square target, PieceState captured)
        {
            var capture = new PieceKillCommand(game, captured);
            var advance = new PieceMoveCommand(game, piece, target);
            moves.Add(new MoveData(target, new CommandContainer(capture, advance)));
        }

        protected static int Forward(PlayerType side) => side == PlayerType.White ? 1 : -1;
    }
}
