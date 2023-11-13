using Chess2D.Commands;
using Chess2D.UI;

namespace Chess2D.Model.PieceMove
{
    public class MoveInfo
    {
        public readonly CellView cellView;
        public readonly ICommand moveCommand;

        public MoveInfo(CellView cellView, ICommand command)
        {
            this.cellView = cellView;
            moveCommand = command;
        }
    }
}