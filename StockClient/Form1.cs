using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace StockClient
{
    public partial class Form1 : Form
    {
        public ApiClient ApiClient;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ApiClient != null)
            {
                listView1.Items.Clear();
                ApiClient.Stop();
            }

            ApiClient = new ApiClient(this.onDataReceived, this.onError);

            ApiClient.Start(textBox1.Text + "/StockInfo", textBox1.Text + "/Symbol");
            if (ApiClient.IsStarted)
                button1.Enabled = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void onDataReceived(List<StockInfo> data)
        {
            if (listView1.InvokeRequired)
            {
                var delegateMethod = new ApiClient.OnDataReceived(onDataReceived);
                listView1.Invoke(delegateMethod, data);
            } else
            {
                listView1.Items.Clear();
                listData(data);
            }

        }

        private void listData(List<StockInfo> data)
        {
            foreach (var item in data)
            {
                ListViewItem listItem = new ListViewItem(item.Symbol.Name);
                listItem.SubItems.Add(item.Bid.ToString());
                listItem.SubItems.Add(item.Ask.ToString());

                listView1.Items.Add(listItem);
            }
        }

        private void onError(Exception e)
        {
            if (listView1.InvokeRequired)
            {
                var delegateMethod = new ApiClient.OnError(onError);
                listView1.Invoke(delegateMethod, e);
            } else
            {
                listView1.Items.Clear();
                listView1.Items.Add(e.Message);
            }
        }
    }

    public class StockInfo
    {
        public DateTime Date { get; set; }
        public Symbol Symbol { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }
    }

    public class Symbol
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public Symbol()
        { }

        public Symbol(string name, int id)
        {
            Name = name;
            Id = id;
        }
    }
}
