using DBContex.Models;

namespace DBContex.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly Context  _dbContext;

        public TagRepository (Context dbContext) {
            _dbContext = dbContext;
        }

        public IEnumerable<Tag> GetAllTags () {
            return _dbContext.Tags.ToList();
        }

        public Tag GetTagById (int id) {
            return _dbContext.Tags.FirstOrDefault(t => t.Id == id);
        }

        public void AddTag (Tag tag) {
            _dbContext.Tags.Add(tag);
            _dbContext.SaveChanges();
        }

        public void UpdateTag (Tag tag) {
            var tags = _dbContext.Tags.Find(tag.Id);
            tags.Name = tag.Name;
            _dbContext.Tags.Update(tags);
            _dbContext.SaveChanges();
        }

        public void DeleteTag (int id) {
            var tag = GetTagById(id);
            _dbContext.Tags.Remove(tag);
            _dbContext.SaveChanges();
        }
    }
}
