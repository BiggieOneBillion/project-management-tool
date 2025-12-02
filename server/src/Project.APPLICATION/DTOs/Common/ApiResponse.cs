namespace Project.APPLICATION.DTOs.Common;

public record ApiResponse<T>(
    bool Success,
    T? Data,
    string? Message,
    List<string>? Errors = null
);
