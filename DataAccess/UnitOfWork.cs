using DataAccess.Repositories;
using DBEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public interface IUnitOfWork
    {
        void InitDBTransaction(System.Data.IsolationLevel il);
        void CommitDBTransaction();
        void RollbackDBTransaction();
        bool IsInTransaction { get; }
        void SaveChanges();
        void Commit();
        void Dispose();

        #region Repositories
        ISearchesRepository SearchesRepository { get; }
        #endregion Repositories
    }
    public class UnitOfWork : IUnitOfWork
    {
        private WordFinderContext dbContext;

        public UnitOfWork(WordFinderContext wordFinderContext, ISearchesRepository searchesRepository)
        {
            dbContext = wordFinderContext;
            _searchesRepository = searchesRepository;
        }

        #region GeneralMethods
        public void InitDBTransaction(System.Data.IsolationLevel il)
        {
            this.dbContext.InitDBTransaction(il);
        }

        public void CommitDBTransaction()
        {
            this.dbContext.CommitDBTransaction();
        }

        public void RollbackDBTransaction()
        {
            this.dbContext.RollbackDBTransaction();
        }

        public bool IsInTransaction
        {
            get { return this.dbContext.IsInTransaction; }
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        public void Commit()
        {
            this.dbContext.Commit();
        }

        public void Dispose()
        {
            this.dbContext.Dispose();
        }
        #endregion GeneralMethods

        ~UnitOfWork()
        {
            Dispose();
        }

        #region Repositories

        private ISearchesRepository _searchesRepository;
        public ISearchesRepository SearchesRepository
        {
            get
            {
                return this._searchesRepository;
            }
        }
        #endregion Repositories
    }
}
