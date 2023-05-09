using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PedroDB; 
public class Database {

    private string name;
    private string path;
    private List<string> collections;

    public Database(DatabaseMeta meta) {
        name = meta.Name;
        path = meta.Path;
        collections = meta.Collections;
    }
    
    // yes duplicated code because fuck it
    private DatabaseMeta ReadDBMeta() {
        char div = Path.DirectorySeparatorChar;
        string json = File.ReadAllText(path + div + name + div + "data.meta");
        DatabaseMeta? meta = JsonSerializer.Deserialize<DatabaseMeta>(json, new JsonSerializerOptions() {
            IncludeFields = true
        });
        if (meta is null) {
            throw new InvalidOperationException("Failed to read database metadata");
        }
        return meta;
    }

    private void WriteDBMeta() {
        char div = Path.DirectorySeparatorChar;
        DatabaseMeta meta = new() {
            Name= name,
            Path= path,
            Collections = collections
        };
        string json = JsonSerializer.Serialize(meta, new JsonSerializerOptions() {
            IncludeFields = true
        });
        File.WriteAllText(path + div + "data.meta", json);
    }

    public IReadOnlyCollection<string> Collections => collections.AsReadOnly();

    public void AddCollection(string name) {
        char div = Path.DirectorySeparatorChar;
        if(name == "data.meta") {
            throw new InvalidOperationException("Invalid collection name");
        }
        if(collections.Contains(name) || File.Exists(path + div + name + ".json")) {
            throw new InvalidOperationException("Collection already exists");
        }

        collections.Add(name);
        File.WriteAllText(path+div+name+".json", string.Empty);
        WriteDBMeta();
    }

    public void RemoveCollection(string name) {
        collections.Remove(name);
        WriteDBMeta();
    }

    public Collection<T> GetCollection<T>(string name) {
        char div = Path.DirectorySeparatorChar;
        return new Collection<T>(path+div+name+".json");
    }
}

public class DatabaseMeta {
    public string Name;
    public List<string> Collections = new();
    public string Path;
}
