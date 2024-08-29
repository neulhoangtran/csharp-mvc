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
using ProductManager.Helper;
using System.Diagnostics;
using ProductManager.Model;
using ProductManager.View;

namespace ProductManager
{
    public partial class FormProductList : Form
    {

        public ProductListController productListController;
        public ProductController productController;
        public FormProductList()
        {
            InitializeComponent();
            productListController = new ProductListController();
            productController = new ProductController();
        }

        private async void FormProductList_Load(object sender, EventArgs e)
        {
            await LoadProductDataAsync();
        }

        private async Task LoadProductDataAsync()
        {
            DataTable products = await productListController.GetAllProductsAsync();
            productListView.DataSource = products;
        }
        private async void btnAddProduct_Click(object sender, EventArgs e)
        {
            //productController.CreateProduct();
            FormAddProduct formAddProduct = new FormAddProduct();
            formAddProduct.ShowDialog();
            //await LoadProductDataAsync();
        }

        private async void FormProductList_Activated(object sender, EventArgs e)
        {
            await LoadProductDataAsync();
        }
    }
}
