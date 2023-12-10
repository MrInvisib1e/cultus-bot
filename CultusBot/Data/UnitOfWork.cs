using CultusBot.Data.Entities;
using CultusBot.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CultusBot.Data
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>
                where TContext : DbContext, IDisposable
    {
        private Dictionary<(Type type, string name), object> _repositories;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">Instance of the db context</param>
        public UnitOfWork(TContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the instance of the specific repository
        /// </summary>
        /// <typeparam name="TEntity">
        /// The entity of the repository. Must be a BaseEntity
        /// </typeparam>
        /// <returns>Instance of the IRepository</returns>
        public IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : BaseEntity
        {
            return (IRepository<TEntity>)GetOrAddRepository(typeof(TEntity), new Repository<TEntity>(Context));
        }

        /// <summary>
        /// Holds the db context 
        /// </summary>
        public TContext Context { get; }

        /// <summary>
        /// Executes the Save action
        /// </summary>
        /// <returns>The number of db operations</returns>
        public int Save()
        {
            return Context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public async Task<DbContext> GetContext()
        {
            return await Task.FromResult(Context);
        }

        /// <summary>
        /// Dispose the context
        /// </summary>
        public void Dispose()
        {
            Context?.Dispose();
        }

        internal object GetOrAddRepository(Type type, object repo)
        {
            _repositories ??= new Dictionary<(Type type, string Name), object>();

            if (_repositories.TryGetValue((type, repo.GetType().FullName), out var repository)) return repository;
            _repositories.Add((type, repo.GetType().FullName), repo);
            return repo;
        }
    }
}
