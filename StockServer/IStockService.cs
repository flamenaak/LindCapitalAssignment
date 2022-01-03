namespace StockServer
{
    /// <summary>
    /// Service which provides StockInfo for symbols it is initialized with
    /// </summary>

    public interface IStockService : IHostedService
    {
        void Initialize(IEnumerable<Symbol> symbols);

        public IEnumerable<StockInfo> Stocks { get; }
        public IEnumerable<Symbol> Symbols { get; }

    }
}