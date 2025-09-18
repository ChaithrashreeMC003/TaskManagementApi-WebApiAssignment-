using AutoMapper;
using TaskBoardApi.Models.Domain;
using TaskBoardApi.Models.DTOs;

namespace TaskBoardApi.Mapper
{
   

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Example: Domain → DTO
            CreateMap<UserProfile, UserProfileDto>().ReverseMap();
            CreateMap<UpdateUserProfileDto, UserProfile>().ReverseMap();

            CreateMap<UpdateUserDto,User>().ReverseMap();
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<CreateUserDto, User>().ReverseMap();
            CreateMap<CreatedUserResponseDto,User>().ReverseMap();

            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<CreateProjectDto, Project>();
            CreateMap<UpdateProjectDto, Project>();

            CreateMap<TaskItem, TaskItemDto>();
            CreateMap<CreateTaskItemDto, TaskItem>();
            CreateMap<UpdateTaskItemDto, TaskItem>();

            CreateMap<ProjectMember, ProjectMemberDto>().ReverseMap();
            CreateMap<PatchProjectDto, ProjectMemberDto>().ReverseMap();
            CreateMap<CreateProjectMemberDto, ProjectMember>();
            // Add more mappings as needed

            CreateMap<Tag, TagDto>();
            CreateMap<CreateTagDto, Tag>();
            CreateMap<PatchTagDto, Tag>();
        }
    }

}
