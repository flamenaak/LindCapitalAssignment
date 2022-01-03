namespace StockServer
{
    /// <summary>
    ///  Implementation of IStockService which can be run in the background to randomly generate and update stock values 
    /// </summary>
    /// 
    public class StockService : BackgroundService, IStockService
    {
        public IEnumerable<StockInfo> Stocks { get; set; }
        public IEnumerable<Symbol> Symbols { get; set; }

        private Random random = new();

        public StockService()
        {
            Stocks = new HashSet<StockInfo>();
            Symbols = new HashSet<Symbol>();
        }

        public void Initialize(IEnumerable<Symbol> symbols)
        {
            Symbols = symbols;
            if (Stocks.Any())
            {
                return;
            }

            foreach (Symbol symbol in Symbols)
            {
                Stocks = Stocks.Append(GenerateStockInfo(symbol));
            }
        }

        private StockInfo GenerateStockInfo(Symbol symbol)
        {
            double ask = random.Next(0, 500);
            double bid = ask + ((random.Next(-1, 2)) * random.NextSingle()%0.5f * ask);
            return new StockInfo { 
                Date = DateTime.Now, 
                Symbol = symbol, 
                Ask = ask,  
                Bid = bid};
        }

        private StockInfo AdjustStockInfo(Symbol symbol)
        {
            if (Symbols.Any(item => item.Id == symbol.Id))
            {
                StockInfo? stock = Stocks.Where(stock => stock.Symbol.Id == symbol.Id).DefaultIfEmpty(null).FirstOrDefault();
                if (stock != null)
                {
                    stock.Ask += (random.Next(-1, 2)) * random.NextSingle() % 0.5f * stock.Ask;
                    stock.Bid += (random.Next(-1, 2)) * random.NextSingle() % 0.5f * stock.Bid;
                    stock.Date = DateTime.Now;

                } else
                {
                    stock = GenerateStockInfo(symbol);
                    Stocks = Stocks.Append(stock);
                }
                return stock;
            }
             else
            {
                throw new KeyNotFoundException("Symbol not registered");
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                UpdateStocksAtRandom();
                await Task.Delay(1000, stoppingToken);
            }

            await Task.CompletedTask;
        }

        private void UpdateStocksAtRandom()
        {
            int count = random.Next(0, Stocks.Count());

            for (int i = 0; i < count; i++)
            {
                int index = random.Next(0, Stocks.Count());
                AdjustStockInfo(Symbols.ElementAt(index));
            }
        }
    }
}
