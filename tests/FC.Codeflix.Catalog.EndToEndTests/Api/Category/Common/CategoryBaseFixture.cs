using FC.Codeflix.Catalog.EndToEndTests.Base;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.Common
{
    public class CategoryBaseFixture : BaseFixture
    {
        public CategoryPersistence Persistence;

        public CategoryBaseFixture() : base()
        {
            Persistence = new CategoryPersistence(CreateDbContext());
        }

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

        public bool GetRandomBoolean() => new Random().NextDouble() < 0.5;

        public DomainEntity.Category GetExampleCategory() => new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());
        public List<DomainEntity.Category> GetExampleCategoriesList(int length = 10)
        => Enumerable.Range(1, length).Select(_ => GetExampleCategory()).ToList();

    }
}
