using Microsoft.EntityFrameworkCore;
using OutboxExample.Application.Repositories;
using OutboxExample.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OutboxExample.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        readonly OutboxExampleDbContext _context;
        public Repository(OutboxExampleDbContext context)
        {
            this._context = context;
        }

        public DbSet<T> Table { get => _context.Set<T>(); }

        public async Task AddAsync(T model)
              => await Table.AddAsync(model);

        public IQueryable<T> GetAll()
            => Table;

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method)
            => Table.Where(method);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
