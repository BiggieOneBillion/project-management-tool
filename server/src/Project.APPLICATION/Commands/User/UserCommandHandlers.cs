using AutoMapper;
using MediatR;
using Project.APPLICATION.DTOs.User;
using Project.CORE.Entities;
using Project.CORE.Exceptions;
using Project.CORE.Interfaces;

namespace Project.APPLICATION.Commands.User;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    
    public CreateUserCommandHandler(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Check if email already exists
        var existingUser = await _repository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new BusinessRuleViolationException("UniqueEmail", $"Email {request.Email} is already in use");
        }
        
        var user = new CORE.Entities.User
        {
            Id = Guid.NewGuid().ToString(),
            Name = request.Name,
            Email = request.Email,
            ImageUrl = request.ImageUrl,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        var created = await _repository.AddAsync(user);
        return _mapper.Map<UserDto>(created);
    }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    
    public UpdateUserCommandHandler(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.Id);
        
        if (user == null)
            throw new EntityNotFoundException("User", request.Id);
        
        user.Name = request.Name;
        user.ImageUrl = request.ImageUrl ?? user.ImageUrl;
        user.UpdatedAt = DateTime.UtcNow;
        
        await _repository.UpdateAsync(user);
        return _mapper.Map<UserDto>(user);
    }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly IUserRepository _repository;
    
    public DeleteUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id);
        return Unit.Value;
    }
}
