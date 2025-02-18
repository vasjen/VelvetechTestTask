using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;
using TodoApi.Services.Interfaces;

namespace TodoApi.Services
{
    public class ItemsRepository<T> : IRepository<T> where T: class,IEntity
     {
        private readonly AppDbContext _context;

        public ItemsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetItems()
         => await _context.Set<T>().ToListAsync();

        public async Task<T> GetItem(long id)
              => await _context.Set<T>().AsNoTracking().Where(p => p.Id == id).SingleAsync();

        public async Task<bool> UpdateItem(T entity)
        {
            if (entity == null)
                return false;
            
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<T> CreateItem(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException($"{nameof(entity)} cann't be a null");

            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteTodoItem(long id)
        {
            var entity = await _context.Set<T>().AsNoTracking().Where(p => p.Id == id).SingleAsync();;
            if (entity == null)
                return false;

            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> IEntityExists(long id) =>
             await _context.Set<T>().AnyAsync(e => e.Id == id);

        
    }
}