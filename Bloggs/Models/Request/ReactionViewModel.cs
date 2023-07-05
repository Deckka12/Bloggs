namespace Bloggs.Models.Request
{
    public class ReactionViewModel
    {
        public int ArticleId { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }
        public int ReactionType { get; set; }

    }
}
