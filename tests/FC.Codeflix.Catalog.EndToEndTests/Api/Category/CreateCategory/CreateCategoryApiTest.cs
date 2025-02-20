﻿using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FluentAssertions;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.CreateCategory
{
    [Collection(nameof(CreateCategoryApiTestFixture))]
    public class CreateCategoryApiTest
    {
        private readonly CreateCategoryApiTestFixture _fixture;

        public CreateCategoryApiTest(CreateCategoryApiTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("End2End/API", "Category - Endpoints")]
        public async Task CreateCategory()
        {
            var input = _fixture.GetExampleInput();

            var output = await _fixture.Api.Post<CategoryModelOutput>(
                "/categories",
                input);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(input.IsActive);
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default);

            var dbCategory = await _fixture.Persistence.GetById(output.Id);

            dbCategory.Should().NotBeNull();
            dbCategory.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be(input.IsActive);
            dbCategory.CreatedAt.Should().NotBeSameDateAs(default);
        }
    }
}
