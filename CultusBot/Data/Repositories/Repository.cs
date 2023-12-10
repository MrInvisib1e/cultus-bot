using CultusBot.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CultusBot.Data.Repositories
{
    /// <summary>
    /// The generic repository implementation.
    /// Only for a basic common CRUD operations
    /// Specific methods should be implemented in entity own repository
    /// </summary>
    /// <typeparam name="T">Repository entity.</typeparam>
    public class Repository<T> : IRepository<T>
            where T : BaseEntity
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="context">An EF Common context</param>
        public Repository(DbContext context)
        {
            _dbContext = context ?? throw new ArgumentException(nameof(context));
            _dbSet = _dbContext.Set<T>();
        }

        /// <summary>
        /// Dispose the context
        /// </summary>
        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        /// <summary>
        /// Sets the entity to deleted state
        /// </summary>
        /// <param name="entity">Entity which should be deleted</param>
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Sets the entities to deleted state
        /// </summary>
        /// <param name="entity">Entities which should be deleted</param>
        public void DeleteMany(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        #region Insert Functions

        /// <summary>
        /// Adds the entity to the db set
        /// </summary>
        /// <param name="entity">Entity which should be saved</param>
        /// <returns>Entity after adding to the db set</returns>
        public virtual T Insert(T entity)
        {
            return _dbSet.Add(entity).Entity;
        }

        /// <summary>
        /// Adds the entity to the db set
        /// </summary>
        /// <param name="entity">Entity which should be saved</param>
        /// <returns>Entity after adding to the db set</returns>
        public virtual async Task<T> InsertAsync(T entity)
        {
            return (await _dbSet.AddAsync(entity)).Entity;
        }

        /// <summary>
        /// Inserts multiple entities in one transaction
        /// </summary>
        /// <param name="entities">Collection of entities to insert</param>
        public virtual void InsertMany(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        #endregion

        #region Update Functions

        /// <summary>
        /// Updates the entity. Does not execute the query
        /// </summary>
        /// <param name="entity">Entity which should be updated</param>
        /// <returns>Entity after updating operation</returns>
        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void UpdateMany(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        #endregion

        #region Get

        /// <summary>
        /// Gets the entity from the storage
        /// </summary>
        /// <param name="id">The id of the entity to get</param>
        /// <returns>The entity from the db if found</returns>
        public T Get<V>(V id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T> GetAsync<V>(V id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual DbSet<T> Table
        {
            get
            {
                return _dbSet;
            }
        }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) 
        /// Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return _dbSet.AsNoTracking();
            }
        }

        #endregion
    }
}
