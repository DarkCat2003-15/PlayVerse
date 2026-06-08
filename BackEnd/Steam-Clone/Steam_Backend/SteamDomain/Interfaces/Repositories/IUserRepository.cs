using SteamDomain.Database.SqlServer.Entities;


namespace SteamDomain.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        new Task<User> Create(User user);
        Task<User?> Get(Guid id);
        Task<User?> Get(string email);
        new IQueryable<User> Queryable();
        Task<bool> IfExists(Guid id);
        new Task<User> Update(User id);
        Task<bool> HasCreated();
        Task<bool> IfExists(string email);
    }
}
