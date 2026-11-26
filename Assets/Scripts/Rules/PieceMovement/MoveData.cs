using Chess2D.Commands;
using Chess2D.Model;

namespace Chess2D.PieceMovement
{
    internal sealed class MoveData
    {
        public Square Target { get; }
        public ICommand Command { get; }
        public PieceState CastlingRook { get; }

        public MoveData(Square target, ICommand command, PieceState castlingRook = null)
        {
            Target = target;
            Command = command;
            CastlingRook = castlingRook;
        }
    }
}
