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
    public class TblLocationDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT loc.*, parentLoc.locationDesc AS parentLocDesc FROM tblLocation loc " +
                                  " LEFT JOIN tblLocation parentLoc ON loc.parentLocId = parentLoc.idLocation";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblLocationTO> SelectAllTblLocation()
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
                List<TblLocationTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
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

        public static List<TblLocationTO> SelectAllTblLocation(Int32 parentLocationId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE ISNULL(loc.parentLocId,0)=" + parentLocationId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLocationTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
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

        public static List<TblLocationTO> SelectAllParentLocation()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE ISNULL(loc.parentLocId,0)=0";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLocationTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
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

        public static List<TblLocationTO> SelectStkNotTakenCompartmentList(DateTime stockDate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() +
                                       " WHERE loc.idLocation NOT IN(Select distinct locationId From tblStockDetails WHERE DAY(createdOn)=" + stockDate.Day + " AND MONTH(createdOn)=" + stockDate.Month + "  AND YEAR(createdOn)=" + stockDate.Year + ")" +
                                       " AND loc.parentLocId IS NOT NULL ";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLocationTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
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

        public static TblLocationTO SelectTblLocation(Int32 idLocation)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE loc.idLocation = " + idLocation +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLocationTO> list = ConvertDTToList(reader);
                if (reader != null)
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

        public static List<TblLocationTO> ConvertDTToList(SqlDataReader tblLocationTODT)
        {
            List<TblLocationTO> tblLocationTOList = new List<TblLocationTO>();
            if (tblLocationTODT != null)
            {
                while(tblLocationTODT.Read())
                {
                    TblLocationTO tblLocationTONew = new TblLocationTO();
                    if (tblLocationTODT["idLocation"] != DBNull.Value)
                        tblLocationTONew.IdLocation = Convert.ToInt32(tblLocationTODT["idLocation"].ToString());
                    if (tblLocationTODT["parentLocId"] != DBNull.Value)
                        tblLocationTONew.ParentLocId = Convert.ToInt32(tblLocationTODT["parentLocId"].ToString());
                    if (tblLocationTODT["createdBy"] != DBNull.Value)
                        tblLocationTONew.CreatedBy = Convert.ToInt32(tblLocationTODT["createdBy"].ToString());
                    if (tblLocationTODT["updatedBy"] != DBNull.Value)
                        tblLocationTONew.UpdatedBy = Convert.ToInt32(tblLocationTODT["updatedBy"].ToString());
                    if (tblLocationTODT["createdOn"] != DBNull.Value)
                        tblLocationTONew.CreatedOn = Convert.ToDateTime(tblLocationTODT["createdOn"].ToString());
                    if (tblLocationTODT["updatedOn"] != DBNull.Value)
                        tblLocationTONew.UpdatedOn = Convert.ToDateTime(tblLocationTODT["updatedOn"].ToString());
                    if (tblLocationTODT["locationDesc"] != DBNull.Value)
                        tblLocationTONew.LocationDesc = Convert.ToString(tblLocationTODT["locationDesc"].ToString());
                    if (tblLocationTODT["parentLocDesc"] != DBNull.Value)
                        tblLocationTONew.ParentLocationDesc = Convert.ToString(tblLocationTODT["parentLocDesc"].ToString());
                    tblLocationTOList.Add(tblLocationTONew);
                }
            }
            return tblLocationTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblLocation(TblLocationTO tblLocationTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblLocationTO, cmdInsert);
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

        public static int InsertTblLocation(TblLocationTO tblLocationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblLocationTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblLocationTO tblLocationTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblLocation]( " +
                                "  [parentLocId]" +
                                " ,[createdBy]" +
                                " ,[updatedBy]" +
                                " ,[createdOn]" +
                                " ,[updatedOn]" +
                                " ,[locationDesc]" +
                                " )" +
                    " VALUES (" +
                                "  @ParentLocId " +
                                " ,@CreatedBy " +
                                " ,@UpdatedBy " +
                                " ,@CreatedOn " +
                                " ,@UpdatedOn " +
                                " ,@LocationDesc " +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdLocation", System.Data.SqlDbType.Int).Value = tblLocationTO.IdLocation;
            cmdInsert.Parameters.Add("@ParentLocId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLocationTO.ParentLocId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLocationTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLocationTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLocationTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLocationTO.UpdatedOn);
            cmdInsert.Parameters.Add("@LocationDesc", System.Data.SqlDbType.NVarChar).Value = tblLocationTO.LocationDesc;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLocationTO.IdLocation = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblLocation(TblLocationTO tblLocationTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblLocationTO, cmdUpdate);
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

        public static int UpdateTblLocation(TblLocationTO tblLocationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblLocationTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblLocationTO tblLocationTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblLocation] SET " + 
            "  [idLocation] = @IdLocation" +
            " ,[parentLocId]= @ParentLocId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[locationDesc] = @LocationDesc" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdLocation", System.Data.SqlDbType.Int).Value = tblLocationTO.IdLocation;
            cmdUpdate.Parameters.Add("@ParentLocId", System.Data.SqlDbType.Int).Value = tblLocationTO.ParentLocId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLocationTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblLocationTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLocationTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblLocationTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@LocationDesc", System.Data.SqlDbType.NVarChar).Value = tblLocationTO.LocationDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblLocation(Int32 idLocation)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idLocation, cmdDelete);
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

        public static int DeleteTblLocation(Int32 idLocation, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idLocation, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idLocation, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblLocation] " +
            " WHERE idLocation = " + idLocation +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idLocation", System.Data.SqlDbType.Int).Value = tblLocationTO.IdLocation;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
