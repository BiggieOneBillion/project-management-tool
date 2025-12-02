using MediatR;
using Project.APPLICATION.DTOs.Comment;

namespace Project.APPLICATION.Queries.Comment;

public record GetTaskCommentsQuery(string TaskId) : IRequest<List<CommentDto>>;
