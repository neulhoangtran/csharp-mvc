using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using ProductManager.Model.User;

namespace ProductManager.Entities
{
    public interface UserInterface
    {
        int Id { get; set; }
        string UserName { get; set; }
        string Email { get; set; }
        string Password { get; set; }


        Task<bool> Create();
        Task<bool> Update();
        Task<bool> Delete(int id);
        Task<DataTable> Fetch();
    }
}
