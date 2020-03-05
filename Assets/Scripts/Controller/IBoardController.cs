using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Controller
{

    public abstract class IBoardController : MonoBehaviour
    {

        public abstract Board GetBoard();

        public abstract Cell GetCell(int x, int y);

        public Cell GetCell(Vector2Int coord)
        {
            return GetCell(coord.x, coord.y);
        }

        public abstract Cell.State GetCellStateForPiece(int x, int y, Piece piece);

        public abstract void ReplacePiece(Piece piece, Vector2Int cellCoord);

        public abstract void HidePiece(Piece piece);
        public abstract void ShowPiece(Piece piece);
    }
}