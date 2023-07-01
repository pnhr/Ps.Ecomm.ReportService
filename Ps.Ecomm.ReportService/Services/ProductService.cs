using Ps.Ecomm.Models;
using Ps.Ecomm.ReportService.DataAccess;
using System.Text.Json;

namespace Ps.Ecomm.ReportService.Services
{
    public class ProductService : IMessageService
    {
        private readonly IReportStorage reportStorage;

        public ProductService(IReportStorage reportStorage)
        {
            this.reportStorage = reportStorage;
        }
        public async Task Process(string message)
        {
            if (message != null)
            {
                var product = JsonSerializer.Deserialize<Product>(message);
                var data = reportStorage.GetAsync().GetAwaiter().GetResult();
                if (data.Any(r => r.ProductName.ToLower() == product.ProductName.ToLower()))
                {
                    return;
                }
                else
                {
                    reportStorage.Add(new Report
                    {
                        ProductName = product.ProductName,
                        Count = AppConstants.DEFAULT_QUANTITY
                    });
                }
            }
        }
    }
}
