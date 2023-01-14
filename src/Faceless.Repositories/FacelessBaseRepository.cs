﻿using Faceless.Domain;
using Faceless.Domain.Entities;
using Faceless.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Faceless.Repositories;

public class FacelessBaseRepository<T> : IFacelessBaseRepository<T> where T : BaseEntity
{
    protected FacelessDbContext _facelessDb;

    public FacelessBaseRepository(FacelessDbContext db)
    {
        _facelessDb = db;
    }

    public async Task AddAsync(T entity)
    {
        await _facelessDb.Set<T>().AddAsync(entity);
        await _facelessDb.SaveChangesAsync();
    }

    public async Task AddAllAsync(IEnumerable<T> entities)
    {
        var tasks = entities.Select(AddAsync);
        await Task.WhenAll(tasks);
    }

    public async Task UpdateAsync(T entity)
    {
        entity.UpdateAt = DateTimeOffset.Now;
        _facelessDb.Set<T>().Update(entity);
        await _facelessDb.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetById(id);
        entity.DeletedAt = DateTimeOffset.Now;
        _facelessDb.Set<T>().Update(entity);
        await _facelessDb.SaveChangesAsync();
    }

    public async Task<T> GetById(Guid id) =>
        await _facelessDb.Set<T>().FindAsync(id);

    public async Task<IEnumerable<T>> GetAll() =>
        await _facelessDb.Set<T>().ToListAsync();
}