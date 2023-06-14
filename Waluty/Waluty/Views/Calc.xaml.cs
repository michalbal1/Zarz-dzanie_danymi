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
    public partial class Calc : ContentPage
    {
        public Calc()
        {
            InitializeComponent();
            PickerDefault();
            Title.Text = "Kalkulator walutowy: \n \n";
        }

        private void PickerDefault()
        {
            PickerFirst.SelectedItem = "PLN";
            PickerSecond.SelectedItem = "PLN";
        }
        public void Change( object sender, EventArgs e)
        {
            string x = PickerFirst.SelectedItem as string;
            string y = PickerSecond.SelectedItem as string;

            PickerFirst.SelectedItem = y;
            PickerSecond.SelectedItem = x;
        }
        public async void Calculator(object sender, EventArgs e)
        {
            using (var HttpClient = new HttpClient())
            {
                try
                {
                    string x = PickerFirst.SelectedItem as string;
                    string y = PickerSecond.SelectedItem as string;
                    var number = Quantity.Text;
                    double quantity = double.Parse(number);
                    Frame1.IsVisible = true;
                    Label1.TextColor = Color.Black;
                    if (x == y)
                    {
                        double result = Math.Round(quantity,2);
                        Label1.Text = "Za sumę: "+quantity+ "("+x+") możesz dokonać wymiany na sumę: "+ result + "(" + y + ")";
                    }

                    else if (x == "PLN")
                    {
                        var url2 = "http://api.nbp.pl/api/exchangerates/rates/A/" + y + "/";
                        var json2 = await HttpClient.GetStringAsync(url2);
                        Root data2 = JsonConvert.DeserializeObject<Root>(json2);

                        double result = Math.Round(quantity / data2.rates[0].mid,2);
                        Label1.Text = "Za sumę: " + quantity + "(" + x + ") możesz dokonać wymiany na sumę: " + result + "(" + y + ")";

                    }
                    else if (y == "PLN")
                    {
                        var url = "http://api.nbp.pl/api/exchangerates/rates/A/" + x + "/";
                        var json = await HttpClient.GetStringAsync(url);
                        Root data = JsonConvert.DeserializeObject<Root>(json);
                        double result = Math.Round(quantity * data.rates[0].mid,2);
                        Label1.Text = "Za sumę: " + quantity + "(" + x + ") możesz dokonać wymiany na sumę: " + result + "(" + y + ")";
                    }
                    else
                    {
                        var url = "http://api.nbp.pl/api/exchangerates/rates/A/" + x + "/";
                        var url2 = "http://api.nbp.pl/api/exchangerates/rates/A/" + y + "/";
                        var json = await HttpClient.GetStringAsync(url);
                        Root data = JsonConvert.DeserializeObject<Root>(json);
                        var json2 = await HttpClient.GetStringAsync(url2);
                        Root data2 = JsonConvert.DeserializeObject<Root>(json2);
                        double result = Math.Round((quantity * data.rates[0].mid) / data2.rates[0].mid,2);
                        Label1.Text = "Za sumę: " + quantity + "(" + x + ") możesz dokonać wymiany na sumę: " + result + "("+y+")";
                    }

                }
                catch
                {
                    Frame1.IsVisible = true;
                    Label1.TextColor = Color.Red;
                    Label1.Text = "Wystąpił błąd sprawdz poprawność wykokanych czynności";
                }

            }

        }
    }
}