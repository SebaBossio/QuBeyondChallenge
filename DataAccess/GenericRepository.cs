using DBEntities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess
{
    public interface IGenericRepository<T> where T : class, IEntityBase
    {
        IQueryable<T> GetAll();
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        T GetSingle(Guid pk);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Add(IEnumerable<T> entityEnumerable);
        void Edit(T entity);
        void Edit(IEnumerable<T> entityEnumerable);
        void Delete(T entity);
        void Delete(IEnumerable<T> entityEnumerable);
        void Detach(T entity);
        void Detach(IEnumerable<T> entityEnumerable);
    }

    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class, IEntityBase
    {
        internal WordFinderContext _context = null;
        internal bool _asNoTracking = false;

        internal GenericRepository(WordFinderContext context, bool asNoTracking = false)
        {
            this._context = context;
            this._asNoTracking = asNoTracking;
        }

        public virtual IQueryable<T> GetAll()
        {
            IQueryable<T> q = this._context.Set<T>();
            if (this._asNoTracking)
            {
                q = q.AsNoTracking();
            }
            return q;
        }

        public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = this._context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            if (this._asNoTracking)
            {
                query = query.AsNoTracking();
            }
            return query;
        }
        public T GetSingle(Guid pk)
        {
            return this._context.Set<T>().Find(pk);
        }
        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> q = this._context.Set<T>().Where(predicate);
            if (this._asNoTracking)
            {
                q = q.AsNoTracking();
            }
            return q;
        }

        public virtual void Add(T entity)
        {
            var dbEntityEntry = this._context.Entry<T>(entity);
            (dbEntityEntry.Entity as IEntityBase).CTS = DateTime.Now;
            this._context.Set<T>().Add(entity);
        }
        public virtual void Add(IEnumerable<T> entityEnumerable)
        {
            foreach (var entity in entityEnumerable)
            {
                this.Add(entity);
            }
        }

        public virtual void Edit(T entity)
        {
            var dbEntityEntry = this._context.Entry<T>(entity);
            (dbEntityEntry.Entity as IEntityBase).MTS = DateTime.Now;
            if (dbEntityEntry.State != EntityState.Added)
            {
                dbEntityEntry.State = EntityState.Modified;
            }
        }
        public virtual void Edit(IEnumerable<T> entityEnumerable)
        {
            foreach (var entity in entityEnumerable)
            {
                this.Edit(entity);
            }
        }

        public virtual void Delete(T entity)
        {
            var dbEntityEntry = this._context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }
        public virtual void Delete(IEnumerable<T> entityEnumerable)
        {
            foreach (var entity in entityEnumerable)
            {
                var dbEntityEntry = this._context.Entry<T>(entity);
                dbEntityEntry.State = EntityState.Deleted;
            }
        }

        public virtual void Detach(T entity)
        {
            var dbEntityEntry = this._context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Detached;
        }
        public virtual void Detach(IEnumerable<T> entityEnumerable)
        {
            foreach (var entity in entityEnumerable)
            {
                var dbEntityEntry = this._context.Entry<T>(entity);
                dbEntityEntry.State = EntityState.Detached;
            }
        }

        protected void RunProcedure(string procedure, int timeout = 10 * 60)
        {
            var dbConn = this._context.GetConnection();
            var conn = new SqlConnection(dbConn.ConnectionString);
            if (conn.State != ConnectionState.Open) conn.Open();

            SqlCommand sqlCommand = new SqlCommand(procedure, conn);
            sqlCommand.CommandTimeout = timeout;
            sqlCommand.ExecuteNonQuery();
        }
    }
}

