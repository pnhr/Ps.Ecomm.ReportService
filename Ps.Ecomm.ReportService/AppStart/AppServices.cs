using MassTransit;
using Microsoft.VisualBasic;
using Ps.Ecomm.Models;
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
            services.AddMassTransit(config =>
            {
                config.AddConsumer<OrderConsumer>();
                config.AddConsumer<ProductConsumer>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(rabbitMqConnStr);
                    cfg.ReceiveEndpoint("order-queue", c =>
                    {
                        c.ConfigureConsumer<OrderConsumer>(ctx);
                    });
                    cfg.ReceiveEndpoint("product-queue", c =>
                    {
                        c.ConfigureConsumer<ProductConsumer>(ctx);
                    });
                });
            });
        }
    }
}
