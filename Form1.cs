using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Collections;
using Agilent.Agilent34401.Interop;
using System.Data.OleDb;
using System.Collections.Specialized;
using System.Data.Odbc;
using System.IO;


namespace 企业人事管理系统
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            Form2 get_value = new Form2();
            Class1 a = new Class1();
            //a.mysql_connection();
            ArrayList login_user = a.mysql_Query("login_user");
            if (login_user.Contains("Administrator")==false)
            {
                数据库维护ToolStripMenuItem.Enabled = false;
            }
            
        }

        private void 系统退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确实要退出系统？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void 基本数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ConnectString = "Server=localhost;User ID=root;Password=123456;Database=human_resource;";
            MySqlConnection connection = new MySqlConnection(ConnectString);
            connection.Open();
            try
            {
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM user_password";
                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adap.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
                int count = dataGridView1.RowCount; //总行数           //得到dataGridview1的总行数
                for (int i = 0; i < count - 1; i++)
                {
                    dataGridView1.Rows[i].Cells[1].Value = "******";  //第二列password显示成****
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Clone();
                }
            }
      

        }

        private void 管理工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(管理工具ToolStripMenuItem.Enabled !=true)
            {
                MessageBox.Show("非Administrator用户登录，无权限！请使用Administrator用户重新登录", "ERROR");
            }
        }

        private void 重新登陆ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 m = new Form2();
            this.Hide();
            m.ShowDialog();
            this.Close();
         
        }

        private void 导入测试数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 基本信息管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 日常记事ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog fd = new OpenFileDialog();//首先根据打开文件对话框，选择excel表格
           //fd.Filter = "表格|*.xl1";//打开文件对话框筛选器
            string strPath;//文件完整的路径名
            StringCollection names = new StringCollection();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    strPath = fd.FileName;
                    //string strCon = "provider=microsoft.jet.oledb.4.0;data source=" + strPath + ";extended properties=excel 8.0";//关键是红色区域
                    //string strCon = "Provider=Microsoft.ACE.OLEDB.12.0;" + strPath + ";extended properties=Excel 12.0 Xml; HDR = YES";//关键是红色区域
                    String strCon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + strPath + ";" + ";Extended Properties=\"Excel 12.0;HDR=NO;IMEX=1\"";
                    //abvoe HDR=YES/NO,Yes 有列名，第一行作为列名，NO没有列名
                    //String strCon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + strPath + ";" + ";Extended Properties=\"asc,csv,txt,tab;HDR=NO;IMEX=1\"";
                    OleDbConnection Con = new OleDbConnection(strCon);//建立连接
                    Con.Open();
                    DataTable sheetNames = Con.GetOleDbSchemaTable
                    (System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    
                    foreach (DataRow dr in sheetNames.Rows)
                    {
                        names.Add(dr[2].ToString());
                    }
                    //string strSql = "select * from [Sheet1$]";//表名的写法也应注意不同，对应的excel表为sheet1，在这里要在其后加美元符号$，并用中括号
                    //string strSql = "select * from ["+names[0] +"$]";//表名的写法也应注意不同，对应的excel表为sheet1，在这里要在其后加美元符号$，并用中括号OleDbCommand Cmd = new OleDbCommand(strSql, Con);//建立要执行的命令
                    string strSql = "select * from ["+ names[0]+"]";
                    OleDbCommand Cmd = new OleDbCommand(strSql, Con);
                    OleDbDataAdapter da = new OleDbDataAdapter(Cmd);//建立数据适配器
                    DataSet ds = new DataSet();//新建数据集
                    da.Fill(ds);//把数据适配器中的数据读到数据集中的一个表中（此处表名为shyman，可以任取表名）
                                          //指定datagridview1的数据源为数据集ds的第一张表（也就是shyman表），也可以写ds.Table["shyman"]

                    dataGridView1.DataSource = ds.Tables[0];
                    Con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);//捕捉异常
                }
            }
        }

        private void 通讯录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            Class1 a = new Class1();
            OpenFileDialog fd = new OpenFileDialog();//首先根据打开文件对话框，选择excel表格
                                                     //fd.Filter = "表格|*.xl1";//打开文件对话框筛选器
            string strPath;//文件完整的路径名
            StringCollection names = new StringCollection();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    strPath = fd.FileName;
                    dt=a.ReadDataFromCsv(strPath);//把数据适配器中的数据读到数据集中的一个表中（此处表名为shyman，可以任取表名）
                                //指定datagridview1的数据源为数据集ds的第一张表（也就是shyman表），也可以写ds.Table["shyman"]

                    dataGridView1.DataSource = dt;
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);//捕捉异常
                }
            }
        }
    }
}
