﻿using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.UpdateCategory
{
    [Collection(nameof(UpdateCategoryApiTestFixture))]
    public class UpdateCategoryApiTest
    {
        private readonly UpdateCategoryApiTestFixture _fixture;

        public UpdateCategoryApiTest(UpdateCategoryApiTestFixture fixture)
        => _fixture = fixture;

        [Fact(DisplayName =nameof(UpdateCategory))]
        [Trait("End2End/API", "Category/Update - Endpoints")]
        public async void UpdateCategory()
        {
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCategoriesList);
            var exampleCategory = exampleCategoriesList[10];
            var input = _fixture.getExampleInput(exampleCategory.Id);

            var (response, output) = await _fixture.ApiClient.Put<CategoryModelOutput>(
                $"/categories/{exampleCategory.Id}",
                input);


            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be((bool)input.IsActive!);
            output.Id.Should().Be(exampleCategory.Id);
            output.CreatedAt.Should().NotBeSameDateAs(default);

            var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);

            dbCategory.Should().NotBeNull();
            dbCategory.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be((bool)input.IsActive!);
        }

        [Fact(DisplayName = nameof(UpdateCategoryOnlyName))]
        [Trait("End2End/API", "Category/Update - Endpoints")]
        public async void UpdateCategoryOnlyName()
        {
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCategoriesList);
            var exampleCategory = exampleCategoriesList[10];
            var input = new UpdateCategoryInput(exampleCategory.Id, _fixture.GetValidCategoryName());

            var (response, output) = await _fixture.ApiClient.Put<CategoryModelOutput>(
                $"/categories/{exampleCategory.Id}",
                input);


            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(exampleCategory.Description);
            output.IsActive.Should().Be((bool)exampleCategory.IsActive!);
            output.Id.Should().Be(exampleCategory.Id);
            output.CreatedAt.Should().NotBeSameDateAs(default);

            var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);

            dbCategory.Should().NotBeNull();
            dbCategory.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.IsActive.Should().Be((bool)exampleCategory.IsActive!);
        }

        [Fact(DisplayName = nameof(UpdateCategoryNameAndDescription))]
        [Trait("End2End/API", "Category/Update - Endpoints")]
        public async void UpdateCategoryNameAndDescription()
        {
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCategoriesList);
            var exampleCategory = exampleCategoriesList[10];
            var input = new UpdateCategoryInput(exampleCategory.Id, _fixture.GetValidCategoryName(), _fixture.GetValidCategoryDescription());

            var (response, output) = await _fixture.ApiClient.Put<CategoryModelOutput>(
                $"/categories/{exampleCategory.Id}",
                input);


            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be((bool)exampleCategory.IsActive!);
            output.Id.Should().Be(exampleCategory.Id);
            output.CreatedAt.Should().NotBeSameDateAs(default);

            var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);

            dbCategory.Should().NotBeNull();
            dbCategory.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be((bool)exampleCategory.IsActive!);
        }

        [Fact(DisplayName = nameof(ErrorWhenNotFound))]
        [Trait("End2End/API", "Category/Update - Endpoints")]
        public async void ErrorWhenNotFound()
        {
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCategoriesList);
            var randomGuid = Guid.NewGuid();
            var input = _fixture.getExampleInput(randomGuid);

            var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>(
                $"/categories/{randomGuid}",
                input);


            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            output.Should().NotBeNull();
            output.Title.Should().Be("Not Found");
            output.Type.Should().Be("NotFound");
            output.Detail.Should().Be($"Category '{randomGuid}' not found.");
            output.Status.Should().Be(StatusCodes.Status404NotFound);

        }

    }
}
