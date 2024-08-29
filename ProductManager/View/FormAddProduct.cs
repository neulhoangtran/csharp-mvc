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

namespace ProductManager.View
{
    public partial class FormAddProduct : Form
    {
        public FormAddProduct()
        {
            InitializeComponent();
        }

        private async void btnCreateTable_Click(object sender, EventArgs e)
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

                ProductListController productListController = new ProductListController();
                // Nếu tất cả các kiểm tra đều hợp lệ, tiếp tục xử lý thêm sản phẩm
                await productListController.addNewProduct(productName, productQuantity, productPrice, productSupplier);
                this.Close();
                MessageBox.Show("Sản phẩm đã được thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                
            }

        }
    }
}
