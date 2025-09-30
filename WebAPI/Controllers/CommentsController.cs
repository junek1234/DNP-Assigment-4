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
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository CommentRepo;

        public CommentsController(ICommentRepository CommentRepo)
        {
            this.CommentRepo = CommentRepo;
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> AddComment([FromBody] CreateCommentDto request)
        {
            Comment Comment = new(request.PostId, request.UserId, request.Body);
            Comment created = await CommentRepo.AddAsync(Comment);
            
            return Created($"/Comments/{created.Id}", created);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<Comment>> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentDto request)
        {
            Comment CommentToUpdate = await CommentRepo.GetSingleAsync(id);
            CommentToUpdate.Body = request.Body;
            //TODO: sth to check if the user wrote this
            await CommentRepo.UpdateAsync(CommentToUpdate);

            return NoContent();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Comment>> GetComment([FromRoute] int id)
        {
            Comment CommentToGet = await CommentRepo.GetSingleAsync(id);
            return Ok(CommentToGet);
        }
        [HttpGet]
        public async Task<ActionResult<List<Comment>>> GetComments()
        {
            List<Comment> Comments = (List<Comment>)CommentRepo.GetMany();
            return Ok(Comments);
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Comment>> DeleteComment([FromRoute] int id)
        {
            await CommentRepo.DeleteAsync(id);
            return NoContent();
        }
    }

    
}
