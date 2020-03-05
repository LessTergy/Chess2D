using Chess2D.Controller;
using Chess2D.UI;

namespace Chess2D.Commands
{

    public class PieceKillCommand : ICommand
    {

        private IBoardController _boardController;
        private Piece _piece;

        public PieceKillCommand(IBoardController boardController, Piece piece)
        {
            _boardController = boardController;
            _piece = piece;
        }

        public void Execute()
        {
            _boardController.HidePiece(_piece);
        }

        public void Undo()
        {
            _boardController.ShowPiece(_piece);
        }
    }
}
