using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProductManager.Entities;

namespace ProductManager.Model.SetupDb
{
    public class CreateProductTable: DbInterface
    {
        public SqlConnect _conn;
        public CreateProductTable(SqlConnect sqlConnect) {
            _conn = sqlConnect;
        }

        public async Task<bool> SetupTable()
        {
            bool tableCreated = await CreateProductTableAsync();
            if (tableCreated)
            {
                bool procedureInsertCreated = await CreateInsertProcedureAsync();
                bool procedureUpdateCreated = await CreateUpdateProcedureAsync();
                bool procedureDeleteCreated = await CreateDeleteProcedureAsync();

                return procedureInsertCreated && procedureUpdateCreated && procedureDeleteCreated;
            }
            return false;
        }

        public async Task<bool> CreateProductTableAsync()
        {
            bool checkProductTable = await _conn.TableExist("product");
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
                    );
                ";

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
                        DROP PROCEDURE dbo.CreateProduct;
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
    }
}
