using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManager.Model;

namespace ProductManager.Controller
{
    public class ProductListController
    
    {
        public readonly ProductList productList;
        

        public ProductListController() {
            productList = new ProductList();
        }
        public async Task<DataTable> GetAllProductsAsync()
        {
            return await productList.FetchProduct();
        }

        public async Task addNewProduct(string name , int qty, decimal price, string supplier)
        {
            Product product = new Product();
            product.ProductName = name;
            product.Quantity = qty;
            product.Price = price;
            product.Supplier = supplier;
            await product.CreateNewProduct(product);
        }
        //public async 

    }
}
