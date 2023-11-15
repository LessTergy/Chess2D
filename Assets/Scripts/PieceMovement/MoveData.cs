using Chess2D.Commands;
using Chess2D.UI;

namespace Chess2D.PieceMovement
{
    public class MoveData
    {
        public readonly CellView cellView;
        public readonly ICommand moveCommand;

        public MoveData(CellView cellView, ICommand moveCommand)
        {
            this.cellView = cellView;
            this.moveCommand = moveCommand;
        }
    }
}