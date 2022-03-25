using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
        CancellationTokenSource  cts = new CancellationTokenSource();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void normalButtom_Click(object sender, RoutedEventArgs e)
        {
            
            var watch = System.Diagnostics.Stopwatch.StartNew();// запускаем секундомер
         var result = DemoMethods.RunDownloadSync();
            PrintResults(result);
            watch.Stop();// останавливаем секундомер
            
            var elapsedMs = watch.ElapsedMilliseconds;
            textBlockWindow.Text += $"Total executaion time : {elapsedMs}";// показываем секунды
        }
        private async void paralellAsyncButtom_Click(object sender, RoutedEventArgs e)
        {
            Progress<ProgressReportModel> progress = new Progress<ProgressReportModel>();
            progress.ProgressChanged += ReportProgress;
            var watch = System.Diagnostics.Stopwatch.StartNew();// запускаем секундомер
            try
            {
                var results = await DemoMethods.RunDownloadParalellAsync(progress/*,cts.Token*/);
                PrintResults(results);
            }
            catch (OperationCanceledException)
            {

                textBlockWindow.Text += $"Dowload operation was cancelled{Environment.NewLine}";
            }
            watch.Stop();// останавливаем секундомер

            var elapsedMs = watch.ElapsedMilliseconds;
            textBlockWindow.Text += $"Total executaion time : {elapsedMs}";// показываем секунды
        }
        private async void AsyncButtom_Click(object sender, RoutedEventArgs e)
        {
            Progress<ProgressReportModel> progress = new Progress<ProgressReportModel>();
            progress.ProgressChanged += ReportProgress;
             var watch = System.Diagnostics.Stopwatch.StartNew();// запускаем секундомер

            try
            {
                var results = await DemoMethods.RunDownloadAsync(progress, cts.Token);
                PrintResults(results);
            }
            catch (OperationCanceledException)
            {

                textBlockWindow.Text += $"The async dowload  was cancelled{Environment.NewLine}";
            }
            watch.Stop();// останавливаем секундомер

            var elapsedMs = watch.ElapsedMilliseconds;
            textBlockWindow.Text += $"Total executaion time : {elapsedMs}";// показываем секунды
        }

        private void ReportProgress(object sender, ProgressReportModel e)
        {
            progressBar.Value = e.ParcentageCounter;
            PrintResults(e.SitesDowloaded);
        }

        private void CancelButtom_Click(object sender, RoutedEventArgs e)
        {
           
            cts.Cancel();

        }
        private void PrintResults(List<WebsiteDataModel> list)
        {
            textBlockWindow.Text = "";
            foreach (WebsiteDataModel data in list)
            {
                textBlockWindow.Text += $"{data.WebsiteUrl} dowloaded: {data.WebsiteData.Length} characters long.{ Environment.NewLine}";
            }
        }

        
    }
}
