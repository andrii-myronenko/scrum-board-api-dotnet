using Microsoft.AspNetCore.Identity;
using ScrumBoardAPI.Data;
using ScrumBoardAPI.Core.Models.User;

namespace ScrumBoardAPI.Core.Contracts;

public interface IAuthManager
{
    Task<IEnumerable<IdentityError>> Register(CreateUserDto createUserDto);

    Task<AuthResponceDto?> Login(LoginDto loginDto);

    Task<string> CreateRefreshToken(AUser user);

    Task<AuthResponceDto?> VerifyRefreshToken(AuthResponceDto requestDto);
}
