using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Model
{

    public class ProductList
    {
        public readonly SqlConnect _sqlConnect;
        public ProductList() {
            _sqlConnect = new SqlConnect();
        }

        public async Task<DataTable> FetchProduct () {
            return await _sqlConnect.FetchAsync("product");
        }
    }
}
