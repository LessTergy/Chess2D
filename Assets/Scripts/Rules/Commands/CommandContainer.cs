namespace Chess2D.Commands
{
    internal sealed class CommandContainer : ICommand
    {
        private readonly ICommand[] _commands;

        public CommandContainer(params ICommand[] commands)
        {
            _commands = commands;
        }

        public void Execute()
        {
            foreach (var command in _commands)
            {
                command.Execute();
            }
        }

        public void Undo()
        {
            for (var i = _commands.Length - 1; i >= 0; i--)
            {
                _commands[i].Undo();
            }
        }
    }
}
