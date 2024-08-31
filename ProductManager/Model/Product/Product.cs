using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProductManager.Entities;

namespace ProductManager.Model
{
    public class Product: ProductInterface
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Supplier { get; set; }

        public async Task<DataTable> Fetch()
        {
            return null;
        }

        public async Task<DataTable> FetchProduct()
        {
            return null;
            //return await _sqlConnect.FetchAsync("product");
        }


        public async Task<bool> Create(Product product)
        {
            try
            {
                using (SqlConnect conn = new SqlConnect())
                {
                    SqlCommand cmd = new SqlCommand("CreateProduct", conn.GetConnection());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ProductName", product.ProductName));
                    cmd.Parameters.Add(new SqlParameter("@Quantity", product.Quantity));
                    cmd.Parameters.Add(new SqlParameter("@Price", product.Price));
                    cmd.Parameters.Add(new SqlParameter("@Supplier", product.Supplier));
                    conn.GetConnection().Open();
                    cmd.ExecuteNonQuery();
                    conn.GetConnection().Close();
                    //MessageBox.Show("Thêm sản phẩm thành công");
                    return true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi");
            }
            return false;
        }
        
        public async Task<bool> Update(Product product)
        {
            try
            {
                using (SqlConnect conn = new SqlConnect())
                {
                    SqlCommand cmd = new SqlCommand("EditProduct", conn.GetConnection());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ProductId", product.ProductId));
                    cmd.Parameters.Add(new SqlParameter("@ProductName", product.ProductName));
                    cmd.Parameters.Add(new SqlParameter("@Quantity", product.Quantity));
                    cmd.Parameters.Add(new SqlParameter("@Price", product.Price));
                    cmd.Parameters.Add(new SqlParameter("@Supplier", product.Supplier));
                    conn.GetConnection().Open();
                    cmd.ExecuteNonQuery();
                    conn.GetConnection().Close();
                    return true;
                    //MessageBox.Show("Thêm sản phẩm thành công");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi {ex.Message}");
            }
            return false;
        }


        public async Task<bool> Delete(int id)
        {
            try
            {
                using (SqlConnect conn = new SqlConnect())
                {
                    SqlCommand cmd = new SqlCommand("DeleteProduct", conn.GetConnection());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ProductId", id));
                    conn.GetConnection().Open();
                    cmd.ExecuteNonQuery();
                    conn.GetConnection().Close();
                    return true;
                    //MessageBox.Show("Thêm sản phẩm thành công");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi {ex.Message}");
            }
            return false;
        }

        public async Task<Product> GetProductById(int productId)
        {
            try
            {
                using (SqlConnect conn = new SqlConnect())
                {
                    bool checkConn = await conn.OpenConnectionAsync();
                    if (checkConn)
                    {
                        string strQuery = $"SELECT * FROM product WHERE ProductId = @ProductId";
                        using (SqlCommand cmd = new SqlCommand(strQuery, conn.GetConnection()))
                        {
                            cmd.Parameters.AddWithValue("@ProductId", productId);
                            using (SqlDataReader reader = await cmd.ExecuteReaderAsync()) {
                                if (reader.Read()) {
                                    Product product = new Product
                                    {
                                        ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                        ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                        Supplier = reader.GetString(reader.GetOrdinal("Supplier"))
                                    };
                                    return product;
                                }
                            }
                        }
                    }
                   
                }
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        } 
    }
}
