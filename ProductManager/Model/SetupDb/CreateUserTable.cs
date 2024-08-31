using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProductManager.Entities;
using ProductManager.Model;

namespace ProductManager.Model.SetupDb
{
    public class CreateUserTable : DbInterface
    {
        private readonly SqlConnect _conn;

        public CreateUserTable(SqlConnect sqlConnect)
        {
            _conn = sqlConnect;
        }

        public async Task<bool> SetupTable()
        {
            bool tableCreated = await CreateUserTableAsync();
            if (tableCreated)
            {
                bool procedureInsertCreated = await CreateInsertUserAsync();
                bool procedureUpdateCreated = await CreateUpdateUserAsync();
                bool procedureDeleteCreated = await CreateDeleteUserAsync();

                return procedureInsertCreated && procedureUpdateCreated && procedureDeleteCreated;
            }
            return false;
        }

        // Create User table with cascading delete and update
        public async Task<bool> CreateUserTableAsync()
        {
            bool checkUserTable = await _conn.TableExist("Users");
            if (checkUserTable)
            {
                return true;
            }
            try
            {
                string createTableQuery = @"
                    CREATE TABLE Users (
                        UserId INT PRIMARY KEY IDENTITY(1,1),
                        UserName NVARCHAR(100) NOT NULL,
                        Email NVARCHAR(255) NOT NULL UNIQUE,
                        Password NVARCHAR(255) NOT NULL,
                        CreatedAt DATETIME DEFAULT GETDATE(),
                        UpdatedAt DATETIME NULL
                    );
                ";

                await _conn.CreateTableAsync(createTableQuery);

                MessageBox.Show("Bảng Users đã được tạo thành công!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo bảng Users: {ex.Message}",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
        }

        // Method to create the procedure to insert a new user
        public async Task<bool> CreateInsertUserAsync()
        {
            try
            {
                string dropProcedure = @"
                    IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.CreateUser') AND type IN (N'P', N'PC'))
                        DROP PROCEDURE dbo.CreateUser;
                ";
                await _conn.ExecuteNonQueryAsync(dropProcedure);

                string createInsertProcedure = @"
                    CREATE PROCEDURE CreateUser
                        @UserName NVARCHAR(100),
                        @Email NVARCHAR(255),
                        @Password NVARCHAR(255)
                    AS
                    BEGIN
                        INSERT INTO Users (UserName, Email, Password, CreatedAt)
                        VALUES (@UserName, @Email, @Password, GETDATE());
                    END;
                ";

                await _conn.ExecuteNonQueryAsync(createInsertProcedure);
                MessageBox.Show("Stored procedure CreateUser đã được tạo thành công!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo stored procedure CreateUser: {ex.Message}",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
        }

        // Method to create the procedure to update an existing user
        public async Task<bool> CreateUpdateUserAsync()
        {
            try
            {
                string dropProcedure = @"
                    IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.EditUser') AND type IN (N'P', N'PC'))
                        DROP PROCEDURE dbo.EditUser;
                ";
                await _conn.ExecuteNonQueryAsync(dropProcedure);

                string createUpdateProcedure = @"
                    CREATE PROCEDURE EditUser
                        @UserId INT,
                        @UserName NVARCHAR(100),
                        @Email NVARCHAR(255),
                        @Password NVARCHAR(255)
                    AS
                    BEGIN
                        UPDATE Users
                        SET UserName = @UserName,
                            Email = @Email,
                            Password = @Password,
                            UpdatedAt = GETDATE()
                        WHERE UserId = @UserId;
                    END;
                ";

                await _conn.ExecuteNonQueryAsync(createUpdateProcedure);
                MessageBox.Show("Stored procedure EditUser đã được tạo thành công!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo stored procedure EditUser: {ex.Message}",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
        }

        // Method to create the procedure to delete a user
        public async Task<bool> CreateDeleteUserAsync()
        {
            try
            {
                string dropProcedure = @"
                    IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.DeleteUser') AND type IN (N'P', N'PC'))
                        DROP PROCEDURE dbo.DeleteUser;
                ";
                await _conn.ExecuteNonQueryAsync(dropProcedure);

                string createDeleteProcedure = @"
                    CREATE PROCEDURE DeleteUser
                        @UserId INT
                    AS
                    BEGIN
                        DELETE FROM Users
                        WHERE UserId = @UserId;
                    END;
                ";

                await _conn.ExecuteNonQueryAsync(createDeleteProcedure);
                MessageBox.Show("Stored procedure DeleteUser đã được tạo thành công!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo stored procedure DeleteUser: {ex.Message}",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
