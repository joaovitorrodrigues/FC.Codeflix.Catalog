using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories
{
    public class ListCategoriesOutput : PaginatedListOutput<CategoryModelOutput>
    {
        public ListCategoriesOutput(int currentPage,
            int perPage,
            int total,
            IReadOnlyList<CategoryModelOutput> items)
            : base(currentPage, perPage, total, items)
        {
        }
    }
}
