using PedroDB.Shell.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedroDB.Shell; 
internal class CommandManager {
    private readonly List<ICommand> commands = new();

    public CommandManager Register<T>() where T : ICommand, new() { 
        if(commands.Any(t => typeof(T) == t.GetType())) {
            return this;
        }

        if (typeof(T) == typeof(NullCommand))
            return this;

        T t = new();
        commands.Add(t);

        return this;
    }

    public ICommand? GetCommand(string name) {
        return commands.FirstOrDefault(c => c.Name == name);
    }
}
