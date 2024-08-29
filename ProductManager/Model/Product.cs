using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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


        public async Task CreateNewProduct(Product product)
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
                    MessageBox.Show("Thêm sản phẩm thành công");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi");
            }
        }
    }
}
