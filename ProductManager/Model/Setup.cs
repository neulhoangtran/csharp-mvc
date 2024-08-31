using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProductManager.Model.SetupDb;

namespace ProductManager.Model
{
    public class Setup : IDisposable
    {
        private SqlConnect _conn;

        // Constructor
        public Setup(string dbHost, string dbDatabase)
        {
            _conn = new SqlConnect(dbHost, dbDatabase);
        }

        // Method to test the database connection
        public async Task<bool> TestConnectionAsync()
        {
            return await _conn.OpenConnectionAsync();
        }

        // Method to create required tables and procedures
        public async Task<bool> CreateRequireTablesAsync()
        {
            CreateProductTable setupProduct = new CreateProductTable(_conn);
            CreateUserTable setupUser = new CreateUserTable(_conn);
            
            
            bool productTb = await setupProduct.SetupTable();
            if (!productTb) {
                MessageBox.Show("Lỗi khi tạo bảng product");
                return false;
            }

            bool userTb = await setupUser.SetupTable();
            if (!userTb)
            {
                MessageBox.Show("Lỗi khi tạo bảng User");
                return false;
            }

            return true;
        }

        // Method to create the Product table
        
        // Method to check if a table exists
        public async Task<bool> TableExist(string tableName)
        {
            bool check = await _conn.TableExist(tableName);
            return check;
        }

        // Dispose method to release resources
        public void Dispose()
        {
            if (_conn != null)
            {
                _conn.Dispose();
                _conn = null;
            }
        }
    }
}
