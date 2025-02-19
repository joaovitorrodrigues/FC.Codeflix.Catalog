using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Infra.Data.EF;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ApplicationUseCases = FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.DeleteCategory
{
    [Collection(nameof(DeleteCategoryTestFixture))]
    public class DeleteCategoryTest
    {
        private readonly DeleteCategoryTestFixture _fixture;

        public DeleteCategoryTest(DeleteCategoryTestFixture fixture)
        => _fixture = fixture;


        [Fact(DisplayName = nameof(DeleteCategory))]
        [Trait("Integration/Application", "DeleteCategory - Use Cases")]
        public async Task DeleteCategory()
        {
            var dbContext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var exampleList = _fixture.GetExampleCategoriesList(10);

            await dbContext.AddRangeAsync(exampleList);
            var categoryExample = _fixture.GetExampleCategory();
            var tracking = await dbContext.AddAsync(categoryExample);
            dbContext.SaveChanges();
            tracking.State = EntityState.Detached;

            var input = new ApplicationUseCases.DeleteCategoryInput(categoryExample.Id);

            var useCase = new ApplicationUseCases.DeleteCategory(repository, unitOfWork);

            await useCase.Handle(input, CancellationToken.None);

            var assertDbContext = _fixture.CreateDbContext(true);
            var dbCategoryDeleted = await assertDbContext.Categories.FindAsync(categoryExample.Id);
            var dbCategories = await assertDbContext.Categories.ToListAsync();
            
            dbCategoryDeleted.Should().BeNull();          
            dbCategories.Should().HaveCount(exampleList.Count);
        }

        [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
        [Trait("Integration/Application", "DeleteCategory - Use Cases")]
        public async Task ThrowWhenCategoryNotFound()
        {
            var dbContext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);

            var exampleGuid = Guid.NewGuid();

            var input = new ApplicationUseCases.DeleteCategoryInput(exampleGuid);

            var useCase = new ApplicationUseCases.DeleteCategory(repository, unitOfWork);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{input.Id}' not found.");

        }
    }
}
