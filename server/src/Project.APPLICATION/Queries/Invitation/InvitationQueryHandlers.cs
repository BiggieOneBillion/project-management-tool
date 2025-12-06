using AutoMapper;
using MediatR;
// using Microsoft.EntityFrameworkCore;
using Project.APPLICATION.DTOs.Invitation;
using Project.CORE.Enums;
using Project.CORE.Interfaces;

namespace Project.APPLICATION.Queries.Invitation;

public class GetWorkspaceInvitationsQueryHandler 
    : IRequestHandler<GetWorkspaceInvitationsQuery, List<InvitationDto>>
{
    private readonly IInvitationRepository _repository;
    private readonly IMapper _mapper;
    
    public GetWorkspaceInvitationsQueryHandler(IInvitationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<List<InvitationDto>> Handle(
        GetWorkspaceInvitationsQuery request, 
        CancellationToken cancellationToken)
    {
        // Get all invitations for the workspace using repository method
        var invitations = await _repository.GetWorkspaceInvitationsAsync(request.WorkspaceId);
        
        // Filter by status if specified
        var filteredInvitations = request.Status == null 
            ? invitations 
            : invitations.Where(i => i.Status == request.Status);
        
        // Order by creation date descending
        var orderedInvitations = filteredInvitations
            .OrderByDescending(i => i.CreatedAt)
            .ToList();
        
        return _mapper.Map<List<InvitationDto>>(orderedInvitations);
    }
}
