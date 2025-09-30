using System;

namespace ApiContracts;

public class CreateCommentDto
{
    public required string Body { set; get; }
    public required int UserId { set; get; }
    public required int PostId { set; get; }
}
