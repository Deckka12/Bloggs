using DBContex.Models;

namespace Bloggs.Services
{
    public interface IArticleServices
    {
         IEnumerable<Article> Articles(Dictionary<string, bool> tagFilters, int page, int pageSize, out int totalArticles, out Dictionary<string, bool> tagFilter);
    }
}
