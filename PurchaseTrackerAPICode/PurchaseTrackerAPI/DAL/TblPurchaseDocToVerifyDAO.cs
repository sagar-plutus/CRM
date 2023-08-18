using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblPurchaseDocToVerifyDAO : ITblPurchaseDocToVerifyDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseDocToVerifyDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPurchaseDocToVerify] WHERE isActive = 1"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblPurchaseDocToVerifyTO> SelectAllTblPurchaseDocToVerify()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseDocType", System.Data.SqlDbType.Int).Value = tblPurchaseDocToVerifyTO.IdPurchaseDocType;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseDocToVerifyTO> list = ConvertDTToList(reader);
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

        public  TblPurchaseDocToVerifyTO SelectTblPurchaseDocToVerify(Int32 idPurchaseDocType)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idPurchaseDocType = " + idPurchaseDocType +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseDocType", System.Data.SqlDbType.Int).Value = tblPurchaseDocToVerifyTO.IdPurchaseDocType;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseDocToVerifyTO> list = ConvertDTToList(reader);
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

        public  List<TblPurchaseDocToVerifyTO> SelectAllTblPurchaseDocToVerify(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseDocType", System.Data.SqlDbType.Int).Value = tblPurchaseDocToVerifyTO.IdPurchaseDocType;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseDocToVerifyTO> list = ConvertDTToList(reader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        public  List<TblPurchaseDocToVerifyTO> ConvertDTToList(SqlDataReader tblPurchaseDocToVerifyTODT)
        {
            List<TblPurchaseDocToVerifyTO> tblPurchaseDocToVerifyTOList = new List<TblPurchaseDocToVerifyTO>();
            if (tblPurchaseDocToVerifyTODT != null)
            {
                while (tblPurchaseDocToVerifyTODT.Read())
                {
                    TblPurchaseDocToVerifyTO tblPurchaseDocToVerifyTONew = new TblPurchaseDocToVerifyTO();
                    if (tblPurchaseDocToVerifyTODT["idPurchaseDocType"] != DBNull.Value)
                        tblPurchaseDocToVerifyTONew.IdPurchaseDocType = Convert.ToInt32(tblPurchaseDocToVerifyTODT["idPurchaseDocType"].ToString());
                    if (tblPurchaseDocToVerifyTODT["isActive"] != DBNull.Value)
                        tblPurchaseDocToVerifyTONew.IsActive = Convert.ToInt32(tblPurchaseDocToVerifyTODT["isActive"].ToString());
                    if (tblPurchaseDocToVerifyTODT["purchaseDocType"] != DBNull.Value)
                        tblPurchaseDocToVerifyTONew.PurchaseDocType = Convert.ToString(tblPurchaseDocToVerifyTODT["purchaseDocType"].ToString());
                    if (tblPurchaseDocToVerifyTODT["masterId"] != DBNull.Value)
                        tblPurchaseDocToVerifyTONew.MasterId = Convert.ToInt32(tblPurchaseDocToVerifyTODT["masterId"].ToString());
                    if (tblPurchaseDocToVerifyTODT["isFromMaster"] != DBNull.Value)
                        tblPurchaseDocToVerifyTONew.IsFromMaster = Convert.ToInt32(tblPurchaseDocToVerifyTODT["isFromMaster"].ToString());
                    tblPurchaseDocToVerifyTOList.Add(tblPurchaseDocToVerifyTONew);
                }
            }
            return tblPurchaseDocToVerifyTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblPurchaseDocToVerify(TblPurchaseDocToVerifyTO tblPurchaseDocToVerifyTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseDocToVerifyTO, cmdInsert);
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

        public  int InsertTblPurchaseDocToVerify(TblPurchaseDocToVerifyTO tblPurchaseDocToVerifyTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseDocToVerifyTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblPurchaseDocToVerifyTO tblPurchaseDocToVerifyTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseDocToVerify]( " + 
            //"  [idPurchaseDocType]" +
            " [isActive]" +
            " ,[purchaseDocType]" +
            " ,[masterId]" +
            " ,[isFromMaster]" +
            " )" +
" VALUES (" +
           // "  @IdPurchaseDocType " +
            " @IsActive " +
            " ,@PurchaseDocType " + 
            " ,@MasterId " +
            " ,@IsFromMaster "+
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

           // cmdInsert.Parameters.Add("@IdPurchaseDocType", System.Data.SqlDbType.Int).Value = tblPurchaseDocToVerifyTO.IdPurchaseDocType;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblPurchaseDocToVerifyTO.IsActive;
            cmdInsert.Parameters.Add("@PurchaseDocType", System.Data.SqlDbType.VarChar).Value = tblPurchaseDocToVerifyTO.PurchaseDocType;
            cmdInsert.Parameters.Add("@MasterId", System.Data.SqlDbType.Int).Value = tblPurchaseDocToVerifyTO.MasterId;
            cmdInsert.Parameters.Add("@IsFromMaster", System.Data.SqlDbType.Int).Value = tblPurchaseDocToVerifyTO.IsFromMaster;

            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblPurchaseDocToVerify(TblPurchaseDocToVerifyTO tblPurchaseDocToVerifyTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseDocToVerifyTO, cmdUpdate);
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

        public  int UpdateTblPurchaseDocToVerify(TblPurchaseDocToVerifyTO tblPurchaseDocToVerifyTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseDocToVerifyTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblPurchaseDocToVerifyTO tblPurchaseDocToVerifyTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseDocToVerify] SET " + 
          //  "  [idPurchaseDocType] = @IdPurchaseDocType" +
            " [isActive]= @IsActive" +
            " ,[purchaseDocType] = @PurchaseDocType" +
            " ,[masterId] = @MasterId" +
            " ,[isFromMaster] = @IsFromMaster" +
            " WHERE  [idPurchaseDocType] = @IdPurchaseDocType "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurchaseDocType", System.Data.SqlDbType.Int).Value = tblPurchaseDocToVerifyTO.IdPurchaseDocType;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblPurchaseDocToVerifyTO.IsActive;
            cmdUpdate.Parameters.Add("@PurchaseDocType", System.Data.SqlDbType.VarChar).Value = tblPurchaseDocToVerifyTO.PurchaseDocType;
            cmdUpdate.Parameters.Add("@MasterId", System.Data.SqlDbType.Int).Value = tblPurchaseDocToVerifyTO.MasterId;
            cmdUpdate.Parameters.Add("@IsFromMaster", System.Data.SqlDbType.Int).Value = tblPurchaseDocToVerifyTO.IsFromMaster;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblPurchaseDocToVerify(Int32 idPurchaseDocType)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPurchaseDocType, cmdDelete);
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

        public  int DeleteTblPurchaseDocToVerify(Int32 idPurchaseDocType, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPurchaseDocType, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idPurchaseDocType, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseDocToVerify] " +
            " WHERE idPurchaseDocType = " + idPurchaseDocType +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPurchaseDocType", System.Data.SqlDbType.Int).Value = tblPurchaseDocToVerifyTO.IdPurchaseDocType;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
