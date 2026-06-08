using SteamDomain.Database.SqlServer.Context;
using SteamDomain.Interfaces.Repositories;

namespace SteamInfrastructure.Persistence.SqlServer.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly SteamContext context;

        public GenericRepository(SteamContext context)
        {
            this.context = context;
        }

        public Task<T> Create(T entity)
        {
            context.Set<T>().Add(entity);
            return Task.FromResult(entity);
        }

        public Task<bool> Delete(T entity)
        {
            context.Set<T>().Remove(entity);
            return Task.FromResult(true);
        }

        public IQueryable<T> Queryable()
        {
            return context.Set<T>().AsQueryable();
        }

        public Task<T> Update(T entity)
        {
            context.Set<T>().Update(entity);
            return Task.FromResult(entity);
        }
    }
}
