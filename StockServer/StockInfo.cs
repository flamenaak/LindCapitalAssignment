namespace StockServer
{
    public class StockInfo
    {
        public DateTime Date { get; set; }

        public Symbol Symbol { get; set; }

        public double Bid { get; set; }

        public double Ask { get; set; }
    }
}