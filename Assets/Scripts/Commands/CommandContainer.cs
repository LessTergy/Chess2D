using System.Collections.Generic;
using System.Linq;

namespace Chess2D.Commands
{
    public class CommandContainer : ICommand
    {

        private readonly List<ICommand> _commands;

        public CommandContainer(params ICommand[] list)
        {
            _commands = list.ToList();
        }

        public void Add(ICommand command)
        {
            _commands.Add(command);
        }

        public void Execute()
        {
            for (var i = 0; i < _commands.Count; i++)
            {
                ICommand command = _commands[i];
                command.Execute();
            }
        }

        public void Undo()
        {
            for (int i = _commands.Count; i > 0; i--)
            {
                ICommand command = _commands[i - 1];
                command.Undo();
            }
        }
    }
}