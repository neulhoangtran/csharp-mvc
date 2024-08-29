using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProductManager.Model;

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

        // Method to test the database connection
        public async Task<bool> TestConnectionAsync()
        {
            return await _conn.OpenAndTestConnectionAsync();
        }

        // Method to create required tables and procedures
        public async Task<bool> CreateRequireTablesAsync()
        {
            bool tableCreated = await CreateProductTableAsync();
            if (tableCreated)
            {
                bool procedure1Created = await CreateInsertProcedureAsync();
                bool procedure2Created = await CreateUpdateProcedureAsync();
                bool procedure3Created = await CreateDeleteProcedureAsync();

                return procedure1Created && procedure2Created && procedure3Created;
            }
            return false;
        }

        // Method to create the Product table
        public async Task<bool> CreateProductTableAsync()
        {
            bool checkProductTable = await TableExist("product");
            if (checkProductTable)
            {
                return true;
            }
            try
            {
                // SQL command to create the product table
                string createTableQuery = @"
                    CREATE TABLE product (
                        ProductId INT PRIMARY KEY IDENTITY(1,1),
                        ProductName NVARCHAR(100),
                        Quantity INT,
                        Price DECIMAL(18,2),
                        Supplier NVARCHAR(100)
                    );
                ";

                // Execute the SQL command to create the table
                await _conn.CreateTableAsync(createTableQuery);

                MessageBox.Show("Bảng Product đã được tạo thành công!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo bảng Product: {ex.Message}",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
        }

        // Method to create the procedure to insert a new product
        public async Task<bool> CreateInsertProcedureAsync()
        {
            try
            {
                string dropProcedure = @"
                        IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.CreateProduct') AND type IN (N'P', N'PC'))
                           DROP PROCEDURE dbo.CreateProduct
                    ";
                await _conn.ExecuteNonQueryAsync(dropProcedure);
                string createInsertProcedure = @"
                    CREATE PROCEDURE CreateProduct
                        @ProductName NVARCHAR(100),
                        @Quantity INT,
                        @Price DECIMAL(18,2),
                        @Supplier NVARCHAR(100)
                    AS
                    BEGIN
                        INSERT INTO product (ProductName, Quantity, Price, Supplier)
                        VALUES (@ProductName, @Quantity, @Price, @Supplier);
                    END;
                ";

                await _conn.ExecuteNonQueryAsync(createInsertProcedure);
                MessageBox.Show("Stored procedure CreateProduct đã được tạo thành công!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo stored procedure CreateProduct: {ex.Message}",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
        }

        // Method to create the procedure to update an existing product
        public async Task<bool> CreateUpdateProcedureAsync()
        {
            try
            {
                string dropProcedure = @"
                    IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.EditProduct') AND type IN (N'P', N'PC'))
                           DROP PROCEDURE dbo.EditProduct;
                ";
                await _conn.ExecuteNonQueryAsync(dropProcedure);
                string createUpdateProcedure = @"
                    CREATE PROCEDURE EditProduct
                        @ProductId INT,
                        @ProductName NVARCHAR(100),
                        @Quantity INT,
                        @Price DECIMAL(18,2),
                        @Supplier NVARCHAR(100)
                    AS
                    BEGIN
                        UPDATE product
                        SET ProductName = @ProductName,
                            Quantity = @Quantity,
                            Price = @Price,
                            Supplier = @Supplier
                        WHERE ProductId = @ProductId;
                    END;
                ";

                await _conn.ExecuteNonQueryAsync(createUpdateProcedure);
                MessageBox.Show("Stored procedure EditProduct đã được tạo thành công!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo stored procedure EditProduct: {ex.Message}",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
        }

        // Method to create the procedure to delete a product
        public async Task<bool> CreateDeleteProcedureAsync()
        {
            try
            {
                string dropProcedure = @"
                    IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.DeleteProduct') AND type IN (N'P', N'PC'))
                           DROP PROCEDURE dbo.DeleteProduct;
                ";
                await _conn.ExecuteNonQueryAsync(dropProcedure);

                string createDeleteProcedure = @"
                    CREATE PROCEDURE DeleteProduct
                        @ProductId INT
                    AS
                    BEGIN
                        DELETE FROM product
                        WHERE ProductId = @ProductId;
                    END;
                ";

                await _conn.ExecuteNonQueryAsync(createDeleteProcedure);
                MessageBox.Show("Stored procedure DeleteProduct đã được tạo thành công!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo stored procedure DeleteProduct: {ex.Message}",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
        }

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
