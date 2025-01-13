using CarRentalApi.Data.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApi.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CarRentalDbContext _db;
        private IDbContextTransaction _transaction;

        public UnitOfWork(CarRentalDbContext db)
        {
            _db = db;
        }

        public async Task BeginTransaction()
        {
           _transaction = await _db.Database.BeginTransactionAsync();
        }

        public async Task CommitTransaction()
        {
            await _transaction.CommitAsync();
        }

        // Garbage Collector'a temizleme iznini verdiğimiz yer
        // silmez -- silinebilr yapar
        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task RollBackTransction()
        {
            await _transaction.RollbackAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
           return await _db.SaveChangesAsync();
        }
    }
}
