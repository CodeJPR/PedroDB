using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedroDB.Shell.Commands;
internal class UseDbCommand : ICommand {
    public string Name => "use";

    public string Args { get; set; }

    public void Execute() {
        PedroShell.CurrentDb = PedroShell.Engine!.GetDatabase(Args);
    }

    public string GetHelp() {
        return @"Use Database Help:

Format: use <DB_NAME>

DB_NAME is the name of the new database to be used.";
    }

    public bool IsValid() {
        if (PedroShell.Engine is null)
            return false;

        Args = Args.Trim();

        try {
            PedroShell.Engine.GetDatabase(Args);
        } catch (Exception) {
            return false;
        }
        return true;
    }
}
