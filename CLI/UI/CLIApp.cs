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
    }
    public async Task StartAsync()
    {
        await createUserView.displayCreateUser();
        await createPostView.displayCreatePost();
        await createCommentView.displayCreateComment();
        displayPostView.DisplayPostsOverview();
        await displayPostView.DisplaySinglePost();
    }
}