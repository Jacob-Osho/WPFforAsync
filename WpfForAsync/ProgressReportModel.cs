using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfForAsync
{
    public class ProgressReportModel
    {
        public int ParcentageCounter { get; set; } = 0;
        public List<WebsiteDataModel> SitesDowloaded { get; set; } = new List<WebsiteDataModel>();
    }
}
