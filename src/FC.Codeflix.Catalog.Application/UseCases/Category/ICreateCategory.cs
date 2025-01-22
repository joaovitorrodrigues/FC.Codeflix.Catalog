namespace FC.Codeflix.Catalog.Application.UseCases.Category
{
    public interface ICreateCategory
    {
        public Task<CreateCategoryOutput> Handle(CreateCategoryInput input, CancellationToken token);
    }
}
