using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Infra.Data.EF;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FluentAssertions;
using ApplicationUseCases = FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.CreateCategory
{
    [Collection(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTest
    {
        private readonly CreateCategoryTestFixture _fixture;

        public CreateCategoryTest(CreateCategoryTestFixture fixture)
         => _fixture = fixture;

        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("Integration/Application", "CreateCategory - Use Cases")]
        public async Task CreateCategory()
        {
            var dbContext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCases.CreateCategory(unitOfWork, repository);

            var input = _fixture.GetInput();


            var output = await useCase.Handle(input, CancellationToken.None);

            var dbCategory = await _fixture.CreateDbContext(true).Categories.FindAsync(output.Id);

            dbCategory.Should().NotBeNull();
            dbCategory.Name.Should().Be(output.Name);
            dbCategory.Description.Should().Be(output.Description);
            dbCategory.Id.Should().Be(output.Id);
            dbCategory.IsActive.Should().Be(output.IsActive);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(input.IsActive);
            (output.Id != Guid.Empty).Should().BeTrue();
            (output.CreatedAt != default).Should().BeTrue();
        }

        [Fact(DisplayName = nameof(CreateCategoryWithOnlyName))]
        [Trait("Integration/Application", "CreateCategory - Use Cases")]
        public async Task CreateCategoryWithOnlyName()
        {
            var dbContext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCases.CreateCategory(unitOfWork, repository);

            var input = new CreateCategoryInput(_fixture.GetValidCategoryName());

            var output = await useCase.Handle(input, CancellationToken.None);


            var dbCategory = await _fixture.CreateDbContext(true).Categories.FindAsync(output.Id);

            dbCategory.Should().NotBeNull();
            dbCategory.Name.Should().Be(output.Name);
            dbCategory.Description.Should().Be("");
            dbCategory.Id.Should().Be(output.Id);
            dbCategory.IsActive.Should().Be(true);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be("");
            output.IsActive.Should().Be(true);
            (output.Id != Guid.Empty).Should().BeTrue();
            (output.CreatedAt != default).Should().BeTrue();
        }

        [Fact(DisplayName = nameof(CreateCategoryWithOnlyNameAndDescription))]
        [Trait("Integration/Application", "CreateCategory - Use Cases")]
        public async Task CreateCategoryWithOnlyNameAndDescription()
        {
            var dbContext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCases.CreateCategory(unitOfWork, repository);

            var input = new CreateCategoryInput(_fixture.GetValidCategoryName(), _fixture.GetValidCategoryDescription());

            var output = await useCase.Handle(input, CancellationToken.None);


            var dbCategory = await _fixture.CreateDbContext(true).Categories.FindAsync(output.Id);

            dbCategory.Should().NotBeNull();
            dbCategory.Name.Should().Be(output.Name);
            dbCategory.Description.Should().Be(output.Description);
            dbCategory.Id.Should().Be(output.Id);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);
            dbCategory.IsActive.Should().Be(true);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(true);
            (output.Id != Guid.Empty).Should().BeTrue();
            (output.CreatedAt != default).Should().BeTrue();
        }

        [Theory(DisplayName = nameof(ThrowWhenCantInstantiateCategory))]
        [Trait("Integration/Application", "CreateCategory - Use Cases")]
        [MemberData(nameof(CreateCategoryTestDataGenerator.GetInvalidInputs),
                    parameters: 6,
                    MemberType = typeof(CreateCategoryTestDataGenerator))]
        public async Task ThrowWhenCantInstantiateCategory(CreateCategoryInput input, string exceptionMessage)
        {
            var dbContext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCases.CreateCategory(unitOfWork, repository);

            Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>().WithMessage(exceptionMessage);
        }
    }
}
