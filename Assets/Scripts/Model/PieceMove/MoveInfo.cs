using Chess2D.Commands;
using Lesstergy.Chess2D;

namespace Chess2D.Model.PieceMove
{

    public class MoveInfo
    {

        public Cell cell;
        public ICommand moveAction;

        public MoveInfo(Cell cell, ICommand command)
        {
            this.cell = cell;
            this.moveAction = command;
        }
    }
}