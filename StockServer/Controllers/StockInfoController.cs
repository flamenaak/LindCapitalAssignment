using Microsoft.AspNetCore.Mvc;

namespace StockServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockInfoController : ControllerBase
    {
        

        private readonly ILogger<StockInfoController> _logger;
        private IStockService stockService;

        public StockInfoController(ILogger<StockInfoController> logger, IStockService service)
        {
            _logger = logger;
            stockService = service;
        }

        [HttpPost(Name = "GetStocks")]
        public ActionResult<IEnumerable<StockInfo>> GetSymbols([FromBody] IEnumerable<int> symbols)
        {
            if (symbols.Count() == 0)
                return stockService.Stocks.ToArray();
            else
                return stockService.Stocks.Where(stock => symbols.Contains(stock.Symbol.Id)).ToArray();
        }
    }
}