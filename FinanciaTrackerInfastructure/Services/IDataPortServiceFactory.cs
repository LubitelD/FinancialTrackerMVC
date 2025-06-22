using FinancialTrackerDomain.Model;

namespace FinancialTrackerInfastructure.Services
{
    public interface IDataPortServiceFactory<TEntity>
        where TEntity : Entity
    {
        IImportService<TEntity>GetImportService(string contentType);
        IExportService<TEntity> GetExportService(string contentType);
    }
}
