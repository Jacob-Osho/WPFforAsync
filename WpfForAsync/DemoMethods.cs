using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfForAsync
{
    public class DemoMethods
    {
        public static async Task<List<WebsiteDataModel>> RunDownloadParalellAsync(IProgress<ProgressReportModel> progress/*, CancellationToken cancellationToken*/)//this is dope!
        {

            var websites = PrepData();
            List<Task<WebsiteDataModel>> tasks = new List<Task<WebsiteDataModel>>();
            
            foreach (string site in websites)
            {
                tasks.Add(DownloadWebsiteAsync(site));// берем дату и урл и  показываем урл 
                //cancellationToken.ThrowIfCancellationRequested();
               
            }
            var output = await Task.WhenAll(tasks);
            return  new List<WebsiteDataModel>(output);
        }
        public static async Task<List<WebsiteDataModel>> RunDownloadAsync(IProgress<ProgressReportModel> progress, CancellationToken cancellationToken)// this ain't so dope!
        {

            var websites = PrepData();
            var output = new List<WebsiteDataModel>();
            ProgressReportModel progressReport = new ProgressReportModel();
            if (websites != null)
            {
                foreach (string site in websites)
                {
                    WebsiteDataModel results = await Task.Run(() => DownloadWebsite(site));// берем дату и урл и  показываем урл 
                    output.Add(results);

                cancellationToken.ThrowIfCancellationRequested();
                progressReport.SitesDowloaded = output;
                    progressReport.ParcentageCounter = (output.Count * 100) / websites.Count;//  
                    progress.Report(progressReport);
                }
            } 
            return output;
        }
        public static List<WebsiteDataModel> RunDownloadSync()
        {
            var websites = PrepData();
            var output = new List<WebsiteDataModel>();

            foreach (string site in websites)
            {
                WebsiteDataModel results = DownloadWebsite(site);// берем дату и урл и  показываем урл 
                output.Add(results);

            }
            return output;
        }
        public static async Task<WebsiteDataModel> DownloadWebsiteAsync(string websiteUrl) // берем дату и урл
        {
            var output = new WebsiteDataModel();
            WebClient client = new WebClient();//дает возможность работать с кодом страницы
            output.WebsiteUrl = websiteUrl;
            output.WebsiteData = await client.DownloadStringTaskAsync(websiteUrl);
            return output;
        }
        public static WebsiteDataModel DownloadWebsite(string websiteUrl) // берем дату и урл
        {
            var output = new WebsiteDataModel();
            WebClient client = new WebClient();//дает возможность работать с кодом страницы
            output.WebsiteUrl = websiteUrl;
            output.WebsiteData = client.DownloadString(websiteUrl);
            return output;
        }
        private static List<string> PrepData()
        {
            var output = new List<string>
            {
                "https://www.google.com",
                "https://www.yahoo.com",
                "https://www.microsoft.com",
                "https://www.codeproject.com",
                "https://www.cnn.com",
                "https://www.stackoverflow.com",
                "https://djinni.co",
                "https://www.youtube.com"
            };
            return output;
        }
      
    }
}
