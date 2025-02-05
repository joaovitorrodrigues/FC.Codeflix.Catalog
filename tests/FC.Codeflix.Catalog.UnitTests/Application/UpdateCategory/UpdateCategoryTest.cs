using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.Domain.Entity;
using FluentAssertions;
using Moq;
using UseCases = FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
namespace FC.Codeflix.Catalog.UnitTests.Application.UpdateCategory
{
    [Collection(nameof(UpdateCategoryTestFixture))]
    public class UpdateCategoryTest
    {
        private readonly UpdateCategoryTestFixture _fixture;

        public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory(DisplayName =nameof(UpdateCategory))]
        [Trait("Application", "UpdateCategory - Use Cases")]
        [MemberData(
            nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), 
            parameters: 10, 
            MemberType = typeof(UpdateCategoryTestDataGenerator)
        )]
        public async Task UpdateCategory(Category exampleCategory, UpdateCategoryInput exampleInput)
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();


            repositoryMock.Setup(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>())).ReturnsAsync(exampleCategory);


            var useCase = new UseCases.UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);

            CategoryModelOutput output = await useCase.Handle(exampleInput, CancellationToken.None);

            output.Should().NotBeNull();
            output.Name.Should().Be(exampleInput.Name);
            output.Description.Should().Be(exampleInput.Description);
            output.IsActive.Should().Be(exampleInput.IsActive);

            repositoryMock.Verify(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()), Times.Once);
            repositoryMock.Verify(x => x.Update(exampleCategory, It.IsAny<CancellationToken>()), Times.Once);

            unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);

        }

    }
}
