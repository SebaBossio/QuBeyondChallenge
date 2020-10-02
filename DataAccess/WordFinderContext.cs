using DBEntities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.Common;

namespace DataAccess
{
    public partial class WordFinderContext : DbContext
    {
        private IDbContextTransaction dbTran;

        public WordFinderContext(DbContextOptions<WordFinderContext> options) : base(options) { }

        ~WordFinderContext()
        {
            this.Dispose();
        }

        public void InitDBTransaction(System.Data.IsolationLevel il)
        {
            this.dbTran = this.Database.BeginTransaction(il);
        }

        public void CommitDBTransaction()
        {
            if (this.dbTran != null)
            {
                this.dbTran.Commit();
                this.dbTran = null;
            }
            else
            {
                throw new ApplicationException("Transacción no iniciada");
            }
        }

        public void RollbackDBTransaction()
        {
            if (this.dbTran != null)
            {
                this.dbTran.Rollback();
                this.dbTran = null;
            }
            else
            {
                throw new ApplicationException("Transacción no iniciada");
            }
        }

        public DbTransaction UnderlyingTransaction
        {
            get
            {
                if (this.dbTran != null)
                {
                    //return dbTran.UnderlyingTransaction;
                    return this.dbTran.GetDbTransaction();
                }
                else
                    return null;
            }
        }

        public bool IsInTransaction
        {
            get { return this.UnderlyingTransaction == null ? false : true; }
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public void Commit()
        {
            this.SaveChanges();
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            if (dbTran != null)
            {
                //Pongo try and catch porque no puedo modificar el timeout en tiempos de ejecución
                try
                {
                    dbTran.Rollback();
                    dbTran.Dispose();
                }
                catch { }
            }
            if (this != null)
            {
                base.Dispose();
            }
        }

        public DbConnection GetConnection()
        {
            return this.Database.GetDbConnection();
        }

        public DbSet<Searches> Searches { get; set; }
    }
}
