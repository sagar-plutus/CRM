using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblLocationDAO : ITblLocationDAO
    {

        private readonly IConnectionString _iConnectionString;
        public TblLocationDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblLocation]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public   List<TblLocationTO> SelectAllTblLocation()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlDataReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idLocation", System.Data.SqlDbType.Int).Value = tblLocationTO.IdLocation;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader TblLocationTO = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLocationTO> list = ConvertDTToList(TblLocationTO);
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

        public  List<TblLocationTO> SelectTblLocation(Int32 idLocation)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idLocation = " + idLocation +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idLocation", System.Data.SqlDbType.Int).Value = tblLocationTO.IdLocation;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader TblLocationTO = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLocationTO> list = ConvertDTToList(TblLocationTO);
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

        // public  List<TblLoadingTO> SelectAllTblLocation(SqlConnection conn, SqlTransaction tran)
        // {
        //     SqlCommand cmdSelect = new SqlCommand();
        //     try
        //     {
        //         cmdSelect.CommandText = SqlSelectQuery();
        //         cmdSelect.Connection = conn;
        //         cmdSelect.Transaction = tran;
        //         cmdSelect.CommandType = System.Data.CommandType.Text;

        //         //cmdSelect.Parameters.Add("@idLocation", System.Data.SqlDbType.Int).Value = tblLocationTO.IdLocation;
        //        SqlDataReader TblLocationTO = cmdSelect.ExecuteReader(CommandBehavior.Default);
        //         List<TblLocationTO> list = ConvertDTToList(TblLocationTO);
        //         return list;
        //     }
        //     catch(Exception ex)
        //     {
        //         return null;
        //     }
        //     finally
        //     {
        //         cmdSelect.Dispose();
        //     }
        // }

         public  List<TblLocationTO> ConvertDTToList(SqlDataReader tblLocationTODT)
        {
            List<TblLocationTO> tblLocationTOList = new List<TblLocationTO>();
            if (tblLocationTODT != null)
            {
                while (tblLocationTODT.Read())
                {
                    TblLocationTO tblLocationTONew = new TblLocationTO();
                    if(tblLocationTODT["idLocation"] != DBNull.Value)
                        tblLocationTONew.IdLocation = Convert.ToInt32( tblLocationTODT["idLocation"].ToString());
                    if(tblLocationTODT["parentLocId"] != DBNull.Value)
                        tblLocationTONew.ParentLocId = Convert.ToInt32( tblLocationTODT["parentLocId"].ToString());
                    if(tblLocationTODT["createdBy"] != DBNull.Value)
                        tblLocationTONew.CreatedBy = Convert.ToInt32( tblLocationTODT["createdBy"].ToString());
                    if(tblLocationTODT["updatedBy"] != DBNull.Value)
                        tblLocationTONew.UpdatedBy = Convert.ToInt32( tblLocationTODT["updatedBy"].ToString());
                    if(tblLocationTODT["mateHandlSystemId"] != DBNull.Value)
                        tblLocationTONew.MateHandlSystemId = Convert.ToInt32( tblLocationTODT["mateHandlSystemId"].ToString());
                    if(tblLocationTODT["createdOn"] != DBNull.Value)
                        tblLocationTONew.CreatedOn = Convert.ToDateTime( tblLocationTODT["createdOn"].ToString());
                    if(tblLocationTODT["updatedOn"] != DBNull.Value)
                        tblLocationTONew.UpdatedOn = Convert.ToDateTime( tblLocationTODT["updatedOn"].ToString());
                    if(tblLocationTODT["compartmentNo"] != DBNull.Value)
                        tblLocationTONew.CompartmentNo = Convert.ToString( tblLocationTODT["compartmentNo"].ToString());
                    if(tblLocationTODT["compartmentSize"] != DBNull.Value)
                        tblLocationTONew.CompartmentSize = Convert.ToString( tblLocationTODT["compartmentSize"].ToString());
                    if(tblLocationTODT["locationDesc"] != DBNull.Value)
                        tblLocationTONew.LocationDesc = Convert.ToString( tblLocationTODT["locationDesc"].ToString());
                    tblLocationTOList.Add(tblLocationTONew);
                }
            }
            return tblLocationTOList;
        }

        #endregion
        
        #region Insertion
        public  int InsertTblLocation(TblLocationTO tblLocationTO)
        {
           String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
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
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public  int InsertTblLocation(TblLocationTO tblLocationTO, SqlConnection conn, SqlTransaction tran)
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
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public  int ExecuteInsertionCommand(TblLocationTO tblLocationTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblLocation]( " + 
            //"  [idLocation]" +
            "  [parentLocId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[mateHandlSystemId]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[compartmentNo]" +
            " ,[compartmentSize]" +
            " ,[locationDesc]" +
            " )" +
" VALUES (" +
            //"  @IdLocation " +
            "  @ParentLocId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@MateHandlSystemId " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@CompartmentNo " +
            " ,@CompartmentSize " +
            " ,@LocationDesc " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdLocation", System.Data.SqlDbType.Int).Value = tblLocationTO.IdLocation;
            cmdInsert.Parameters.Add("@ParentLocId", System.Data.SqlDbType.Int).Value = tblLocationTO.ParentLocId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLocationTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblLocationTO.UpdatedBy;
            cmdInsert.Parameters.Add("@MateHandlSystemId", System.Data.SqlDbType.Int).Value = tblLocationTO.MateHandlSystemId;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLocationTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblLocationTO.UpdatedOn;
            cmdInsert.Parameters.Add("@CompartmentNo", System.Data.SqlDbType.VarChar).Value = tblLocationTO.CompartmentNo;
            cmdInsert.Parameters.Add("@CompartmentSize", System.Data.SqlDbType.VarChar).Value = tblLocationTO.CompartmentSize;
            cmdInsert.Parameters.Add("@LocationDesc", System.Data.SqlDbType.NVarChar).Value = tblLocationTO.LocationDesc;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblLocation(TblLocationTO tblLocationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
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

        public  int UpdateTblLocation(TblLocationTO tblLocationTO, SqlConnection conn, SqlTransaction tran)
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

        public  int ExecuteUpdationCommand(TblLocationTO tblLocationTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblLocation] SET " + 
            //"  [idLocation] = @IdLocation" +
            "  [parentLocId]= @ParentLocId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[mateHandlSystemId]= @MateHandlSystemId" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[compartmentNo]= @CompartmentNo" +
            " ,[compartmentSize]= @CompartmentSize" +
            " ,[locationDesc] = @LocationDesc" +
            " WHERE idLocation = @IdLocation "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdLocation", System.Data.SqlDbType.Int).Value = tblLocationTO.IdLocation;
            cmdUpdate.Parameters.Add("@ParentLocId", System.Data.SqlDbType.Int).Value = tblLocationTO.ParentLocId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLocationTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblLocationTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@MateHandlSystemId", System.Data.SqlDbType.Int).Value = tblLocationTO.MateHandlSystemId;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLocationTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblLocationTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@CompartmentNo", System.Data.SqlDbType.VarChar).Value = tblLocationTO.CompartmentNo;
            cmdUpdate.Parameters.Add("@CompartmentSize", System.Data.SqlDbType.VarChar).Value = tblLocationTO.CompartmentSize;
            cmdUpdate.Parameters.Add("@LocationDesc", System.Data.SqlDbType.NVarChar).Value = tblLocationTO.LocationDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblLocation(Int32 idLocation)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
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

        public  int DeleteTblLocation(Int32 idLocation, SqlConnection conn, SqlTransaction tran)
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

        public  int ExecuteDeletionCommand(Int32 idLocation, SqlCommand cmdDelete)
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
