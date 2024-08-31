using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProductManager.Model;

namespace ProductManager.Controller
{
    public class UserController
    {
        // The User object that will be managed by this controller
        public User user;

        // Constructor initializes the user object
        public UserController()
        {
            user = new User();
        }

        // Method to fetch all users from the database
        public async Task<DataTable> GetAllUsersAsync()
        {
            return await user.Fetch();
        }

        // Method to add a new user
        public async Task<bool> AddNewUser(string username, string email, string password)
        {
            user.UserName = username;
            user.Email = email;
            user.Password = password;
            return await user.Create(user);
        }
        public async Task<bool> AddInitialUser(string host, string database, string username, string email, string password)
        {
            user.UserName = username;
            user.Email = email;
            user.Password = password;
            return await user.AddInitialUser(host, database, user);
        }

        // Method to update an existing user
        public async Task EditUser(int userId, string username, string email, string password)
        {
            user.Id = userId;
            user.UserName = username;
            user.Email = email;
            user.Password = password;
            await user.Update(user);
        }

        // Method to remove a user by their ID
        public async Task RemoveUser(int userId)
        {
            await user.Delete(userId);
        }

        // Method to fetch a user by their ID
        public async Task<User> GetUserById(int userId)
        {
            User user = new User();
            User currentUser = await user.GetUserById(userId);
            return currentUser;
        }
    }
}
