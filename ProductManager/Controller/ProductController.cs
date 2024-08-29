using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProductManager.Model;

namespace ProductManager.Controller
{
    public class ProductController
    {
        public void CreateProduct()
        {
            Product newProduct = new Product();
            newProduct.ProductName = "Product Test";
            newProduct.Quantity = 99;
            newProduct.Price = 80;
            newProduct.Supplier = "Supplier Test";

            newProduct.CreateNewProduct(newProduct);

            MessageBox.Show("Tessttttttttttttttt-----");
        }
    }
}
