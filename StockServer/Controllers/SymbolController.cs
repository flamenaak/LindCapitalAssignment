using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StockServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SymbolController : ControllerBase
    {
        private IStockService stockService;


        public SymbolController(IStockService service)
        {
            stockService = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Symbol>> GetSymbols()
        {
            return stockService.Symbols.ToArray();
        }
    }
}
