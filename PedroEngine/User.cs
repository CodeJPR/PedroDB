using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PedroDB; 
public class User {
    public string Username { get; set; }

    public string PasswordHash { get; set; }


    public User(string username, string password) {
        Username = username;
        SHA512 sha = SHA512.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(password);
        byte[] hash = sha.ComputeHash(bytes);
        PasswordHash = Convert.ToBase64String(hash);
    }

    public User() {
        Username = "";
        PasswordHash = ""; 
    }

    public override bool Equals(object? obj) {
        return obj is User user &&
               Username == user.Username &&
               PasswordHash == user.PasswordHash;
    }

    public override int GetHashCode() {
        return HashCode.Combine(Username, PasswordHash);
    }
}
