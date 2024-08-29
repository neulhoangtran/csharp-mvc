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
    public partial class FormDeleteProduct : Form
    {
        int _productID;
        public FormDeleteProduct(int productId)
        {
            InitializeComponent();
            _productID = productId;
            //setProductData();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private async void btnConfirm_Click(object sender, EventArgs e)
        {
            ProductController productController = new ProductController();
            await productController.RemoveProduct(_productID);
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
