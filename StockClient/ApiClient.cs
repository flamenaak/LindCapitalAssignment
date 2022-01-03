using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace StockClient
{
    public partial class ApiClient
    {
        public delegate void OnDataReceived(List<StockInfo> data);
        public delegate void OnError(Exception e);

        private const int POLL_INTERVAL = 1000;
        public HttpClient HttpClient;
        private Thread thread;
        private CancellationToken cancellationToken;
        private CancellationTokenSource source;
        public bool IsStarted { get; set; }
        private OnDataReceived onDataReceived;
        private OnError onError;

        public IEnumerable<Symbol> Symbols { get; set; }

        private string url;
        

        public ApiClient(OnDataReceived onDataReceived, OnError onError)
        {
            HttpClient = new HttpClient();
            thread = new Thread(DoTask);
            source = new CancellationTokenSource();
            cancellationToken = source.Token;
            IsStarted = false;
            this.onDataReceived = onDataReceived;
            this.onError = onError;
        }

        private async void DoTask()
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var builder = new UriBuilder(url);
                var queryString = new NameValueCollection();
                var serializer = new JavaScriptSerializer();
                
                IEnumerable<int> symbols = Symbols.Select(symbol => symbol.Id);
                var dataString = await HttpClient.PostAsync(builder.ToString(), JsonContent.Create(symbols));
                var deserializedData = serializer.Deserialize<List<StockInfo>>(await dataString.Content.ReadAsStringAsync());

                onDataReceived(deserializedData);
                Thread.Sleep(POLL_INTERVAL);
            }
        }

        public async void Start(string stockUrl, string symbolUrl)
        {
            try
            {
                Symbols = await GetSymbols(symbolUrl);
                this.url = stockUrl;
                IsStarted = true;
                thread.Start();
            } catch (Exception ex)
            {
                onError(ex);
            }
        }

        private async Task<IEnumerable<Symbol>> GetSymbols(string symbolUrl)
        {
            var dataString = await HttpClient.GetStringAsync(symbolUrl);
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<IEnumerable<Symbol>>(dataString);         
        }

        public void Stop()
        {
            source.Cancel();
        }
    }
}
