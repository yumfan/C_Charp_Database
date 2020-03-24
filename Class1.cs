using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.IO;

namespace 企业人事管理系统
{
    class Class1
    {
        public ArrayList mysql_Query(string a)
        {
            string ConnectString = "Server=localhost;User ID=root;Password=123456;Database=human_resource;";
            MySqlConnection connection = new MySqlConnection(ConnectString);
            string input = a;
            ArrayList return_value = new ArrayList();
            //string[] return_user = new string[10];
            //string[] return_password = new string[10];
            connection.Open();
            try
            {
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT " + a+ " FROM user_password";
                MySqlDataReader sdr_1 = cmd.ExecuteReader();
                //cmd.CommandText = "SELECT password FROM user_password";
                //MySqlDataReader sdr_2 = cmd.ExecuteReader();
                //int i = 0;
                while (sdr_1.Read())
                {
                    //nickname = reader.GetString("nickname");
                    //id = reader.GetInt32("id");
                    //Console.WriteLine(/*"昵称=" + nickname + */"+id=" + id);
                    //return_user[i] = sdr_1["User"].ToString();
                    return_value.Add(sdr_1[a].ToString());
                    //i++;
                }
                sdr_1.Close();
                
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
            connection.Close();
            connection.Dispose();
            return return_value;   
        }

        public void save_User(string a)
        {
            string ConnectString = "Server=localhost;User ID=root;Password=123456;Database=human_resource;";
            MySqlConnection connection = new MySqlConnection(ConnectString);
            string input = a;
            connection.Open();
            try
            {
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE human_resource.user_password SET login_user = '" + a + "' WHERE user = ('Administrator');"; 
                cmd.ExecuteNonQuery();
                //cmd.CommandText = "SELECT password FROM user_password";
                //MySqlDataReader sdr_2 = cmd.ExecuteReader();
                //int i = 0;
                //sdr_1.Close();

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
            connection.Close();
            connection.Dispose();
            //return return_value;
        }

        public DataTable ReadDataFromCsv(string file)
        {
            DataTable dt = null;

            if (File.Exists(file))
            {
                #region 如果文件存在
                dt = new DataTable();
                FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Read);

                StreamReader sr = new StreamReader(fs, Encoding.Default);

                /*string head = sr.ReadLine();
                string[] headNames = head.Split('\t');
                for (int i = 0; i < headNames.Length; i++)
                {
                  dt.Columns.Add(headNames[i], typeof(string));
                }*/
                int index = 0;
                while (!sr.EndOfStream)
                {
                    #region ==循环读取文件==
                    string lineStr = sr.ReadLine();
                    if (lineStr == null || lineStr.Length == 0)
                        continue;
                    string[] values = lineStr.Split('\t');
                    #region ==添加行数据==
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (index == 0)
                        {
                            dt.Columns.Add(values[i], typeof(string));
                        }
                        else
                        {
                            dr[i] = values[i];
                        }
                    }
                    dt.Rows.Add(dr);
                    index++;
                    #endregion
                    #endregion
                }
                fs.Close();
                sr.Close();
                #endregion
            }
            return dt;
        }
    }
}
