using System.Threading.Tasks;
using CLI.UI.ManageComments;
using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using Entities;
using RepositoryContracts;

public class CLIApp
{
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;
    private CreateUserView createUserView;
    private CreatePostView createPostView;
    private CreateCommentView createCommentView;
    private DisplayPostView displayPostView;

    public CLIApp(IUserRepository userRepository, ICommentRepository commentRepository, IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
        createUserView = new(userRepository);
        createPostView = new(postRepository, userRepository);
        createCommentView = new(commentRepository, userRepository, postRepository);
        displayPostView = new(postRepository, commentRepository);
        generateDummyData();
    }
    public async Task StartAsync()
    {
        int choice = -1;
        while (choice != 0)
        {
            Console.WriteLine("Menu");
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. Create Post");
            Console.WriteLine("3. Create Comment");
            Console.WriteLine("4. Posts Overview");
            Console.WriteLine("5. Post Info");
            Console.WriteLine("Select choice (number):");
            choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    await createUserView.displayCreateUser();
                    break;
                case 2:
                    await createPostView.displayCreatePost();
                    break;
                case 3:
                    await createCommentView.displayCreateComment();
                    break;
                case 4:
                    displayPostView.DisplayPostsOverview();
                    break;
                case 5:
                    await displayPostView.DisplaySinglePost();
                    break;
                case 0:
                    Console.WriteLine("Exiting...");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
    public async Task generateDummyData()
    {
        List<User> users = new List<User>();
        for (int i = 1; i <= 5; i++)
        {
            User user = new User { Username = $"DummyUser{i}", Password = $"Password{i}" };
            User addedUser = await userRepository.AddAsync(user);
            users.Add(addedUser);
        }

        List<Post> posts = new List<Post>();
        for (int i = 1; i <= 5; i++)
        {
            Post post = new Post
            {
                Title = $"Dummy Post {i}",
                Body = $"This is the body of dummy post {i}.",
                UserId = users[(i - 1) % users.Count].Id
            };
            Post addedPost = await postRepository.AddAsync(post);
            posts.Add(addedPost);
        }

        for (int i = 1; i <= 5; i++)
        {
            Comment comment = new Comment
            {
                Body = $"This is dummy comment {i}.",
                UserId = users[(i - 1) % users.Count].Id,
                PostId = posts[(i - 1) % posts.Count].Id
            };
            await commentRepository.AddAsync(comment);
        }
    }
}