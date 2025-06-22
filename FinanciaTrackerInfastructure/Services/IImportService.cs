using FinancialTrackerDomain.Model;

namespace FinancialTrackerInfastructure.Services
{
    public interface IImportService<TEntity>
    where TEntity : Entity
    {
        Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken);
    }
}
