using DBContex.Models;

namespace DBContex.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly Context _context;

        public UserRepository (Context context) {
            _context = context;
        }

        public IEnumerable<User> GetAllUsers () {
           
            return _context.Users.ToList();
        }

        //public User GetUserById (int id) {
        //    return _context.Users.FirstOrDefault(u => u.Id == id);
        //}

        public void AddUser (User user) {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser (User user) {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        //public void DeleteUser (int id) {
        //    var user = _context.Users.FirstOrDefault(u => u.Id == id);
        //    _context.Users.Remove(user);
        //    _context.SaveChanges();
        //}
    }
}
