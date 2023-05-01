using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedroDB.Engine; 
public class Program {
    public static void Main(string[] args) {
        PedroEngine engine = new(new() { DatabasePath = Environment.CurrentDirectory+"/db" });

        engine.AddDatabase("library");

    }
}
