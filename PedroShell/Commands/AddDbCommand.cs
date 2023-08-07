using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedroDB.Shell.Commands;
internal class AddDbCommand : ICommand {
    public string Name => "add_db";

    public string Args { get; set; }

    public void Execute() {
        PedroShell.Engine!.AddDatabase(Args);
    }

    public string GetHelp() {
        return @"Add Database Help:

Format: add_db <DB_NAME>

DB_NAME is the name of the new database to be created";
    }

    public bool IsValid() {
        Args = Args.Trim();
        if (PedroShell.Engine is null)
            return false;

        if (Args.Contains(' '))
            return false;

        if (Args.EndsWith(".meta"))
            return false;

        if (Args.EndsWith(".index"))
            return false;

        if (Args.Contains(Path.DirectorySeparatorChar) || Args.Contains(Path.AltDirectorySeparatorChar))
            return false;

        return true;
    }
}
