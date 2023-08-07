using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedroDB.Shell.Commands;
internal class LoadEngineCommand : ICommand {
    public string Name { get; } = "load";
    public string Args { get; set; } = "";


    public void Execute() {
        if(Args.Trim() == ".") {
            Args = Environment.CurrentDirectory;
        }

        PedroShell.EnginePath = Args;

        EngineConfiguration config = new() {
            DatabasePath = PedroShell.EnginePath + "/db"
        };

        PedroShell.EngineConfig = config;
        PedroShell.Engine = new(config);
    }

    public string GetHelp() {
        return @"Load Engine Help:

Format: load <PATH>

PATH must be a valid path to the engine location on disk";
    }

    public bool IsValid() => Directory.Exists(Args);
}
