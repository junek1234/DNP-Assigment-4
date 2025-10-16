using System;
using System.Net.Http;
using System.Threading.Tasks;
using ApiContracts;
using Entities;

public class CLIApp
{
    private readonly JsonClient _jsonClient;

    public CLIApp()
    {
        var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5299/") };
        _jsonClient = new JsonClient(httpClient);
    }

    public async Task StartAsync()
    {
        int choice = -1;
        while (choice != 0)
        {
            Console.WriteLine();
            Console.WriteLine("Menu");
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. Create Post");
            Console.WriteLine("3. Create Comment");
            Console.WriteLine("4. List Posts");
            Console.WriteLine("5. Get Post by Id");
            Console.WriteLine("6. List Users");
            Console.WriteLine("7. Get User by Id");
            Console.WriteLine("8. List Comments");
            Console.WriteLine("9. Get Comment by Id");
            Console.WriteLine("10. Delete Post");
            Console.WriteLine("11. Delete User");
            Console.WriteLine("12. Delete Comment");
            Console.WriteLine("13. Update Comment");
            Console.WriteLine("14. Update User");
            Console.WriteLine("15. Update Post");
            Console.WriteLine("Select choice (number):");

            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                choice = -1;
                continue;
            }

            try
            {
                switch (choice)
                {
                    case 1:
                        await CreateUser();
                        break;
                    case 2:
                        await CreatePost();
                        break;
                    case 3:
                        await CreateComment();
                        break;
                    case 4:
                        await ListPosts();
                        break;
                    case 5:
                        await GetPost();
                        break;
                    case 6:
                        await ListUsers();
                        break;
                    case 7:
                        await GetUser();
                        break;
                    case 8:
                        await ListComments();
                        break;
                    case 9:
                        await GetComment();
                        break;
                    case 10:
                        await DeletePost();
                        break;
                    case 11:
                        await DeleteUser();
                        break;
                    case 12:
                        await DeleteComment();
                        break;
                    case 13:
                        await UpdateComment();
                        break;
                    case 14:
                        await UpdateUser();
                        break;
                    case 15:
                        await UpdatePost();
                        break;
                    case 0:
                        Console.WriteLine("Exiting...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }



    // --- Users ---
    private async Task CreateUser()
    {
        Console.WriteLine("Enter username:");
        var name = Console.ReadLine() ?? string.Empty;
        Console.WriteLine("Enter password:");
        var password = Console.ReadLine() ?? string.Empty;

        var createDto = new CreateUserDto { UserName = name, Password = password };
        var created = await _jsonClient.CreateUserAsync(createDto);
        Console.WriteLine($"Created user request sent for username: {createDto.UserName}");
    }

    private async Task ListUsers()
    {
        var users = await _jsonClient.GetUsersAsync();
        foreach (var u in users)
        {
            Console.WriteLine($"Id: {u.Id} | Username: {u.Username}");
        }
    }

    private async Task GetUser()
    {
        Console.WriteLine("Enter user id:");
        if (!int.TryParse(Console.ReadLine(), out var id)) return;
        var user = await _jsonClient.GetUserAsync(id);
        if (user == null) Console.WriteLine("User not found.");
        else Console.WriteLine($"Id: {user.Id} | Username: {user.Username}");
    }

    private async Task DeleteUser()
    {
        Console.WriteLine("Enter user id to delete:");
        if (!int.TryParse(Console.ReadLine(), out var id)) return;
        await _jsonClient.DeleteUserAsync(id);
        Console.WriteLine("Deleted (or attempted) user.");
    }

    private async Task UpdateUser()
    {
        Console.WriteLine("Enter user id to update:");
        if (!int.TryParse(Console.ReadLine(), out var id)) return;
        var oldUser = await _jsonClient.GetUserAsync(id);
        if (oldUser == null)
        {
            Console.WriteLine("User not found.");
            return;
        }
        Console.WriteLine($"Username: {oldUser.Username}");
        Console.WriteLine("Password: ");
        var oldPassword = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("Enter new username:");
        var name = Console.ReadLine() ?? string.Empty;
        Console.WriteLine("Enter new password:");
        var password = Console.ReadLine() ?? string.Empty;

        var updatedUser = new UpdateUserDto
        {
            OldUserName = oldUser.Username,
            OldPassword = oldPassword,
            NewUserName = name,
            NewPassword = password
        };

        await _jsonClient.UpdateUserAsync(id, updatedUser);
        Console.WriteLine("Updated (or attempted) user.");
    }
    // --- Posts ---
    private async Task CreatePost()
    {
        Console.WriteLine("Enter title:");
        var title = Console.ReadLine() ?? string.Empty;
        Console.WriteLine("Enter body/content:");
        var content = Console.ReadLine() ?? string.Empty;
        Console.WriteLine("Enter user id (author):");
        int.TryParse(Console.ReadLine(), out var userId);

        var dto = new CreatePostDto { Title = title, Body = content, UserId = userId };
        var created = await _jsonClient.CreatePostAsync(dto);
        Console.WriteLine($"Created post request sent for title: {dto.Title}");
    }

    private async Task ListPosts()
    {
        var posts = await _jsonClient.GetPostsAsync();
        foreach (var p in posts)
        {
            Console.WriteLine($"Id: {p.Id} | Title: {p.Title} | UserId: {p.UserId}");
        }
    }

    private async Task GetPost()
    {
        Console.WriteLine("Enter post id:");
        if (!int.TryParse(Console.ReadLine(), out var id)) return;
        var post = await _jsonClient.GetPostAsync(id);
        if (post == null) Console.WriteLine("Post not found.");
        else Console.WriteLine($"Id: {post.Id} | Title: {post.Title} | Body: {post.Body} | UserId: {post.UserId}");
    }

    private async Task DeletePost()
    {
        Console.WriteLine("Enter post id to delete:");
        if (!int.TryParse(Console.ReadLine(), out var id)) return;
        await _jsonClient.DeletePostAsync(id);
        Console.WriteLine("Deleted (or attempted) post.");
    }
    private async Task UpdatePost()
    {
        Console.WriteLine("Enter post id to update:");
        if (!int.TryParse(Console.ReadLine(), out var id)) return;

        var oldPost = await _jsonClient.GetPostAsync(id);
        if (oldPost == null)
        {
            Console.WriteLine("Post not found.");
            return;
        }

        Console.WriteLine($"Current title: {oldPost.Title}");
        Console.WriteLine($"Current body: {oldPost.Body}");

        Console.WriteLine("Enter new title:");
        var title = Console.ReadLine() ?? string.Empty;
        Console.WriteLine("Enter new body:");
        var body = Console.ReadLine() ?? string.Empty;

        var updated = new UpdatePostDto { Title = title, Body = body };
        await _jsonClient.UpdatePostAsync(id, updated);
        Console.WriteLine("Updated (or attempted) post.");
    }


    // --- Comments ---
    private async Task CreateComment()
    {
        Console.WriteLine("Enter post id:");
        int.TryParse(Console.ReadLine(), out var postId);
        Console.WriteLine("Enter user id:");
        int.TryParse(Console.ReadLine(), out var userId);
        Console.WriteLine("Enter content:");
        var content = Console.ReadLine() ?? string.Empty;

        var dto = new CreateCommentDto { PostId = postId, UserId = userId, Body = content };
        var created = await _jsonClient.CreateCommentAsync(dto);
        Console.WriteLine($"Created comment request sent for post id: {dto.PostId}");
    }

    private async Task ListComments()
    {
        var comments = await _jsonClient.GetCommentsAsync();
        foreach (var c in comments)
        {
            Console.WriteLine($"Id: {c.Id} | PostId: {c.PostId} | UserId: {c.UserId} | Body: {c.Body}");
        }
    }

    private async Task GetComment()
    {
        Console.WriteLine("Enter comment id:");
        if (!int.TryParse(Console.ReadLine(), out var id)) return;
        var c = await _jsonClient.GetCommentAsync(id);
        if (c == null) Console.WriteLine("Comment not found.");
        else Console.WriteLine($"Id: {c.Id} | PostId: {c.PostId} | UserId: {c.UserId} | Body: {c.Body}");
    }

    private async Task DeleteComment()
    {
        Console.WriteLine("Enter comment id to delete:");
        if (!int.TryParse(Console.ReadLine(), out var id)) return;
        await _jsonClient.DeleteCommentAsync(id);
        Console.WriteLine("Deleted (or attempted) comment.");
    }

    private async Task UpdateComment()
    {
        Console.WriteLine("Enter comment id to update:");
        if (!int.TryParse(Console.ReadLine(), out var id)) return;

        var oldComment = await _jsonClient.GetCommentAsync(id);
        if (oldComment == null)
        {
            Console.WriteLine("Comment not found.");
            return;
        }

        Console.WriteLine($"Current body: {oldComment.Body}");
        Console.WriteLine("Enter new body:");
        var body = Console.ReadLine() ?? string.Empty;

        var updated = new UpdateCommentDto { Body = body };
        await _jsonClient.UpdateCommentAsync(id, updated);
        Console.WriteLine("Updated (or attempted) comment.");
    }
}