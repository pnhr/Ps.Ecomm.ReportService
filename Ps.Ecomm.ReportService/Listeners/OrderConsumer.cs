using MassTransit;
using Ps.Ecomm.Models;
using Ps.Ecomm.ReportService.DataAccess;
using Ps.Ecomm.ReportService.Services;
using System.Text.Json;

namespace Ps.Ecomm.ReportService.Listeners
{
    public class OrderConsumer : IConsumer<OrderDetail>
    {
        private readonly IReportStorage reportStorage;

        public OrderConsumer(IReportStorage reportStorage)
        {
            this.reportStorage = reportStorage;
        }
        public async Task Consume(ConsumeContext<OrderDetail> context)
        {
            var order = context.Message;
            var data = await reportStorage.GetAsync();
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
