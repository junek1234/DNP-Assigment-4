using System;
using Entities;
using InMemoryRepositories;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;
    public CreatePostView(IPostRepository postRepository, IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }
    public async Task displayCreatePost()
    {
        Console.WriteLine("Create new post");
        Console.WriteLine("Title: ");
        string? title = Console.ReadLine();
        Console.WriteLine("Body: ");
        string? body = Console.ReadLine();
      
        int userId = -1;
        while (0 == 0)
        {
            Console.WriteLine("User Id: ");
            try
            {
                userId = int.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Not an int!");
            }
             try
            {
                await userRepository.GetSingleAsync(userId);
                break;
            }
            catch
            {
                Console.WriteLine($"User with id: {userId} is not in the system!");
            }
        }


      


        Post post = new Post
        {
            Title = title,
            Body = body,
            UserId = userId
        };
        Post created = await postRepository.AddAsync(post);
        Console.WriteLine($"Post with title: {created.Title} and body: {created.Body} written by user: {created.UserId}");
    }
}
