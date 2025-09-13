using System;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class DisplayPostView
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;
    public DisplayPostView(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
    }
    public async Task DisplaySinglePost()
    {
        int postId = -1;
        Post post;
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
                post = await postRepository.GetSingleAsync(postId);
                break;
            }
            catch
            {
                Console.WriteLine($"Post with id: {postId} is not in the system!");
            }
        }
        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"Body: {post.Body}");
        List<Comment> comments = commentRepository.GetMany().ToList();
        Console.WriteLine("Comments: ");
        foreach (Comment comment in comments)
        {
            if (comment.PostId == post.Id)
            {
                Console.WriteLine($"{comment.Body} /written by user with Id: {comment.UserId}");
            }
        }

    }
    public void DisplayPostsOverview()
    {
        List<Post> posts = postRepository.GetMany().ToList();
        foreach (Post post in posts)
        {
            Console.WriteLine($"[Title: {post.Title}, Id: {post.Id}]");
        }
    }
}
