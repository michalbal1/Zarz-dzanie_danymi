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
    public partial class Details : ContentPage
    {
        string x;
        public Details(string x)
        {
            InitializeComponent();
            this.x = x;
            Show_Details();
            date.Date = DateTime.Today;

        }
        public async void Show_Details()
        {
            using (var httpClients = new HttpClient())
            {


                DateTime date_to_selected = date.Date;
                var year = date_to_selected.Year;
                var month = date_to_selected.Month.ToString("00");
                var day = date_to_selected.Day.ToString("00");

                try
                {
                    string url = "http://api.nbp.pl/api/exchangerates/rates/A/" + x + "/" + year + "-" + month + "-" + day + "/?format=json";
                    var json = await httpClients.GetStringAsync(url);
                    Root data = JsonConvert.DeserializeObject<Root>(json);
                    Rate datarate = JsonConvert.DeserializeObject<Rate>(json);
                    string url2 = "http://api.nbp.pl/api/exchangerates/rates/A/" + x + "/?format=json";
                    var json2 = await httpClients.GetStringAsync(url2); ;
                    Root data2 = JsonConvert.DeserializeObject<Root>(json2);
                    TitleToFirstLabel.Text = "Szczegóły: \n";
                    myFrame.IsVisible = true;
                    TheFirstLabel.Text = " Numer tabeli: " + data.rates[0].no + "\n Nazwa:" + data.currency + "\n Kod: " + data.code + "\n Kurs z wybranego dnia:" + data.rates[0].mid;
                    //TheFirstLabel.Text = "Kurs dla " +data.currency +"("+data.code+")\n" + "w podanym dniu wynosił: "+ data.rates[0].mid;
                    
                    var difference = Math.Round(data.rates[0].mid - data2.rates[0].mid, 4);
                    if (difference < 0)
                    {
                        SecondSpan.TextColor = Color.Red;
                    }
                    else if (difference > 0)
                    {
                        SecondSpan.TextColor = Color.Green;
                    }
                    else
                    {
                        SecondSpan.TextColor= Color.Black;
                    }
                    Frame1.IsVisible = true;
                    FirstSpan.TextColor = Color.Black;
                    FirstSpan.Text="Różnica między obecnym notowaniem a z dnia " + day +"-"+ month+"-" + year + "\n Wynosi: ";

                    SecondSpan.Text = "("+difference+")";
                }
                catch (HttpRequestException e)
                {
                    Frame1.IsVisible = true;
                    FirstSpan.TextColor = Color.Red;
                    TheFirstLabel.Text = "";
                    myFrame.IsVisible = false;
                    FirstSpan.Text = "W tym dniu nie było notowania\n Notowanie są od poniedziału do piątku, nie wliczając świąt ";
                    SecondSpan.Text = "";
                }




            }
        }
        public void DateSelected(object sender, DateChangedEventArgs e)
        {

            Show_Details();
        }
        async void OnLabelTapped(object sender, EventArgs args)
        {
            var x = this.x;
            await Navigation.PushAsync(new Graph(x));
        }
    }
}