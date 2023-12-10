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
    public interface IRepository<T> : IDisposable
        where T : BaseEntity
    {
        /// <summary>
        /// Adds the entity to the db set
        /// </summary>
        /// <param name="entity">Entity which should be saved</param>
        /// <returns>Entity after adding to the db set</returns>
        T Insert(T entity);

        /// Adds the entity to the db set
        /// </summary>
        /// <param name="entity">Entity which should be saved</param>
        /// <returns>Entity after adding to the db set</returns>
        Task<T> InsertAsync(T entity);

        /// <summary>
        /// Inserts multiple entities in one transaction
        /// </summary>
        /// <param name="entities">Collection of entities to insert</param>
        void InsertMany(IEnumerable<T> entities);

        /// <summary>
        /// Updates the entity. Does not execute the query
        /// </summary>
        /// <param name="entity">Entity which should be updated</param>
        /// <returns>Entity after updating operation</returns>
        void Update(T entity);

        void UpdateMany(IEnumerable<T> entities);

        /// <summary>
        /// Sets the entity to deleted state
        /// </summary>
        /// <param name="entity">Entity which should be deleted</param>
        void Delete(T entity);

        /// <summary>
        /// Sets the entities to deleted state
        /// </summary>
        /// <param name="entity">Entities which should be deleted</param>
        void DeleteMany(IEnumerable<T> entities);

        /// <summary>
        /// Gets the entity from the storage
        /// </summary>
        /// <param name="id">The id of the entity to get</param>
        /// <returns>The entity from the db if found</returns>
        T Get<V>(V id);

        Task<T> GetAsync<V>(V id);
        /// <summary>
        /// Gets a table
        /// </summary>
        DbSet<T> Table { get; }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) 
        /// Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<T> TableNoTracking { get; }
    }
}
