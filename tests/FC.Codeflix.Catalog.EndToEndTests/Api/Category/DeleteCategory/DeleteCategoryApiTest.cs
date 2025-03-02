using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.DeleteCategory
{
    [Collection(nameof(DeleteCategoryApiTestFixture))]
    public class DeleteCategoryApiTest: IDisposable
    {
        private readonly DeleteCategoryApiTestFixture _fixture;

        public DeleteCategoryApiTest(DeleteCategoryApiTestFixture fixture)
        => _fixture = fixture;

        [Fact(DisplayName = nameof(DeleteCategory))]
        [Trait("EndToEnd/API", "Category/Delete - Endpoints")]
        public async void DeleteCategory()
        {
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCategoriesList);
            var exampleCategory = exampleCategoriesList[10];


            var (response, output) = await _fixture.ApiClient.Delete<object>($"/categories/{exampleCategory.Id}");


            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
            output.Should().BeNull();

            var persistenceCategory = await _fixture.Persistence.GetById(exampleCategory.Id);

            persistenceCategory.Should().BeNull();
            
        }

        [Fact(DisplayName = nameof(ErrorWhenNotFound))]
        [Trait("EndToEnd/API", "Category/Delete - Endpoints")]
        public async void ErrorWhenNotFound()
        {
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCategoriesList);
            var exampleRandomGuid = Guid.NewGuid();


            var (response, output) = await _fixture.ApiClient.Delete<ProblemDetails>($"/categories/{exampleRandomGuid}");


            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            output.Should().NotBeNull();
            output.Status.Should().Be(StatusCodes.Status404NotFound);
            output.Title.Should().Be("Not Found");
            output.Type.Should().Be("NotFound");
            output.Detail.Should().Be($"Category '{exampleRandomGuid}' not found.");

        }

        public void Dispose()
        => _fixture.CleanPersistence();
    }
}
