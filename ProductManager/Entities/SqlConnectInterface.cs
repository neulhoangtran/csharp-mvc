using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Entities
{
    public interface SqlConnectInterface: IDisposable
    {
        Task<bool> OpenAndTestConnectionAsync();
        Task<bool> TableExist(string tableName);
        Task CreateTableAsync(string createTableQuery);
    }
}
