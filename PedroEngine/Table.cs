using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PedroEngine {
    internal class Table {
        private readonly string tablePath;

        public string Name { get; set; }

        public Table(string name, string databasePath) {
            Name = name;
            tablePath = $"{databasePath}/{Name}.table";
        }

        private IEnumerable<string> ReadRows() {
            using StreamReader sr = new(tablePath);
            string? line;
            while((line = sr.ReadLine()) is not null ) {
                if(line == "ROW START") {
                    StringBuilder sb = new();
                    string? jsonLine;
                    while((jsonLine=sr.ReadLine()) != "ROW END") {
                        sb.Append(jsonLine);
                    }
                    yield return sb.ToString();
                }
            }
        }

        private void AppendRow(string json) {
            using FileStream sr = new(tablePath, FileMode.Append);
            using StreamWriter sw = new(sr);
            sw.WriteLine("ROW START");
            sw.WriteLine(json);
            sw.WriteLine("ROW END");
        }

        public void Add<T>(T item) {
            var json = JsonSerializer.Serialize(item);
            AppendRow(json);
        }

        public IEnumerable<T> Get<T>(Predicate<T> predicate) {
            var jsons = ReadRows();
            return jsons.Select(json => JsonSerializer.Deserialize<T>(json))
                .Where(t => t is not null)
                .Cast<T>()
                .Where(t => predicate(t))
                ?? new List<T>();
        }
    }
}
