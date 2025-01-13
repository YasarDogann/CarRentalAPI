using CarRentalApi.Data.Context;
using CarRentalApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApi.Data.Repositories
{
    // neden kulanıyoruz? DbContext içindeki methodları kendimizce şekillendirmek istediğimizde
    // veya DbContext içerisindek metholdarı kullanarak farklı yeni methodlar oluşturmak istediğimizde
    // Veri tabanı ile Data katmanı arasına filtre oluyor
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly CarRentalDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(CarRentalDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            entity.CreatedDate = DateTime.Now;
            _dbSet.Add(entity);
            //_context.SaveChanges();
        }

        public void Delete(TEntity entity, bool softDelete = true)
        {
            if (softDelete)
            {
                entity.ModifiedDate = DateTime.Now;
                entity.IsDeleted = true;
                _dbSet.Update(entity);
                //_context.SaveChanges();
            }
            else
            {
                _dbSet.Remove(entity); // HArd Delete
            }
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            Delete(entity); 
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate is null ? _dbSet : _dbSet.Where(predicate); 
        }

        public TEntity GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Update(TEntity entity)
        {
            entity.ModifiedDate = DateTime.Now;
            _dbSet.Update(entity);
           // _context.SaveChanges();

        }
    }
}
