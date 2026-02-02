using NewsReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsReader.Services
{
    public sealed class NewsApiOptions 
    {
        public string BaseUrl { get; set; } = "https://newsapi.org";
        public string ApiKey { get; set; } = "";
        public string Country { get; set; } = "se";
        public int PageSize { get; set; } = 20;

       
    }
}
