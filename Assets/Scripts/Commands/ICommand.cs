namespace Chess2D.Commands
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}