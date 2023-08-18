
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
    public class TblPurchaseParitySummaryDAO : ITblPurchaseParitySummaryDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseParitySummaryDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods

        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tblParitySummaryPurchase.*,stateName FROM [tblParitySummaryPurchase] tblParitySummaryPurchase" +
                                  " LEFT JOIN dimState ON idState=stateId";
            return sqlSelectQry;
        }

        #endregion

        #region Selection

        public  List<TblPurchaseParitySummaryTO> SelectAllActivePurchaseCompetitorMaterialList(Int32 organizationId, DateTime fromDate, DateTime toDate, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = " select PurchaseCompetitorExt.idPurCompetitorExt, userTemp.userDisplayName,PurchaseCompetitorExt.organizationId,PurchaseCompetitorUpdates.informerName,PurchaseCompetitorUpdates.dealerId,PurchaseCompetitorUpdates.otherSourceId, PurchaseCompetitorUpdates.updateDatetime,PurchaseCompetitorExt.materialType,PurchaseCompetitorExt.materialGrade, " +
                    " PurchaseCompetitorUpdates.price,Organization.firmName,PurchaseCompetitorUpdates.competitorOrgId from[dbo].[tblPurchaseCompetitorExt]" +
                    " PurchaseCompetitorExt" +
                    " INNER JOIN dbo.tblPurchaseCompetitorUpdates PurchaseCompetitorUpdates" +
                    " ON PurchaseCompetitorUpdates.IdPurcompetitorExt= PurchaseCompetitorExt.idPurCompetitorExt" +
                    " AND PurchaseCompetitorUpdates.competitorOrgId= PurchaseCompetitorExt.organizationId INNER JOIN tblOrganization Organization ON Organization.idOrganization= PurchaseCompetitorUpdates.competitorOrgId " +
                    " INNER JOIN tblUser userTemp" +
                    " ON userTemp.idUser= PurchaseCompetitorUpdates.createdBy ";
                if(organizationId != 0)
                {
                    cmdSelect.CommandText += " Where PurchaseCompetitorUpdates.competitorOrgId=  " + organizationId +
                    " AND CONVERT (DATE, PurchaseCompetitorUpdates.createdOn,103) BETWEEN @fromDate AND @toDate ORDER BY PurchaseCompetitorUpdates.createdOn DESC";
                }
                else
                {
                    cmdSelect.CommandText += " Where CONVERT (DATE, PurchaseCompetitorUpdates.createdOn,103) BETWEEN @fromDate AND @toDate ORDER BY PurchaseCompetitorUpdates.createdOn DESC";
                }
                
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate.Date.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate.Date.ToString(Constants.AzureDateFormat);

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseParitySummaryTO> list = ConvertDTToListCompetitor(sqlReader);
                
                return list;
               
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public  TblPurchaseParitySummaryTO SelectStatesActiveParitySummary(Int32 stateId, Int32 brandId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE stateId = " + stateId + " AND isActive=1 ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseParitySummaryTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        //public  List<TblPurchaseParitySummaryTO> SelectActiveParitySummaryTOList(int dealerId, SqlConnection conn, SqlTransaction tran)
        //{
        //    SqlCommand cmdSelect = new SqlCommand();
        //    SqlDataReader sqlReader = null;
        //    try
        //    {
        //        cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive = 1" +
        //                                " AND stateId IN(SELECT stateId FROM tblOrganization " +
        //                                " INNER JOIN " +
        //                                " ( " +
        //                                " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
        //                                " INNER JOIN tblAddress ON idAddr = addressId " +
        //                                " WHERE addrTypeId = 1 AND organizationId =" + dealerId +
        //                                " ) addrDtl " +
        //                                " ON idOrganization = organizationId WHERE tblOrganization.isActive = 1 AND idOrganization=" + dealerId + " )";

        //        cmdSelect.Connection = conn;
        //        cmdSelect.Transaction = tran;
        //        cmdSelect.CommandType = System.Data.CommandType.Text;

        //        sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
        //        List<TblPurchaseParitySummaryTO> list = ConvertDTToList(sqlReader);
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        if (sqlReader != null)
        //            sqlReader.Dispose();
        //        cmdSelect.Dispose();
        //    }
        //}

        public  List<TblPurchaseParitySummaryTO> SelectAllMaterialReasonsList(Int32 stateId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "Select idProdClass,prodClassDesc from tblProdClassification where itemProdCatId='6' AND isActive='1'";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseParitySummaryTO> list = ConvertDTToMaterialList(sqlReader);
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

        public  List<TblPurchaseParitySummaryTO> ConvertDTToMaterialList(SqlDataReader tblPurchaseParitySummaryTODT)
        {
            List<TblPurchaseParitySummaryTO> tblParitySummaryTOList = new List<TblPurchaseParitySummaryTO>();
            if (tblPurchaseParitySummaryTODT != null)
            {
                while (tblPurchaseParitySummaryTODT.Read())
                {
                    TblPurchaseParitySummaryTO tblParitySummaryTONew = new TblPurchaseParitySummaryTO();
                    if (tblPurchaseParitySummaryTODT["idProdClass"] != DBNull.Value)
                        tblParitySummaryTONew.IdProdClass = Convert.ToInt32(tblPurchaseParitySummaryTODT["idProdClass"].ToString());
                    if (tblPurchaseParitySummaryTODT["prodClassDesc"] != DBNull.Value)
                        tblParitySummaryTONew.ProdClassDesc = Convert.ToString(tblPurchaseParitySummaryTODT["prodClassDesc"].ToString());
                    tblParitySummaryTOList.Add(tblParitySummaryTONew);
                }
            }
            return tblParitySummaryTOList;
        }

        public  List<TblPurchaseParitySummaryTO> ConvertDTToListCompetitor(SqlDataReader tblPurchaseParitySummaryTODT)
        {
            List<TblPurchaseParitySummaryTO> tblParitySummaryTOList = new List<TblPurchaseParitySummaryTO>();
            if (tblPurchaseParitySummaryTODT != null)
            {
                while (tblPurchaseParitySummaryTODT.Read())
                {
                    TblPurchaseParitySummaryTO tblParitySummaryTONew = new TblPurchaseParitySummaryTO();
                    if (tblPurchaseParitySummaryTODT["price"] != DBNull.Value)
                        tblParitySummaryTONew.Price = Convert.ToDouble(tblPurchaseParitySummaryTODT["price"].ToString());
                    if (tblPurchaseParitySummaryTODT["idPurCompetitorExt"] != DBNull.Value)
                        tblParitySummaryTONew.IdPurCompetitorExt = Convert.ToString(tblPurchaseParitySummaryTODT["idPurCompetitorExt"].ToString());
                    if (tblPurchaseParitySummaryTODT["OrganizationId"] != DBNull.Value)
                        tblParitySummaryTONew.OrganizationId = Convert.ToString(tblPurchaseParitySummaryTODT["OrganizationId"].ToString());
                    if (tblPurchaseParitySummaryTODT["MaterialType"] != DBNull.Value)
                        tblParitySummaryTONew.MaterialType = Convert.ToString(tblPurchaseParitySummaryTODT["MaterialType"].ToString());
                    if (tblPurchaseParitySummaryTODT["MaterialGrade"] != DBNull.Value)
                        tblParitySummaryTONew.MaterialGrade = Convert.ToString(tblPurchaseParitySummaryTODT["MaterialGrade"].ToString());
                    if (tblPurchaseParitySummaryTODT["FirmName"] != DBNull.Value)
                        tblParitySummaryTONew.FirmName = Convert.ToString(tblPurchaseParitySummaryTODT["FirmName"].ToString());
                    if (tblPurchaseParitySummaryTODT["CompetitorOrgId"] != DBNull.Value)
                        tblParitySummaryTONew.CompetitorOrgId = Convert.ToString(tblPurchaseParitySummaryTODT["CompetitorOrgId"].ToString());
                    //Prajakta[2018-04-18] Added
                    if (tblPurchaseParitySummaryTODT["informerName"] != DBNull.Value)
                        tblParitySummaryTONew.InformerName = Convert.ToString(tblPurchaseParitySummaryTODT["informerName"].ToString());
                   if (tblPurchaseParitySummaryTODT["userDisplayName"] != DBNull.Value)
                        tblParitySummaryTONew.CreatedByName = Convert.ToString(tblPurchaseParitySummaryTODT["userDisplayName"].ToString());
                   if (tblPurchaseParitySummaryTODT["updateDatetime"] != DBNull.Value)
                        tblParitySummaryTONew.UpdateDatetime = Convert.ToDateTime(tblPurchaseParitySummaryTODT["updateDatetime"].ToString());
                    if (tblPurchaseParitySummaryTODT["dealerId"] != DBNull.Value)
                        tblParitySummaryTONew.DealerId = Convert.ToInt32(tblPurchaseParitySummaryTODT["dealerId"].ToString());
                    if (tblPurchaseParitySummaryTODT["otherSourceId"] != DBNull.Value)
                        tblParitySummaryTONew.OtherSourceId = Convert.ToInt32(tblPurchaseParitySummaryTODT["otherSourceId"].ToString());
                   
                  
                    tblParitySummaryTOList.Add(tblParitySummaryTONew);
                }
            }
            return tblParitySummaryTOList;
        }

        public  List<TblPurchaseParitySummaryTO> ConvertDTToList(SqlDataReader tblPurchaseParitySummaryTODT)//Need To check...
        {
            List<TblPurchaseParitySummaryTO> tblParitySummaryTOList = new List<TblPurchaseParitySummaryTO>();
            if (tblPurchaseParitySummaryTODT != null)
            {
                while (tblPurchaseParitySummaryTODT.Read())
                {
                    TblPurchaseParitySummaryTO tblParitySummaryTONew = new TblPurchaseParitySummaryTO();
                    if (tblPurchaseParitySummaryTODT["idParityPurchase"] != DBNull.Value)
                        tblParitySummaryTONew.IdParity = Convert.ToInt32(tblPurchaseParitySummaryTODT["idParityPurchase"].ToString());
                    if (tblPurchaseParitySummaryTODT["createdBy"] != DBNull.Value)
                        tblParitySummaryTONew.CreatedBy = Convert.ToInt32(tblPurchaseParitySummaryTODT["createdBy"].ToString());
                    if (tblPurchaseParitySummaryTODT["isActive"] != DBNull.Value)
                        tblParitySummaryTONew.IsActive = Convert.ToInt32(tblPurchaseParitySummaryTODT["isActive"].ToString());
                    if (tblPurchaseParitySummaryTODT["createdOn"] != DBNull.Value)
                        tblParitySummaryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseParitySummaryTODT["createdOn"].ToString());
                    if (tblPurchaseParitySummaryTODT["remark"] != DBNull.Value)
                        tblParitySummaryTONew.Remark = Convert.ToString(tblPurchaseParitySummaryTODT["remark"].ToString());
                    if (tblPurchaseParitySummaryTODT["stateId"] != DBNull.Value)
                        tblParitySummaryTONew.StateId = Convert.ToInt32(tblPurchaseParitySummaryTODT["stateId"].ToString());
                    if (tblPurchaseParitySummaryTODT["stateName"] != DBNull.Value)
                        tblParitySummaryTONew.StateName = Convert.ToString(tblPurchaseParitySummaryTODT["stateName"].ToString());
                    // if (tblPurchaseParitySummaryTODT["baseValCorAmt"] != DBNull.Value)
                    //     tblParitySummaryTONew.BaseValCorAmt = Convert.ToDouble(tblPurchaseParitySummaryTODT["baseValCorAmt"].ToString());
                    // if (tblPurchaseParitySummaryTODT["freightAmt"] != DBNull.Value)
                    //     tblParitySummaryTONew.FreightAmt = Convert.ToDouble(tblPurchaseParitySummaryTODT["freightAmt"].ToString());
                    // if (tblPurchaseParitySummaryTODT["expenseAmt"] != DBNull.Value)
                    //     tblParitySummaryTONew.ExpenseAmt = Convert.ToDouble(tblPurchaseParitySummaryTODT["expenseAmt"].ToString());
                    // if (tblPurchaseParitySummaryTODT["otherAmt"] != DBNull.Value)
                    //     tblParitySummaryTONew.OtherAmt = Convert.ToDouble(tblPurchaseParitySummaryTODT["otherAmt"].ToString());
                    if (tblPurchaseParitySummaryTODT["ProdClassid"] != DBNull.Value)
                             tblParitySummaryTONew.ProdClassId =Convert.ToInt32(tblPurchaseParitySummaryTODT["ProdClassid"].ToString());

                    tblParitySummaryTOList.Add(tblParitySummaryTONew);
                }
            }
            return tblParitySummaryTOList;
        }

        #endregion

        #region Insertion

        public  int InsertTblParitySummary(TblPurchaseParitySummaryTO tblParitySummaryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblParitySummaryTO, cmdInsert);
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

        public  int InsertTblParitySummary(TblPurchaseParitySummaryTO tblParitySummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblParitySummaryTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblPurchaseParitySummaryTO tblParitySummaryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblParitySummaryPurchase]( " +
                            "  [createdBy]" +
                            " ,[isActive]" +
                            " ,[createdOn]" +
                            " ,[remark]" +
                            " ,[stateId]" +
                            //" ,[baseValCorAmt]" +
                            //" ,[freightAmt]" +
                            //" ,[expenseAmt]" +
                            //" ,[otherAmt]" +
                            ", [ProdClassid]" +
                            " )" +
                " VALUES (" +
                            "  @CreatedBy " +
                            " ,@IsActive " +
                            " ,@CreatedOn " +
                            " ,@Remark " +
                            " ,@stateId " +
                            //" ,@baseValCorAmt " +
                            //" ,@freightAmt " +
                            //" ,@expenseAmt " +
                            //" ,@otherAmt " +
                            " ,@ProdClassid " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdParity", System.Data.SqlDbType.Int).Value = tblParitySummaryTO.IdParity;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblParitySummaryTO.CreatedBy;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblParitySummaryTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblParitySummaryTO.CreatedOn;
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.Remark);
            cmdInsert.Parameters.Add("@stateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.StateId);
            // cmdInsert.Parameters.Add("@baseValCorAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.BaseValCorAmt);
            // cmdInsert.Parameters.Add("@freightAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.FreightAmt);
            // cmdInsert.Parameters.Add("@expenseAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.ExpenseAmt);
            // cmdInsert.Parameters.Add("@otherAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.OtherAmt);
            cmdInsert.Parameters.Add("@ProdClassid", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.ProdClassId);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblParitySummaryTO.IdParity = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        
        #endregion

        #region Updation

        public  int UpdateTblParitySummary(TblPurchaseParitySummaryTO tblParitySummaryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblParitySummaryTO, cmdUpdate);
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

        public  int UpdateTblParitySummary(TblPurchaseParitySummaryTO tblParitySummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblParitySummaryTO, cmdUpdate);
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

        public  int DeactivateAllParitySummary(Int32 stateId, Int32 materialId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblParitySummaryPurchase] SET " +
                                   " [isActive]= @IsActive" +
                                   " WHERE stateId=@StateId" +
                                   " AND ProdClassid=@ProdClassId";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = 0;
                cmdUpdate.Parameters.Add("@StateId", System.Data.SqlDbType.Int).Value = stateId;
                cmdUpdate.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = materialId;
                return cmdUpdate.ExecuteNonQuery();
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

        public  int ExecuteUpdationCommand(TblPurchaseParitySummaryTO tblParitySummaryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblParitySummaryPurchase] SET " +
            "  [createdBy]= @CreatedBy" +
            " ,[isActive]= @IsActive" +
            " ,[createdOn]= @CreatedOn" +
            " ,[remark] = @Remark" +
            " ,[stateId] = @stateId" +
            // " ,[baseValCorAmt] = @baseValCorAmt" +
            // " ,[freightAmt] = @freightAmt" +
            // " ,[expenseAmt] = @expenseAmt" +
            // " ,[otherAmt] = @otherAmt" +
             " ,[ProdClassid] = @ProdClassid" +
            " WHERE [idParity] = @IdParity";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdParity", System.Data.SqlDbType.Int).Value = tblParitySummaryTO.IdParity;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblParitySummaryTO.CreatedBy;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblParitySummaryTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblParitySummaryTO.CreatedOn;
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.Remark);
            cmdUpdate.Parameters.Add("@stateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.StateId);
            // cmdUpdate.Parameters.Add("@baseValCorAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.BaseValCorAmt);
            // cmdUpdate.Parameters.Add("@freightAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.FreightAmt);
            // cmdUpdate.Parameters.Add("@expenseAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.ExpenseAmt);
            // cmdUpdate.Parameters.Add("@otherAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.OtherAmt);
            cmdUpdate.Parameters.Add("@ProdClassid", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.ProdClassId);

            return cmdUpdate.ExecuteNonQuery();
        }
        
        #endregion

    }
}
