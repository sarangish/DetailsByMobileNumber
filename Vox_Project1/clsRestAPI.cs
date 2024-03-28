using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net;
using System.IO;
using System.Security.Claims;
using System.IdentityModel;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
//using IMCWhatsApp.BusinessLayer;
//using IMCWhatsApp.DataAccessLayer;

namespace Vox_Project1
{

    public class clsRestAPI
    {

        Tools toLog = new Tools();


        Connectioncls con = new Connectioncls();

        public static String ReadTemplate(String filename)
        {
            try
            {
                String Template;
                System.IO.TextReader TextFileStream;
                TextFileStream = System.IO.File.OpenText(AppDomain.CurrentDomain.BaseDirectory + "RESTTEMPLATES\\" + filename);
                Template = TextFileStream.ReadToEnd();
                try
                {
                    TextFileStream.Close();
                    TextFileStream.Dispose();
                }
                catch (Exception)
                {

                    //throw;
                }

                return Template;
            }
            catch (Exception)
            {

                //throw;
            }
            return "";
        }

        

        public static bool ExecuteSQL(String SQL)
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["IRIS"].ConnectionString))
                {
                    conn.Open();
                    System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand(SQL, conn);
                    sqlComm.CommandType = CommandType.Text;
                    sqlComm.ExecuteNonQuery();
                    sqlComm.Dispose();
                    conn.Dispose();
                    return true;
                }
            }
            catch (Exception ex)
            {

                logEvent("Execute SQL:" + ex.Message, "", 0);
                return false;
            }
        }


        
       

        public static String GetDataValue(String FieldName, String SessionId,bool RawValue=false)
        {
            try
            {
                DataConnection data = new DataConnection();

                logEvent("GetDataValue:" + FieldName + ",", "", 0);
                logEvent("GetDataValue:" + SessionId + ",", "", 0);
                DataSet ds = new DataSet("DataValue");
                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["IRIS"].ConnectionString))
                {
                    string sSql = "SELECT BOTID FROM CUSTOMER_CONTEXT WITH(NOLOCK) WHERE SessionId='" + SessionId + "'";
                    DataTable Dt = data.ExecuteAndGetDataIRISBOT(sSql);

                    string botid = Dt.Rows[0]["BOTID"].ToString();

                    

                    //System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand("SELECT DataValue FROM IRIS_SessionData WHERE SessionId='" + SessionId + "' and DataId='" + FieldName + "'", conn);
                    System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand("SELECT DataValue FROM IRIS_SessionData WHERE SessionId='" + SessionId + "' and DataId='" + FieldName + "' AND BOTID='" + botid + "'", conn);
                    sqlComm.CommandType = CommandType.Text;

                    System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if(RawValue==false)
                        return ds.Tables[0].Rows[0][0].ToString().Replace("\"","");
                        else
                            return ds.Tables[0].Rows[0][0].ToString();
                    }
                    return "";
                }
            }
            catch (Exception ex)
            {

                logEvent("GetDataValue:" + FieldName + "," + SessionId + ":" + ex.Message, "", 0);
                return "ERROR";
            }
        }

        public static String GetDataValueJson(String FieldName, String SessionId)
        {
            try
            {
                DataConnection data = new DataConnection();
                logEvent("GetDataValueJson:" + FieldName + ",", "", 0);
                logEvent("GetDataValueJson:" + SessionId + ",", "", 0);
                DataSet ds = new DataSet("DataValue");
                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["IRIS"].ConnectionString))
                {
                    string sSql = "SELECT BOTID FROM CUSTOMER_CONTEXT WITH(NOLOCK) WHERE SessionId='" + SessionId + "'";
                    DataTable Dt = data.ExecuteAndGetDataIRISBOT(sSql);

                    string botid = Dt.Rows[0]["BOTID"].ToString();

                    logEvent("GetDataValueJson-botid:" + botid + ",", "", 0);


                    //System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand("SELECT DataValue FROM IRIS_SessionData WHERE SessionId='" + SessionId + "' and DataId='" + FieldName + "'", conn);
                    System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand("SELECT DataValue FROM IRIS_SessionData WHERE SessionId='" + SessionId + "' and DataId='" + FieldName + "' AND BOTID='" + botid + "'", conn);
                    sqlComm.CommandType = CommandType.Text;

                    

                    System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return ds.Tables[0].Rows[0][0].ToString();
                    }
                    return "";
                }
            }
            catch (Exception ex)
            {

                logEvent("GetDataValueJson:" + FieldName + "," + SessionId + ":" + ex.Message, "", 0);
                return "ERROR";
            }
        }



       


      

        public static String GetSessionId(String MobileNumber)
        {
            try
            {
                DataSet ds = new DataSet("DataValue");
                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["IRIS"].ConnectionString))
                {
                    System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand("SELECT top 1 SessionId FROM CUSTOMER_CONTEXT WHERE CustomerId='" + MobileNumber + "' order by MODIFIEDTIME desc", conn);
                    logEvent("GetSessionId Query:"  + ":" + sqlComm.ToString(), "", 0);
                    sqlComm.CommandType = CommandType.Text;

                    System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        logEvent("GetSessionId:" + MobileNumber + ":" + ds.Tables[0].Rows[0][0].ToString(), "", 0);
                        return ds.Tables[0].Rows[0][0].ToString();
                    }
                    return "ERROR";
                }
            }
            catch (Exception ex)
            {
                logEvent("GetSessionId:" + MobileNumber + ":" + ex.Message, "", 0);
                return "ERROR";
            }
        }


        public static void logEvent(string msg, string type, int level)
        {
            try
            {
                string strPath = AppDomain.CurrentDomain.BaseDirectory + "/LOGS/" + type + DateTime.Now.ToString("yyyyMMdd") + ".LOG";
                System.IO.File.AppendAllText(strPath, DateTime.Now.ToString("hhmmss") + "-" + msg + Environment.NewLine);
            }
            catch (Exception cx) { }
        }
        
    }
}