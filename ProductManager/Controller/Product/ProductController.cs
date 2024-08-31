using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProductManager.Model;

namespace ProductManager.Controller
{
    public class ProductController
    {
        //public void CreateProduct()
        //{
        //    Product newProduct = new Product();
        //    newProduct.ProductName = "Product Test";
        //    newProduct.Quantity = 99;
        //    newProduct.Price = 80;
        //    newProduct.Supplier = "Supplier Test";

        //    newProduct.CreateNewProduct(newProduct);

        //    //MessageBox.Show("Tessttttttttttttttt-----");
        //}

        public Product product;

        public ProductController() {
            product = new Product();
        }  

        public async Task<DataTable> GetAllProductsAsync()
        {
            return await product.FetchProduct();
        }

        public async Task addNewProduct(string name, int qty, decimal price, string supplier)
        {
            
            product.ProductName = name;
            product.Quantity = qty;
            product.Price = price;
            product.Supplier = supplier;
            await product.Create(product);
        }
        
        public async Task EditProduct(int productId, string name, int qty, decimal price, string supplier)
        {
         
            product.ProductName = name;
            product.ProductId = productId;
            product.Quantity = qty;
            product.Price = price;
            product.Supplier = supplier;
            await product.Update(product);
        }

        public async Task RemoveProduct(int productId)
        {
            await product.Delete(productId);
        }

        public async Task<Product> GetProductById(int productId) {
            Product product = new Product();
            Product currentProduct = await product.GetProductById(productId);
            return currentProduct;
        }
    }
}
