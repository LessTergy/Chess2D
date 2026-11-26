using System.Collections.Generic;
using Chess2D.Commands;
using Chess2D.Model;

namespace Chess2D.PieceMovement
{
    internal sealed class KingCastlingMove : PieceMoveAlgorithm
    {
        public override List<MoveData> GetAvailableMoves(PieceState king, GameModel game)
        {
            var moves = new List<MoveData>();
            var side = king.PlayerType;
            var startRank = side == PlayerType.White ? GameConstants.StartIndex : GameConstants.FinishIndex;
            if (king.Square != new Square(4, startRank) || king.HasMoved || game.IsKingInCheck(side))
            {
                return moves;
            }

            AddCastlingMove(moves, game, king, -1);
            AddCastlingMove(moves, game, king, 1);
            return moves;
        }

        private static void AddCastlingMove(List<MoveData> moves, GameModel game, PieceState king, int direction)
        {
            var from = king.Square;
            var rookFile = direction < 0 ? GameConstants.StartIndex : GameConstants.FinishIndex;
            var rook = game.GetPieceAt(new Square(rookFile, from.Y));
            if (rook == null || rook.Type != PieceType.Rook || rook.PlayerType != king.PlayerType || rook.HasMoved)
            {
                return;
            }

            for (var file = from.X + direction; file != rookFile; file += direction)
            {
                if (game.GetPieceAt(new Square(file, from.Y)) != null)
                {
                    return;
                }
            }

            var transit = from.Offset(direction, 0);
            if (game.LeavesKingInCheck(new PieceMoveCommand(game, king, transit), king.PlayerType))
            {
                return;
            }

            var target = from.Offset(direction * 2, 0);
            var castling = new CommandContainer(new PieceMoveCommand(game, king, target), new PieceMoveCommand(game, rook, transit));
            if (!game.LeavesKingInCheck(castling, king.PlayerType))
            {
                moves.Add(new MoveData(target, castling, rook));
            }
        }
    }
}
