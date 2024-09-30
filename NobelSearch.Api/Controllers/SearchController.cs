using Microsoft.AspNetCore.Mvc;
using NobelSearch.Api.Services;

namespace NobelSearch.Api.Controllers
{
    [ApiController]
    [Route("search")]
    public class SearchController : ControllerBase
    {
        private readonly INobelPrizeService _nobelPrizeService;

        public SearchController(INobelPrizeService nobelPrizeService)
        {
            _nobelPrizeService = nobelPrizeService;
        }

        [HttpGet("name")]
        public async Task<IActionResult> SearchByName([FromQuery] string q)
        {
            var results = await _nobelPrizeService.SearchByNameAsync(q);
            return Ok(results);
        }

        [HttpGet("category")]
        public async Task<IActionResult> SearchByCategory([FromQuery] string q)
        {
            var results = await _nobelPrizeService.SearchByCategoryAsync(q);
            return Ok(results);
        }

        [HttpGet("description")]
        public async Task<IActionResult> SearchByDescription([FromQuery] string q)
        {
            var results = await _nobelPrizeService.SearchByDescriptionAsync(q);
            return Ok(results);
        }
    }
}
