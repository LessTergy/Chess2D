using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesstergy.Chess2D {

    public class PieceKillCommand : ICommand {

        private IBoardContoller boardController;
        private Piece piece;

        public PieceKillCommand(IBoardContoller boardController, Piece piece) {
            this.boardController = boardController;
            this.piece = piece;
        }
        
        public void Execute() {
            boardController.HidePiece(piece);
        }

        public void Undo() {
            boardController.ShowPiece(piece);
        }
    }
}
