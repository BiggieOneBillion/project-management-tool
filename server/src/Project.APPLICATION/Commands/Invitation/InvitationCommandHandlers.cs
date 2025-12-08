using AutoMapper;
using MediatR;
using Project.APPLICATION.DTOs.Invitation;
using Project.APPLICATION.Services;
using Project.CORE.Entities;
using Project.CORE.Enums;
using Project.CORE.Interfaces;
using System.Security.Cryptography;

namespace Project.APPLICATION.Commands.Invitation;

public class InviteToWorkspaceCommandHandler : IRequestHandler<InviteToWorkspaceCommand, InvitationDto>
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public InviteToWorkspaceCommandHandler(
        IInvitationRepository invitationRepository,
        IWorkspaceRepository workspaceRepository,
        IUserRepository userRepository,
        IMapper mapper)
    {
        _invitationRepository = invitationRepository;
        _workspaceRepository = workspaceRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<InvitationDto> Handle(InviteToWorkspaceCommand request, CancellationToken cancellationToken)
    {
        // Verify workspace exists and get with members to check permissions
        var workspace = await _workspaceRepository.GetWithMembersAsync(request.WorkspaceId);
        if (workspace == null)
        {
            throw new InvalidOperationException("Workspace not found");
        }

        // Authorization: Only workspace owner or admin can invite users
        var isOwner = workspace.OwnerId == request.InvitedById;
        var member = workspace.Members.FirstOrDefault(m => m.UserId == request.InvitedById);
        var isAdmin = member?.Role == CORE.ValueObjects.WorkspaceRole.ADMIN;

        if (!isOwner && !isAdmin)
        {
            throw new UnauthorizedAccessException("Only workspace owners or admins can invite users");
        }

        // Check if user is already invited or is a member
        var existingInvitation = await _invitationRepository.GetPendingInvitationAsync(
            request.Email, request.WorkspaceId, null);
        
        if (existingInvitation != null)
        {
            throw new InvalidOperationException("User already has a pending invitation to this workspace");
        }

        // Create invitation
        var invitation = new CORE.Entities.Invitation
        {
            Id = Guid.NewGuid().ToString(),
            Email = request.Email,
            Token = GenerateInvitationToken(),
            Type = InvitationType.WORKSPACE,
            WorkspaceId = request.WorkspaceId,
            InvitedById = request.InvitedById,
            Status = InvitationStatus.PENDING,
            ExpiresAt = DateTime.UtcNow.AddDays(7), // 7 days expiry
            CreatedAt = DateTime.UtcNow
        };

        var created = await _invitationRepository.AddAsync(invitation);
        
        // TODO: Send invitation email
        
        return _mapper.Map<InvitationDto>(created);
    }

    private string GenerateInvitationToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes).Replace("+", "-").Replace("/", "_").TrimEnd('=');
    }
}

public class InviteToProjectCommandHandler : IRequestHandler<InviteToProjectCommand, InvitationDto>
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public InviteToProjectCommandHandler(
        IInvitationRepository invitationRepository,
        IProjectRepository projectRepository,
        IMapper mapper)
    {
        _invitationRepository = invitationRepository;
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<InvitationDto> Handle(InviteToProjectCommand request, CancellationToken cancellationToken)
    {
        // Verify project exists
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project == null)
        {
            throw new InvalidOperationException("Project not found");
        }

        // Check if user is already invited
        var existingInvitation = await _invitationRepository.GetPendingInvitationAsync(
            request.Email, null, request.ProjectId);
        
        if (existingInvitation != null)
        {
            throw new InvalidOperationException("User already has a pending invitation to this project");
        }

        // Create invitation
        var invitation = new CORE.Entities.Invitation
        {
            Id = Guid.NewGuid().ToString(),
            Email = request.Email,
            Token = GenerateInvitationToken(),
            Type = InvitationType.PROJECT,
            ProjectId = request.ProjectId,
            InvitedById = request.InvitedById,
            Status = InvitationStatus.PENDING,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };

        var created = await _invitationRepository.AddAsync(invitation);
        
        // TODO: Send invitation email
        
        return _mapper.Map<InvitationDto>(created);
    }

    private string GenerateInvitationToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes).Replace("+", "-").Replace("/", "_").TrimEnd('=');
    }
}

public class AcceptInvitationCommandHandler : IRequestHandler<AcceptInvitationCommand, InvitationDto>
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public AcceptInvitationCommandHandler(
        IInvitationRepository invitationRepository,
        IWorkspaceRepository workspaceRepository,
        IProjectRepository projectRepository,
        IUserRepository userRepository,
        IMapper mapper)
    {
        _invitationRepository = invitationRepository;
        _workspaceRepository = workspaceRepository;
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<InvitationDto> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
    {
        // Find invitation by token
        var invitation = await _invitationRepository.GetByTokenAsync(request.Token);
        
        if (invitation == null)
        {
            throw new InvalidOperationException("Invalid invitation token");
        }

        if (invitation.Status != InvitationStatus.PENDING)
        {
            throw new InvalidOperationException("Invitation is no longer valid");
        }

        if (invitation.ExpiresAt < DateTime.UtcNow)
        {
            invitation.Status = InvitationStatus.EXPIRED;
            await _invitationRepository.UpdateAsync(invitation);
            throw new InvalidOperationException("Invitation has expired");
        }

        // If userId is provided, add user to workspace/project
        if (!string.IsNullOrEmpty(request.UserId))
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            if (invitation.Type == InvitationType.WORKSPACE && invitation.WorkspaceId != null)
            {
                // Add user to workspace
                var workspace = await _workspaceRepository.GetWithMembersAsync(invitation.WorkspaceId);
                if (workspace != null)
                {
                    var member = new WorkspaceMember
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = user.Id,
                        WorkspaceId = workspace.Id,
                        Role = CORE.ValueObjects.WorkspaceRole.MEMBER,
                        JoinedAt = DateTime.UtcNow
                    };
                    
                    workspace.Members.Add(member);
                    await _workspaceRepository.UpdateAsync(workspace);
                }
            }
            else if (invitation.Type == InvitationType.PROJECT && invitation.ProjectId != null)
            {
                // Add user to project
                var project = await _projectRepository.GetWithMembersAsync(invitation.ProjectId);
                if (project != null)
                {
                    var member = new ProjectMember
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = user.Id,
                        ProjectId = project.Id
                    };
                    
                    project.Members.Add(member);
                    await _projectRepository.UpdateAsync(project);
                }
            }
        }

        // Mark invitation as accepted
        invitation.Status = InvitationStatus.ACCEPTED;
        await _invitationRepository.UpdateAsync(invitation);

        return _mapper.Map<InvitationDto>(invitation);
    }
}

public class RevokeInvitationCommandHandler : IRequestHandler<RevokeInvitationCommand, Unit>
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IWorkspaceRepository _workspaceRepository;

    public RevokeInvitationCommandHandler(
        IInvitationRepository invitationRepository,
        IWorkspaceRepository workspaceRepository)
    {
        _invitationRepository = invitationRepository;
        _workspaceRepository = workspaceRepository;
    }

    public async Task<Unit> Handle(RevokeInvitationCommand request, CancellationToken cancellationToken)
    {
        var invitation = await _invitationRepository.GetByIdAsync(request.InvitationId);
        
        if (invitation == null)
        {
            throw new InvalidOperationException("Invitation not found");
        }

        if (invitation.Status != InvitationStatus.PENDING)
        {
            throw new InvalidOperationException("Only pending invitations can be revoked");
        }

        // Authorization: Only workspace owner or admin can revoke invitations
        if (invitation.WorkspaceId != null)
        {
            var workspace = await _workspaceRepository.GetWithMembersAsync(invitation.WorkspaceId);
            if (workspace != null)
            {
                var isOwner = workspace.OwnerId == request.UserId;
                var member = workspace.Members.FirstOrDefault(m => m.UserId == request.UserId);
                var isAdmin = member?.Role == CORE.ValueObjects.WorkspaceRole.ADMIN;

                if (!isOwner && !isAdmin)
                {
                    throw new UnauthorizedAccessException("Only workspace owners or admins can revoke invitations");
                }
            }
        }

        invitation.Status = InvitationStatus.REVOKED;
        await _invitationRepository.UpdateAsync(invitation);

        return Unit.Value;
    }
}
