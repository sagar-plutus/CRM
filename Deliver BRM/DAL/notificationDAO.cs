using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SalesTrackerAPI.StaticStuff;
using System.Data;

namespace SalesTrackerAPI.DAL
{
    public class notificationDAO
    {

        public static NotificationTO SelectNotificationTO(Int32 idAlertInstance )
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            try
            {
                
                conn.Open();
                cmdSelect.CommandText = "select tblAlertDefinition.navigationUrl,module.androidUrl from tblAlertInstance " +
                    "left join tblAlertDefinition on tblAlertInstance.alertDefinitionId = tblAlertDefinition.idAlertDef " +
                    "left join tblModule module on module.idModule = tblAlertDefinition.moduleId where idAlertInstance = @alertInstaceId";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@alertInstaceId", System.Data.SqlDbType.Int).Value = idAlertInstance;
                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                NotificationTO list = ConvertDTToList(rdr);
                rdr.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static NotificationTO ConvertDTToList(SqlDataReader NotificationDT)
        {
            List<NotificationTO> NotificationList = new List<NotificationTO>();
            if (NotificationDT != null)
            {
                while (NotificationDT.Read())
                {
                    NotificationTO NotificationTO = new NotificationTO();
                    if (NotificationDT["navigationUrl"] != DBNull.Value)
                        NotificationTO.NavigationUrl = Convert.ToString(NotificationDT["navigationUrl"].ToString());
                    if (NotificationDT["androidUrl"] != DBNull.Value)
                        NotificationTO.AndroidUrl = Convert.ToString(NotificationDT["androidUrl"].ToString());
                    NotificationList.Add(NotificationTO);
                }
            }
            return NotificationList[0];
        }
    }
}
