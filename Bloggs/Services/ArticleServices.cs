using DBContex.Models;
using DBContex.Repository;
using System.Diagnostics;

namespace Bloggs.Services
{
    public class ArticleServices : IArticleServices
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ITagRepository _tagRepository;
        public ArticleServices(IArticleRepository articleRepository, ITagRepository tagRepository)
        {
            this._articleRepository = articleRepository;
            _tagRepository = tagRepository;
        }

        public IEnumerable<Article> Articles(Dictionary<string, bool> tagFilters, int page, int pageSize, out int totalArticles, out Dictionary<string, bool> tagFilter)
        {
            var articles = _articleRepository.GetAllPosts().Where(a => tagFilters.All(tf => a.Tags.Any(t => t.Name == tf.Key && tf.Value)))
                     .Where(a => tagFilters.All(tf => a.Tags.Any(t => t.Name == tf.Key)))
                 .OrderByDescending(a => a.PublicationDate)
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize);

            if (tagFilters != null && tagFilters.Any())
            {
                totalArticles = _articleRepository.GetAllPosts().Where(a => tagFilters.All(tf => a.Tags.Any(t => t.Name == tf.Key && tf.Value)))
                    .Where(a => tagFilters.All(tf => a.Tags.Any(t => t.Name == tf.Key))).Count();
                tagFilter = _tagRepository.GetAllTags()
                .ToDictionary(t => t.Name, t => tagFilters != null && tagFilters.ContainsKey(t.Name) && tagFilters[t.Name]);
            }
            else
            {
                totalArticles = _articleRepository.GetAllPosts().Count();
                tagFilter = _tagRepository.GetAllTags()
        .ToDictionary(t => t.Name, t => tagFilters?.GetValueOrDefault(t.Name) ?? false);
            }
            return articles;
        }
    }
}
