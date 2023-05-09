using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PedroDB.Engine;

public record Person(string Name, int Age);
public class Program {
    public static void Main(string[] args) {
        PedroEngine engine = new(new() { DatabasePath = Environment.CurrentDirectory+"/db" });

        Database db = engine.GetDatabase("library")!;
        Collection<Person> people = db.GetCollection<Person>("users");
        people.Add(new("Pedro", 18));
        people.Add(new("John", 20));
        people.Add(new("Jane", 21));
        foreach(var p in people) {
            Console.WriteLine(p.Name);
            Console.WriteLine(p.Age);
        }
    }
}
