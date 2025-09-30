using System.Collections;
using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository PostRepo;

        public PostsController(IPostRepository PostRepo)
        {
            this.PostRepo = PostRepo;
        }

        [HttpPost]
        public async Task<ActionResult<Post>> AddPost([FromBody] CreatePostDto request)
        {
            Post Post = new(request.Title, request.Body, request.UserId);
            Post created = await PostRepo.AddAsync(Post);
            PostDto dto = new()
            {
                Id = created.Id,
                Title = created.Title,
                Body = created.Body
            };
            return Created($"/Posts/{dto.Id}", created);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<Post>> UpdatePost([FromRoute] int id, [FromBody] UpdatePostDto request)
        {
            Post PostToUpdate = await PostRepo.GetSingleAsync(id);
            PostToUpdate.Body = request.Body;
            PostToUpdate.Title = request.Title;
            //TODO: sth to check if the user wrote this
            await PostRepo.UpdateAsync(PostToUpdate);

            return NoContent();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Post>> GetPost([FromRoute] int id)
        {
            Post PostToGet = await PostRepo.GetSingleAsync(id);
            return Ok(PostToGet);
        }
        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetPosts()
        {
            List<Post> Posts = (List<Post>)PostRepo.GetMany();
            return Ok(Posts);
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Post>> DeletePost([FromRoute] int id)
        {
            await PostRepo.DeleteAsync(id);
            return NoContent();
        }
    }

    
}
