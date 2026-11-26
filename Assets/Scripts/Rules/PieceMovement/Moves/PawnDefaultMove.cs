using System.Collections.Generic;
using Chess2D.Commands;
using Chess2D.Model;

namespace Chess2D.PieceMovement
{
    internal sealed class PawnDefaultMove : PieceMoveAlgorithm
    {
        public override List<MoveData> GetAvailableMoves(PieceState piece, GameModel game)
        {
            var moves = new List<MoveData>();
            var white = piece.PlayerType == PlayerType.White;
            var startRank = white ? GameConstants.StartIndex + 1 : GameConstants.FinishIndex - 1;
            var distance = piece.Square.Y == startRank ? 2 : 1;
            for (var step = 1; step <= distance; step++)
            {
                var target = piece.Square.Offset(0, Forward(piece.PlayerType) * step);
                if (game.GetCellStateForMove(target, piece) != CellState.Free)
                {
                    break;
                }

                moves.Add(new MoveData(target, new PieceMoveCommand(game, piece, target)));
            }

            return moves;
        }
    }
}
