using System;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class CreateCommentView
{
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;
    private readonly IPostRepository postRepository;
    public CreateCommentView(ICommentRepository commentRepository, IUserRepository userRepository, IPostRepository postRepository)
    {
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
        this.postRepository = postRepository;
    }
    public async Task displayCreateComment()
    {
        Console.WriteLine("Create new comment");
        Console.WriteLine("Body: ");
        string? body = Console.ReadLine();
        int userId = -1;
        int postId = -1;
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
        while (0 == 0)
        {
            Console.WriteLine("Post Id:");
            
            try
            {
                postId = int.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Not an int!");
            }

            try
            {
                await postRepository.GetSingleAsync(postId);
                break;
            }
            catch
            {
                Console.WriteLine($"Post with id: {postId} is not in the system!");
            }

        }
            
        


        Comment comment = new Comment(userId, postId, body);
        Comment created = await commentRepository.AddAsync(comment);
        Console.WriteLine($"Comment with Body: {created.Body} written by user: {created.UserId} on the post: {created.PostId}");
    }

}
