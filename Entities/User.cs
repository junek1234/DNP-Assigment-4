using System;

namespace Entities;

public class User
{
    public User(string userName, string password)
    {
        Username = userName;
        Password = password;
    }

    public int Id { set; get; }
    public string Username { set; get; }
    public string Password { set; get; }

   
}
