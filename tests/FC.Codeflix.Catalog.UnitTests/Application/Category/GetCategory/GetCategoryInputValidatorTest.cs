﻿using FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;
using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.GetCategory
{
    [Collection(nameof(GetCategoryTestFixture))]
    public class GetCategoryInputValidatorTest
    {
        private readonly GetCategoryTestFixture _fixture;

        public GetCategoryInputValidatorTest(GetCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(ValidatorOk))]
        [Trait("Application", "GetCategoryInputValidation - Use Cases")]
        public void ValidatorOk()
        {
            var validInput = new GetCategoryInput(Guid.NewGuid());

            var validator = new GetCategoryInputValidator();

            var validationResult = validator.Validate(validInput);

            validationResult.Should().NotBeNull();
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().HaveCount(0);
        }

        [Fact(DisplayName = nameof(InvalidWhenEmptyGuidId))]
        [Trait("Application", "GetCategoryInputValidation - Use Cases")]
        public void InvalidWhenEmptyGuidId()
        {
            var invalidInput = new GetCategoryInput(Guid.Empty);

            var validator = new GetCategoryInputValidator();

            var validationResult = validator.Validate(invalidInput);

            validationResult.Should().NotBeNull();
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().HaveCount(1);
            validationResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
        }

    }
}
