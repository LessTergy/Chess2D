using Chess2D.Controller;
using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Commands
{
    public class PieceMoveCommand : ICommand
    {
        private readonly BoardController _boardController;
        private readonly PieceView _piece;
        private readonly Vector2Int _cellCoord;

        private readonly Vector2Int _previousCellCoord;

        public PieceMoveCommand(BoardController boardController, PieceView piece, Vector2Int cellCoord)
        {
            _boardController = boardController;
            _piece = piece;
            _cellCoord = cellCoord;

            _previousCellCoord = piece.cellCoord;
        }

        public void Execute()
        {
            _boardController.ReplacePiece(_piece, _cellCoord);
        }

        public void Undo()
        {
            _boardController.ReplacePiece(_piece, _previousCellCoord);
        }
    }
}
