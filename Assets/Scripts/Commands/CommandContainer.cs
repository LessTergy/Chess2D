using System.Collections.Generic;
using System.Linq;

public class CommandContainer : ICommand {

    private List<ICommand> commands;

    public CommandContainer(params ICommand[] list) {
        commands = list.ToList();
    }

    public void Add(ICommand command) {
        commands.Add(command);
    }

    public void Execute() {
        for (int i = 0; i < commands.Count; i++) {
            commands[i].Execute();
        }
    }

    public void Undo() {
        for (int i = commands.Count; i > 0; i--) {
            commands[i - 1].Undo();
        }
    }
}
