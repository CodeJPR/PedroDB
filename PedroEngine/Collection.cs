using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PedroDB;
public class Collection<T> : IEnumerable<T>, IDisposable {

    private readonly string path;
    FileStream fileStream;

    internal Collection(string path) {
        this.path = path;
    }

    private void OpenStream() {
        if (fileStream is not null)
            return;

        fileStream = new(path, FileMode.Open, FileAccess.ReadWrite);
    }

    #region Public Methods

    /// <summary>
    /// Appends the given object to the end of the collection
    /// </summary>
    /// <param name="t">The object that will be appended</param>
    public void Add(T t) {
        OpenStream();
        var json = JsonSerializer.Serialize(t);
        long oldPos = fileStream.Position;
        fileStream.Seek(0, SeekOrigin.End);
        StreamWriter sw = new(fileStream, leaveOpen: true);
        sw.Write(json);
        sw.Write('\n');
        sw.Close();

        fileStream.Seek(oldPos, SeekOrigin.Begin);
    }

    /// <summary>
    /// Uses the default comparator to find the first element that matches the given object
    /// </summary>
    /// <param name="t">The object that will be deleted</param>
    public bool Remove(T t) {
        OpenStream();
        long oldpos = fileStream.Position;
        using var streamReader = new StreamReader(fileStream, leaveOpen: true);
        using var streamWriter = new StreamWriter(fileStream, leaveOpen: true);
        bool res = false;
        try {
            while (!streamReader.EndOfStream) {
                string line = streamReader.ReadLine() ?? "";
                T? obj = JsonSerializer.Deserialize<T>(line);
                if (obj is null) {
                    continue;
                }
                if (obj.Equals(t)) {
                    streamWriter.Write("");
                    res = true;
                    break;
                }
            }
        } finally {
            fileStream.Seek(oldpos, SeekOrigin.Begin);    
        }
        return res;
    }

    /// <summary>
    /// Removes all elements that match the given predicate
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public int Remove(Predicate<T> predicate) {
        OpenStream();
        long oldpos = fileStream.Position;
        using var streamReader = new StreamReader(fileStream, leaveOpen: true);
        using var streamWriter = new StreamWriter(fileStream, leaveOpen: true);
        int count = 0;
        try {
            while (!streamReader.EndOfStream) {
                string line = streamReader.ReadLine() ?? "";
                T? obj = JsonSerializer.Deserialize<T>(line);
                if (obj is null) {
                    continue;
                }
                if (predicate(obj)) {
                    streamWriter.Write("");
                    count++;
                    break;
                }
            }
        } finally {
            fileStream.Seek(oldpos, SeekOrigin.Begin);
        }
        return count;
    }

    /// <summary>
    /// Replaces the first element that matches the given object with the new one
    /// </summary>
    /// <param name="oldValue">The value that will be replaced</param>
    /// <param name="newValue">The value that will be inserted</param>
    public void Replace(T oldValue, T newValue) {
        OpenStream();
        long oldpos = fileStream.Position;
        using var streamReader = new StreamReader(fileStream, leaveOpen: true);
        using var streamWriter = new StreamWriter(fileStream, leaveOpen: true);
        try {
            while (!streamReader.EndOfStream) {
                string line = streamReader.ReadLine() ?? "";
                T? obj = JsonSerializer.Deserialize<T>(line);
                if (obj is null) {
                    continue;
                }
                if (obj.Equals(oldValue)) {
                    streamWriter.Write(JsonSerializer.Serialize(newValue));
                    break;
                }
            }
        } finally {
            fileStream.Seek(oldpos, SeekOrigin.Begin);
        }   
    }

    /// <summary>
    /// Transforms objects that match the given predicate with the given transform function
    /// </summary>
    /// <param name="predicate">The function that matches the objects</param>
    /// <param name="transform">The function that modifies the object</param>
    /// <returns>The amount of itens transformed</returns>
    public int Update(Predicate<T> predicate, Func<T, T> transform) {
        OpenStream();
        long oldpos = fileStream.Position;
        using var streamReader = new StreamReader(fileStream, leaveOpen: true);
        using var streamWriter = new StreamWriter(fileStream, leaveOpen: true);
        int count = 0;
        try {
            while (!streamReader.EndOfStream) {
                string line = streamReader.ReadLine() ?? "";
                T? obj = JsonSerializer.Deserialize<T>(line);
                if (obj is null) {
                    continue;
                }
                if (predicate(obj)) {
                    streamWriter.Write(JsonSerializer.Serialize(transform(obj)));
                    count++;
                }
            }
        } finally {
            fileStream.Seek(oldpos, SeekOrigin.Begin);
        }
        return count;
    }


    #endregion

    #region Interfaces

    /// <summary>
    /// Closes the file stream that reads the file for this collection
    /// </summary>
    void IDisposable.Dispose() {
        fileStream.Dispose();
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator() {
        OpenStream();
        using var streamReader = new StreamReader(fileStream, leaveOpen: true);
        while (!streamReader.EndOfStream) {
            string line = streamReader.ReadLine() ?? "";
            T? obj = JsonSerializer.Deserialize<T>(line);
            if(obj is null) {
                continue;
            }
            yield return obj;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        OpenStream();
        using var streamReader = new StreamReader(fileStream, leaveOpen: true);
        while (!streamReader.EndOfStream) {
            string line = streamReader.ReadLine() ?? "";
            T? obj = JsonSerializer.Deserialize<T>(line);
            if(obj is null) {
                continue;
            }
            yield return obj;
        }
    }

    #endregion
}