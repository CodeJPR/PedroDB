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

    private string path;
    FileStream fileStream = null;

    internal Collection(string path) {
        this.path = path;
    }

    private void OpenStream() {
        if (fileStream is not null)
            return;

        fileStream = new(path, FileMode.Open, FileAccess.ReadWrite);
    }

    #region Public Methods

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

    #endregion

    #region Interfaces


    void IDisposable.Dispose() {
        fileStream.Close();
        GC.SuppressFinalize(this);
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