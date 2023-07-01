using Ps.Ecomm.Models;
using Ps.Ecomm.ReportService.DataAccess;

namespace Ps.Ecomm.ReportService.Services
{
    public class MessageFactory
    {
        private readonly IReportStorage reportStorage;
        public MessageFactory(IReportStorage reportStorage)
        {
            this.reportStorage = reportStorage;
        }
        public IMessageService GetInstance(string type)
        {
            if (typeof(Product).FullName == type)
            {
                return new ProductService(reportStorage);
            }
            else if (typeof(OrderDetail).FullName == type)
            {
                return new OrderService(reportStorage);
            }
            return null;
        }
    }
}
