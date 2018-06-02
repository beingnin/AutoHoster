using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Web.Administration;
using System.IO;
using System.Net;

namespace AutoHoster.Hoster
{
    public class Host
    {
        public static bool AddPool(string poolname, bool enable32bitOn64, ManagedPipelineMode mode, string runtimeVersion = "v4.0")
        {
            try
            {
                using (ServerManager server = new ServerManager())
                {
                    ApplicationPool pool = server.ApplicationPools.Add(poolname);
                    pool.ManagedRuntimeVersion = runtimeVersion;
                    pool.Enable32BitAppOnWin64 = enable32bitOn64;
                    pool.ManagedPipelineMode = mode;
                    server.CommitChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool CopyResources(string SourcePath, string DestinationPath)
        {
            try
            {
                if (!Directory.Exists(DestinationPath))
                {
                    foreach (string dirPath in Directory.GetDirectories(SourcePath, "*", SearchOption.AllDirectories))
                    {
                        Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));
                    }
                    foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
                    {
                        File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
                    
                
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool AddSite(string websiteName, string hostname, string phyPath, string appPool,string port,string subDomain)
        {
            try
            {
                using (ServerManager server = new ServerManager())
                {
                    string nextPort = string.IsNullOrWhiteSpace(port)? Convert.ToString(Host.NextPort()):port;

                    server.Sites.Add(websiteName, "http", "166.62.88.14" + ":80:" + subDomain+"." + hostname, phyPath);
                    server.Sites[websiteName].ApplicationDefaults.ApplicationPoolName = appPool;

                    foreach (var item in server.Sites[websiteName].Applications)
                    {
                        item.ApplicationPoolName = appPool;
                    }
                    server.CommitChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static int NextPort()
        {
            using (ServerManager server = new ServerManager())
            {
                int Max = 0;
                foreach (var site in server.Sites)
                {
                    string Binding = site.Bindings[0].BindingInformation;
                    int port = site.Bindings[0].EndPoint.Port;
                    if (Convert.ToInt32(port) > Max)
                    {
                        Max = Convert.ToInt32(port);
                    }
                }
                if (Max < 1000)
                {
                    return 1000;
                }
                return Max + 1;
            }
        }
        public static int FindPort(string website)
        {
            try
            {
            ServerManager server = new ServerManager();
            return server.Sites[website].Bindings[0].EndPoint.Port;

            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        public static bool RegisterSubDomain(string subDomain)
        {
            try
            {
                string data = "[{\"type\": \"A\",\"name\": \"" + subDomain + "\",\"data\": \"166.62.88.14\",\"ttl\": 3600}]";
                Uri uri = new Uri("https://api.godaddy.com/v1/domains/bisells.com/records");
                WebClient client = new WebClient();
                client.Headers.Add(HttpRequestHeader.Accept, "application/json");
                client.Headers.Add(HttpRequestHeader.Authorization, "sso-key dLDFGNn7Ht6i_Gc7NL7ficNQrHnhGnorNAV:UtZSwVVqaXnqbPHdKGeUWH");
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                client.UploadString(uri,"PATCH",data);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {

            }
        }
    }
}
