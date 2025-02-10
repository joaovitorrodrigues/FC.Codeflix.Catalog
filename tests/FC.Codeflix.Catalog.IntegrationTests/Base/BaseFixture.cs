using Bogus;

namespace FC.Codeflix.Catalog.IntegrationTests.Base
{
    public abstract class BaseFixture
    {
        protected Faker Faker { get; set; }
        public BaseFixture()
        {
            Faker = new Faker("pt_BR");
        }
    }
}
