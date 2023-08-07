using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedroDB.Shell.Commands;
internal interface ICommand {

    /// <summary>
    /// The name of the command
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The arguments for this command
    /// </summary>
    public string Args { get; set; }

    /// <summary>
    /// Executes a command on the shell.
    /// </summary>
    void Execute();

    /// <summary>
    /// Checks if this command is correctly formatted for execution.
    /// </summary>
    /// <returns></returns>
    bool IsValid();

    /// <summary>
    /// Returns a string with the help text for this command.
    /// </summary>
    /// <returns></returns>
    string GetHelp();

}
