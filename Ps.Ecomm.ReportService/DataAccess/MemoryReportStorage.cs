using Ps.Ecomm.Models;

namespace Ps.Ecomm.ReportService.DataAccess
{
    public class MemoryReportStorage : IReportStorage
    {
        private readonly IList<Report> reports = new List<Report>();
        public Task Add(Report report)
        {
            reports.Add(report);
            return Task.CompletedTask;
        }
        public async Task<IEnumerable<Report>> GetAsync()
        {
            return await Task.FromResult(reports);
        }
    }
}
