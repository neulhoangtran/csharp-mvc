﻿using System;
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

        public ProductController productController;
        public FormProductList()
        {
            InitializeComponent();
            productController = new ProductController();
        }

        private async void FormProductList_Load(object sender, EventArgs e)
        {
            await LoadProductDataAsync();
        }

        private async Task LoadProductDataAsync()
        {
            DataTable products = await productController.GetAllProductsAsync();
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

        private void btnEditProduct_Click(object sender, EventArgs e)
        {
            int productId;
            productId = (int)productListView.CurrentRow.Cells[0].Value;
            FormEditProduct formEditProduct = new FormEditProduct(productId);
            formEditProduct.ShowDialog();

        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            int productId;
            productId = (int)productListView.CurrentRow.Cells[0].Value;
            FormDeleteProduct formDeleteProduct = new FormDeleteProduct(productId);
            formDeleteProduct.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
