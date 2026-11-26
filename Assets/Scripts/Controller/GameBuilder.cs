using System.Collections.Generic;
using Chess2D.Model;

namespace Chess2D.Controller
{
    public static class GameBuilder
    {
        public static GameModel Build(ArrangementConfig arrangement)
        {
            return new GameModel(GetStartingPieces(arrangement));
        }

        private static IEnumerable<PieceState> GetStartingPieces(ArrangementConfig arrangement)
        {
            foreach (var side in new[] { PlayerType.White, PlayerType.Black })
            {
                foreach (var cell in arrangement.GetCells(side))
                {
                    yield return new PieceState(cell.pieceType, side, new Square(cell.coord.x, cell.coord.y));
                }
            }
        }
    }
}
