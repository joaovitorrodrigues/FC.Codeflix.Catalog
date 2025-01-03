using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FluentAssertions;
using Moq;

namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory
{
    public class CreateCategoryTest
    {
        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("Application", "CreateCategory - Use Cases")]
        public async void CreateCategory()
        {
            var repositoryMock = new Mock<ICategoryRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var useCase = new CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

            var input = new CreateCategoryInput("category name", "category description", true);


            var output =  await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(repository => repository.Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);

            unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()), Times.Once);

            output.Should().NotBeNull();
            output.Name.Should().Be("category name");
            output.Description.Should().Be("category description");
            output.IsActive.Should().BeTrue();
            (output.Id != null && output.Id != Guid.Empty).Should().BeTrue();
            (output.CreatedAt != null && output.CreatedAt != default).Should().BeTrue();
        }
    }
}
