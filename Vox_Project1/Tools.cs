using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;


namespace Vox_Project1
{
    public class Tools
    {
       
public void Logging(string str, string sType)

        {

            try

            {

                string file = AppDomain.CurrentDomain.BaseDirectory + "/LOGS/" + sType + DateTime.Now.ToString("ddMMyy") + ".LOG";

                File.AppendAllText(file, DateTime.Now.ToLongTimeString() + "***" + str + Environment.NewLine);

            }

            catch (Exception cx)

            {

            }

        }
    }
}