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

namespace ProductManager
{
    public partial class FormProductList : Form
    {

        public ProductListController productListController;
        public FormProductList()
        {
            InitializeComponent();
            productListController = new ProductListController();  
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }



        private async void FormProductList_Load(object sender, EventArgs e)
        {
            
            DataTable products = await productListController.GetAllProductsAsync();
            productListView.DataSource = products;
            // In dữ liệu DataTable lên console để debug
            

        }
    }
}
