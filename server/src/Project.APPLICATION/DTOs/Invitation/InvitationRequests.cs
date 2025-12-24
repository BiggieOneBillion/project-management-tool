using System.Security.Cryptography.X509Certificates;
using Project.APPLICATION.Queries.Project;
using Project.CORE.ValueObjects;

namespace Project.APPLICATION.DTOs.Invitation;

// public record InviteToWorkspaceRequest(
//     string WorkspaceId,
//     string Email,
//     WorkspaceRole Role = WorkspaceRole.MEMBER
// );

public class InviteToWorkspaceRequest {

    public string WorkspaceId {get; set;} = "";
    public string Email {get; set;} = "";
    public WorkspaceRole Role {get; set;} = WorkspaceRole.MEMBER;
}


public record InviteToProjectRequest(
    string ProjectId,
    string Email
);
