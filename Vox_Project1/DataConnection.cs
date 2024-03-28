using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Vox_Project1
{
    public class DataConnection
    {
        SqlCommand cmd = new SqlCommand();

        public DataTable ExecuteAndGetData(string strCmdText)
        {
            Connectioncls con = new Connectioncls();
            
            DataTable Dt = null; SqlDataAdapter Da = null;
            try
            {
                using (SqlConnection conVOX = new SqlConnection(ConfigurationManager.ConnectionStrings["VOXCON"].ConnectionString))
                {
                    conVOX.Open();
                    cmd = new SqlCommand(strCmdText, conVOX);
                    cmd.CommandType = CommandType.Text;
                    Dt = new DataTable();
                    Da = new SqlDataAdapter(cmd);
                    Da.Fill(Dt);
                    Da.Dispose();
                }
            }
            catch (Exception ex)
            {
                clsRestAPI.logEvent("ExecuteAndGetData :-" + ex.Message, "E",1);
            }
            return Dt;
        }

        public DataTable ExecuteAndGetDataIRISBOT(string strCmdText)
        {
            
            DataTable Dt = null; SqlDataAdapter Da = null;
            try
            {
                using (SqlConnection conIRIS = new SqlConnection(ConfigurationManager.ConnectionStrings["IRIS"].ConnectionString))
                {
                    conIRIS.Open();
                    cmd = new SqlCommand(strCmdText, conIRIS);
                    cmd.CommandType = CommandType.Text;
                    Dt = new DataTable();
                    Da = new SqlDataAdapter(cmd);
                    Da.Fill(Dt);
                    Da.Dispose();
                }
            }
            catch (Exception ex)
            {
                clsRestAPI.logEvent("ExecuteAndGetDataIRISBOT :-" + ex.Message, "E",1);
            }
            return Dt;
        }
    }
}