using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProductManager.Entities;

namespace ProductManager.Model
{
    public class User : UserInterface
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        // Method to create a new user
        public async Task<bool> Create(User user)
        {
            try
            {
                using (SqlConnect conn = new SqlConnect())
                {
                    SqlCommand cmd = new SqlCommand("CreateUser", conn.GetConnection());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserName", user.UserName));
                    cmd.Parameters.Add(new SqlParameter("@Email", user.Email));
                    cmd.Parameters.Add(new SqlParameter("@Password", user.Password));
                    await conn.OpenConnectionAsync();
                    await cmd.ExecuteNonQueryAsync();
                    conn.GetConnection().Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating user: {ex.Message}");
                return false;
            }
        }
        
        public async Task<bool> AddInitialUser(string host, string db, User user)
        {
            try

            {

                using (SqlConnect conn = new SqlConnect(host, db))
                {

                    await conn.OpenConnectionAsync();

                    // Check if the username already exists
                    string checkUserQuery = "SELECT COUNT(*) FROM [Users] WHERE UserName = @UserName";
                    using (SqlCommand checkCmd = new SqlCommand(checkUserQuery, conn.GetConnection()))
                    {
                        checkCmd.Parameters.Add(new SqlParameter("@UserName", user.UserName));
                        int userCount = (int)await checkCmd.ExecuteScalarAsync();

                        if (userCount > 0)
                        {
                            MessageBox.Show("UserName đã tồn tại, hãy tạo tài khoản khác", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    }

                    SqlCommand cmd = new SqlCommand("CreateUser", conn.GetConnection());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserName", user.UserName));
                    cmd.Parameters.Add(new SqlParameter("@Email", user.Email));
                    cmd.Parameters.Add(new SqlParameter("@Password", user.Password));
                    //await conn.OpenConnectionAsync();
                    await cmd.ExecuteNonQueryAsync();
                    conn.GetConnection().Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating user: {ex.Message}");
                return false;
            }
        }

        // Method to update an existing user
        public async Task<bool> Update(User user)
        {
            try
            {
                using (SqlConnect conn = new SqlConnect())
                {
                    SqlCommand cmd = new SqlCommand("EditUser", conn.GetConnection());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserId", user.Id));
                    cmd.Parameters.Add(new SqlParameter("@UserName", user.UserName));
                    cmd.Parameters.Add(new SqlParameter("@Email", user.Email));
                    cmd.Parameters.Add(new SqlParameter("@Password", user.Password));
                    await conn.OpenConnectionAsync();
                    await cmd.ExecuteNonQueryAsync();
                    conn.GetConnection().Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating user: {ex.Message}");
                return false;
            }
        }

        // Method to delete a user by ID
        public async Task<bool> Delete(int id)
        {
            try
            {
                using (SqlConnect conn = new SqlConnect())
                {
                    SqlCommand cmd = new SqlCommand("DeleteUser", conn.GetConnection());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserId", id));
                    await conn.OpenConnectionAsync();
                    await cmd.ExecuteNonQueryAsync();
                    conn.GetConnection().Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting user: {ex.Message}");
                return false;
            }
        }

        // Method to fetch all users
        public async Task<DataTable> Fetch()
        {
            try
            {
                using (SqlConnect conn = new SqlConnect())
                {
                    bool checkConn = await conn.OpenConnectionAsync();
                    if (checkConn)
                    {
                        string query = "SELECT * FROM Users";
                        using (SqlCommand cmd = new SqlCommand(query, conn.GetConnection()))
                        {
                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            DataTable table = new DataTable();
                            adapter.Fill(table);
                            return table;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching users: {ex.Message}");
            }
            return null;
        }

        // Method to get a user by ID
        public async Task<User> GetUserById(int userId)
        {
            try
            {
                using (SqlConnect conn = new SqlConnect())
                {
                    bool checkConn = await conn.OpenConnectionAsync();
                    if (checkConn)
                    {
                        string query = "SELECT * FROM Users WHERE UserId = @UserId";
                        using (SqlCommand cmd = new SqlCommand(query, conn.GetConnection()))
                        {
                            cmd.Parameters.AddWithValue("@UserId", userId);
                            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                            {
                                if (reader.Read())
                                {
                                    User user = new User
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("UserId")),
                                        UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                        Email = reader.GetString(reader.GetOrdinal("Email")),
                                        Password = reader.GetString(reader.GetOrdinal("Password"))
                                    };
                                    return user;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching user: {ex.Message}");
            }
            return null;
        }

        public Task<bool> Create()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update()
        {
            throw new NotImplementedException();
        }
    }
}
