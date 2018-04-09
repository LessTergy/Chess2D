using UnityEngine;

namespace Lesstergy.Chess2D {

    public class PieceMoveCommand : ICommand {

        private IBoardContoller boardController;
        private Piece piece;
        private Vector2Int cellCoord;

        private Vector2Int prevCellCoord;

        public PieceMoveCommand(IBoardContoller boardController, Piece piece, Vector2Int cellCoord) {
            this.boardController = boardController;
            this.piece = piece;
            this.cellCoord = cellCoord;

            prevCellCoord = piece.cellCoord;
        }
    
        public void Execute() {
            boardController.ReplacePiece(piece, cellCoord);
        }

        public void Undo() {
            boardController.ReplacePiece(piece, prevCellCoord);
        }
    }

}
