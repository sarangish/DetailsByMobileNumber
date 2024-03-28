using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Configuration;
using System.IO;
using Microsoft.TeamFoundation.WorkItemTracking.Client.Wiql;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Vox_Project1
{
    public partial class Post : System.Web.UI.Page
    {
        Connectioncls con = new Connectioncls();

        Tools toLog = new Tools();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.Expires = -1;
                Response.CacheControl = "no-cache";
                Response.Clear();
                Response.ContentType = "text/xml";
                string MobileNumber;
                string SessionId;
                string Template;

                
                Template = clsRestAPI.ReadTemplate("UPDATECUSTOMER.TXT");
                MobileNumber = Request.QueryString["Mobilenumber"].ToString();
                toLog.Logging("MobileNumber" + MobileNumber, "T");
                SessionId = clsRestAPI.GetSessionId(MobileNumber);
                toLog.Logging("SessionId" + SessionId, "T");
                string DateOfBirth = clsRestAPI.GetDataValue("D00000016563", SessionId);
                toLog.Logging("DateOfBirth" + DateOfBirth, "T");

                string dobCon = DateTime.ParseExact(DateOfBirth, "dd/MM/yyyy", null).ToString("yyyy-MM-dd");
                string FullName = clsRestAPI.GetDataValue("D00000016562", SessionId);
                string Gender = clsRestAPI.GetDataValue("D00000016560", SessionId);
                string Email = clsRestAPI.GetDataValue("D00000016564", SessionId);

                Template = Template.Replace("[@NAME@]", FullName);
                Template = Template.Replace("[@DOB@]", dobCon);
                Template = Template.Replace("[@PHONE@]", MobileNumber);
                Template = Template.Replace("[@GENDER@]", Gender);
                Template = Template.Replace("[@EMAIL@]", Email);
                toLog.Logging("Request" + Template, "T");



                string response = PostRequestOdoo(ConfigurationManager.AppSettings["RESTURL"].ToString(), Template);
                string xmlresponse = CustomerRegistrationXML(response);
                Response.Write(xmlresponse);
                Response.End();
            }
            catch(Exception ex)
            {
                toLog.Logging("UpdateCustomer Page_Load" + ex.Message, "E");
            }
        }

       
        public string CustomerRegistrationXML(string response)
        {
            string Custresponse = "";
            try
            {
                StringBuilder RegistrationXml = new StringBuilder("<CustomerRegistration>");
                XmlDocument doc = (XmlDocument)Newtonsoft.Json.JsonConvert.DeserializeXmlNode(response, "Response");
                StringReader sr1 = new StringReader(doc.DocumentElement.OuterXml);
                DataSet ds1 = new DataSet();
                ds1.ReadXml(sr1);
                string StatusCode = ds1.Tables[1].Rows[0][1].ToString();
                RegistrationXml.Append("<StatusCode>");
                RegistrationXml.Append(StatusCode);
                RegistrationXml.Append("</StatusCode>");
                RegistrationXml.Append("</CustomerRegistration>");
                Custresponse = RegistrationXml.ToString();
                return Custresponse;
            }
            catch(Exception ex)
            {
                toLog.Logging(" CustomerRegistrationXML: " + ex.Message, "E");
            }
            return Custresponse;


        }
        private static string PostRequestOdoo(string url, String json)
        {
            //Tools objCommon = new Tools();
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Method = "POST";
                //httpWebRequest.Headers.Add("D360-Api-Key", ConfigurationManager.AppSettings["APIKEY"].ToString());
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                String result = "";
                try
                {
                    using (var response = httpWebRequest.GetResponse() as HttpWebResponse)
                    {
                        if (httpWebRequest.HaveResponse && response != null)
                        {
                            using (var reader = new StreamReader(response.GetResponseStream()))
                            {
                                result = reader.ReadToEnd();

                                return result;
                            }

                        }
                    }
                }
                catch (WebException e)
                {
                    if (e.Response != null)
                    {
                        using (var errorResponse = (HttpWebResponse)e.Response)
                        {
                            using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                            {
                                string error = reader.ReadToEnd();
                                result = error;
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception Ex)
            {
                clsRestAPI.logEvent("PostRequest:" + Ex.Message, "E",1);
                return "ERROR";
            }
        }



    }
}