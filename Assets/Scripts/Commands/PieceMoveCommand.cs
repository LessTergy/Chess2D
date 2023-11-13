using Chess2D.Controller;
using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Commands
{

    public class PieceMoveCommand : ICommand
    {

        private IBoardController _boardController;
        private PieceView _piece;
        private Vector2Int _cellCoord;

        private Vector2Int _prevCellCoord;

        public PieceMoveCommand(IBoardController boardController, PieceView piece, Vector2Int cellCoord)
        {
            _boardController = boardController;
            _piece = piece;
            _cellCoord = cellCoord;

            _prevCellCoord = piece.cellCoord;
        }

        public void Execute()
        {
            _boardController.ReplacePiece(_piece, _cellCoord);
        }

        public void Undo()
        {
            _boardController.ReplacePiece(_piece, _prevCellCoord);
        }
    }

}
