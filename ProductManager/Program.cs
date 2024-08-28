using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetEnv;
using ProductManager.Helper;
using System.Data.SqlClient;
using ProductManager.Model;

namespace ProductManager
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        //[STAThread]
        public static SqlConnection sqlConn;
        static async Task Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            // Tải biến môi trường từ file .env
            Env.Load();

            if (EnvFunc.ConfigFileExists())
            {
                Application.Run(new FormProductList());

                //LoadConfig();
                bool checkDbConn = await InitialDbConnect();
                if (!checkDbConn) {
                    MessageBox.Show("Please check the connection to database", "Error");
                } 
            }
            else
            {
                Application.Run(new FormSetup());
            }
        }


        public static async Task<bool> InitialDbConnect()
        {
            try
            {
                SqlConnect _sqlConn = new SqlConnect();
                
                    bool checkConn = await _sqlConn.OpenAndTestConnectionAsync();
                    if (checkConn) {
                        sqlConn = _sqlConn.GetConnection();
                        return true;
                    }
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        

        private static void LoadConfig()
        {
            // Lấy thông tin từ các biến môi trường
            DatabaseConfig.Server = Environment.GetEnvironmentVariable("DB_SERVER");
            DatabaseConfig.Database = Environment.GetEnvironmentVariable("DB_DATABASE");
            //DatabaseConfig.Username = Environment.GetEnvironmentVariable("DB_USERNAME");
            //DatabaseConfig.Password = Environment.GetEnvironmentVariable("DB_PASSWORD");
        }

    }


    public static class DatabaseConfig
    {
        public static string Server;
        public static string Database;
        //public static string Username;
        //public static string Password;
    }
}
