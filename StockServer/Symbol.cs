namespace StockServer
{
    public class Symbol
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public Symbol(string name, int id)
        {
            Name = name;
            Id = id;
        }
    }
}