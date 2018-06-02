using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Xml;

namespace AutoHoster.Hoster
{
    public class Organisation
    {
        public int OrganisationId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int MyProperty { get; set; }
        public string CityOrRegion { get; set; }
        public string OfficeNo { get; set; }
        public string MobileNumber { get; set; }
        public Organisation()
        {

        }
        public Organisation(string Code)
        {
            this.Code = Code;
        }

        public string CreateOrganisation()
        {
            string resourcesFromAPP = ConfigurationSettings.AppSettings["ResourcesFromAPP"].ToString();
            string resourcesFromAPI = ConfigurationSettings.AppSettings["ResourcesFromAPI"].ToString();
            string resourcesTo = ConfigurationSettings.AppSettings["ResourcesTo"].ToString();
            string serverName = ConfigurationSettings.AppSettings["Server"].ToString();
            string dbBackupPath = ConfigurationSettings.AppSettings["DBBackupPath"].ToString();
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.AppendLine("Operation Log | Organisation : " + this.Code + " | UTC Date:" + DateTime.UtcNow);

                if (Host.CopyResources(resourcesFromAPP, resourcesTo + "\\Organisation" + this.Code + "\\App"))
                {
                    sb.AppendLine("Copy Resources for App: Success");
                }
                else
                {
                    sb.AppendLine("Copy Resources for App: Failed");
                }
                if (Host.AddPool("BisellsApp" + this.Code, true, Microsoft.Web.Administration.ManagedPipelineMode.Integrated, "v4.0"))
                {
                    sb.AppendLine("Adding Application pool for App: Success");
                }
                else
                {
                    sb.AppendLine("Adding Application pool for App: Failed");
                }
                if (Host.AddSite("BisellsApp" + this.Code, "bisells.com", resourcesTo + "\\Organisation" + this.Code + "\\App", "BisellsApp" + this.Code, string.Empty,"com"+this.Code))
                {
                    sb.AppendLine("Adding Application for App: Success");
                }
                else
                {
                    sb.AppendLine("Adding Application for App: Failed");
                }
                if (Host.CopyResources(resourcesFromAPI, resourcesTo + "\\Organisation" + this.Code + "\\Api"))
                {
                    sb.AppendLine("Copy Resources for Api: Success");
                }
                else
                {
                    sb.AppendLine("Copy Resources for Api: Failed");
                }
                if (Host.AddPool("BisellsApi" + this.Code, true, Microsoft.Web.Administration.ManagedPipelineMode.Integrated, "v4.0"))
                {
                    sb.AppendLine("Adding Application pool for Api: Success");
                }
                else
                {
                    sb.AppendLine("Adding Application pool for Api: Failed");
                }
                if (Host.AddSite("BisellsApi" + this.Code, "bisells.com", resourcesTo + "\\Organisation" + this.Code + "\\Api", "BisellsApi" + this.Code, string.Empty,"api"+this.Code))
                {
                    sb.AppendLine("Adding Application for Api: Success");
                }
                else
                {
                    sb.AppendLine("Adding Application for Api: Failed");
                }
                if (this.AlterConnectionString())
                {
                    sb.AppendLine("web Configuration: Success");
                }
                else
                {
                    sb.AppendLine("web Configuration: Failed");
                }
                if (Host.RegisterSubDomain("com"+this.Code.ToString()))
                {
                    sb.AppendLine("Creating Sub Domain for App: Success");
                }
                else
                {
                    sb.AppendLine("Creating Sub Domain for App: Failed");
                }
                if (Host.RegisterSubDomain("api"+this.Code.ToString()))
                {
                    sb.AppendLine("Creating Sub Domain for Api: Success");
                }
                else
                {
                    sb.AppendLine("Creating Sub Domain for Api: Failed");
                }
                if (DatabaseConfiguration.CreateDB(serverName, "Bisells" + this.Code, dbBackupPath,this.Code))
                {
                    sb.AppendLine("Creating Database : Success");
                }
                else
                {
                    sb.AppendLine("Creating Database : Failed");
                }
                                
                sb.AppendLine("----------------------------------------------------");
                if (!File.Exists(@"c:\users\public\OrganisationHost.log"))
                {
                    File.Create(@"c:\users\public\OrganisationHost.log");
                    File.WriteAllText(@"c:\users\public\OrganisationHost.log", sb.ToString(), Encoding.UTF8);
                }
                else
                {
                    File.WriteAllText(@"c:\users\public\OrganisationHost.log", File.ReadAllText(@"c:\users\public\OrganisationHost.log") + sb.ToString(), Encoding.UTF8);
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                if (!File.Exists(@"c:\users\public\OrganisationHost.log"))
                {
                    File.Create(@"c:\users\public\OrganisationHost.log");
                    File.WriteAllText(@"c:\users\public\OrganisationHost.log", sb.ToString(), Encoding.UTF8);
                }
                else
                {
                    File.WriteAllText(@"c:\users\public\OrganisationHost.log", File.ReadAllText(@"c:\users\public\OrganisationHost.log") + sb.ToString(), Encoding.UTF8);
                }
                return sb.ToString();
            }
            finally
            {

            }
        }

        private bool AlterConnectionString()
        {
            try
            {
                string configPathApp = ConfigurationSettings.AppSettings["ResourcesTo"].ToString() + "\\Organisation" + this.Code + "\\App\\web.config";
                string configPathApi = ConfigurationSettings.AppSettings["ResourcesTo"].ToString() + "\\Organisation" + this.Code + "\\Api\\web.config";
                string newConString = ConfigurationSettings.AppSettings["NewConnectionString"].ToString()+ "database=Bisells"+this.Code+";";

                XmlDocument webconfig = new XmlDocument();
                webconfig.Load(configPathApp);
                webconfig.SelectSingleNode("configuration/connectionStrings/add[@name = \"Connection\"]").Attributes["connectionString"].Value = newConString;
                int port = Host.FindPort("BisellsApi" + this.Code);
                if (port != 0)
                {
                    webconfig.SelectSingleNode("configuration/appSettings/add[@key = \"APIURL\"]").Attributes["value"].Value = "http://localhost:"+port+"/";
                }
                webconfig.Save(configPathApp);
                webconfig.Load(configPathApi);
                webconfig.SelectSingleNode("configuration/connectionStrings/add[@name = \"Connection\"]").Attributes["connectionString"].Value = newConString;
                webconfig.Save(configPathApi);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }

}
