using Chess2D.Model;

namespace Chess2D.Commands
{
    internal sealed class PieceKillCommand : ICommand
    {
        private readonly GameModel _game;
        private readonly PieceState _piece;

        public PieceKillCommand(GameModel game, PieceState piece)
        {
            _game = game;
            _piece = piece;
        }

        public void Execute() => _game.RemovePiece(_piece);
        public void Undo() => _game.PlacePiece(_piece);
    }
}
