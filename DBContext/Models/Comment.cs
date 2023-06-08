namespace DBContext.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime PublicationDate { get; set; }

        public int AuthorId { get; set; }
        public virtual User Author { get; set; }

        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }
    }
}
