using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProductManager.Model;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;


namespace ProductManager.Controller
{
    public class SetupController : IDisposable
    {
        private SqlConnect _conn;

        // Constructor
        public SetupController(string dbHost, string dbDatabase)
        {
            _conn = new SqlConnect(dbHost, dbDatabase);
        }

        // Phương thức để kiểm tra kết nối cơ sở dữ liệu
        public async Task<bool> TestConnectionAsync()
        {
            return await _conn.OpenAndTestConnectionAsync();
        }

        // Phương thức để tạo các bảng yêu cầu
        public async Task<bool> CreateRequireTablesAsync()
        {
            return await CreateProductTableAsync();
        }

        // Phương thức để tạo bảng Product
        public async Task<bool> CreateProductTableAsync()
        {
            bool checkProductTable = await TableExist("product");
            if (checkProductTable)
            {
                return true;
            }
            try
            {
                string createTableQuery = @"
                    CREATE TABLE product (
                        ProductId INT PRIMARY KEY IDENTITY(1,1),
                        ProductName NVARCHAR(100),
                        Quantity INT,
                        Price DECIMAL(18,2),
                        Supplier NVARCHAR(100)
                    )";

                await _conn.CreateTableAsync(createTableQuery);

                MessageBox.Show("Bảng Product đã được tạo thành công!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo bảng: {ex.Message}",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
        }

        public async Task<bool> TableExist(string tableName)
        {
            bool check  = await _conn.TableExist(tableName);
            return check;
        }


        // Phương thức Dispose để giải phóng tài nguyên
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
