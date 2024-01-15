using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;

namespace 卓汇数据追溯系统
{
    class SQLServer
    {
        public object locker = new object();
        //private string MySqlCon = "Data Source=(local);Initial Catalog=ZHH;Integrated Security=True";
        private string MySqlCon = "Data Source=(local);Initial Catalog=ZHH;User Id=sa; Password=123456;";
        public DataTable ExecuteQuery(string sqlStr) //用于查询；其实是相当于提供一个可以传参的函数，到时候写一个sql语句，存在string里，传给这个函数，就会自动执行。
        {
            lock (locker)
            {
                SqlConnection con = new SqlConnection(MySqlCon);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlStr;
                DataTable dt = new DataTable();
                SqlDataAdapter msda;
                msda = new SqlDataAdapter(cmd);
                msda.Fill(dt);
                con.Close();
                return dt;
            }
        }
        public int ExecuteUpdate(string sqlStr)//用于增删改
        {
            lock (locker)
            {
                SqlConnection con = new SqlConnection(MySqlCon);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlStr;
                int iud = 0;
                iud = cmd.ExecuteNonQuery();

                con.Close();
                return iud;
            }
        }


    }
}
