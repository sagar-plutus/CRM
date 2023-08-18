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
    public class TblRateDeclareReasonsDAO : ITblRateDeclareReasonsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblRateDeclareReasonsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM tblPurchaseRateDeclareReasons"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblRateDeclareReasonsTO> SelectAllTblRateDeclareReasons()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRateDeclareReasonsTO> list = ConvertDTToList(sqlReader);
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

        public  TblRateDeclareReasonsTO SelectTblRateDeclareReasons(Int32 idRateReason)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idRateReason = " + idRateReason +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRateDeclareReasonsTO> list = ConvertDTToList(reader);
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

        public  List<TblRateDeclareReasonsTO> ConvertDTToList(SqlDataReader tblRateDeclareReasonsTODT)
        {
            List<TblRateDeclareReasonsTO> tblRateDeclareReasonsTOList = new List<TblRateDeclareReasonsTO>();
            if (tblRateDeclareReasonsTODT != null)
            {
                while(tblRateDeclareReasonsTODT.Read())
                {
                    TblRateDeclareReasonsTO tblRateDeclareReasonsTONew = new TblRateDeclareReasonsTO();
                    if (tblRateDeclareReasonsTODT["idRateReason"] != DBNull.Value)
                        tblRateDeclareReasonsTONew.IdRateReason = Convert.ToInt32(tblRateDeclareReasonsTODT["idRateReason"].ToString());
                    if (tblRateDeclareReasonsTODT["createdBy"] != DBNull.Value)
                        tblRateDeclareReasonsTONew.CreatedBy = Convert.ToInt32(tblRateDeclareReasonsTODT["createdBy"].ToString());
                    if (tblRateDeclareReasonsTODT["createdOn"] != DBNull.Value)
                        tblRateDeclareReasonsTONew.CreatedOn = Convert.ToDateTime(tblRateDeclareReasonsTODT["createdOn"].ToString());
                    if (tblRateDeclareReasonsTODT["reasonDesc"] != DBNull.Value)
                        tblRateDeclareReasonsTONew.ReasonDesc = Convert.ToString(tblRateDeclareReasonsTODT["reasonDesc"].ToString());
                    tblRateDeclareReasonsTOList.Add(tblRateDeclareReasonsTONew);
                }
            }
            return tblRateDeclareReasonsTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblRateDeclareReasons(TblRateDeclareReasonsTO tblRateDeclareReasonsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblRateDeclareReasonsTO, cmdInsert);
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

        public  int InsertTblRateDeclareReasons(TblRateDeclareReasonsTO tblRateDeclareReasonsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblRateDeclareReasonsTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblRateDeclareReasonsTO tblRateDeclareReasonsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblRateDeclareReasons]( " + 
                                "  [createdBy]" +
                                " ,[createdOn]" +
                                " ,[reasonDesc]" +
                                " )" +
                    " VALUES (" +
                                "  @CreatedBy " +
                                " ,@CreatedOn " +
                                " ,@ReasonDesc " + 
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

           // cmdInsert.Parameters.Add("@IdRateReason", System.Data.SqlDbType.Int).Value = tblRateDeclareReasonsTO.IdRateReason;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblRateDeclareReasonsTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblRateDeclareReasonsTO.CreatedOn;
            cmdInsert.Parameters.Add("@ReasonDesc", System.Data.SqlDbType.VarChar).Value = tblRateDeclareReasonsTO.ReasonDesc;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblRateDeclareReasonsTO.IdRateReason = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public  int UpdateTblRateDeclareReasons(TblRateDeclareReasonsTO tblRateDeclareReasonsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblRateDeclareReasonsTO, cmdUpdate);
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

        public  int UpdateTblRateDeclareReasons(TblRateDeclareReasonsTO tblRateDeclareReasonsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblRateDeclareReasonsTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblRateDeclareReasonsTO tblRateDeclareReasonsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblRateDeclareReasons] SET " + 
            "  [idRateReason] = @IdRateReason" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[reasonDesc] = @ReasonDesc" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdRateReason", System.Data.SqlDbType.Int).Value = tblRateDeclareReasonsTO.IdRateReason;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblRateDeclareReasonsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblRateDeclareReasonsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@ReasonDesc", System.Data.SqlDbType.VarChar).Value = tblRateDeclareReasonsTO.ReasonDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblRateDeclareReasons(Int32 idRateReason)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idRateReason, cmdDelete);
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

        public  int DeleteTblRateDeclareReasons(Int32 idRateReason, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idRateReason, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idRateReason, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblRateDeclareReasons] " +
            " WHERE idRateReason = " + idRateReason +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idRateReason", System.Data.SqlDbType.Int).Value = tblRateDeclareReasonsTO.IdRateReason;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
