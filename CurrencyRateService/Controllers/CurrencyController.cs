using CurrencyRateService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyRateService _service;
        private readonly CurrencyDbContext _db;

        public CurrencyController(ICurrencyRateService service, CurrencyDbContext db)
        {
            _service = service;
            _db = db;

        }

        [HttpPost("get-rate")]
        public async Task<IActionResult> GetRate([FromBody] CurrencyRateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Source) || string.IsNullOrWhiteSpace(request.Target))
                return BadRequest("Source and Target currencies are required.");

            try
            {
                var rate = await _service.GetRateAsync(request.Source, request.Target, request.Date);
                return Ok(new { Rate = rate });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllRates()
        {
            var rates = await _db.CurrencyRates
                .OrderByDescending(r => r.Date)
                .ToListAsync();
            return Ok(rates);
        }


    }

}
