using Ps.Ecomm.Models;

namespace Ps.Ecomm.ReportService.DataAccess
{
    public interface IReportStorage
    {
        Task Add(Report report);
        Task<IEnumerable<Report>> GetAsync();
    }
}