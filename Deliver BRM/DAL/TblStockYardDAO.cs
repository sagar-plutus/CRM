using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.DAL
{
    public class TblStockYardDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT stockYard.*,handlSys.mateHandlSysDesc FROM [tblStockYard] stockYard" +
                                  " LEFT JOIN dimMateHandlSys handlSys" +
                                  " ON stockYard.mateHandlSystemId=handlSys.idMateHandlSys";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblStockYardTO> SelectAllTblStockYard()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockYardTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblStockYardTO SelectTblStockYard(Int32 idStockYard)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idStockYard = " + idStockYard +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockYardTO> list = ConvertDTToList(reader);
                reader.Dispose();
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblStockYardTO> ConvertDTToList(SqlDataReader tblStockYardTODT)
        {
            List<TblStockYardTO> tblStockYardTOList = new List<TblStockYardTO>();
            if (tblStockYardTODT != null)
            {
               while(tblStockYardTODT.Read())
                {
                    TblStockYardTO tblStockYardTONew = new TblStockYardTO();
                    if (tblStockYardTODT["idStockYard"] != DBNull.Value)
                        tblStockYardTONew.IdStockYard = Convert.ToInt32(tblStockYardTODT["idStockYard"].ToString());
                    if (tblStockYardTODT["mateHandlSystemId"] != DBNull.Value)
                        tblStockYardTONew.MateHandlSystemId = Convert.ToInt32(tblStockYardTODT["mateHandlSystemId"].ToString());
                    if (tblStockYardTODT["createdBy"] != DBNull.Value)
                        tblStockYardTONew.CreatedBy = Convert.ToInt32(tblStockYardTODT["createdBy"].ToString());
                    if (tblStockYardTODT["createdOn"] != DBNull.Value)
                        tblStockYardTONew.CreatedOn = Convert.ToDateTime(tblStockYardTODT["createdOn"].ToString());
                    if (tblStockYardTODT["location"] != DBNull.Value)
                        tblStockYardTONew.Location = Convert.ToString(tblStockYardTODT["location"].ToString());
                    if (tblStockYardTODT["compartmentNo"] != DBNull.Value)
                        tblStockYardTONew.CompartmentNo = Convert.ToString(tblStockYardTODT["compartmentNo"].ToString());
                    if (tblStockYardTODT["compartmentSize"] != DBNull.Value)
                        tblStockYardTONew.CompartmentSize = Convert.ToString(tblStockYardTODT["compartmentSize"].ToString());
                    if (tblStockYardTODT["mateHandlSysDesc"] != DBNull.Value)
                        tblStockYardTONew.MateHandlSystem = Convert.ToString(tblStockYardTODT["mateHandlSysDesc"].ToString());
                    tblStockYardTOList.Add(tblStockYardTONew);
                }
            }
            return tblStockYardTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblStockYard(TblStockYardTO tblStockYardTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblStockYardTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public static int InsertTblStockYard(TblStockYardTO tblStockYardTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblStockYardTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblStockYardTO tblStockYardTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblStockYard]( " +
                                "  [mateHandlSystemId]" +
                                " ,[createdBy]" +
                                " ,[createdOn]" +
                                " ,[location]" +
                                " ,[compartmentNo]" +
                                " ,[compartmentSize]" +
                                " )" +
                    " VALUES (" +
                                "  @MateHandlSystemId " +
                                " ,@CreatedBy " +
                                " ,@CreatedOn " +
                                " ,@Location " +
                                " ,@CompartmentNo " +
                                " ,@CompartmentSize " +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdStockYard", System.Data.SqlDbType.Int).Value = tblStockYardTO.IdStockYard;
            cmdInsert.Parameters.Add("@MateHandlSystemId", System.Data.SqlDbType.Int).Value = tblStockYardTO.MateHandlSystemId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblStockYardTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblStockYardTO.CreatedOn;
            cmdInsert.Parameters.Add("@Location", System.Data.SqlDbType.VarChar).Value = tblStockYardTO.Location;
            cmdInsert.Parameters.Add("@CompartmentNo", System.Data.SqlDbType.VarChar).Value = tblStockYardTO.CompartmentNo;
            cmdInsert.Parameters.Add("@CompartmentSize", System.Data.SqlDbType.VarChar).Value = tblStockYardTO.CompartmentSize;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblStockYardTO.IdStockYard = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblStockYard(TblStockYardTO tblStockYardTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblStockYardTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                
                
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblStockYard(TblStockYardTO tblStockYardTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblStockYardTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                
                
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblStockYardTO tblStockYardTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblStockYard] SET " + 
            "  [idStockYard] = @IdStockYard" +
            " ,[mateHandlSystemId]= @MateHandlSystemId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[location]= @Location" +
            " ,[compartmentNo]= @CompartmentNo" +
            " ,[compartmentSize] = @CompartmentSize" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdStockYard", System.Data.SqlDbType.Int).Value = tblStockYardTO.IdStockYard;
            cmdUpdate.Parameters.Add("@MateHandlSystemId", System.Data.SqlDbType.Int).Value = tblStockYardTO.MateHandlSystemId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblStockYardTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblStockYardTO.CreatedOn;
            cmdUpdate.Parameters.Add("@Location", System.Data.SqlDbType.VarChar).Value = tblStockYardTO.Location;
            cmdUpdate.Parameters.Add("@CompartmentNo", System.Data.SqlDbType.VarChar).Value = tblStockYardTO.CompartmentNo;
            cmdUpdate.Parameters.Add("@CompartmentSize", System.Data.SqlDbType.VarChar).Value = tblStockYardTO.CompartmentSize;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblStockYard(Int32 idStockYard)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idStockYard, cmdDelete);
            }
            catch(Exception ex)
            {
                
                
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteTblStockYard(Int32 idStockYard, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idStockYard, cmdDelete);
            }
            catch(Exception ex)
            {
                
                
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteDeletionCommand(Int32 idStockYard, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblStockYard] " +
            " WHERE idStockYard = " + idStockYard +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idStockYard", System.Data.SqlDbType.Int).Value = tblStockYardTO.IdStockYard;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
