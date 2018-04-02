using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesstergy.Chess2D {

    public class PieceKillCommand : ICommand {

        private IBoardContoller boardController;
        private Cell cell;
        private Piece piece;

        public PieceKillCommand(IBoardContoller boardController, Cell cell, Piece piece) {
            this.boardController = boardController;
            this.cell = cell;
            this.piece = piece;
        }
        
        public void Execute() {
            boardController.HidePiece(cell, piece);
        }

        public void Undo() {
            boardController.ShowPiece(cell, piece);
        }
    }
}
