using ScrumBoardAPI.Data;
using ScrumBoardAPI.Models.User;

namespace ScrumBoardAPI.Models.Workspace;

public class GetWorkspaceDetailsDto : GetWorkspaceDto
{
    public IList<GetTaskDto> Tasks { get; set; }

    public IList<GetUserDto> Users { get; set; }

    public GetUserDto Admin { get; set; }

    public GetWorkspaceDetailsDto(string name, List<GetTaskDto> tasks, List<GetUserDto> users, GetUserDto admin) : base(name)
    {
        this.Tasks = tasks;
        this.Users = users;
        this.Admin = admin;
    }
}
