using System;

namespace ApiContracts;

public class PostDto
{
    public required int Id{ set; get; }
    public required string Title { get; set; }
    public required string Body { get; set; }

}
