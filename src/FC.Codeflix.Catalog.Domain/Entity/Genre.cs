using FC.Codeflix.Catalog.Domain.SeedWork;
using FC.Codeflix.Catalog.Domain.Validation;
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

            Validate();
        }
        public void Activate()
        {
            IsActive = true;
            Validate();
        }
        public void Deactivate()
        {
            IsActive = false;
            Validate();
        }

        public void Update(string name)
        {
            Name = name;
            Validate();
        }

        private void Validate()
            => DomainValidation.NotNullOrEmpty(Name, nameof(Name));

    }
}
