﻿namespace Ps.Ecomm.ReportService.AppStart
{
    public static class AppMiddlewares
    {
        public static void AddMiddlewares(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ps Ecomm Report Service");
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
