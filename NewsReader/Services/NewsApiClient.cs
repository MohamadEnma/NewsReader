using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NewsReader.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace NewsReader.Services
{
    public sealed class NewsApiClient : INewsApiClient
    {
        private readonly HttpClient _http;
        private readonly NewsApiOptions _opt;

        public NewsApiClient(HttpClient http, IOptions<NewsApiOptions> opt)
        {
            _http = http;
            _opt = opt.Value;
        }

        public async Task<IReadOnlyList<ArticleDto>> GetTopHeadlinesAsync(NewsQuery query, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(_opt.ApiKey))
                throw new InvalidOperationException("NewsApi:ApiKey saknas i appsettings.json.");

            var country = string.IsNullOrWhiteSpace(query.Country) ? _opt.Country : query.Country;
            var pageSize = query.PageSize <= 0 ? _opt.PageSize : query.PageSize;

            var qb = HttpUtility.ParseQueryString(string.Empty);
            qb["apiKey"] = _opt.ApiKey;
            qb["country"] = "us";
            qb["pageSize"] = pageSize.ToString();

            if (!string.IsNullOrWhiteSpace(query.Category))
                qb["category"] = query.Category;

            if (!string.IsNullOrWhiteSpace(query.SearchText))
                qb["q"] = query.SearchText;

            var url = $"/v2/top-headlines?{qb}"; // enligt docs: /v2/top-headlines :contentReference[oaicite:4]{index=4}

            using var resp = await _http.GetAsync(url, ct);

            if (resp.StatusCode == HttpStatusCode.Unauthorized)
                throw new InvalidOperationException("Ogiltig API-nyckel (401).");

            if (!resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync(ct);
                throw new HttpRequestException($"NewsAPI-fel {(int)resp.StatusCode}: {body}");
            }

            var dto = await resp.Content.ReadFromJsonAsync<NewsApiResponseDto>(cancellationToken: ct);
            return dto?.Articles ?? new List<ArticleDto>();

        }
    }
}