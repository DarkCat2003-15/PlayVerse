using SteamApplication.Models.Dtos;
using SteamApplication.Models.Request.Users;
using SteamApplication.Models.Response;
using SteamDomain.Database.SqlServer.Entities;

namespace SteamApplication.Interfaces.Servicie
{
    public interface IUserService
    {
        public Task<GenericResponse<UserDto>> Create(CreateUsersRequest model);
        public Task<GenericResponse<UserDto>> Update(Guid id, UpdateUserRequest model, Guid userId);
        public GenericResponse<List<UserDto>> Get(FilterUserRequest model);
        public Task<GenericResponse<UserDto>> Get(Guid collaboratorId);
        public Task<GenericResponse<UserDto>> Me(Guid userId);
        public Task<GenericResponse<bool>> Delete(Guid collaboratorId);
        Task<User> GetExecutor(string value);
        public Task CreateFirstUser();

    }
}
