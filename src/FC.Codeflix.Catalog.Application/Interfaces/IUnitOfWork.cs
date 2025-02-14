namespace FC.Codeflix.Catalog.Application.Interfaces
{
    public interface IUnitOfWork
    {
        public Task Commit(CancellationToken cancellationToken);
        Task Rollback(CancellationToken cancellationToken);
    }
}
