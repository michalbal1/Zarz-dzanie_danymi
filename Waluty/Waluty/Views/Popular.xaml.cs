using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Waluty.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Popular : ContentPage
    {
        DateTime thisDay = DateTime.Today;
        public Popular()
        {

            InitializeComponent();
            Tytul.Text = "Kursy walut w dniu:";
            aktualna.Text = thisDay.ToString("D");
            Show();

        }
        public async void Show()
        {
            var result = await Takerates();
            foreach (var element in result)
            {
                Frame myFrame = new Frame
                {
                    BorderColor = Color.Silver,
                    CornerRadius = 10,
                    HasShadow = true,
                    Margin = 17,
                    Padding = 10,
                    BackgroundColor = Color.Gainsboro,

                    Content = new Label
                    {
                        Text = element.currency + "(" + element.code + ")" + " " + element.rates[0].mid,
                        FontSize = 20,
                        TextColor = Color.Black,
                    }
                };
                First.Children.Add(myFrame);
            }
        }
        public Task<List<Root>> Takerates()
        {


            using (var httpClient = new HttpClient())
            {

                string url = "https://api.nbp.pl/api/exchangerates/rates/a/usd/?format=json";
                string url2 = "https://api.nbp.pl/api/exchangerates/rates/a/chf/?format=json";
                string url3 = "https://api.nbp.pl/api/exchangerates/rates/a/EUR/?format=json";
                string url4 = "https://api.nbp.pl/api/exchangerates/rates/a/GBP/?format=json";

                var json = httpClient.GetStringAsync(url);
                Root data = JsonConvert.DeserializeObject<Root>(json.Result);

                var json2 = httpClient.GetStringAsync(url2);
                Root data2 = JsonConvert.DeserializeObject<Root>(json2.Result);

                var json3 = httpClient.GetStringAsync(url3);
                Root data3 = JsonConvert.DeserializeObject<Root>(json3.Result);

                var json4 = httpClient.GetStringAsync(url4);
                Root data4 = JsonConvert.DeserializeObject<Root>(json4.Result);



                List<Root> prelist = new List<Root>() { data, data2, data3, data4 };


                Task<List<Root>> finallist = Task.FromResult(prelist);


                return finallist;
            }

        }
    }
}