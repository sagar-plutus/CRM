using Microsoft.Extensions.Logging;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.IoT;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using PurchaseTrackerAPI.Models;

namespace PurchaseTrackerAPI.DAL
{
    public class CommonDAO : Icommondao
    {
        private readonly IConnectionString _iConnectionString;
        private readonly IModbusRefConfig _iModbusRefConfig;
        private readonly Idimensiondao _Idimensiondao;
        static readonly object uniqueModBusRefIdLock = new object();

        public CommonDAO(IConnectionString iConnectionString, IModbusRefConfig iModbusRefConfig, Idimensiondao idimensiondao)
        {
            _iConnectionString = iConnectionString;
            _iModbusRefConfig = iModbusRefConfig;
            _Idimensiondao = idimensiondao;
        }
        public System.DateTime SelectServerDateTime()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String sqlQuery = Startup.SERVER_DATETIME_QUERY_STRING;
                /*To get Server Date Time for Local DB*/
                //String sqlQuery = "SELECT CURRENT_TIMESTAMP AS ServerDate";
                //*To get Server Date Time for Azure Server DB*/
                //String sqlQuery = " declare @Dfecha as datetime " +
                //            " DECLARE @D AS datetimeoffset " +
                //            " set @Dfecha= SYSDATETIME()   " +
                //            " SET @D = CONVERT(datetimeoffset, @Dfecha) AT TIME ZONE 'India Standard Time'" +
                //            " select CONVERT(datetime, @D)";

                cmdSelect = new SqlCommand(sqlQuery, conn);

                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                while (dateReader.Read())
                {
                    if (TimeZoneInfo.Local.Id != "India Standard Time")
                    {
                        DateTime d = Convert.ToDateTime(dateReader[0]).ToLocalTime();
                        d = d.AddMilliseconds(-d.Millisecond);
                        return d;
                    }
                    else
                    {
                        DateTime d = Convert.ToDateTime(dateReader[0]);
                        d = d.AddMilliseconds(-d.Millisecond);
                        return d;
                    }
                }
                DateTime dt = new DateTime();
                dt = dt.AddMilliseconds(-dt.Millisecond);
                return dt;
            }
            catch (Exception ex)
            {
                DateTime dt = new DateTime();
                dt = dt.AddMilliseconds(-dt.Millisecond);
                return dt;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }

        }


        public DateTime ServerDateTime
        {
            get
            {
                return SelectServerDateTime();
            }
        }
        public void SetDateStandards(Object classTO, Double timeOffsetMins)
        {

            Type type = classTO.GetType();
            PropertyInfo[] propertyInfoArray = type.GetProperties();

            for (int j = 0; j < propertyInfoArray.Length; j++)
            {
                PropertyInfo propInfo = propertyInfoArray[j];
                if (propInfo.PropertyType == typeof(DateTime))
                {
                    DateTime columnValue = Convert.ToDateTime(propInfo.GetValue(classTO, null));
                    columnValue = columnValue.AddMinutes(timeOffsetMins);
                    propInfo.SetValue(classTO, columnValue);
                }
            }

        }

        #region GetNextAvailableModRefIdNew
        //@KKM Added for IOT
        public int GetNextAvailableModRefIdNew()
        {
            lock (uniqueModBusRefIdLock)
            { 
            int modRefNumber = 0;
            List<int> list = _Idimensiondao.GeModRefMaxData();
            if (list != null && list.Count > 0)
            {
                int maxNumber = 1;
                modRefNumber = GetAvailNumber(list, maxNumber);
            }
            else
            {
                modRefNumber = 1;
            }
            bool isInList = list.Contains(modRefNumber);
            if (isInList)
                return 0;
            else
                list.Add(modRefNumber);
            // Random num = new Random();
            // modRefNumber= num.Next(101, 255);
            return modRefNumber;

        }
        }


        public int GetAvailNumber(List<int> list, int maxNumber)
        {
            if (list.Contains(maxNumber))
            {
                if (maxNumber > 255)
                {
                    return 0;
                }
                maxNumber++;
                return GetAvailNumber(list, maxNumber);
            }
            else
            {
                return maxNumber;
            }
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        

        #endregion
    }
}
