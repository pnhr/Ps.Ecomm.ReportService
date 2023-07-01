using MassTransit;
using Ps.Ecomm.Models;
using Ps.Ecomm.ReportService.DataAccess;
using System.Text.Json;

namespace Ps.Ecomm.ReportService.Listeners
{
    public class ProductConsumer : IConsumer<Product>
    {
        private readonly IReportStorage reportStorage;

        public ProductConsumer(IReportStorage reportStorage)
        {
            this.reportStorage = reportStorage;
        }
        public async Task Consume(ConsumeContext<Product> context)
        {
            var product = context.Message;
            var data = await reportStorage.GetAsync();
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
