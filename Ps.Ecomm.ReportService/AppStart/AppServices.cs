using Microsoft.VisualBasic;
using Ps.Ecomm.Models;
using Ps.Ecomm.PlaneRabbitMQ;
using Ps.Ecomm.ReportService.DataAccess;
using Ps.Ecomm.ReportService.Listeners;
using RabbitMQ.Client;

namespace Ps.Ecomm.ReportService.AppStart
{
    public static class AppServices
    {
        public static void AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Ps Ecomm Report Service",
                    Version = "v1"
                });
            });

            var dbConnStr = config.GetConnectionString("AppDb");
            var rabbitMqConnStr = config.GetConnectionString("RabbitMqConnStr");
            services.AddSingleton<IReportStorage, MemoryReportStorage>();
            services.AddSingleton<IConnectionProvider>(new ConnectionProvider(rabbitMqConnStr));
            services.AddSingleton<ISubscriber>(x => new Subscriber(x.GetService<IConnectionProvider>(),
                                                                MQConstants.EXCHANGE_REPORT,
                                                                MQConstants.QUEUE_REPORT_ORDER,
                                                                MQConstants.ROUTE_KEY_REPORT_ALL,
                                                                ExchangeType.Topic));

            services.AddHostedService<ReportDataCollector>();

        }
    }
}
