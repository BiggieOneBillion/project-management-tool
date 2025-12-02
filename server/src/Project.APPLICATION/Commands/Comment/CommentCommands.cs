using MediatR;
using Project.APPLICATION.DTOs.Comment;

namespace Project.APPLICATION.Commands.Comment;

public record CreateCommentCommand(
    string TaskId,
    string UserId,
    string Content
) : IRequest<CommentDto>;

public record UpdateCommentCommand(
    string Id,
    string Content
) : IRequest<CommentDto>;

public record DeleteCommentCommand(string Id) : IRequest<Unit>;
