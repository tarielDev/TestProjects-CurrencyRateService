using Microsoft.AspNetCore.Mvc;

namespace CurrencyRateService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyRateService _service;

        public CurrencyController(ICurrencyRateService service)
        {
            _service = service;
        }

        [HttpGet("{currency}/{date}")]
        public async Task<IActionResult> Get(string currency, DateTime date)
        {
            var rate = await _service.GetRateAsync(currency.ToUpper(), date);
            return Ok(new { Currency = currency, Date = date.ToShortDateString(), Rate = rate });
        }
    }

}
