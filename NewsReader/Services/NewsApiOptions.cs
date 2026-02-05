using NewsReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsReader.Services
{
    public class NewsApiOptions
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string Country { get; set; } = "us";
        public int PageSize { get; set; } = 20;
    }
}
