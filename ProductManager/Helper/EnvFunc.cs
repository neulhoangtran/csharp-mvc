using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace ProductManager.Helper
{
    public static class EnvFunc
    {

        public static string GetRootDirectory()
        {
            // Lấy thư mục hiện tại (bin\Debug)
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Di chuyển lên hai cấp để đến thư mục gốc của dự án
            string rootDirectory = Directory.GetParent(Directory.GetParent(baseDirectory).FullName).FullName;

            return rootDirectory;
        }


        public static bool ConfigFileExists()
        {
            // Lấy đường dẫn tuyệt đối đến file .env
            string rootDirectory = GetRootDirectory();
            string fileName = ".env";
            string fullPath = Path.Combine(rootDirectory, fileName);
            //MessageBox.Show(fullPath);
            return System.IO.File.Exists(fullPath);
        }


        public static string getEnvFile()
        {
            string rootDirectory = GetRootDirectory();
            string fileName = ".env";
            return Path.Combine(rootDirectory, fileName);
        }

        public static void SaveConnectionInfoToEnv(string dbHost, string dbDatabase)
        {
            string envFilePath = getEnvFile();
            var envContents = new List<string>();

            // Đọc nội dung hiện tại của file .env nếu tồn tại
            if (File.Exists(envFilePath))
            {
                envContents = File.ReadAllLines(envFilePath).ToList();
            }

            // Kiểm tra và cập nhật hoặc thêm DB_HOST
            bool dbHostExists = false;
            for (int i = 0; i < envContents.Count; i++)
            {
                if (envContents[i].StartsWith("DB_HOST="))
                {
                    envContents[i] = $"DB_HOST={dbHost}";
                    dbHostExists = true;
                    break;
                }
            }
            if (!dbHostExists)
            {
                envContents.Add($"DB_HOST={dbHost}");
            }

            // Kiểm tra và cập nhật hoặc thêm DB_DATABASE
            bool dbDatabaseExists = false;
            for (int i = 0; i < envContents.Count; i++)
            {
                if (envContents[i].StartsWith("DB_DATABASE="))
                {
                    envContents[i] = $"DB_DATABASE={dbDatabase}";
                    dbDatabaseExists = true;
                    break;
                }
            }
            if (!dbDatabaseExists)
            {
                envContents.Add($"DB_DATABASE={dbDatabase}");
            }

            //MessageBox.Show(envFilePath);
            //MessageBox.Show(string.Join(Environment.NewLine, envContents));

            // Ghi lại nội dung vào file .env
            File.WriteAllLines(envFilePath, envContents);
        }

    }
}
