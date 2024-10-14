using Microsoft.AspNetCore.Mvc;
using ParkingService.DbContexts.Models;

namespace ParkingService.Controllers
{
    [Route("api/[controller][action]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        private readonly ILogger<ParkingController> _logger;

        public ParkingController(ILogger<ParkingController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public void PostParkingTransaction(ParkingTransaction parkingTransaction)
        {

        }
    }
}
