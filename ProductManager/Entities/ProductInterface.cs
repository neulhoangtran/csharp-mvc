using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManager.Model;

namespace ProductManager.Entities
{
    public interface ProductInterface
    {
        int ProductId { get; set; }
        string ProductName { get; set; }
        int Quantity { get; set; }
        decimal Price { get; set; }
        string Supplier { get; set; }

        Task<bool> Create(Product product);
        Task<bool> Update(Product product);
        Task<bool> Delete(int id);
        Task<DataTable> Fetch();

    }
}
