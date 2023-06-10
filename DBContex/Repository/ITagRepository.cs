using DBContex.Models;

namespace DBContex.Repository
{
    public interface ITagRepository
    {
        IEnumerable<Tag> GetAllTags ();
        Tag GetTagById (int id);
        void AddTag (Tag tag);
        Tag GetTahByName(string name);
        void UpdateTag (Tag tag);
        void DeleteTag (int id);
    }
}
