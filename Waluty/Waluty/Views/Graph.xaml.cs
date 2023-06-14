using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Waluty.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Graph : ContentPage
    {
        string x;
        public Graph(string x)
        {
            
            InitializeComponent();
            this.x = x;
        }

       public async void  Get_Data(Object sender, EventArgs e)
        {

            Button button = (Button)sender;
            string count = string.Empty;
            using (var httpClients = new HttpClient())
            {
                
                if (button.Text == "5")
                {
                    count = "5";
                }
                else if (button.Text == "10")
                {
                    count = "10";
                }
                first.Text = count;

                string url= "http://api.nbp.pl/api/exchangerates/rates/A/"+ x +"/last/"+count+"/" ;
               var json = await httpClients.GetStringAsync(url);
               Root data = JsonConvert.DeserializeObject<Root>(json);


                second.Text = "" + data.rates[4].mid;
                


            }
        }
       
    }
}