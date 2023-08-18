using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;


namespace SalesTrackerAPI.DAL
{
    public class DimStateDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimState]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<DimStateTO> SelectAllDimState()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idState", System.Data.SqlDbType.Int).Value = dimStateTO.IdState;
                SqlDataReader allstatesDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimStateTO> allStatesList = ConvertDTToList(allstatesDT);
                if (allStatesList != null)
                    return allStatesList;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllDimState");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static DimStateTO SelectDimState(Int32 idState)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idState = " + idState + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idState", System.Data.SqlDbType.Int).Value = dimStateTO.IdState;
                SqlDataReader statesDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimStateTO> statesList = ConvertDTToList(statesDT);
                if (statesList != null)
                    return statesList[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectDimState");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<DimStateTO> SelectAllDimState(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idState", System.Data.SqlDbType.Int).Value = dimStateTO.IdState;
                SqlDataReader allstatesDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimStateTO> allStatesList = ConvertDTToList(allstatesDT);
                if (allStatesList != null)
                    return allStatesList;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllDimState");
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        #endregion

        public static List<DimStateTO> ConvertDTToList(SqlDataReader statesDT)
        {
            List<DimStateTO> stateTOList = new List<DimStateTO>();
            if (stateTOList != null)
            {
                while (statesDT.Read())
                {
                    DimStateTO dimStateTONew = new DimStateTO();
                    if (statesDT["idState"] != DBNull.Value)
                        dimStateTONew.IdState = Convert.ToInt32(statesDT["idState"].ToString());
                    if (statesDT["stateCode"] != DBNull.Value)
                        dimStateTONew.StateCode = statesDT["stateCode"].ToString();
                    if (statesDT["stateName"] != DBNull.Value)
                        dimStateTONew.StateName = statesDT["stateName"].ToString();
                    if (statesDT["stateOrUTCode"] != DBNull.Value)
                        dimStateTONew.StateOrUTCode = statesDT["stateOrUTCode"].ToString();
                    stateTOList.Add(dimStateTONew);
                }
            }
            return stateTOList;
        }

        #region Insertion
        public static int InsertDimState(DimStateTO dimStateTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimStateTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public static int InsertDimState(DimStateTO dimStateTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimStateTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(DimStateTO dimStateTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimState]( " +
           // "  [idState]" +
            " [stateCode]" +
            " ,[stateName]" +
            " ,[stateOrUTCode]" +
            " )" +
" VALUES (" +
           //"  @IdState " +
            " @StateCode " +
            " ,@StateName " +
            " ,@StateOrUTCode " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdState", System.Data.SqlDbType.Int).Value = dimStateTO.IdState;
            cmdInsert.Parameters.Add("@StateCode", System.Data.SqlDbType.NVarChar).Value = dimStateTO.StateCode;
            cmdInsert.Parameters.Add("@StateName", System.Data.SqlDbType.NVarChar).Value = dimStateTO.StateName;
            cmdInsert.Parameters.Add("@StateOrUTCode", System.Data.SqlDbType.NVarChar).Value = dimStateTO.StateOrUTCode;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public static int UpdateDimState(DimStateTO dimStateTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimStateTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateDimState(DimStateTO dimStateTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimStateTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(DimStateTO dimStateTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimState] SET " +
            //"  [idState] = @IdState" +
            " [stateCode]= @StateCode" +
            " ,[stateName]= @StateName" +
            " ,[stateOrUTCode] = @StateOrUTCode" +
            " WHERE [idState] =  @IdState ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdState", System.Data.SqlDbType.Int).Value = dimStateTO.IdState;
            cmdUpdate.Parameters.Add("@StateCode", System.Data.SqlDbType.NVarChar).Value = dimStateTO.StateCode;
            cmdUpdate.Parameters.Add("@StateName", System.Data.SqlDbType.NVarChar).Value = dimStateTO.StateName;
            cmdUpdate.Parameters.Add("@StateOrUTCode", System.Data.SqlDbType.NVarChar).Value = dimStateTO.StateOrUTCode;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteDimState(Int32 idState)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idState, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteDimState(Int32 idState, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idState, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteDeletionCommand(Int32 idState, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimState] " +
            " WHERE idState = " + idState + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idState", System.Data.SqlDbType.Int).Value = dimStateTO.IdState;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
    }
}
