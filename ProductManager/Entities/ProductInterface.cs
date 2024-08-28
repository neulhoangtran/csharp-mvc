using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Entities
{
    public interface ProductInterface
    {
        int ProductId { get; set; }
        string ProductName { get; set; }
        int Quantity { get; set; }
        decimal Price { get; set; }
        string Supplier { get; set; }
    }
}
