using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.EndToEndTests.Extensions.DateTime;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.GetCategory
{
    class GetCategoryResponse
    {
        public CategoryModelOutput Data { get; set; }

        public GetCategoryResponse(CategoryModelOutput data)
        {
            Data = data;
        }
    }
    [Collection(nameof(GetCategoryApiTestFixture))]
    public class GetCategoryApiTest : IDisposable
    {
        private readonly GetCategoryApiTestFixture _fixture;

        public GetCategoryApiTest(GetCategoryApiTestFixture fixture)
        => _fixture = fixture;


        [Fact(DisplayName = nameof(GetCategory))]
        [Trait("EndToEnd/API", "Category/Get - Endpoints")]
        public async Task GetCategory()
        {

            var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCategoriesList);
            var exampleCategory = exampleCategoriesList[10];


            var (response, output) = await _fixture.ApiClient.Get<GetCategoryResponse>($"/categories/{exampleCategory.Id}");


            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Data.Id.Should().Be(exampleCategory.Id);
            output.Data.Name.Should().Be(exampleCategory.Name);
            output.Data.Description.Should().Be(exampleCategory.Description);
            output.Data.IsActive.Should().Be(exampleCategory.IsActive);
            output.Data.CreatedAt.TrimMilliseconds().Should().Be(exampleCategory.CreatedAt.TrimMilliseconds());
        }


        [Fact(DisplayName = nameof(ErrorWhenNotExists))]
        [Trait("EndToEnd/API", "Category/Get - Endpoints")]
        public async Task ErrorWhenNotExists()
        {

            var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCategoriesList);
            var randomGuid = Guid.NewGuid();


            var (response, output) = await _fixture.ApiClient.Get<ProblemDetails>($"/categories/{randomGuid}");


            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

            output.Should().NotBeNull();
            output.Title.Should().Be($"Not Found");
            output.Detail.Should().Be($"Category '{randomGuid}' not found.");
            output.Status.Should().Be(StatusCodes.Status404NotFound);
            output.Type.Should().Be("NotFound");

        }

        public void Dispose()
        => _fixture.CleanPersistence();
    }
}

