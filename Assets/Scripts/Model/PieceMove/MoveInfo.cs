using Chess2D.Commands;
using Chess2D.UI;

namespace Chess2D.Model.PieceMove
{
    public class MoveInfo
    {
        public readonly Cell cell;
        public readonly ICommand moveCommand;

        public MoveInfo(Cell cell, ICommand command)
        {
            this.cell = cell;
            moveCommand = command;
        }
    }
}