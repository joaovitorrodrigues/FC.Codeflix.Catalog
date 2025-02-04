﻿using Moq;
using FluentAssertions;
using UseCases = FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using FC.Codeflix.Catalog.Application.Exceptions;

namespace FC.Codeflix.Catalog.UnitTests.Application.DeleteCategory
{
    [Collection(nameof(DeleteCategoryTestFixture))]
    public class DeleteCategoryTest
    {
        private readonly DeleteCategoryTestFixture _fixture;

        public DeleteCategoryTest(DeleteCategoryTestFixture fixture) => _fixture = fixture;

        [Fact(DisplayName = nameof(DeleteCategory))]
        [Trait("Application", "DeleteCategory - Use Cases")]
        public async Task DeleteCategory()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var categoryExample = _fixture.GetValidCategory();

            repositoryMock.Setup(x => x.Get(categoryExample.Id, It.IsAny<CancellationToken>())).ReturnsAsync(categoryExample);

            var input = new UseCases.DeleteCategoryInput(categoryExample.Id);

            var useCase = new UseCases.DeleteCategory(repositoryMock.Object, unitOfWorkMock.Object);

            await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(x => x.Get(categoryExample.Id, It.IsAny<CancellationToken>()), Times.Once);
            repositoryMock.Verify(x => x.Delete(categoryExample, It.IsAny<CancellationToken>()), Times.Once);
            unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
        [Trait("Application", "DeleteCategory - Use Cases")]
        public async Task ThrowWhenCategoryNotFound()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var exampleGuid = Guid.NewGuid();

            repositoryMock.Setup(x => x.Get(exampleGuid, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException($"Category '{exampleGuid}' not found"));

            var input = new UseCases.DeleteCategoryInput(exampleGuid);

            var useCase = new UseCases.DeleteCategory(repositoryMock.Object, unitOfWorkMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>();         

        }


    }
}
