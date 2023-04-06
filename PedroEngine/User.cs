using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PedroEngine; 
internal class User {
    public string Username { get; set; } = "";

    public string Password { get; set; } = "";

    public string PasswordHash {
        get {
            SHA512 sha = SHA512.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(Password);
            byte[] hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }

    public User(string username, string password) {
        Username = username;
        Password = password;
    }
}
