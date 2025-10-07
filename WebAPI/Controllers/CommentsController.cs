using System.Collections;
using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository CommentRepo;
        private readonly IUserRepository userRepository;

        public CommentsController(ICommentRepository CommentRepo, IUserRepository userRepository)
        {
            this.CommentRepo = CommentRepo;
            this.userRepository = userRepository;
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
        public async Task<ActionResult<List<Comment>>> GetComments([FromQuery] int? writtenById = null, [FromQuery] string? writtenByName = null, [FromQuery] int? postId = null)
        {
            IEnumerable<Comment> Comments = CommentRepo.GetMany();
            
           
            if (writtenById!=null)
            {
                Comments = Comments.Where(c => c.UserId == writtenById);
            }

            if (writtenByName!=null)
            {
                var userIds = userRepository.GetMany()
                    .Where(u => u.Username.Equals(writtenByName))
                    .Select(u => u.Id);
                Comments = Comments.Where(c => userIds.Contains(c.UserId));
            }
            if (postId != null)
            {
                Comments = Comments.Where(c => c.PostId == postId);
            }

            return Ok(Comments.ToList());
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Comment>> DeleteComment([FromRoute] int id)
        {
            await CommentRepo.DeleteAsync(id);
            return NoContent();
        }
    }

    
}
