using Bloggs.Models.Request;
using Bloggs.Models.Response;
using DBContex.Models;

namespace Bloggs.Services
{
    public interface IArticleServices
    {
        IEnumerable<Article> Articles(Dictionary<string, bool> tagFilters, int page, int pageSize, out int totalArticles, out Dictionary<string, bool> tagFilter);
        Task CreateArticle(ArticleAddViewModel add, string userIdStr);
        ArticleViewModel GetArticleById(int id);
        ArticleViewModel GetArticleViewModel(int id);
    }
}
