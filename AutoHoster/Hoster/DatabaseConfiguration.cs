using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Management.Smo;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;

namespace AutoHoster.Hoster
{
    public class DatabaseConfiguration
    {
        public static bool CreateDB(string serverName, string dbName, string backupPath,string organisationCode)
        {
            Server server = new Server(serverName);
            try
            {
                server.ConnectionContext.LoginSecure = false;
                server.ConnectionContext.Login = ConfigurationSettings.AppSettings["DbUser"].ToString();
                server.ConnectionContext.Password = ConfigurationSettings.AppSettings["DbPassword"].ToString();
                string mdfLogicalName = ConfigurationSettings.AppSettings["MDFLogicalName"].ToString();
                string ldfLogicalName = ConfigurationSettings.AppSettings["LDFLogicalName"].ToString();
                string dbRelocationPath = ConfigurationSettings.AppSettings["DBRelocationPath"].ToString();

                Restore restore = new Restore();
                restore.Database = dbName;
                restore.Action = RestoreActionType.Database;
                restore.Devices.AddDevice(backupPath, DeviceType.File);
                restore.NoRecovery = false;
                System.Data.DataTable logicalFiles = restore.ReadFileList(server);
                if(!System.IO.Directory.Exists(dbRelocationPath+"Organisations\\Organisation"+ organisationCode))
                {
                    System.IO.Directory.CreateDirectory(dbRelocationPath + "Organisations\\Organisation" + organisationCode);
                }
                restore.RelocateFiles.Add(new RelocateFile(mdfLogicalName, dbRelocationPath + "Organisations\\Organisation" + organisationCode+"\\" + dbName + ".mdf"));
                restore.RelocateFiles.Add(new RelocateFile(ldfLogicalName, dbRelocationPath + "Organisations\\Organisation" + organisationCode + "\\"+dbName + ".ldf"));
                restore.PercentCompleteNotification = 10;
                server.ConnectionContext.Connect();
                int count = server.Databases.Count;
                restore.SqlRestore(server);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (server.ConnectionContext.IsOpen)
                {
                    server.ConnectionContext.Disconnect();
                }
            }
        }



    }
}
