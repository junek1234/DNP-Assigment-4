using System;
using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string filePath = "Users.json";

    public UserFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<User> AddAsync(User User)
    {
        string UsersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> Users = JsonSerializer.Deserialize<List<User>>(UsersAsJson)!;
        int maxId = Users.Count > 0 ? Users.Max(c => c.Id) : 0;
        User.Id = maxId + 1;
        Users.Add(User);
        UsersAsJson = JsonSerializer.Serialize(Users);
        await File.WriteAllTextAsync(filePath, UsersAsJson);
        return User;
    }

    public async Task DeleteAsync(int id)
    {
        string UsersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> Users = JsonSerializer.Deserialize<List<User>>(UsersAsJson)!;
        User? UserToRemove = Users.SingleOrDefault(p => p.Id == id);
        if (UserToRemove is null)
        {
            throw new InvalidOperationException(
                            $"User with ID '{id}' not found");
        }
        Users.Remove(UserToRemove);
        UsersAsJson = JsonSerializer.Serialize(Users);
        await File.WriteAllTextAsync(filePath, UsersAsJson);
    }

    public IQueryable<User> GetMany()
    {
        string UsersAsJson = File.ReadAllTextAsync(filePath).Result;
        List<User> Users = JsonSerializer.Deserialize<List<User>>(UsersAsJson)!;
        return Users.AsQueryable();
    }


    public async Task<User> GetSingleAsync(int id)
    {
        string UsersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> Users = JsonSerializer.Deserialize<List<User>>(UsersAsJson)!;
        User? User = Users.SingleOrDefault(p => p.Id == id);
         if (User is null)
        {
            throw new InvalidOperationException(
                            $"User with ID '{id}' not found");
        }
        return User;
    }

    public async Task UpdateAsync(User User)
    {
        string UsersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> Users = JsonSerializer.Deserialize<List<User>>(UsersAsJson)!;
        User? UserToUpdate = Users.SingleOrDefault(User);
         if (UserToUpdate is null)
        {
            throw new InvalidOperationException(
                            $"User with ID '{User.Id}' not found");
        }
        Users.Remove(UserToUpdate);
        Users.Add(User);
        
        UsersAsJson = JsonSerializer.Serialize(Users);
        await File.WriteAllTextAsync(filePath, UsersAsJson);
    }
}