using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.APPLICATION.Commands.Invitation;
using Project.APPLICATION.DTOs.Invitation;
using Project.APPLICATION.Queries.Invitation;
using Project.CORE.Enums;
using System.Security.Claims;

namespace Project.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class InvitationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public InvitationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("workspace")]
    public async Task<IActionResult> InviteToWorkspace([FromBody] InviteToWorkspaceRequest request)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine("User ID: " + userId);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { success = false, message = "Invalid token" });
            }

            var command = new InviteToWorkspaceCommand(
                request.WorkspaceId,
                request.Email,
                request.Role,
                userId
            );

            Console.WriteLine("Command created for email: " + request.Email);

            var invitation = await _mediator.Send(command);

            return Ok(new
            {
                success = true,
                data = invitation,
                message = "Invitation sent successfully"
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPost("project")]
    public async Task<IActionResult> InviteToProject([FromBody] InviteToProjectRequest request)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { success = false, message = "Invalid token" });
            }

            var command = new InviteToProjectCommand(
                request.ProjectId,
                request.Email,
                userId
            );

            var invitation = await _mediator.Send(command);

            return Ok(new
            {
                success = true,
                data = invitation,
                message = "Invitation sent successfully"
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPost("accept/{token}")]
    [AllowAnonymous]
    public async Task<IActionResult> AcceptInvitation(string token)
    {
        try
        {
            // Get userId if user is authenticated
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var command = new AcceptInvitationCommand(token, userId);
            var invitation = await _mediator.Send(command);

            return Ok(new
            {
                success = true,
                data = invitation,
                message = "Invitation accepted successfully"
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPost("revoke/{invitationId}")]
    public async Task<IActionResult> RevokeInvitation(string invitationId)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { success = false, message = "Invalid token" });
            }

            var command = new RevokeInvitationCommand(invitationId, userId);
            await _mediator.Send(command);

            return Ok(new
            {
                success = true,
                message = "Invitation revoked successfully"
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("workspace/{workspaceId}")]
    public async Task<IActionResult> GetWorkspaceInvitations(
        string workspaceId,
        [FromQuery] InvitationStatus? status = InvitationStatus.PENDING)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { success = false, message = "Invalid token" });
            }

            var query = new GetWorkspaceInvitationsQuery(workspaceId, status);
            var invitations = await _mediator.Send(query);

            return Ok(new
            {
                success = true,
                data = invitations,
                message = $"Retrieved {invitations.Count} invitation(s)"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("pending")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPendingInvitations([FromQuery] string email)
    {
        try
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { success = false, message = "Email is required" });
            }

            var query = new GetUserPendingInvitationsQuery(email);
            var invitations = await _mediator.Send(query);

            return Ok(new
            {
                success = true,
                data = invitations,
                message = $"Retrieved {invitations.Count} pending invitation(s)"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
}
