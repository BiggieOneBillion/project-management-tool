namespace Project.APPLICATION.DTOs.Comment;

public record CreateCommentDto(
    string TaskId = "",
    string UserId = "",
    string Content = "",
    string? ParentId = null
);
