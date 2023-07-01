using Ps.Ecomm.Models;
using Ps.Ecomm.ReportService.DataAccess;
using System.Text.Json;

namespace Ps.Ecomm.ReportService.Services
{
    public class OrderService : IMessageService
    {
        private readonly IReportStorage reportStorage;

        public OrderService(IReportStorage reportStorage)
        {
            this.reportStorage = reportStorage;
        }
        public async Task Process(string message)
        {
            if (message != null)
            {
                var order = JsonSerializer.Deserialize<OrderDetail>(message);
                var data = reportStorage.GetAsync().GetAwaiter().GetResult();
                if (data.Any(r => r.ProductName.ToLower() == order.ProductName.ToLower()))
                {
                    data.First(r => r.ProductName.ToLower() == order.ProductName.ToLower()).Count -= order.Quantity;
                }
                else
                {
                    reportStorage.Add(new Report
                    {
                        ProductName = order.ProductName,
                        Count = AppConstants.DEFAULT_QUANTITY - order.Quantity
                    });
                }
            }
        }
    }
}
