using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Controller
{
    public static class BoardExtension
    {
        public static CellView GetCell(this IBoardController boardController, Vector2Int coord)
        {
            return boardController.GetCell(coord.x, coord.y);
        }
    }
}