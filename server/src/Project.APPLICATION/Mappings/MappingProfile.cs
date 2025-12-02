using AutoMapper;
using Project.APPLICATION.DTOs.Workspace;
using Project.APPLICATION.DTOs.Project;
using Project.APPLICATION.DTOs.Task;
using Project.APPLICATION.DTOs.User;
using Project.APPLICATION.DTOs.Comment;
using Project.CORE.Entities;

namespace Project.APPLICATION.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, UserDto>();
        
        // Workspace mappings
        CreateMap<Workspace, WorkspaceDto>()
            .ForMember(dest => dest.MemberCount, opt => opt.MapFrom(src => src.Members.Count))
            .ForMember(dest => dest.ProjectCount, opt => opt.MapFrom(src => src.Projects.Count));
        
        CreateMap<Workspace, WorkspaceDetailDto>();
        
        CreateMap<WorkspaceMember, WorkspaceMemberDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
        
        // Project mappings
        CreateMap<ProjectEntity, ProjectDto>()
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.TaskCount, opt => opt.MapFrom(src => src.Tasks.Count))
            .ForMember(dest => dest.MemberCount, opt => opt.MapFrom(src => src.Members.Count));
        
        CreateMap<ProjectEntity, ProjectDetailDto>()
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        
        CreateMap<ProjectMember, ProjectMemberDto>();
        
        // Task mappings
        CreateMap<TaskEntity, TaskDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Comments.Count));
        
        CreateMap<TaskEntity, TaskDetailDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()));
        
        // Comment mappings
        CreateMap<Comment, CommentDto>();
    }
}
