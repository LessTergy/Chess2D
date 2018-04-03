namespace Lesstergy.Chess2D {

    public class MoveInfo {

        public Cell cell;
        public ICommand moveAction;

        public MoveInfo(Cell cell, ICommand command) {
            this.cell = cell;
            this.moveAction = command;
        }
    }
}
