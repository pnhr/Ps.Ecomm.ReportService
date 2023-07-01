using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ps.Ecomm.ReportService.DataAccess;

namespace Ps.Ecomm.ReportService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportStorage reportStorage;

        public ReportsController(IReportStorage reportStorage)
        {
            this.reportStorage = reportStorage;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await reportStorage.GetAsync();
            return Ok(response);
        }
    }
}
