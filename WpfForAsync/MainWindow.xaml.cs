using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfForAsync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void normalButtom_Click(object sender, RoutedEventArgs e)
        {
           
            var watch = System.Diagnostics.Stopwatch.StartNew();// запускаем секундомер
            RunDownloadSync();
            watch.Stop();// останавливаем секундомер
            
            var elapsedMs = watch.ElapsedMilliseconds;
            textBlockWindow.Text += $"Total executaion time : {elapsedMs}";// показываем секунды
        }
        private void RunDownloadSync()
        {
            var websites = PrepData();
            foreach(string site in websites)
            {
                WebsiteDataModel results = DownloadWebsite(site);// берем дату и урл и  показываем урл 
                ReportWebsiteInfo(results); // показываем урл ,дату   и /n 
            }
        }
        private List<string> PrepData()
        {
            var output = new List<string>();
            textBlockWindow.Text = "";
            output.Add("https://www.google.com");
            output.Add("https://www.yahoo.com");
            output.Add("https://www.microsoft.com");
            output.Add("https://www.codeproject.com");
            output.Add("https://www.cnn.com");
            output.Add("https://stackoverflow.com");
            return output;
        }
        private WebsiteDataModel DownloadWebsite(string websiteUrl) // берем дату и урл
        {
            var output = new WebsiteDataModel();
            WebClient client = new WebClient();//дает возможность работать с кодом страницы
            output.WebsiteUrl = websiteUrl;
           output.WebsiteData = client.DownloadString(websiteUrl);
            return output;
        }
        private  async Task<WebsiteDataModel> DownloadWebsiteAsync(string websiteUrl) // берем дату и урл
        {
            var output = new WebsiteDataModel();
            WebClient client = new WebClient();//дает возможность работать с кодом страницы
            output.WebsiteUrl = websiteUrl;
            output.WebsiteData = await client.DownloadStringTaskAsync(websiteUrl);
            return output;
        }
        private void ReportWebsiteInfo(WebsiteDataModel data)// показываем урл ,дату   и /n 
        {
            textBlockWindow.Text += $"{data.WebsiteUrl} dowloaded: {data.WebsiteData.Length} characters long.{ Environment.NewLine}";
        }
     
        private async Task RunDownloadAsync()// this ain't so dope!
        {

            var websites = PrepData();
            foreach (string site in websites)
            {
                WebsiteDataModel results = await Task.Run(() => DownloadWebsite(site));// берем дату и урл и  показываем урл 
                ReportWebsiteInfo(results); // показываем урл ,дату   и /n 
            }
        }
        private async Task RunDownloadParalellAsync()//this is dope!
        {

            var websites = PrepData();
            List<Task<WebsiteDataModel>> tasks = new List<Task<WebsiteDataModel>>();
            foreach (string site in websites)
            {
               tasks.Add(DownloadWebsiteAsync(site));// берем дату и урл и  показываем урл 
                
            }
          
            WebsiteDataModel [] result = await Task.WhenAll(tasks);
            foreach(WebsiteDataModel results in result)
            {
                ReportWebsiteInfo(results);
            }
           
        }
        private async void asyncButtom_Click(object sender, RoutedEventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();// запускаем секундомер
           await  RunDownloadParalellAsync();
            watch.Stop();// останавливаем секундомер

            var elapsedMs = watch.ElapsedMilliseconds;
            textBlockWindow.Text += $"Total executaion time : {elapsedMs}";// показываем секунды
        }
    }
}
