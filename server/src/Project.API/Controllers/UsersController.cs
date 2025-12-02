using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.APPLICATION.DTOs.User;
using Project.CORE.Entities;
using Project.CORE.Interfaces;

namespace Project.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UsersController> _logger;
    
    public UsersController(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<UsersController> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var users = await _userRepository.GetAllAsync();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return Ok(new { success = true, data = userDtos });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting users");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            
            if (user == null)
                return NotFound(new { success = false, message = "User not found" });
            
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(new { success = true, data = userDto });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user {Id}", id);
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto request)
    {
        try
        {
            // Check if email already exists
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return Conflict(new { success = false, message = "Email already exists" });
            }
            
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Email = request.Email,
                ImageUrl = request.ImageUrl,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            var created = await _userRepository.AddAsync(user);
            var userDto = _mapper.Map<UserDto>(created);
            
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, 
                new { success = true, data = userDto, message = "User created successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateUserDto request)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            
            if (user == null)
                return NotFound(new { success = false, message = "User not found" });
            
            user.Name = request.Name;
            user.ImageUrl = request.ImageUrl ?? user.ImageUrl;
            
            await _userRepository.UpdateAsync(user);
            var userDto = _mapper.Map<UserDto>(user);
            
            return Ok(new { success = true, data = userDto, message = "User updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user {Id}", id);
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _userRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user {Id}", id);
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
}
