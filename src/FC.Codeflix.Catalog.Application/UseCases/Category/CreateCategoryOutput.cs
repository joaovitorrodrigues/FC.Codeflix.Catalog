using DomainEntity =  FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.Application.UseCases.Category
{
    public class CreateCategoryOutput
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public CreateCategoryOutput(Guid id, string name, string description, DateTime createdAt, bool isActive = true)
        {
            Id = id;
            Name = name;
            Description = description;
            CreatedAt = createdAt;
            IsActive = isActive;                
        }

        public static CreateCategoryOutput FromCategory(DomainEntity.Category category)
        {
            return new CreateCategoryOutput(
                category.Id,
                category.Name,
                category.Description,
                category.CreatedAt,
                category.IsActive
            );
        }

    }
}
