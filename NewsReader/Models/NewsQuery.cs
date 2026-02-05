using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsReader.Models
{
    public sealed class NewsQuery
    {
        public string Country { get; set; } = "us";
        public string? SearchText { get; set; }
        public int PageSize { get; set; } = 20;
        public string Category { get; set; } = "general";

        public int Page { get; set; } = 1;
    }
}
