﻿using DBContex.Models;

namespace DBContex.Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers ();
        //User GetUserById (int id);
        void AddUser (User user);
        void UpdateUser (User user);
       // void DeleteUser (int id);
    }
}
