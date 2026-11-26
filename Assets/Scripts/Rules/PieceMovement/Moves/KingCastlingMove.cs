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
            var queenSide = GetCastlingRook(game, king, GameConstants.StartIndex);
            var kingSide = GetCastlingRook(game, king, GameConstants.FinishIndex);
            if ((queenSide == null && kingSide == null) || game.IsKingInCheck(king.PlayerType))
            {
                return moves;
            }

            AddCastlingMove(moves, game, king, queenSide);
            AddCastlingMove(moves, game, king, kingSide);
            return moves;
        }

        internal static PieceState GetCastlingRook(GameModel game, PieceState king, int file)
        {
            var rank = king.PlayerType == PlayerType.White ? GameConstants.StartIndex : GameConstants.FinishIndex;
            if (king.HasMoved || king.Square != new Square(4, rank))
            {
                return null;
            }

            var rook = game.GetPieceAt(new Square(file, rank));
            return rook?.Type == PieceType.Rook && rook.PlayerType == king.PlayerType && !rook.HasMoved ? rook : null;
        }

        private static void AddCastlingMove(List<MoveData> moves, GameModel game, PieceState king, PieceState rook)
        {
            if (rook == null)
            {
                return;
            }

            var from = king.Square;
            var direction = rook.Square.X < from.X ? -1 : 1;
            for (var file = from.X + direction; file != rook.Square.X; file += direction)
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
