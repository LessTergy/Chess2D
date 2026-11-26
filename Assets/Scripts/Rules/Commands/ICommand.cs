namespace Chess2D.Commands
{
    internal interface ICommand
    {
        void Execute();
        void Undo();
    }
}
