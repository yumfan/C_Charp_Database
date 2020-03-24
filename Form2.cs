using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace 企业人事管理系统
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            //int w = System.Windows.Forms.SystemInformation.WorkingArea.Width;
            //int h = System.Windows.Forms.SystemInformation.WorkingArea.Height;
            //this.Location = new Point(w / 2 - 250, h / 2 - 180);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Class1 a = new Class1();
            //a.mysql_connection();
            ArrayList login_user = a.mysql_Query("user");
            ArrayList login_password = a.mysql_Query("password");
            a.save_User(user.Text);

            //if (user.Text == "fan" && password.Text == "8888")//用户名和密码自行编写
            //if ((Array.IndexOf<string>(login_user, user.Text) != -1 && (Array.IndexOf<string>(login_user, password.Text) != -1)))
            if(login_user.Contains(user.Text) && login_password.Contains(password.Text))
            {
                Form1 m = new Form1();
                this.Hide();
                m.ShowDialog();
                this.Close();

            }
            else
                //MessageBox.Show("输入错误！！！");
                MessageBox.Show("密码输入错误!请重新输入密码", "ERROR");
                user.Text = password.Text = "";
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确实要退出系统？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        /// 鼠标按键事件。 
        /// 如果检查到按下的是回车键，则发一个消息，模拟键盘按以下Tab键，以使输入焦点转移到下一个文本框（或其他焦点可停留的控件） /// </summary> 
        /// <param name="sender"></param> /// <param name="e"></param> 
        private void User_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SendKeys.Send("{tab}");
            }
        }

        private void Password_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SendKeys.Send("{tab}");
            }
        }
    }
}
