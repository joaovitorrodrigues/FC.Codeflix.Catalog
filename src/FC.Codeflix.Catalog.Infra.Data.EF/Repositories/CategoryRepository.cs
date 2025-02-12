﻿using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.Infra.Data.EF.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CodeflixCatalogDbContext _dbContext;
        private DbSet<Category> _categories => _dbContext.Set<Category>();

        public CategoryRepository(CodeflixCatalogDbContext dbContext)
            => _dbContext = dbContext;

        public async Task Insert(Category aggregate, CancellationToken cancellationToken)
        => await _categories.AddAsync(aggregate, cancellationToken);

        public Task Delete(Category aggregate, CancellationToken _)
         => Task.FromResult(_categories.Remove(aggregate));

        public async Task<Category> Get(Guid id, CancellationToken cancellationToken)
        {
            var category = await _categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            NotFoundException.ThrowIfNull(category, $"Category '{id}' not found.");
            return category!;
        }

        public async Task<SearchOutput<Category>> Search(SearchInput input, CancellationToken cancellationToken)
        {
            var total = await _categories.CountAsync();
            var items = await _categories.ToListAsync();

            return new(input.Page,input.PerPage, total, items);
        }

        public Task Update(Category aggregate, CancellationToken _)
        => Task.FromResult(_categories.Update(aggregate));

    }
}
