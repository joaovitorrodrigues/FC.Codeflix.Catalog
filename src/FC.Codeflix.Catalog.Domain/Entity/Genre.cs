using FC.Codeflix.Catalog.Domain.SeedWork;
namespace FC.Codeflix.Catalog.Domain.Entity
{
    public class Genre : AggregateRoot
    {

        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public Genre(string name, bool isActive = true)
        {
            Name = name;
            IsActive = isActive;
            CreatedAt = DateTime.Now;
        }
    }
}
