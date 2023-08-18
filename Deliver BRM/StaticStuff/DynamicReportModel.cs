using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.StaticStuff
{
    public class DynamicReportModel
    {
        //private static dynamic ToExpando(IDataRecord record)
        //{
        //    var expandoObject = new ExpandoObject() as IDictionary<string, object>;

        //    for (var i = 0; i < record.FieldCount; i++)
        //    {
        //        string coloumnName = record.GetName(i);
        //        object value = record[i];
        //        expandoObject.Add(record.GetName(i), record[i]);

        //    }

        //    return expandoObject;
        //}

        private static List<DynamicReportTO> SqlDataReaderToExpando(SqlDataReader reader)
        {
            List<DynamicReportTO> list = new List<DynamicReportTO>();

            while (reader.Read())
            {
                DynamicReportTO dynamicReportTO = new DynamicReportTO();
                List<DropDownTO> dropDownList = new List<DropDownTO>();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = reader.GetName(i);
                    dropDownTO.Tag = reader[i];
                    dropDownList.Add(dropDownTO);
                }
                dynamicReportTO.DropDownList = dropDownList;
                list.Add(dynamicReportTO);
            }
            return list;
        }

        public static IEnumerable<dynamic> GetDynamicSqlData(string connectionstring, string sql, params SqlParameter[] commandParmeter)
        {
            using (var conn = new SqlConnection(connectionstring))
            {
                using (var comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    if (commandParmeter != null)
                    {
                        foreach (SqlParameter parm in commandParmeter)
                        {
                            if (parm.Value == null)
                            {
                                parm.Value = DBNull.Value;
                            }
                            comm.Parameters.Add(parm);
                        }
                    }
                    using (var reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return SqlDataReaderToExpandoV1(reader);
                        }
                    }
                    conn.Close();
                }
            }
        }

        private static dynamic SqlDataReaderToExpandoV1(SqlDataReader reader)
        {
            var expandoObject = new ExpandoObject() as IDictionary<string, object>;

            for (var i = 0; i < reader.FieldCount; i++)
            {
                string name = reader.GetName(i);
                name = name.Replace('_', ' ');
                expandoObject.Add(name, reader[i]);

            }

            return expandoObject;
        }

    }
}
