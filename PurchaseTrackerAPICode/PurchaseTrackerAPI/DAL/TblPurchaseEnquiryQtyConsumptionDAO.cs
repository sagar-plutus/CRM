using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace PurchaseTrackerAPI.DAL
{
    public class TblPurchaseEnquiryQtyConsumptionDAO : ITblPurchaseEnquiryQtyConsumptionDAO
    {
        private readonly IConnectionString _iConnectionString;

        public TblPurchaseEnquiryQtyConsumptionDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPurchaseEnquiryQtyConsumption]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblPurchaseEnquiryQtyConsumptionTO> SelectAllTblPurchaseEnquiryQtyConsumption()
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

                //cmdSelect.Parameters.Add("@idPurEnqQtyCons", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryQtyConsumptionTO.IdPurEnqQtyCons;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryQtyConsumptionTO> list = ConvertDTToList(reader);
                reader.Dispose();
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

        public List<TblPurchaseEnquiryQtyConsumptionTO> SelectAllTblPurchaseEnquiryQtyConsumption(DateTime serverDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE DAY(createdOn)= " + serverDate.Day + " AND MONTH(createdOn)=" + serverDate.Month + " AND YEAR(createdOn)= " + serverDate.Year; ;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurEnqQtyCons", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryQtyConsumptionTO.IdPurEnqQtyCons;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryQtyConsumptionTO> list = ConvertDTToList(reader);
                reader.Dispose();
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
        public List<TblPurchaseEnquiryQtyConsumptionTO> SelectTblPurchaseEnquiryQtyConsumption(Int32 idPurEnqQtyCons)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idPurEnqQtyCons = " + idPurEnqQtyCons + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryQtyConsumptionTO> list = ConvertDTToList(reader);
                reader.Dispose();
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

        public List<TblPurchaseEnquiryQtyConsumptionTO> SelectAllTblPurchaseEnquiryQtyConsumption(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurEnqQtyCons", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryQtyConsumptionTO.IdPurEnqQtyCons;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryQtyConsumptionTO> list = ConvertDTToList(reader);
                reader.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        public List<TblPurchaseEnquiryQtyConsumptionTO> ConvertDTToList(SqlDataReader tblPurchaseEnquiryQtyConsumptionTODT)
        {
            List<TblPurchaseEnquiryQtyConsumptionTO> tblPurchaseEnquiryQtyConsumptionTOList = new List<TblPurchaseEnquiryQtyConsumptionTO>();
            if (tblPurchaseEnquiryQtyConsumptionTODT != null)
            {
                while (tblPurchaseEnquiryQtyConsumptionTODT.Read())
                {
                    TblPurchaseEnquiryQtyConsumptionTO tblPurchaseEnquiryQtyConsumptionTONew = new TblPurchaseEnquiryQtyConsumptionTO();
                    if (tblPurchaseEnquiryQtyConsumptionTODT["idPurEnqQtyCons"] != DBNull.Value)
                        tblPurchaseEnquiryQtyConsumptionTONew.IdPurEnqQtyCons = Convert.ToInt32(tblPurchaseEnquiryQtyConsumptionTODT["idPurEnqQtyCons"].ToString());
                    if (tblPurchaseEnquiryQtyConsumptionTODT["purchaseEnqId"] != DBNull.Value)
                        tblPurchaseEnquiryQtyConsumptionTONew.PurchaseEnqId = Convert.ToInt32(tblPurchaseEnquiryQtyConsumptionTODT["purchaseEnqId"].ToString());
                    if (tblPurchaseEnquiryQtyConsumptionTODT["statusId"] != DBNull.Value)
                        tblPurchaseEnquiryQtyConsumptionTONew.StatusId = Convert.ToInt32(tblPurchaseEnquiryQtyConsumptionTODT["statusId"].ToString());
                    if (tblPurchaseEnquiryQtyConsumptionTODT["createdBy"] != DBNull.Value)
                        tblPurchaseEnquiryQtyConsumptionTONew.CreatedBy = Convert.ToInt32(tblPurchaseEnquiryQtyConsumptionTODT["createdBy"].ToString());
                    if (tblPurchaseEnquiryQtyConsumptionTODT["createdOn"] != DBNull.Value)
                        tblPurchaseEnquiryQtyConsumptionTONew.CreatedOn = Convert.ToDateTime(tblPurchaseEnquiryQtyConsumptionTODT["createdOn"].ToString());
                    if (tblPurchaseEnquiryQtyConsumptionTODT["consumptionQty"] != DBNull.Value)
                        tblPurchaseEnquiryQtyConsumptionTONew.ConsumptionQty = Convert.ToDouble(tblPurchaseEnquiryQtyConsumptionTODT["consumptionQty"].ToString());
                    if (tblPurchaseEnquiryQtyConsumptionTODT["remark"] != DBNull.Value)
                        tblPurchaseEnquiryQtyConsumptionTONew.Remark = Convert.ToString(tblPurchaseEnquiryQtyConsumptionTODT["remark"].ToString());
                    if (tblPurchaseEnquiryQtyConsumptionTODT["isAuto"] != DBNull.Value)
                        tblPurchaseEnquiryQtyConsumptionTONew.IsAuto = Convert.ToInt32(tblPurchaseEnquiryQtyConsumptionTODT["isAuto"].ToString());
                    tblPurchaseEnquiryQtyConsumptionTOList.Add(tblPurchaseEnquiryQtyConsumptionTONew);
                }
            }
            return tblPurchaseEnquiryQtyConsumptionTOList;
        }


        #endregion

        #region Insertion
        public int InsertTblPurchaseEnquiryQtyConsumption(TblPurchaseEnquiryQtyConsumptionTO tblPurchaseEnquiryQtyConsumptionTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseEnquiryQtyConsumptionTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int InsertTblPurchaseEnquiryQtyConsumption(TblPurchaseEnquiryQtyConsumptionTO tblPurchaseEnquiryQtyConsumptionTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseEnquiryQtyConsumptionTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(TblPurchaseEnquiryQtyConsumptionTO tblPurchaseEnquiryQtyConsumptionTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseEnquiryQtyConsumption]( " +
            //"  [idPurEnqQtyCons]" +
            "  [purchaseEnqId]" +
            " ,[statusId]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[consumptionQty]" +
            " ,[remark]" +
            " ,[isAuto]" +
            " )" +
" VALUES (" +
            //"  @IdPurEnqQtyCons " +
            "  @PurchaseEnqId " +
            " ,@StatusId " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@ConsumptionQty " +
            " ,@Remark " +
            " ,@IsAuto " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdPurEnqQtyCons", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryQtyConsumptionTO.IdPurEnqQtyCons;
            cmdInsert.Parameters.Add("@PurchaseEnqId", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryQtyConsumptionTO.PurchaseEnqId;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryQtyConsumptionTO.StatusId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryQtyConsumptionTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseEnquiryQtyConsumptionTO.CreatedOn;
            cmdInsert.Parameters.Add("@ConsumptionQty", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryQtyConsumptionTO.ConsumptionQty;
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.VarChar).Value = tblPurchaseEnquiryQtyConsumptionTO.Remark;
            cmdInsert.Parameters.Add("@IsAuto", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryQtyConsumptionTO.IsAuto;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public int UpdateTblPurchaseEnquiryQtyConsumption(TblPurchaseEnquiryQtyConsumptionTO tblPurchaseEnquiryQtyConsumptionTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseEnquiryQtyConsumptionTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateTblPurchaseEnquiryQtyConsumption(TblPurchaseEnquiryQtyConsumptionTO tblPurchaseEnquiryQtyConsumptionTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseEnquiryQtyConsumptionTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(TblPurchaseEnquiryQtyConsumptionTO tblPurchaseEnquiryQtyConsumptionTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseEnquiryQtyConsumption] SET " +
            //"  [idPurEnqQtyCons] = @IdPurEnqQtyCons" +
            "  [purchaseEnqId]= @PurchaseEnqId" +
            " ,[statusId]= @StatusId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[consumptionQty]= @ConsumptionQty" +
            " ,[remark] = @Remark" +
            " ,[isAuto] = @IsAuto" +
            " WHERE idPurEnqQtyCons = @IdPurEnqQtyCons ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurEnqQtyCons", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryQtyConsumptionTO.IdPurEnqQtyCons;
            cmdUpdate.Parameters.Add("@PurchaseEnqId", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryQtyConsumptionTO.PurchaseEnqId;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryQtyConsumptionTO.StatusId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryQtyConsumptionTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseEnquiryQtyConsumptionTO.CreatedOn;
            cmdUpdate.Parameters.Add("@ConsumptionQty", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryQtyConsumptionTO.ConsumptionQty;
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.VarChar).Value = tblPurchaseEnquiryQtyConsumptionTO.Remark;
            cmdUpdate.Parameters.Add("@IsAuto", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryQtyConsumptionTO.IsAuto;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblPurchaseEnquiryQtyConsumption(Int32 idPurEnqQtyCons)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPurEnqQtyCons, cmdDelete);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteTblPurchaseEnquiryQtyConsumption(Int32 idPurEnqQtyCons, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPurEnqQtyCons, cmdDelete);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idPurEnqQtyCons, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseEnquiryQtyConsumption] " +
            " WHERE idPurEnqQtyCons = " + idPurEnqQtyCons + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPurEnqQtyCons", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryQtyConsumptionTO.IdPurEnqQtyCons;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
