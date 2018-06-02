using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoHoster.Hoster;
using System.IO;
using System.Xml;

namespace AutoHoster
{
    class Program
    {
        static void Main(string[] args)
        {
            #region test
            //Host.CopyResources(@"D:\Builds\BisellsWebApp", @"D:\BisellsERPProduct\Company 0001");

            ////creating database
            //DatabaseConfiguration.CreateDB(@"maclinkserver\mssql_dev", "testDBForbisells", @"E:\Development\Share\nithin\testbackup.bak");
            ////XmlDocument xml = new XmlDocument();
            ////xml.Load("D:\\conn.config");
            ////XmlNode Connection = xml.SelectSingleNode("Connections / Connection[@Name = \"BasicConnection\"]");
            ////string con = Connection.InnerText;
            //try
            //{
            //    Console.WriteLine("Do you want to create an Application Pool:y/n");
            //    string response = Console.ReadLine();
            //    if (response.ToString() == "y")
            //    {
            //        Console.Write("Please enter Application Pool Name:");
            //        string poolname = Console.ReadLine();
            //        bool isEnable32bit = false;
            //        ManagedPipelineMode mode = ManagedPipelineMode.Classic;
            //        Console.Write("Need to enable 32 bit on Windows 64 bit?y/n [Applicable for 64 bit OS]: y/n?");
            //        string enable32bit = Console.ReadLine();
            //        if (enable32bit.ToLower() == "y")
            //        {
            //            isEnable32bit = true;
            //        }
            //        Console.Write("Please select Pipeline Mode: 1 for Classic, 2 for Integrated:");
            //        string pipelinemode = Console.ReadLine();
            //        if (pipelinemode.ToLower() == "2")
            //        {
            //            mode = ManagedPipelineMode.Integrated;
            //        }
            //        Console.Write("Please select Runtime Version for Application Pool: 1 for v2.0, 2 for v4.0:");
            //        string runtimeVersion = Console.ReadLine() == "1" ? "v2.0" : "v4.0";

            //        Host.AddPool(poolname, isEnable32bit, mode, runtimeVersion);
            //        Console.WriteLine("Application Pool created successfully...");
            //    }
            //    Console.WriteLine("Do you want to create a website:y/n");
            //    response = Console.ReadLine();
            //    if (response.ToString() == "y")
            //    {
            //        Console.Write("Please enter website name:");
            //        string websiteName = Console.ReadLine();
            //        Console.Write("Please enter host name:");
            //        string hostname = Console.ReadLine();
            //        Console.Write("Please enter physical path to point for website:");
            //        string phypath = Console.ReadLine();
            //        Console.WriteLine("Please enter Application pool Name:");
            //        foreach (var pool in new ServerManager().ApplicationPools)
            //        {
            //            Console.WriteLine(pool.Name);
            //        }
            //        Console.WriteLine("");
            //        Console.Write("Please enter Application pool Name for web site:");
            //        string poolName = Console.ReadLine();
            //        Host.AddSite(websiteName, hostname, phypath, poolName);
            //        Console.WriteLine("Web site created successfully...");
            //        Console.ReadLine();
            //    }


            #endregion
            
                Organisation org = new Organisation("00002");
                Console.WriteLine(org.CreateOrganisation());
                Console.ReadLine();
        }
    }
}
