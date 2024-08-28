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

        //public async 

    }
}
