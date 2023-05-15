using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PedroDB; 
public class Database {

    private readonly string name;
    private readonly string path;
    private readonly List<string> collections;

    internal Database(DatabaseMeta meta) {
        name = meta.Name;
        path = meta.Path;
        collections = meta.Collections;
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

    /// <summary>
    /// Returns a read-only list of the existing collections
    /// </summary>
    public IReadOnlyCollection<string> Collections => collections.AsReadOnly();

    /// <summary>
    /// Creates a new collection.
    /// </summary>
    /// <param name="name">The name of the collection</param>
    /// <exception cref="InvalidOperationException">If the collection already exists or 
    /// an invalid name is provided</exception>
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

    /// <summary>
    /// Creates and returns a new collection.
    /// </summary>
    /// <typeparam name="T">The underlying type of the data</typeparam>
    /// <param name="name">The name of the collection to be created</param>
    /// <returns>The object representing the collection</returns>
    public Collection<T> AddCollection<T>(string name) {
        AddCollection(name);
        return GetCollection<T>(name, false);
    }

    /// <summary>
    /// Removes a collection from the database.
    /// </summary>
    /// <param name="name">The name of the collection that will be deleted</param>
    /// <param name="softDelete"><see langword="true"/> to keep the collection file, 
    /// <see langword="false"/> to delete the file</param>
    public void RemoveCollection(string name, bool softDelete = true) {
        collections.Remove(name);
        WriteDBMeta();
        if(!softDelete) {
            char div = Path.DirectorySeparatorChar;
            File.Delete(path+div+name+".json");
        }
    }

    /// <summary>
    /// Returns the collection with the given name and type. 
    /// If the type <typeparamref name="T"/> does not match the type of the collection, 
    /// an exception will be thrown.
    /// </summary>
    /// <typeparam name="T">The type of the underlying data</typeparam>
    /// <param name="name">The name of the collection</param>
    /// <param name="create"><see langword="true"/> to create a new collection if it doesn't exist</param>
    /// <returns>The object used to control the collection</returns>
    public Collection<T> GetCollection<T>(string name, bool create = false) {
        if(!collections.Contains(name)) {
            if(create) {
                AddCollection(name);
            } else {
                throw new InvalidOperationException("Collection does not exist");
            }
        }
        char div = Path.DirectorySeparatorChar;
        return new Collection<T>(path+div+name+".json");
    }
}

public class DatabaseMeta {
    public string Name { get; set; } = "";
    public List<string> Collections { get; set; } = new();
    public string Path { get; set; } = "";
}
