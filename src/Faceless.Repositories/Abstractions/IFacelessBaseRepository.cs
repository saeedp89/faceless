using System.Linq.Expressions;
using Faceless.Domain;
using Faceless.Domain.Entities;

namespace Faceless.Repositories.Abstractions;

public interface IFacelessBaseRepository<T> where T : BaseEntity
{
    Task AddAsync(T entity);
    Task AddAllAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
    Task DeleteAllAsync(List<Guid> ids);
    Task<T?> GetById(Guid id);
    Task<IEnumerable<T>> GetAll(Expression<Func<T,bool>> predicate);
}