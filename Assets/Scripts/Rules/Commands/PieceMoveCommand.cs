using Chess2D.Model;

namespace Chess2D.Commands
{
    internal sealed class PieceMoveCommand : ICommand
    {
        private readonly GameModel _game;
        private readonly PieceState _piece;
        private readonly Square _from;
        private readonly Square _to;

        public PieceMoveCommand(GameModel game, PieceState piece, Square to)
        {
            _game = game;
            _piece = piece;
            _from = piece.Square;
            _to = to;
        }

        public void Execute() => _game.MovePiece(_piece, _to);
        public void Undo() => _game.MovePiece(_piece, _from);
    }
}
