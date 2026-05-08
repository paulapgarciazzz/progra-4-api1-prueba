using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace progra_4_api1_prueba2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private static readonly Stopwatch Uptime = Stopwatch.StartNew();

        // GET api/health
        [HttpGet]
        public IActionResult Get() =>
            Ok(new
            {
                status = "Healthy",
                uptime = Uptime.Elapsed.ToString(),
                timestamp = DateTime.UtcNow
            });

        // GET api/health/ready
        [HttpGet("ready")]
        public IActionResult Ready() =>
            Ok(new { status = "Ready", timestamp = DateTime.UtcNow });

        // GET api/health/live
        [HttpGet("live")]
        public IActionResult Live() =>
            Ok(new { status = "Alive", timestamp = DateTime.UtcNow });
    }
}
