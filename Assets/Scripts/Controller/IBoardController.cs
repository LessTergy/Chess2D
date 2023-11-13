using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Controller
{
    public interface IBoardController
    {
        public BoardView BoardView { get; }

        public CellView GetCell(int x, int y);

        public CellState GetCellStateForPiece(int x, int y, PieceView piece);

        public void ReplacePiece(PieceView piece, Vector2Int cellCoord);

        public void HidePiece(PieceView piece);
        public void ShowPiece(PieceView piece);
    }
}