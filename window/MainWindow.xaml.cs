using Newtonsoft.Json;
using System;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;
using static WeatherApp.Models.WeatherModel;
using System.Text;
using System.Timers;
using System.Threading.Tasks;

namespace WeatherApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
        public static string Metric = "metric";
        static root Info;
        public MainWindow()
        {
            InitializeComponent();
            GetWeather();
        }

        private string ApiKey = "badd09470b6f3c13dcd793207e86b4a5";
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (town.Text == "")
            {
                MessageBox.Show("Введите город!", "WeatherApp");
            }
            else
            {
                using (WebClient web = new WebClient())
                {
                    try
                    {
                        string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&lang=ru&appid={1}&units={2}", town.Text, ApiKey, Metric);//
                        var json = web.DownloadString(url);
                        Info = JsonConvert.DeserializeObject<root>(json);
                        Uri resourceUri = new Uri("https://openweathermap.org/img/w/" + Info.weather[0].icon + ".png", UriKind.Absolute);
                        weatherimage.Source = new BitmapImage(resourceUri);
                        town.Text = UnicodeToUTF8(Info.name);
                        temp.Text = Info.main.temp.ToString();
                        weather.Text = UnicodeToUTF8(Info.weather[0].description);
                        sunrise.Text = ConvertFromUnixTimestamp(Info.sys.sunrise + Info.timezone).ToString();
                        sunset.Text = ConvertFromUnixTimestamp(Info.sys.sunset + Info.timezone).ToString();
                        humidity.Text = Info.main.humidity.ToString();
                        pressure.Text = (Info.main.pressure * 0.75).ToString();
                        windspeed.Text = Info.wind.speed.ToString();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\nВозможна ошибка в правописании города!","Ошибка");
                    }
                   
                }
            }
        }
        void GetWeather()
        {
            using (WebClient web = new WebClient())
            {

                string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q=Kazan&lang=ru&appid={0}&units={1}",ApiKey,Metric);//
                var json = web.DownloadString(url);
                Info = JsonConvert.DeserializeObject<root>(json);
                Uri resourceUri = new Uri("https://openweathermap.org/img/w/" + Info.weather[0].icon + ".png", UriKind.Absolute);
                weatherimage.Source = new BitmapImage(resourceUri);
                town.Text = UnicodeToUTF8(Info.name);
                temp.Text = Info.main.temp.ToString();
                weather.Text = UnicodeToUTF8(Info.weather[0].description);
                sunrise.Text = ConvertFromUnixTimestamp(Info.sys.sunrise + Info.timezone).ToString();
                sunset.Text = ConvertFromUnixTimestamp(Info.sys.sunset + Info.timezone).ToString();
                humidity.Text = Info.main.humidity.ToString();
                pressure.Text = (Info.main.pressure * 0.75).ToString();
                windspeed.Text = Info.wind.speed.ToString();
            }
        }
        static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
        static string UnicodeToUTF8(string text)
        {
            Encoding utf8 = Encoding.GetEncoding("UTF-8");
            Encoding win1251 = Encoding.GetEncoding("Windows-1251");

            byte[] utf8Bytes = win1251.GetBytes(text);
            byte[] win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);

            return win1251.GetString(win1251Bytes);
        }
    }
}
