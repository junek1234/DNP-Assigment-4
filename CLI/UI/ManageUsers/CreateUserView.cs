using System;
using System.Threading.Tasks;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository userRepository;
    public CreateUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }
    public async Task displayCreateUser()
    {
        Console.WriteLine("Create new user");
        Console.WriteLine("Username: ");
        string? username = Console.ReadLine();
        Console.WriteLine("Password: ");
        string? password = Console.ReadLine();
        User user = new User
        {
            Username = username,
            Password = password
        };
        User created = await userRepository.AddAsync(user);
        Console.WriteLine($"User with username: {created.Username} assigned with Id: {created.Id}");
    }
}
