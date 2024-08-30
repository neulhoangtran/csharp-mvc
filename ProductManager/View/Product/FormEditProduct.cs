using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProductManager.Controller;
using ProductManager.Model;

namespace ProductManager.View
{
    public partial class FormEditProduct : Form
    {

        int _productID;
        public FormEditProduct(int productId)
        {
            InitializeComponent();
            _productID = productId;
            setProductData();
            //MessageBox.Show(productId.ToString());

        }

        public async Task setProductData()
        {
            ProductController productController = new ProductController();
            Product product = await productController.GetProductById(_productID);
            if (product != null) { 
                txtName.Text = string.IsNullOrEmpty(product.ProductName) ? "" : product.ProductName;
                txtQuantity.Text = product.Quantity == 0 ? "" : product.Quantity.ToString();
                txtPrice.Text = product.Price == 0 ? "" : product.Price.ToString();
                txtSupplier.Text = string.IsNullOrEmpty(product.Supplier) ? "" : product.Supplier;
            }
        }
        private void FormEditProduct_Load(object sender, EventArgs e)
        {

        }

        private async void btnSaveProduct_Click(object sender, EventArgs e)
        {
            // Lấy giá trị từ TextBox
            string productName = txtName.Text;
            string productQuantityText = txtQuantity.Text;
            string productPriceText = txtPrice.Text;
            string productSupplier = txtSupplier.Text;

            // Kiểm tra xem các trường thông tin có rỗng hay không
            if (string.IsNullOrEmpty(productName) ||
                string.IsNullOrEmpty(productQuantityText) ||
                string.IsNullOrEmpty(productPriceText) ||
                string.IsNullOrEmpty(productSupplier))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Chuyển đổi các giá trị từ chuỗi sang số
                int productQuantity;
                decimal productPrice;

                // Kiểm tra và chuyển đổi giá trị số lượng
                if (!int.TryParse(productQuantityText, out productQuantity))
                {
                    MessageBox.Show("Vui lòng nhập số lượng hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra và chuyển đổi giá trị giá
                if (!decimal.TryParse(productPriceText, out productPrice))
                {
                    MessageBox.Show("Vui lòng nhập giá hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ProductController productController = new ProductController();
                // Nếu tất cả các kiểm tra đều hợp lệ, tiếp tục xử lý thêm sản phẩm
                await productController.EditProduct(_productID, productName, productQuantity, productPrice, productSupplier);
                this.Close();
                MessageBox.Show("Sản phẩm đã được cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
        }
    }
}
