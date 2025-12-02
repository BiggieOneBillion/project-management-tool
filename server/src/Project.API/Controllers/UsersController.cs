using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.APPLICATION.Commands.User;
using Project.APPLICATION.Queries.User;

namespace Project.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllUsersQuery();
            var users = await _mediator.Send(query);
            return Ok(new { success = true, data = users });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var query = new GetUserByIdQuery(id);
            var user = await _mediator.Send(query);
            
            if (user == null)
                return NotFound(new { success = false, message = "User not found" });
            
            return Ok(new { success = true, data = user });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        try
        {
            var query = new GetUserByEmailQuery(email);
            var user = await _mediator.Send(query);
            
            if (user == null)
                return NotFound(new { success = false, message = "User not found" });
            
            return Ok(new { success = true, data = user });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
    {
        try
        {
            var user = await _mediator.Send(command);
            return CreatedAtAction(
                nameof(GetById),
                new { id = user.Id },
                new { success = true, data = user, message = "User created successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateUserCommand command)
    {
        try
        {
            var updateCommand = command with { Id = id };
            var user = await _mediator.Send(updateCommand);
            return Ok(new { success = true, data = user, message = "User updated successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var command = new DeleteUserCommand(id);
            await _mediator.Send(command);
            return Ok(new { success = true, message = "User deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
}
