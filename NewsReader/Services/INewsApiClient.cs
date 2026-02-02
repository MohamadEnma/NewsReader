using NewsReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsReader.Services
{
    public interface INewsApiClient
    {
        Task<IReadOnlyList<ArticleDto>> GetTopHeadlinesAsync(NewsQuery query, CancellationToken ct = default);

    }

}