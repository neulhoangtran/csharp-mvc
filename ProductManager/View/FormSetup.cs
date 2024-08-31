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
                MessageBox.Show("Thiếu host hoặc database name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SetupController setup = new SetupController(txtServer.Text, txtDatabase.Text);
            bool isConnected = await setup.TestConnectionAsync();
            if (isConnected)
            {
                MessageBox.Show("Kết nối thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Kết nối thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return;
        }


        private async void btnCreateTable_Click(object sender, EventArgs e)
        {


            if (string.IsNullOrEmpty(txtServer.Text) || string.IsNullOrEmpty(txtDatabase.Text))
            {
                MessageBox.Show("Thiếu host hoặc database name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(txtUser.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập tài khoản admin!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SetupController setup = new SetupController(txtServer.Text, txtDatabase.Text);
            bool isConnected = await setup.TestConnectionAsync();

            if (!isConnected)
            {
                MessageBox.Show("Kết nối thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            bool check = await setup.CreateRequireTablesAsync();
            if (!check)
            {
                //MessageBox.Show("Kết nối thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            //

            UserController user = new UserController();
            bool isCreatedAcc = await user.AddInitialUser(txtServer.Text, txtDatabase.Text, txtUser.Text, "", txtPassword.Text);

            if (!isCreatedAcc) {
                MessageBox.Show("Không thể tạo tài khoản admin", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } 

            bool envCheck = await EnvFunc.SaveConnectionInfoToEnv(txtServer.Text, txtDatabase.Text);

            if (envCheck) {
                // Mở form mới (FormProductList)
                FormProductList productListForm = new FormProductList();
                productListForm.Show();

                //productListForm.ShowDialog();
                this.Hide();
                this.Close();
            } else
            {
                MessageBox.Show("Không thể lưu file env", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
        }
    }
}
