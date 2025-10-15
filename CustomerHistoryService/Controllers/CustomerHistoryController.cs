using CustomerHistoryService.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CustomerHistoryService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerHistoryController : Controller
    {
        private readonly ICustomerHistoryRepository _repository;
        private readonly ICustomerHistoryReportService _reportService;

        public CustomerHistoryController(ICustomerHistoryRepository repository, ICustomerHistoryReportService reportService)
        {
            _repository = repository;
            _reportService = reportService;
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> Get(string email)
        {
            var history = await _repository.GetByEmailAsync(email);
            return history is null ? NotFound() : Ok(history);
        }

        //[HttpGet("{email}")]
        //public async Task<IActionResult> GetCustomerHistory(string email)
        //{
        //    var report = await _reportService.GetCustomerHistoryReportAsync(email);
        //    return report is null ? NotFound() : Ok(report);
        //}
    }
}
