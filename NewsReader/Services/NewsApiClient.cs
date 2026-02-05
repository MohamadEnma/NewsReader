using Microsoft.Extensions.Options;
using NewsReader.Models;
using System.Net.Http.Json;
using System.Diagnostics;

namespace NewsReader.Services
{
    public class NewsApiClient : INewsApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly NewsApiOptions _opt;

        public NewsApiClient(HttpClient http, IOptions<NewsApiOptions> opt)
        {
            _httpClient = http;
            _opt = opt.Value;

            if (!_httpClient.DefaultRequestHeaders.Contains("User-Agent"))
            {
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "MauiNewsReader");
            }
        }

        public async Task<IReadOnlyList<ArticleDto>> GetTopHeadlinesAsync(NewsQuery query, CancellationToken ct = default)
        {
            
            var searchTerm = string.IsNullOrWhiteSpace(query.SearchText) ? "latest" : query.SearchText;
            var categoryParam = string.IsNullOrWhiteSpace(query.Category) ? "&category=general" : $"&category={query.Category}";


            var url = $"{_opt.BaseUrl}/v2/everything?q={searchTerm}&pageSize={query.PageSize}&apiKey={_opt.ApiKey}";

            try
            {
                var response = await _httpClient.GetFromJsonAsync<NewsApiResponseDto>(url, ct);
                return response?.Articles ?? new List<ArticleDto>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"API Error: {ex.Message}");
                return new List<ArticleDto>();
            }
        }
    }
}