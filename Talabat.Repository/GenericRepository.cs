﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;

        public GenericRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();

        }

        public async Task<T> GetByIdAsync(int id)
        {

            return await _context.Set<T>().FindAsync(id);

        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).ToListAsync();
        }

        public async Task<T> GetByIdWithSpecAsync(ISpecifications<T> spec)
        {

            return await ApplySpecifications(spec).FirstOrDefaultAsync();
            
        }

        private IQueryable<T> ApplySpecifications(ISpecifications<T> spec)
        {
            return  SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), spec);
        }

        public Task<int> GetCountWithSpecAsync(ISpecifications<T> spec)
        {
            return ApplySpecifications(spec).CountAsync();
        }

        public async Task Add(T item)
        {
            await _context.Set<T>().AddAsync(item);
        }
    }
}
