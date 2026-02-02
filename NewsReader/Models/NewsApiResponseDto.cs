using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsReader.Models
{
    public class NewsApiResponseDto
    {
        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("totalResults")]
        public int? TotalResults { get; set; }

        [JsonProperty("articles")]
        public List<ArticleDto>? Articles { get; set; }
    }
}
