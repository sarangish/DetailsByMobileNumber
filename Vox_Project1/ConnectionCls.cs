using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Vox_Project1
{
    public class Connectioncls
    {
        SqlConnection con;
        SqlCommand cmd;
        public Connectioncls()
        {
            con = new SqlConnection("Server=192.168.2.16;Database=IrisEngage_Live;User Id=sa;Password=v0x123#;MultipleActiveResultSets=true");
        }

        public int NonQueryFn(string sqlquery) //insert, delete, update
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            cmd = new SqlCommand(sqlquery, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return i;
        }

        public string ScalarFn(string sqlquery) //sum, avg, count
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            cmd = new SqlCommand(sqlquery, con);
            con.Open();
            string i = cmd.ExecuteScalar().ToString();
            con.Close();

            return i;
        }
        public SqlDataReader ReaderFn(string sqlquery) //select
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            cmd = new SqlCommand(sqlquery, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            return dr;
        }
        public DataSet DataAdapterFn(string sqlquery)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            SqlDataAdapter da = new SqlDataAdapter(sqlquery, con);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
        }
        public DataTable DatatableFn(string sqlquery)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            SqlDataAdapter da = new SqlDataAdapter(sqlquery, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt;
        }

        
    }
}

//    nbnbv