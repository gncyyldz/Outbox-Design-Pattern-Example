using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OutboxExample.Application.Repositories
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetWhere(Expression<Func<T, bool>> method);
        Task AddAsync(T model);
        Task SaveChangesAsync();
    }
}
