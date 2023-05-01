using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PedroDB;
public class PedroEngine {

	private EngineConfiguration config;

	public PedroEngine(EngineConfiguration configuration) {
		config = configuration;
		Init();
	}

	private void Init() {
		if(!Directory.Exists(config.DatabasePath)) {
			bool result = Create();
			if(!result) {
                throw new InvalidOperationException("Failed to create database");
			}
		} else {
			bool result = Load();
			if(!result) {
                throw new InvalidOperationException("Failed to load database");
            }
		}
	}

	private bool Create() {
		Directory.CreateDirectory(config.DatabasePath);
		EngineMeta meta = new();
		meta.Users.Add(new User("admin", "admin"));
        WriteEngineMeta(meta);
		return true;
	}

	private bool Load() {
		return true;
	}


	private EngineMeta ReadEngineMeta() {
		char div = Path.DirectorySeparatorChar;
		string json = File.ReadAllText(config.DatabasePath+div+"data.meta");
		EngineMeta? meta = JsonSerializer.Deserialize<EngineMeta>(json);
		if(meta is null) {
            throw new InvalidOperationException("Failed to read metadata");
        }
		return meta;
	}

	private void WriteEngineMeta(EngineMeta meta){
		char div = Path.DirectorySeparatorChar;
        string json = JsonSerializer.Serialize(meta);
        File.WriteAllText(config.DatabasePath+div+"data.meta", json);
    }
    private DatabaseMeta ReadDBMeta(string db) {
		char div = Path.DirectorySeparatorChar;
        string json = File.ReadAllText(config.DatabasePath+div+db+div+"data.meta");
        DatabaseMeta? meta = JsonSerializer.Deserialize<DatabaseMeta>(json);
        if (meta is null) {
            throw new InvalidOperationException("Failed to read database metadata");
        }
        return meta;
    }

    private void WriteDBMeta(string db, DatabaseMeta meta) {
		char div = Path.DirectorySeparatorChar;
        string json = JsonSerializer.Serialize(meta);
        File.WriteAllText(config.DatabasePath+div+db+div+"data.meta", json);
    }


    #region Public Methods

    public bool AddUser(string username, string password) {
        EngineMeta meta = ReadEngineMeta();
        if (meta.Users.Any(u => u.Username == username)) {
            return false;
        }
        meta.Users.Add(new User(username, password));
        WriteEngineMeta(meta);
        return true;
    }

    public bool RemoveUser(string username) {
        EngineMeta meta = ReadEngineMeta();
        if (!meta.Users.Any(u => u.Username == username)) {
            return false;
        }
        meta.Users.RemoveAll(u => u.Username == username);
        WriteEngineMeta(meta);
        return true;
    }

	public void AddDatabase(string name) {
		char div = Path.DirectorySeparatorChar;
		if (name == "data.meta") {
			throw new InvalidOperationException("Invalid database name");
		}
		if (Directory.Exists(name)) {
			throw new InvalidOperationException("Database already exists");
		}
		var meta = ReadEngineMeta();
		meta.Databases.Add(name);
		WriteEngineMeta(meta);
		Directory.CreateDirectory(config.DatabasePath + div + name);
		DatabaseMeta dbMeta = new();
		WriteDBMeta(name, dbMeta);
	}


    #endregion
    class EngineMeta {
		public List<string> Databases { get; set; } = new();
		public List<User> Users { get; set; } = new();
	}

	class DatabaseMeta {
		public List<string> Collections = new();
	}
}
