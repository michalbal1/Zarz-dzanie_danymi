using Newtonsoft.Json;
using System;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Waluty.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Screach : ContentPage
    {
        
        DateTime thisDay = DateTime.Today;
        public Screach()
        {

            InitializeComponent();
            Date.Text = "Kurs walut w dniu:\n" + thisDay.ToString("D");
            
           
            

        }

        public async void Takerates(object sender, EventArgs e)
        {
            using (var httpCilents = new HttpClient())
            {
                var x = Code.Text;
                try
                {

                    string url = "http://api.nbp.pl/api/exchangerates/rates/A/" + x + "/last/2/?format=json";
                    var json = await httpCilents.GetStringAsync(url);
                    Root data = JsonConvert.DeserializeObject<Root>(json);
                   



                    
                    TheFirstLabel.Text = "Nazwa: "+data.currency+"\n" + "Kod: "+data.code+"\n"+"Kurs: " + data.rates[0].mid+"\n";
                    Frame2.IsVisible = true;
                    Frame3.IsVisible = true;





                    var difference = Math.Round(data.rates[0].mid - data.rates[1].mid , 4);

                    if (difference < 0)
                    {
                        TheSecondLabel.TextColor = Color.Red;
                    }
                    else if (difference > 0)
                    {
                        TheSecondLabel.TextColor = Color.Green;
                    }
                    else
                    {
                        TheSecondLabel.TextColor = Color.Black;
                    }
                    TitleToSecondLabel.Text = "Różnica między bieżącym notowaniem a poprzednim wynosi: \n";
                    TheSecondLabel.Text = "(" + difference + ")";
                    Frame1.IsVisible = true;

                    
                    Deatils.Text = "\n Szczegóły";

                }

                catch (HttpRequestException h)
                {
                    TheFirstLabel.Text = "";
                    TitleToSecondLabel.Text = "";
                    Deatils.Text = "";
                    Frame1.IsVisible = true;
                    Frame2.IsVisible = false;
                    Frame3.IsVisible = false;
                    TheSecondLabel.TextColor = Color.Red;
                    TheSecondLabel.Text = "Wystąpił błąd, sprawdź poprawność kodu w tabeli";
                }
            }



        }
        async void OnLabelTapped(object sender, EventArgs args)
        {
            var x = Code.Text;
            await Navigation.PushAsync(new Details(x));
        }

    }
}
