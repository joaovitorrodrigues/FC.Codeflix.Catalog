using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.Infra.Data.EF;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FluentAssertions;
using EntityDomain = FC.Codeflix.Catalog.Domain.Entity;
using ApplicationUseCases = FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.SeedWork;
using FC.Codeflix.Catalog.Domain.Exceptions;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.UpdateCategory
{
    [Collection(nameof(UpdateCategoryTestFixture))]
    public class UpdateCategoryTest
    {
        private readonly UpdateCategoryTestFixture _fixture;

        public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
           => _fixture = fixture;

        [Theory(DisplayName = nameof(UpdateCategory))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        [MemberData(
            nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
            parameters: 5,
            MemberType = typeof(UpdateCategoryTestDataGenerator)
        )]
        public async Task UpdateCategory(EntityDomain.Category exampleCategory, UpdateCategoryInput exampleInput)
        {
            var dbContext = _fixture.CreateDbContext();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
            var trackingInfo = await dbContext.AddAsync(exampleCategory);
            dbContext.SaveChanges();
            trackingInfo.State = Microsoft.EntityFrameworkCore.EntityState.Detached;

            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);

            var useCase = new ApplicationUseCases.UpdateCategory(repository, unitOfWork);

            CategoryModelOutput output = await useCase.Handle(exampleInput, CancellationToken.None);

            var dbCategory = await _fixture.CreateDbContext(true).Categories.FindAsync(output.Id);

            dbCategory.Should().NotBeNull();
            dbCategory.Name.Should().Be(exampleInput.Name);
            dbCategory.Description.Should().Be(exampleInput.Description);
            dbCategory.Id.Should().Be(exampleInput.Id);
            dbCategory.IsActive.Should().Be((bool)exampleInput.IsActive!);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);

            output.Should().NotBeNull();
            output.Name.Should().Be(exampleInput.Name);
            output.Description.Should().Be(exampleInput.Description);
            output.IsActive.Should().Be((bool)exampleInput.IsActive!);



        }

        [Theory(DisplayName = nameof(UpdateCategoryWithoutIsActive))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        [MemberData(
         nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
         parameters: 10,
         MemberType = typeof(UpdateCategoryTestDataGenerator)
     )]
        public async Task UpdateCategoryWithoutIsActive(EntityDomain.Category exampleCategory, UpdateCategoryInput exampleInput)
        {

            var input = new UpdateCategoryInput(exampleInput.Id, exampleInput.Name, exampleInput.Description);

            var dbContext = _fixture.CreateDbContext();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
            var trackingInfo = await dbContext.AddAsync(exampleCategory);
            dbContext.SaveChanges();
            trackingInfo.State = Microsoft.EntityFrameworkCore.EntityState.Detached;

            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);

            var useCase = new ApplicationUseCases.UpdateCategory(repository, unitOfWork);

            CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

            var dbCategory = await _fixture.CreateDbContext(true).Categories.FindAsync(output.Id);

            dbCategory.Should().NotBeNull();
            dbCategory.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.Id.Should().Be(input.Id);
            dbCategory.IsActive.Should().Be((bool)exampleCategory.IsActive!);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be((bool)exampleCategory.IsActive!);

        }

        [Theory(DisplayName = nameof(UpdateCategoryOnlyName))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        [MemberData(
         nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
         parameters: 10,
         MemberType = typeof(UpdateCategoryTestDataGenerator)
    )]
        public async Task UpdateCategoryOnlyName(EntityDomain.Category exampleCategory, UpdateCategoryInput exampleInput)
        {

            var input = new UpdateCategoryInput(exampleInput.Id, exampleInput.Name);

            var dbContext = _fixture.CreateDbContext();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
            var trackingInfo = await dbContext.AddAsync(exampleCategory);
            dbContext.SaveChanges();
            trackingInfo.State = Microsoft.EntityFrameworkCore.EntityState.Detached;

            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);

            var useCase = new ApplicationUseCases.UpdateCategory(repository, unitOfWork);

            CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

            var dbCategory = await _fixture.CreateDbContext(true).Categories.FindAsync(output.Id);

            dbCategory.Should().NotBeNull();
            dbCategory.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.Id.Should().Be(input.Id);
            dbCategory.IsActive.Should().Be((bool)exampleCategory.IsActive!);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(exampleCategory.Description);
            output.IsActive.Should().Be((bool)exampleCategory.IsActive!);

        }

        [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        public async Task ThrowWhenCategoryNotFound()
        {
            var input = _fixture.GetValidInput();
            var dbContext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);


            var useCase = new ApplicationUseCases.UpdateCategory(repository, unitOfWork);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{input.Id}' not found.");

        }

        [Theory(DisplayName = nameof(UpdateThrowsWhenCantInstantiateCategory))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetInvalidInputs),
        parameters: 6,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
   )]
        public async Task UpdateThrowsWhenCantInstantiateCategory(UpdateCategoryInput input, string expectedExceptionMessage)
        {

            var dbContext = _fixture.CreateDbContext();
            var exampleCategories = _fixture.GetExampleCategoriesList();
            input.Id = exampleCategories[0].Id;
            await dbContext.AddRangeAsync(exampleCategories);
            dbContext.SaveChanges();

            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);

            var useCase = new ApplicationUseCases.UpdateCategory(repository, unitOfWork);

            var task = async () => await useCase.Handle(input, CancellationToken.None);


            await task.Should().ThrowAsync<EntityValidationException>().WithMessage(expectedExceptionMessage);
        }


    }
}
