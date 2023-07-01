using Ps.Ecomm.Models;
using Ps.Ecomm.PlaneRabbitMQ;
using Ps.Ecomm.ReportService.DataAccess;
using Ps.Ecomm.ReportService.Services;
using System.Collections;
using System.Text;
using System.Text.Json;

namespace Ps.Ecomm.ReportService.Listeners
{
    public class ReportDataCollector : IHostedService
    {
        private readonly ISubscriber subscriber;
        private readonly IReportStorage reportStorage;

        public ReportDataCollector(ISubscriber subscriber, IReportStorage reportStorage)
        {
            this.subscriber = subscriber;
            this.reportStorage = reportStorage;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            subscriber.Subscribe(Process);
            return Task.CompletedTask;
        }

        private bool Process(string message, IDictionary<string, object> headers)
        {
            if (headers != null)
            {
                string objTypeStr = Encoding.Default.GetString((byte[])headers[MQConstants.OBJECT_TYPE]);
                MessageFactory factory = new MessageFactory(reportStorage);
                var processObj = factory.GetInstance(objTypeStr);
                if (processObj != null)
                    processObj.Process(message);
                else
                    return false;
                return true;
            }
            return false;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
