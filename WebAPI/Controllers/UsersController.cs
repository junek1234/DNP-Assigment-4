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
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepo;

        public UsersController(IUserRepository userRepo)
        {
            this.userRepo = userRepo;
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddUser([FromBody] CreateUserDto request)
        {
            await VerifyUserNameIsAvailableAsync(request.UserName);

            User user = new(request.UserName, request.Password);
            User created = await userRepo.AddAsync(user);
            UserDto dto = new()
            {
                Id = created.Id,
                UserName = created.Username
            };
            return Created($"/users/{dto.Id}", created);
        }

        private async Task VerifyUserNameIsAvailableAsync(string userName)
        {
            //TODO
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<User>> UpdateUser([FromRoute] int id, [FromBody] UpdateUserDto request)
        {
            User userToUpdate = await userRepo.GetSingleAsync(id);
            userToUpdate.Username = request.NewUserName;
            userToUpdate.Password = request.NewPassword;
            await userRepo.UpdateAsync(userToUpdate);

            return NoContent();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetUser([FromRoute] int id)
        {
            User userToGet = await userRepo.GetSingleAsync(id);
            UserDto dto = new()
            {
                Id = userToGet.Id,
                UserName = userToGet.Username
            };
            return Ok(dto);
        }
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers([FromQuery] string? titleContains = null, [FromQuery] int? userId = null)
        {
            List<User> users = (List<User>)userRepo.GetMany();
            return Ok(users);
        }
    }

    
}
