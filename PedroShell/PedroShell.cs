using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PedroDB;
using PedroDB.Shell.Commands;

namespace PedroDB.Shell;

public static class PedroShell {

    private static CommandManager commandManager = new();

    private static string enginePath = "";
    public static string EnginePath { get => enginePath; set => enginePath = value; }

    public static bool IsDbLoaded { get; set; } = false;

    public static Database? CurrentDb { get; set; } = null;

    public static EngineConfiguration? EngineConfig { get; set; } = null;

    public static PedroEngine? Engine { get; set; } = null;

    public static void Main(string[] args) {
        Flag flag = new(args);
        commandManager.Register<LoadEngineCommand>()
            .Register<AddDbCommand>()
            .Register<ListCommand>()
            .Register<UseDbCommand>();

        if(flag.TryGetFlagValue("-path", out enginePath)) {
            IsDbLoaded = true;
        }
        
        // start read-run-print loop
        bool running = true;
        while (running) {
            Console.Write("> ");
            string input = Console.ReadLine() ?? "";
            ICommand cmd = EvalCommand(input);
            cmd.Args = input.Remove(input.IndexOf(cmd.Name), cmd.Name.Length).Trim();
            ExecuteCommand(cmd);
        }


        //EngineConfiguration config = new() {
        //    DatabasePath = Environment.CurrentDirectory + "/db"
        //};
        //PedroEngine engine = new(config);

        //engine.AddDatabase("library");
        //Database db = engine.GetDatabase("library");
        //db.AddCollection("users");
        //Collection<Person> people = db.GetCollection<Person>("users");
        //people.Add(new("Pedro", 18));
        //people.Add(new("John", 20));
        //people.Add(new("Jane", 21));
        //foreach(var p in people) {
        //    Console.WriteLine(p.Name);
        //    Console.WriteLine(p.Age);
        //}
    }

    private static ICommand EvalCommand(string input) {
        string cmdName = input.Split(' ')[0];

        var cmd = commandManager.GetCommand(cmdName);
        if(cmd == null) {
            Console.WriteLine($"Command {cmdName} not found");
            return new NullCommand();
        }
        return cmd;
    }

    private static void ExecuteCommand(ICommand cmd) {
        if (cmd.GetType() == typeof(NullCommand))
            return;

        if (!cmd.IsValid()) {
            Console.WriteLine($"Command {cmd.Name} is not valid");
            Console.WriteLine(cmd.GetHelp());
            return;
        }

        cmd.Execute();
    }
}
