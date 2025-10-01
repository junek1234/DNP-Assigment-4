using System;

namespace Entities;

public class Comment
{
    public Comment(int postId, int userId, string body)
    {
        PostId = postId;
        UserId = userId;
        Body = body;
    }

    public int Id { set; get; }
    public string? Body { set; get; }
    public int UserId { set; get; }
    public int PostId { set; get; }
}
