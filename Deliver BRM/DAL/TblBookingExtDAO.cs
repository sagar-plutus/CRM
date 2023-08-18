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
    public class TblBookingExtDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT bookingDtl.* ,material.materialSubType,booking.bookingDatetime,isConfirmed,isJointDelivery,cdStructure,noOfDeliveries " +
                                  " , prodCat.prodCateDesc AS prodCatDesc ,prodSpec.prodSpecDesc " +
                                  ",category.prodClassDesc as categoryName ,subCategory.prodClassDesc as subCategoryName " +
                                  ",specification.prodClassDesc as specificationName,item.itemName " +
                                  " FROM tblBookingExt bookingDtl " +
                                  " LEFT JOIN tblBookings booking " +
                                  " ON booking.idBooking = bookingDtl.bookingId " +
                                  " LEFT JOIN tblMaterial material " +
                                  " ON material.idMaterial = bookingDtl.materialId " +
                                  " LEFT JOIN  dimProdCat prodCat ON prodCat.idProdCat=bookingDtl.prodCatId" +
                                  " LEFT JOIN  dimProdSpec prodSpec ON prodSpec.idProdSpec=bookingDtl.prodSpecId" +
                                  " LEFT JOIN tblProductItem item ON item.idProdItem = bookingDtl.prodItemId " +
                                  " LEFT JOIN tblProdClassification specification ON item.prodClassId = specification.idProdClass " +
                                  " LEFT JOIN tblProdClassification subCategory ON specification.parentProdClassId = subCategory.idProdClass " +
                                  " LEFT JOIN tblProdClassification category ON subCategory.parentProdClassId = category.idProdClass";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblBookingExtTO> SelectAllTblBookingExt()
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

                //cmdSelect.Parameters.Add("@idBookingExt", System.Data.SqlDbType.Int).Value = tblBookingExtTO.IdBookingExt;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingExtTO> list = ConvertDTToList(sqlReader);
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

        public static List<TblBookingExtTO> SelectAllTblBookingExt(int bookingId, SqlConnection conn, SqlTransaction tran)
        {
            SqlDataReader sqlReader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE bookingId=" + bookingId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idBookingExt", System.Data.SqlDbType.Int).Value = tblBookingExtTO.IdBookingExt;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingExtTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static List<TblBookingExtTO> SelectAllTblBookingExt(int bookingId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE bookingId=" + bookingId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idBookingExt", System.Data.SqlDbType.Int).Value = tblBookingExtTO.IdBookingExt;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingExtTO> list = ConvertDTToList(sqlReader);
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

        public static List<TblBookingExtTO> SelectEmptyBookingExtList(int prodCatgId,Int32 prodSpecId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT tblMaterial.idMaterial AS materialId, tblMaterial.materialSubType , " +
                                        " dimProdCat.idProdCat AS prodCatId,dimProdCat.prodCateDesc prodCatDesc, " +
                                        " dimProdSpec.idProdSpec AS prodSpecId ,dimProdSpec.prodSpecDesc " +
                                        " FROM tblMaterial " +
                                        " FULL OUTER JOIN dimProdCat ON 1 = 1 " +
                                        " FULL OUTER JOIN dimProdSpec ON 1 = 1" +
                                        " WHERE idProdCat=" + prodCatgId + " AND idProdSpec=" + prodSpecId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idBookingExt", System.Data.SqlDbType.Int).Value = tblBookingExtTO.IdBookingExt;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingExtTO> tblBookingExtTOList = new List<Models.TblBookingExtTO>();

                while (sqlReader.Read())
                {
                    TblBookingExtTO tblBookingExtTONew = new TblBookingExtTO();
                    if (sqlReader["materialId"] != DBNull.Value)
                        tblBookingExtTONew.MaterialId = Convert.ToInt32(sqlReader["materialId"].ToString());
                    if (sqlReader["materialSubType"] != DBNull.Value)
                        tblBookingExtTONew.MaterialSubType = Convert.ToString(sqlReader["materialSubType"].ToString());
                    if (sqlReader["prodCatId"] != DBNull.Value)
                        tblBookingExtTONew.ProdCatId = Convert.ToInt32(sqlReader["prodCatId"].ToString());
                    if (sqlReader["prodSpecId"] != DBNull.Value)
                        tblBookingExtTONew.ProdSpecId = Convert.ToInt32(sqlReader["prodSpecId"].ToString());
                    if (sqlReader["prodCatDesc"] != DBNull.Value)
                        tblBookingExtTONew.ProdCatDesc = Convert.ToString(sqlReader["prodCatDesc"].ToString());
                    if (sqlReader["prodSpecDesc"] != DBNull.Value)
                        tblBookingExtTONew.ProdSpecDesc = Convert.ToString(sqlReader["prodSpecDesc"].ToString());

                    tblBookingExtTOList.Add(tblBookingExtTONew);
                }
                return tblBookingExtTOList;
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

        public static List<TblBookingExtTO> SelectBookingDetails(Int32 dealerId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE dealerOrgId=" + dealerId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idBookingExt", System.Data.SqlDbType.Int).Value = tblBookingExtTO.IdBookingExt;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingExtTO> list = ConvertDTToList(sqlReader);
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

        public static TblBookingExtTO SelectTblBookingExt(Int32 idBookingExt)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idBookingExt = " + idBookingExt +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingExtTO> list = ConvertDTToList(reader);
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


        public static List<TblBookingExtTO> ConvertDTToList(SqlDataReader tblBookingExtTODT)
        {
            List<TblBookingExtTO> tblBookingExtTOList = new List<TblBookingExtTO>();
            if (tblBookingExtTODT != null)
            {
                while (tblBookingExtTODT.Read())
                {
                    TblBookingExtTO tblBookingExtTONew = new TblBookingExtTO();
                    if (tblBookingExtTODT["idBookingExt"] != DBNull.Value)
                        tblBookingExtTONew.IdBookingExt = Convert.ToInt32(tblBookingExtTODT["idBookingExt"].ToString());
                    if (tblBookingExtTODT["bookingId"] != DBNull.Value)
                        tblBookingExtTONew.BookingId = Convert.ToInt32(tblBookingExtTODT["bookingId"].ToString());
                    if (tblBookingExtTODT["materialId"] != DBNull.Value)
                        tblBookingExtTONew.MaterialId = Convert.ToInt32(tblBookingExtTODT["materialId"].ToString());
                    if (tblBookingExtTODT["bookedQty"] != DBNull.Value)
                        tblBookingExtTONew.BookedQty = Convert.ToDouble(tblBookingExtTODT["bookedQty"].ToString());
                    if (tblBookingExtTODT["balanceQty"] != DBNull.Value)
                        tblBookingExtTONew.BalanceQty = Convert.ToDouble(tblBookingExtTODT["balanceQty"].ToString());
                    if (tblBookingExtTODT["rate"] != DBNull.Value)
                        tblBookingExtTONew.Rate = Convert.ToDouble(tblBookingExtTODT["rate"].ToString());

                    if (tblBookingExtTODT["materialSubType"] != DBNull.Value)
                        tblBookingExtTONew.MaterialSubType = Convert.ToString(tblBookingExtTODT["materialSubType"].ToString());

                    if (tblBookingExtTODT["bookingDatetime"] != DBNull.Value)
                        tblBookingExtTONew.BookingDatetime = Convert.ToDateTime(tblBookingExtTODT["bookingDatetime"].ToString());

                    if (tblBookingExtTODT["isConfirmed"] != DBNull.Value)
                        tblBookingExtTONew.IsConfirmed = Convert.ToInt32(tblBookingExtTODT["isConfirmed"].ToString());

                    if (tblBookingExtTODT["noOfDeliveries"] != DBNull.Value)
                        tblBookingExtTONew.NoOfDeliveries = Convert.ToInt32(tblBookingExtTODT["noOfDeliveries"].ToString());

                    if (tblBookingExtTODT["isJointDelivery"] != DBNull.Value)
                        tblBookingExtTONew.IsJointDelivery = Convert.ToInt32(tblBookingExtTODT["isJointDelivery"].ToString());

                    if (tblBookingExtTODT["cdStructure"] != DBNull.Value)
                        tblBookingExtTONew.CdStructure = Convert.ToDouble(tblBookingExtTODT["cdStructure"].ToString());
                    if (tblBookingExtTODT["prodCatId"] != DBNull.Value)
                        tblBookingExtTONew.ProdCatId = Convert.ToInt32(tblBookingExtTODT["prodCatId"].ToString());
                    if (tblBookingExtTODT["prodSpecId"] != DBNull.Value)
                        tblBookingExtTONew.ProdSpecId = Convert.ToInt32(tblBookingExtTODT["prodSpecId"].ToString());
                    if (tblBookingExtTODT["prodCatDesc"] != DBNull.Value)
                        tblBookingExtTONew.ProdCatDesc = Convert.ToString(tblBookingExtTODT["prodCatDesc"].ToString());
                    if (tblBookingExtTODT["prodSpecDesc"] != DBNull.Value)
                        tblBookingExtTONew.ProdSpecDesc = Convert.ToString(tblBookingExtTODT["prodSpecDesc"].ToString());
                    if (tblBookingExtTODT["prodItemId"] != DBNull.Value)
                        tblBookingExtTONew.ProdItemId = Convert.ToInt32(tblBookingExtTODT["prodItemId"].ToString());
                    if (tblBookingExtTODT["categoryName"] != DBNull.Value)
                        tblBookingExtTONew.CategoryName = Convert.ToString(tblBookingExtTODT["categoryName"].ToString());
                    if (tblBookingExtTODT["subCategoryName"] != DBNull.Value)
                        tblBookingExtTONew.SubCategoryName = Convert.ToString(tblBookingExtTODT["subCategoryName"].ToString());
                    if (tblBookingExtTODT["specificationName"] != DBNull.Value)
                        tblBookingExtTONew.SpecificationName = Convert.ToString(tblBookingExtTODT["specificationName"].ToString());
                    if (tblBookingExtTODT["itemName"] != DBNull.Value)
                        tblBookingExtTONew.ItemName = Convert.ToString(tblBookingExtTODT["itemName"].ToString());
                    if (tblBookingExtTODT["loadingLayerId"] != DBNull.Value)
                        tblBookingExtTONew.LoadingLayerId = Convert.ToInt32(tblBookingExtTODT["loadingLayerId"].ToString());

                    if (tblBookingExtTODT["scheduleId"] != DBNull.Value)
                        tblBookingExtTONew.ScheduleId = Convert.ToInt32(tblBookingExtTODT["scheduleId"].ToString());

                    if (tblBookingExtTONew.ProdItemId > 0)
                    {
                        tblBookingExtTONew.DisplayName = tblBookingExtTONew.CategoryName + "-" + tblBookingExtTONew.SubCategoryName + "-" + tblBookingExtTONew.SpecificationName + "-" + tblBookingExtTONew.ItemName;
                    }
                    else
                    {
                        tblBookingExtTONew.DisplayName = tblBookingExtTONew.MaterialSubType+ "-" + tblBookingExtTONew.ProdCatDesc + "-" + tblBookingExtTONew.ProdSpecDesc;
                    }
                    tblBookingExtTOList.Add(tblBookingExtTONew);
                }
            }
            return tblBookingExtTOList;
        }

        public static List<TblBookingExtTO> SelectAllTblBookingExtListBySchedule(int scheduleId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE scheduleId=" + scheduleId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idBookingExt", System.Data.SqlDbType.Int).Value = tblBookingExtTO.IdBookingExt;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingExtTO> list = ConvertDTToList(sqlReader);
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

        #endregion

        #region Insertion
        public static int InsertTblBookingExt(TblBookingExtTO tblBookingExtTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblBookingExtTO, cmdInsert);
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

        public static int InsertTblBookingExt(TblBookingExtTO tblBookingExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblBookingExtTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblBookingExtTO tblBookingExtTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblBookingExt]( " +
                            "  [bookingId]" +
                            " ,[materialId]" +
                            " ,[bookedQty]" +
                            " ,[rate]" +
                            " ,[prodCatId]" +
                            " ,[prodSpecId]" +
                            " ,[prodItemId]" +
                            " ,[scheduleId]" +
                            " ,[balanceQty]" +
                            " ,[loadingLayerId]" +
                            " )" +
                " VALUES (" +
                            "  @BookingId " +
                            " ,@MaterialId " +
                            " ,@BookedQty " +
                            " ,@Rate " +
                            " ,@prodCatId " +
                            " ,@prodSpecId " +
                            " ,@ProdItemId " +
                            " ,@ScheduleId " +
                            " ,@BalanceQty " +
                            " ,@LoadingLayerId " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";

            //cmdInsert.Parameters.Add("@IdBookingExt", System.Data.SqlDbType.Int).Value = tblBookingExtTO.IdBookingExt;
            cmdInsert.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblBookingExtTO.BookingId;
            cmdInsert.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.MaterialId);
            cmdInsert.Parameters.Add("@BookedQty", System.Data.SqlDbType.NVarChar).Value = tblBookingExtTO.BookedQty;
            cmdInsert.Parameters.Add("@Rate", System.Data.SqlDbType.NVarChar).Value = tblBookingExtTO.Rate;
            cmdInsert.Parameters.Add("@prodCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.ProdCatId);
            cmdInsert.Parameters.Add("@prodSpecId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.ProdSpecId);
            cmdInsert.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.ProdItemId);
            cmdInsert.Parameters.Add("@ScheduleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.ScheduleId);
            cmdInsert.Parameters.Add("@BalanceQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.BalanceQty);
            cmdInsert.Parameters.Add("@LoadingLayerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.LoadingLayerId);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblBookingExtTO.IdBookingExt = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblBookingExt(TblBookingExtTO tblBookingExtTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblBookingExtTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblBookingExt(TblBookingExtTO tblBookingExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblBookingExtTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblBookingExtTO tblBookingExtTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblBookingExt] SET " +
                            "  [bookingId]= @BookingId" +
                            " ,[materialId]= @MaterialId" +
                            " ,[bookedQty]= @BookedQty" +
                            " ,[rate] = @Rate" +
                            " ,[prodCatId] = @prodCatId" +
                            " ,[prodSpecId] = @prodSpecId" +
                            " ,[prodItemId] = @ProdItemId" +
                            " ,[scheduleId] = @ScheduleId" +
                            " ,[balanceQty] = @BalanceQty" +
                            " ,[loadingLayerId] = @LoadingLayerId" +
                            " WHERE [idBookingExt] = @IdBookingExt ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdBookingExt", System.Data.SqlDbType.Int).Value = tblBookingExtTO.IdBookingExt;
            cmdUpdate.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblBookingExtTO.BookingId;
            cmdUpdate.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.MaterialId);
            cmdUpdate.Parameters.Add("@BookedQty", System.Data.SqlDbType.NVarChar).Value = tblBookingExtTO.BookedQty;
            cmdUpdate.Parameters.Add("@Rate", System.Data.SqlDbType.NVarChar).Value = tblBookingExtTO.Rate;
            cmdUpdate.Parameters.Add("@prodCatId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.ProdCatId);
            cmdUpdate.Parameters.Add("@prodSpecId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.ProdSpecId);
            cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.ProdItemId);
            cmdUpdate.Parameters.Add("@ScheduleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.ScheduleId);
            cmdUpdate.Parameters.Add("@BalanceQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.BalanceQty);
            cmdUpdate.Parameters.Add("@LoadingLayerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.LoadingLayerId);
            return cmdUpdate.ExecuteNonQuery();
        }

        public static int DeleteTblBookingExtBySchedule(Int32 scheduleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommandByScheduleId(scheduleId, cmdDelete);
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
        public static int ExecuteDeletionCommandByScheduleId(Int32 scheduleId, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblBookingExt] " +
            " WHERE scheduleId = " + scheduleId + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idBookingExt", System.Data.SqlDbType.Int).Value = tblBookingExtTO.IdBookingExt;
            return cmdDelete.ExecuteNonQuery();
        }

        public static int UpdateTblBookingExtBalanceQty(TblBookingExtTO tblBookingExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblBookingExt] SET " +
                              " [balanceQty] = @BalanceQty" +
                              //" ,[pendingUomQty] = @pendingUomQty" +
                              " WHERE [idBookingExt] = @IdBookingExt ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdBookingExt", System.Data.SqlDbType.Int).Value = tblBookingExtTO.IdBookingExt;
                cmdUpdate.Parameters.Add("@BalanceQty", System.Data.SqlDbType.NVarChar).Value = tblBookingExtTO.BalanceQty;
                //cmdUpdate.Parameters.Add("@pendingUomQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingExtTO.PendingUomQty);

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

        #endregion

        #region Deletion
        public static int DeleteTblBookingExt(Int32 idBookingExt)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idBookingExt, cmdDelete);
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

        public static int DeleteTblBookingExt(Int32 idBookingExt, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idBookingExt, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idBookingExt, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblBookingExt] " +
            " WHERE idBookingExt = " + idBookingExt +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idBookingExt", System.Data.SqlDbType.Int).Value = tblBookingExtTO.IdBookingExt;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
