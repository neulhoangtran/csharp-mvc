using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using ProductManager.Controller;
using ProductManager.Helper;

namespace ProductManager
{
    public partial class FormSetup : Form
    {
        public FormSetup()
        {
            InitializeComponent();
            
        }


        private async void btnTestConnection_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtServer.Text) || string.IsNullOrEmpty(txtDatabase.Text))
            {
                MessageBox.Show("Missing Host or Database name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                using (SetupController setup = new SetupController(txtServer.Text, txtDatabase.Text)) { 
                    bool isConnected = await setup.TestConnectionAsync();
                    if (isConnected)
                    {
                        MessageBox.Show("Kết nối thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Kết nối thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private async void btnCreateTable_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtServer.Text) || string.IsNullOrEmpty(txtDatabase.Text))
            {
                MessageBox.Show("Missing Host or Database name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                using (SetupController setup = new SetupController(txtServer.Text, txtDatabase.Text))
                {
                    bool isConnected = await setup.TestConnectionAsync();
                    if (isConnected)
                    {
                        bool check = await setup.CreateRequireTablesAsync();
                        if (check) {
                            EnvFunc.SaveConnectionInfoToEnv(txtServer.Text, txtDatabase.Text);
                            this.Hide();

                            // Mở form mới (FormProductList)
                            FormProductList productListForm = new FormProductList();
                            productListForm.Show();
                            this.Close();
                        } 
                    }
                    else
                    {
                        MessageBox.Show("Kết nối thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
