using FC.Codeflix.Catalog.Application.UseCases.Category;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using Moq;
using UseCases = FC.Codeflix.Catalog.Application.UseCases.Category;

namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory
{
    [Collection(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTest
    {
        private readonly CreateCategoryTestFixture _fixture;

        public CreateCategoryTest(CreateCategoryTestFixture fixture) => _fixture = fixture;


        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("Application", "CreateCategory - Use Cases")]
        public async Task CreateCategory()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var useCase = new UseCases.CreateCategory(unitOfWorkMock.Object, repositoryMock.Object);

            var input = _fixture.GetInput();


            var output = await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(repository => repository.Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);

            unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()), Times.Once);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(input.IsActive);
            (output.Id != Guid.Empty).Should().BeTrue();
            (output.CreatedAt != default).Should().BeTrue();
        }

        [Theory(DisplayName = nameof(ThrowWhenCantInstantiateAggregateAsync))]
        [Trait("Application", "CreateCategory - Use Cases")]
        [MemberData(nameof(GetInvalidInputs))]
        public async Task ThrowWhenCantInstantiateAggregateAsync(CreateCategoryInput input, string exceptionMessage)
        {
            var useCase = new UseCases.CreateCategory(_fixture.GetUnitOfWorkMock().Object, _fixture.GetRepositoryMock().Object);

            Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>().WithMessage(exceptionMessage);
        }

        public static IEnumerable<object[]> GetInvalidInputs()
        {
            var fixture = new CreateCategoryTestFixture();
            var invalidInputsList = new List<object[]>();

            var invalidInputShortName = fixture.GetInput();
            invalidInputShortName.Name = invalidInputShortName.Name.Substring(0, 2);

            invalidInputsList.Add(new object[] { invalidInputShortName, "Name should be at least 3 characters long" });



            var invalidInputTooLongName = fixture.GetInput();
            invalidInputTooLongName.Name = "";
            while (invalidInputTooLongName.Name.Length < 255)
            {
                invalidInputTooLongName.Name = $"{invalidInputTooLongName.Name} {fixture.Faker.Commerce.ProductName}";
            }

            invalidInputsList.Add(new object[] { invalidInputTooLongName, "Name should be less or equal 255 characters long" });


            var invalidInputDescriptionNull = fixture.GetInput();
            invalidInputDescriptionNull.Description = null!;
            invalidInputsList.Add(new object[] { invalidInputDescriptionNull, "Description should not be null" });


            var invalidInputTooLongDescription = fixture.GetInput();
            invalidInputTooLongDescription.Description = "";
            while (invalidInputTooLongDescription.Description.Length < 10000)
            {
                invalidInputTooLongDescription.Description = $"{invalidInputTooLongDescription.Description} {fixture.Faker.Commerce.ProductDescription}";
            }

            invalidInputsList.Add(new object[] { invalidInputTooLongDescription, "Description should be less or equal 10000 characters long" });

            return invalidInputsList;



        }
        [Fact(DisplayName = nameof(CreateCategoryWithOnlyName))]
        [Trait("Application", "CreateCategory - Use Cases")]
        public async Task CreateCategoryWithOnlyName()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var useCase = new UseCases.CreateCategory(unitOfWorkMock.Object, repositoryMock.Object);

            var input = new CreateCategoryInput(_fixture.GetValidCategoryName());

            var output = await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(repository => repository.Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);

            unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()), Times.Once);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be("");
            output.IsActive.Should().Be(true);
            (output.Id != Guid.Empty).Should().BeTrue();
            (output.CreatedAt != default).Should().BeTrue();
        }

        [Fact(DisplayName = nameof(CreateCategoryWithOnlyNameAndDescription))]
        [Trait("Application", "CreateCategory - Use Cases")]
        public async Task CreateCategoryWithOnlyNameAndDescription()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var useCase = new UseCases.CreateCategory(unitOfWorkMock.Object, repositoryMock.Object);

            var input = new CreateCategoryInput(_fixture.GetValidCategoryName(), _fixture.GetValidCategoryDescription());

            var output = await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(repository => repository.Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);

            unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()), Times.Once);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(true);
            (output.Id != Guid.Empty).Should().BeTrue();
            (output.CreatedAt != default).Should().BeTrue();
        }
    }
}
