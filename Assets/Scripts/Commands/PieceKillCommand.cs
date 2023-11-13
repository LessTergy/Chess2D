﻿using Chess2D.Controller;
using Chess2D.UI;

namespace Chess2D.Commands
{

    public class PieceKillCommand : ICommand
    {
        private readonly IBoardController _boardController;
        private readonly PieceView _piece;

        public PieceKillCommand(IBoardController boardController, PieceView piece)
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
