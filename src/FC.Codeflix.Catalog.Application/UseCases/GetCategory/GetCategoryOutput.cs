using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.Application.UseCases.GetCategory
{
    public class GetCategoryOutput
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public GetCategoryOutput(Guid id, string name, string description, DateTime createdAt, bool isActive = true)
        {
            Id = id;
            Name = name;
            Description = description;
            CreatedAt = createdAt;
            IsActive = isActive;
        }

        public static GetCategoryOutput FromCategory(DomainEntity.Category category)
        => new(
                category.Id,
                category.Name,
                category.Description,
                category.CreatedAt,
                category.IsActive
            );


    }
}
