using StockServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var stockService = new StockService();
var symbols = new List<Symbol>();
string[] symbolNames = new[]
    {
            "Google", "IMB", "Apple", "Samsung", "Philips", "Microsoft"
    };

int newId = 0;
foreach (var symbol in symbolNames)
{
    symbols = symbols.Append(new Symbol(symbol, newId++)).ToList();
}
stockService.Initialize(symbols);

builder.Services.AddSingleton<IStockService>(stockService);
builder.Services.AddHostedService<StockService>(_ => stockService);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();
