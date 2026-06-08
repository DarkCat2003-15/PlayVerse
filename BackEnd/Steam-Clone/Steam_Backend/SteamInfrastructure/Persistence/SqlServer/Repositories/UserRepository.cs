using Microsoft.EntityFrameworkCore;
using SteamDomain.Database.SqlServer.Context;
using SteamDomain.Database.SqlServer.Entities;
using SteamDomain.Interfaces.Repositories;


namespace SteamInfrastructure.Persistence.SqlServer.Repositories
{
    public class UserRepository(SteamContext context) : IUserRepository
    {
        public async Task<User> Create(User user)
        {
            try
            {
                // insert
                await context.Users.AddAsync(user);

                // execution // commit
                await context.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }

        public Task<bool> Delete(User Entity)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> Get(Guid id)
        {
            try
            {
                return await context.Users.FirstOrDefaultAsync(x => x.UserId == id && x.DeletedAt == null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User?> Get(string email)
        {
            try
            {
                return await context.Users.FirstOrDefaultAsync(x => x.Email == email && x.DeletedAt == null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> HasCreated()
        {
            try
            {
                return await context.Users.AnyAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> IfExists(Guid id)
        {
            try
            {
                return await context.Users.AnyAsync(x => x.UserId == id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> IfExists(string email)
        {
            return await context.Users.AnyAsync(x => x.Email == email && x.DeletedAt == null);
        }



        public IQueryable<User> Queryable()
        {
            try
            {
                return context.Users.Where(x => x.DeletedAt == null).AsQueryable();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<User> Update(User user)
        {
            try
            {
                context.Users.Update(user);
                await context.SaveChangesAsync();

                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
