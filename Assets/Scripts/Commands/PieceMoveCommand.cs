using UnityEngine;

namespace Lesstergy.Chess2D {

    public class PieceMoveCommand : ICommand {

        private IBoardContoller boardController;
        private Vector2Int startPosition;
        private Vector2Int endPosition;

        public PieceMoveCommand(IBoardContoller boardController, Vector2Int startPosition, Vector2Int endPosition) {
            this.boardController = boardController;
            this.startPosition = startPosition;
            this.endPosition = endPosition;
        }
    
        public void Execute() {
            boardController.ReplacePiece(startPosition, endPosition);
        }

        public void Undo() {
            boardController.ReplacePiece(endPosition, startPosition);
        }
    }

}
