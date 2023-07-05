using Bloggs.Models.Request;
using Bloggs.Models.Response;
using DBContex.Models;
using DBContex.Repository;
using NLog;
using System.Diagnostics;

namespace Bloggs.Services
{
    public class ArticleServices : IArticleServices
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ITagRepository _tagRepository;
        private static readonly NLog.ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly ICommentRepository _commentRepository;
        public ArticleServices(IArticleRepository articleRepository, ITagRepository tagRepository, ICommentRepository commentRepository)
        {
            this._articleRepository = articleRepository;
            _tagRepository = tagRepository;
            _commentRepository = commentRepository;
        }

        public IEnumerable<Article> Articles(Dictionary<string, bool> tagFilters, int page, int pageSize, out int totalArticles, out Dictionary<string, bool> tagFilter)
        {
            var articles = _articleRepository.GetAllPosts()
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
        public async Task CreateArticle(ArticleAddViewModel add, string userIdStr)
        {
            var Tags = new List<Tag>();
            foreach (var tag in add.TagIds)
            {
                Tags.Add(_tagRepository.GetTagById(tag));
            }
            Article model = new Article();
            model.AuthorId = userIdStr;
            model.PublicationDate = DateTime.Now;
            model.Content = add.Content;
            model.Title = add.Title;
            model.Tags = Tags;
            _articleRepository.AddPost(model);
            Logger.Info($"Пользователь {userIdStr} создал статью {add.Title}");
        }
        public ArticleViewModel GetArticleById(int id)
        {
            var article = _articleRepository.GetPostById(id);
            var comments = _commentRepository.GetCommentsByPostId(id);
            Dictionary<string, bool> tagFilters = article.Tags.ToDictionary(t => t.Name, t => true);
            var vm = new ArticleViewModel
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                CreatedAt = article.PublicationDate,
                AuthorId = article.AuthorId,
                AuthorName = article.Author.UserName,
                Comments = comments.Select(c => new CommentViewModel
                {
                    Id = c.Id,
                    Content = c.Text,
                    CreatedAt = c.PublicationDate,
                    AuthorName = c.Author.UserName
                }).ToList(),
                tags = _tagRepository.GetAllTags().
                ToDictionary(t => t.Name, t => tagFilters != null && tagFilters.ContainsKey(t.Name) && tagFilters[t.Name]),
            };

            return vm;
        }
        public ArticleViewModel GetArticleViewModel(int id)
        {
            var article = _articleRepository.GetPostById(id);
            var comments = _commentRepository.GetCommentsByPostId(id);
            var tagFilters = article.Tags.ToDictionary(t => t.Name, t => true);
            var commentViewModels = comments.Select(c => new CommentViewModel
            {
                Id = c.Id,
                Content = c.Text,
                CreatedAt = c.PublicationDate,
                AuthorName = c.Author.UserName
            }).ToList();
            var tagViewModels = _tagRepository.GetAllTags()
                                .ToDictionary(t => t.Name, t => tagFilters != null && tagFilters.ContainsKey(t.Name) && tagFilters[t.Name]);

            var vm = new ArticleViewModel
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                CreatedAt = article.PublicationDate,
                AuthorId = article.AuthorId,
                AuthorName = article.Author.UserName,
                Comments = commentViewModels,
                tags = tagViewModels,
            };

            return vm;

        }
    }
}
