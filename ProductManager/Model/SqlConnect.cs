using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManager.Entities;
using DotNetEnv;
using System.Windows.Forms;
using System.Data;
using System.Data.Common;

namespace ProductManager.Model
{
    public class SqlConnect : SqlConnectInterface
    {
        private readonly string sqlStr;
        private SqlConnection conn;
        private bool testConn;

        public SqlConnect(string host = null, string database = null)
        {
            try
            {
                // load env
                Env.Load();

                // check host and database , if not as a param will get from env
                string dbHost = !string.IsNullOrEmpty(host) ? host : Env.GetString("DB_HOST");
                string dbDatabase = !string.IsNullOrEmpty(database) ? database : Env.GetString("DB_DATABASE");

                // check empty
                if (string.IsNullOrEmpty(dbHost) || string.IsNullOrEmpty(dbDatabase))
                {
                    MessageBox.Show("Missing HOST or Database Name");
                    Environment.Exit(1);
                }

                string initialConnectStr = $"Data Source= {dbHost}; Initial Catalog={dbDatabase}; Integrated Security = true";
                conn = new SqlConnection(initialConnectStr);

            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Can't connect to database");
                Environment.Exit(1);
            }
            
        }

        public SqlConnection GetConnection()
        {
            return conn;
        }


        public async Task<bool> OpenAndTestConnectionAsync()
        {
            try
            {
                await conn.OpenAsync();
                testConn = conn.State  == ConnectionState.Open;
                return testConn;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> TableExist(string tablename)
        {
            if (!testConn)
            {
                MessageBox.Show("Database connection is not open.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            try
            {
                string query = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @tablename";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TableName", tablename);
                    int count = (int)await cmd.ExecuteScalarAsync();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task CreateTableAsync(string createTableQuery)
        {

            try
            {
                using (SqlCommand cmd = new SqlCommand(createTableQuery, conn))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating table: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public async Task<DataTable> FetchAsync (string tablename)
        {

            // check connection
            if (!testConn)
            {
                //return null;
                bool connected = await OpenAndTestConnectionAsync();
                if (!connected)
                {
                    //MessageBox.Show("Failed to open database connection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            // check table exist
            bool tableEx = await TableExist(tablename);
            if (!tableEx)
            {
                return null;
            }

            try
            {
                string queryStr = $"Select * from {tablename}";
                //MessageBox.Show(queryStr);
                using (SqlCommand cmd = new SqlCommand(queryStr, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    //int recordCount = table       ;
                    //MessageBox.Show(recordCount);
                    //MessageBox.Show($"Số lượng bản ghi: {recordCount}", "Kết quả truy vấn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"error: {ex.Message}", "Kết quả truy vấn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null ;
            }
        }
        public void Dispose()
        {
            if (conn != null && conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Dispose();
        }
    }
}
