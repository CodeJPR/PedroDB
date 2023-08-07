using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedroDB.Shell.Commands;
internal class ListCommand : ICommand {
    public string Name => "list";

    public string Args { get; set; }

    public void Execute() {
        var split = Args.Split(' ');

        switch (split[0]) {
            case "db":
            case "databases":
                var dbs = PedroShell.Engine!.Databases;
                foreach(var db in dbs) {
                    Console.WriteLine(db);
                }
                break;
            case "col":
            case "collections":
                var cols = PedroShell.CurrentDb!.Collections;
                foreach (var col in cols) {
                    Console.WriteLine(col);
                }
                break;
            case "entries":
                var collection = PedroShell.CurrentDb!.GetCollection<dynamic>(split[1]);

                foreach(var entry in collection) {
                    Console.WriteLine(entry);
                }

                break;
        }
    }

    public string GetHelp() {
        return @"List Help:

Format: list <ITEM> [COLLECTION_NAME]

ITEM must be one of the following:
    - databases OR db
    - collections OR col
    - entries
If 'entries' is typed, must be followed by collection name";
    }

    public bool IsValid() {
        if(PedroShell.Engine is null) 
            return false;

        Args = Args.Trim();
        var split = Args.Split(' ');
        if (split[0] != "databases" && split[0] != "db" && 
            split[0] != "collections" && split[0] != "col" && 
            split[0] != "entries")
            return false;

        if (split[0] == "collections" || split[0] == "col") {
            if (PedroShell.CurrentDb is null) {
                return false;
            }
        }

        if (split[0] == "entries") {
            if (split.Length != 2)
                return false;

            if (PedroShell.CurrentDb is null) {
                return false;
            }

            if (!PedroShell.CurrentDb.HasCollection(split[1])) {
                return false;
            }
        }
        return true;
    }
}
