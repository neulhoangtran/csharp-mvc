 using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProductManager.Model;

namespace ProductManager.Controller
{
    public class SetupController
    {
        public Setup setup;

        // Constructor
        public SetupController(string dbHost, string dbDatabase)
        {
            setup = new Setup(dbHost, dbDatabase);
        }

        // Method to test the database connection
        public async Task<bool> TestConnectionAsync()
        {
            return await setup.TestConnectionAsync();
        }

        // Method to create required tables and procedures
        public async Task<bool> CreateRequireTablesAsync()
        {
            return await setup.CreateRequireTablesAsync();
        }

       

        // Method to check if a table exists
        public async Task<bool> TableExist(string tableName)
        {
            bool check = await setup.TableExist(tableName);
            return check;
        }

       
    }
}
