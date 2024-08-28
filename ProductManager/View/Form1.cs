using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void productQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void productPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép số (0-9), dấu chấm (.), và phím Backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Chỉ cho phép một dấu chấm
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Lấy giá trị từ các TextBox
            //int productId = int.Parse(productName.Text);

            // Kiểm tra nếu bất kỳ TextBox nào để trống
            if (string.IsNullOrWhiteSpace(productName.Text) ||
                string.IsNullOrWhiteSpace(productQty.Text) ||
                string.IsNullOrWhiteSpace(productPrice.Text) ||
                string.IsNullOrWhiteSpace(productSupplier.Text))
            {
                // Hiển thị thông báo lỗi nếu có TextBox nào trống
                MessageBox.Show("Vui lòng nhập đủ thông tin.", "Thiếu thông tin bắt buộc", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Dừng thực hiện tiếp nếu có lỗi
            }

            string _productName = string.Empty;
            int quantity = 0;
            decimal price = 0;
            string supplier = string.Empty;


             _productName = productName.Text;
             quantity = int.Parse(productQty.Text);
             price = decimal.Parse(productPrice.Text);
             supplier = productSupplier.Text;




            string inforText = $"Product Name: {_productName}\n" +
                       $"Quantity: {quantity}\n" +
                       $"Price: {price}\n" +
                       $"Supplier: {supplier}";

            // Gán nội dung vào RichTe
            infor.Text = inforText;
        }

        private void productName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
