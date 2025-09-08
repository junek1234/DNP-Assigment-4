using Entities;
using RepositoryContracts;


namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    private List<User> Users;
    public UserInMemoryRepository()
    {
        User user = new();
        user.username = "user1";
        user.password = "password1";
        
        AddAsync(user);
    }
    public Task<User> AddAsync(User User)
    {
        User.Id = Users.Any()
        ? Users.Max(p => p.Id) + 1
        : 1;
        Users.Add(User);
        return Task.FromResult(User);
    }


    public Task DeleteAsync(int id)
    {
        User? UserToRemove = Users.SingleOrDefault(p => p.Id == id);
        if (UserToRemove is null)
        {
            throw new InvalidOperationException(
                            $"User with ID '{id}' not found");
        }

        Users.Remove(UserToRemove);
        return Task.CompletedTask;
    }


    public IQueryable<User> GetManyAsync()
    {
        return Users.AsQueryable();
    }


    public Task<User> GetSingleAsync(int id)
    {
        User? User = Users.SingleOrDefault(p => p.Id == id);
        if (User is null)
        {
            throw new InvalidOperationException(
                            $"User with ID '{id}' not found");
        }
        return Task.FromResult(User);
    }


    public Task UpdateAsync(User User)
    {
        User? existingUser = Users.SingleOrDefault(p => p.Id == User.Id);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                    $"User with ID '{User.Id}' not found");
        }

        Users.Remove(existingUser);
        Users.Add(User);

        return Task.CompletedTask;
    }

}
