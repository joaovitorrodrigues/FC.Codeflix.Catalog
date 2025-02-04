﻿using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.UnitTests.Common;
using Moq;


namespace FC.Codeflix.Catalog.UnitTests.Application.GetCategory
{
    [CollectionDefinition(nameof(GetCategoryTestFixture))]
    public class GetCategoryTestFixtureCollection: ICollectionFixture<GetCategoryTestFixture> { }
    public class GetCategoryTestFixture : BaseFixture
    {
        public Mock<ICategoryRepository> GetRepositoryMock() => new();
        public string GetValidCategoryName()
        {
            string categoryName = "";
            while (categoryName.Length < 3)
            {
                categoryName = Faker.Commerce.Categories(1)[0];
            }
            if (categoryName.Length > 255)
                categoryName = categoryName[..255];

            return categoryName;
        }

        public string GetValidCategoryDescription()
        {
            string categoryDescription = "";
            categoryDescription = Faker.Commerce.ProductDescription();
            if (categoryDescription.Length > 10000)
                categoryDescription = categoryDescription[..10000];

            return categoryDescription;
        }
        public Category GetValidCategory()
            => new(GetValidCategoryName(), GetValidCategoryDescription());
    }
}
