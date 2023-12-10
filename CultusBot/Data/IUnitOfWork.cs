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
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the instance of the specific repository
        /// </summary>
        /// <typeparam name="TEntity">
        /// The entity of the repository. Must be a BaseEntity
        /// </typeparam>
        /// <returns>Instance of the IRepository</returns>
        IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : BaseEntity;

        /// <summary>
        /// Executes the Save action
        /// </summary>
        /// <returns>The number of db operations</returns>
        int Save();

        Task<int> SaveAsync();
    }

    /// <summary>
    /// Generic implementation of the Unit of Work
    /// for the specific context
    /// </summary>
    /// <typeparam name="TContext">Generic context entity. Should be DbContext</typeparam>
    public interface IUnitOfWork<out TContext> : IUnitOfWork where TContext : DbContext
    {
        /// <summary>
        /// Holds the db context 
        /// </summary>
        TContext Context { get; }
    }
}
