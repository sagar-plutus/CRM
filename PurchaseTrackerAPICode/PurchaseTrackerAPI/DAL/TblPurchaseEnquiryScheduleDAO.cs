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
    public class TblPurchaseEnquiryScheduleDAO : ITblPurchaseEnquiryScheduleDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseEnquiryScheduleDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tblPurchaseScheduleSummary.*, dimState.stateName AS vehicleStateName, vehicleTypeDesc " +
                                  " FROM tblPurchaseScheduleSummary tblPurchaseScheduleSummary " +
                                  " LEFT JOIN dimState dimState ON dimState.idState = tblPurchaseScheduleSummary.vehicleStateId " +
                                  " LEFT JOIN dimVehicleType ON dimVehicleType.idVehicleType = tblPurchaseScheduleSummary.vehicleTypeId ";


            //" SELECT *, [tblPurchaseEnquirySchedule].Qty as ScheduleQty,[tblPurchaseVehicleDetails].Qty as Qty, tblProductItem.itemName FROM [tblPurchaseEnquirySchedule]" +
            //                   " Left JOIN [tblPurchaseVehicleDetails] ON[tblPurchaseVehicleDetails].schedulePurchaseId =[tblPurchaseEnquirySchedule].[idSchedulePurchase]" +
            //                   " INNER JOIN tblProductItem ON tblProductItem.idProdItem = [tblPurchaseVehicleDetails].prodItemId ";

            //" INNER JOIN tblProductItem ON tblProductItem.idProdItem = tblPurchaseEnquirySchedule.prodItemId " +
            //                   " Left JOIN [tblPurchaseVehicleDetails]" +
            //                   " ON[tblPurchaseVehicleDetails].schedulePurchaseId =[tblPurchaseEnquirySchedule].[idSchedulePurchase]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblPurchaseEnquiryScheduleTO> SelectAllTblPurchaseEnquiryScheduleList(Int32 purchaseEnquiryId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM ( " +
                                        " SELECT tblPurchaseScheduleSummary.*, dimState.stateName AS vehicleStateName, vehicleTypeDesc, " +
                                        " MAX(statusId) OVER(PARTITION BY purchaseEnquiryId, supplierId, vehicleNo, scheduleDate  ORDER BY createdOn DESC) AS maxStatus, " +
                                        " ROW_NUMBER() OVER(PARTITION BY purchaseEnquiryId, supplierId, vehicleNo, scheduleDate ORDER BY createdOn asc) AS RN " +
                                        " FROM tblPurchaseScheduleSummary tblPurchaseScheduleSummary " +
                                        " LEFT JOIN dimState dimState ON dimState.idState = tblPurchaseScheduleSummary.vehicleStateId " +
                                        " LEFT JOIN dimVehicleType ON dimVehicleType.idVehicleType = tblPurchaseScheduleSummary.vehicleTypeId " +
                                        " ) SRC WHERE RN = 1 AND purchaseEnquiryId =" + purchaseEnquiryId; // + " ORDER BY VehicleNo asc";
                
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryScheduleTO> list = ConvertDTToList(reader);
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


        public  List<TblPurchaseEnquiryScheduleTO> SelectAllTblPurchaseEnquiryScheduleList(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran)
        {

            SqlCommand cmdSelect = new SqlCommand();
            try
            {

                cmdSelect.CommandText = SqlSelectQuery() + " WHERE purchaseEnquiryId =" + purchaseEnquiryId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryScheduleTO> list = ConvertDTToList(reader);
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

        public  List<TblPurchaseEnquiryScheduleTO> ConvertDTToList(SqlDataReader tblPurchaseEnquiryScheduleTODT)
        {
            List<TblPurchaseEnquiryScheduleTO> tblPurchaseEnquiryScheduleTOList = new List<TblPurchaseEnquiryScheduleTO>();
            if (tblPurchaseEnquiryScheduleTODT != null)
            {
                while (tblPurchaseEnquiryScheduleTODT.Read())
                {
                    TblPurchaseEnquiryScheduleTO tblPurchaseEnquiryScheduleTONew = new TblPurchaseEnquiryScheduleTO();
                    if (tblPurchaseEnquiryScheduleTODT["idPurchaseScheduleSummary"] != DBNull.Value)
                        tblPurchaseEnquiryScheduleTONew.IdSchedulePurchase = Convert.ToInt32(tblPurchaseEnquiryScheduleTODT["idPurchaseScheduleSummary"].ToString());
                    if (tblPurchaseEnquiryScheduleTODT["purchaseEnquiryId"] != DBNull.Value)
                        tblPurchaseEnquiryScheduleTONew.PurchaseEnquiryId = Convert.ToInt32(tblPurchaseEnquiryScheduleTODT["purchaseEnquiryId"].ToString());
                    
                    if (tblPurchaseEnquiryScheduleTODT["scheduleDate"] != DBNull.Value)
                        tblPurchaseEnquiryScheduleTONew.ScheduleDate = Convert.ToDateTime(tblPurchaseEnquiryScheduleTODT["scheduleDate"].ToString());


                    if (tblPurchaseEnquiryScheduleTODT["Qty"] != DBNull.Value)
                    {
                        tblPurchaseEnquiryScheduleTONew.Qty = Convert.ToDouble(tblPurchaseEnquiryScheduleTODT["Qty"].ToString());
                    }
                    //if (tblPurchaseEnquiryScheduleTODT["ScheduleQty"] != DBNull.Value)
                    //{
                    //    tblPurchaseEnquiryScheduleTONew.ScheduleQty = Convert.ToDouble(tblPurchaseEnquiryScheduleTODT["ScheduleQty"].ToString());
                    //}

                    if (tblPurchaseEnquiryScheduleTODT["remark"] != DBNull.Value)
                        tblPurchaseEnquiryScheduleTONew.Remark = Convert.ToString(tblPurchaseEnquiryScheduleTODT["remark"].ToString());

                    if (tblPurchaseEnquiryScheduleTODT["createdBy"] != DBNull.Value)
                        tblPurchaseEnquiryScheduleTONew.CreatedBy = Convert.ToInt32(tblPurchaseEnquiryScheduleTODT["createdBy"].ToString());
                    if (tblPurchaseEnquiryScheduleTODT["createdOn"] != DBNull.Value)
                        tblPurchaseEnquiryScheduleTONew.CreatedOn = Convert.ToDateTime(tblPurchaseEnquiryScheduleTODT["createdOn"].ToString());

                    if (tblPurchaseEnquiryScheduleTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseEnquiryScheduleTONew.UpdatedBy = Convert.ToInt32(tblPurchaseEnquiryScheduleTODT["updatedBy"].ToString());
                    if (tblPurchaseEnquiryScheduleTODT["updatedOn"] != DBNull.Value)
                        tblPurchaseEnquiryScheduleTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseEnquiryScheduleTODT["updatedOn"].ToString());

                    //remove/not consider vehicleNo and itemId column from schedule
                    if (tblPurchaseEnquiryScheduleTODT["vehicleNo"] != DBNull.Value)
                        tblPurchaseEnquiryScheduleTONew.VehicleNo = Convert.ToString(tblPurchaseEnquiryScheduleTODT["vehicleNo"].ToString());

                    // if (tblPurchaseEnquiryScheduleTODT["maxStatus"] != DBNull.Value)
                    //     tblPurchaseEnquiryScheduleTONew.StatusId = Convert.ToInt32(tblPurchaseEnquiryScheduleTODT["maxStatus"]);
                    else if (tblPurchaseEnquiryScheduleTODT["statusId"] != DBNull.Value)
                        tblPurchaseEnquiryScheduleTONew.StatusId = Convert.ToInt32(tblPurchaseEnquiryScheduleTODT["statusId"]);

                    //if (tblPurchaseEnquiryScheduleTODT["itemName"] != DBNull.Value)
                    //    tblPurchaseEnquiryScheduleTONew.ItemName = Convert.ToString(tblPurchaseEnquiryScheduleTODT["itemName"].ToString());

                    //  tblPurchaseEnquiryScheduleTONew.ScheduleDateStr = tblPurchaseEnquiryScheduleTONew.ScheduleDate.ToString("dd/MMM/yyyy");

                    if (tblPurchaseEnquiryScheduleTODT["driverName"] != DBNull.Value)
                        tblPurchaseEnquiryScheduleTONew.DriverName = Convert.ToString(tblPurchaseEnquiryScheduleTODT["driverName"]);

                    if (tblPurchaseEnquiryScheduleTODT["driverContactNo"] != DBNull.Value)
                        tblPurchaseEnquiryScheduleTONew.DriverContactNo = Convert.ToString(tblPurchaseEnquiryScheduleTODT["driverContactNo"]);

                    if (tblPurchaseEnquiryScheduleTODT["transporterName"] != DBNull.Value)
                        tblPurchaseEnquiryScheduleTONew.TransporterName = Convert.ToString(tblPurchaseEnquiryScheduleTODT["transporterName"]);

                    if (tblPurchaseEnquiryScheduleTODT["vehicleTypeId"] != DBNull.Value)
                        tblPurchaseEnquiryScheduleTONew.VehicleTypeId = Convert.ToInt32(tblPurchaseEnquiryScheduleTODT["vehicleTypeId"].ToString());

                    if (tblPurchaseEnquiryScheduleTODT["vehicleTypeDesc"] != DBNull.Value)
                        tblPurchaseEnquiryScheduleTONew.VehicleTypeName = Convert.ToString(tblPurchaseEnquiryScheduleTODT["vehicleTypeDesc"]);

                    if (tblPurchaseEnquiryScheduleTODT["freight"] != DBNull.Value)
                        tblPurchaseEnquiryScheduleTONew.Freight = Convert.ToDouble(tblPurchaseEnquiryScheduleTODT["freight"].ToString());

                    if (tblPurchaseEnquiryScheduleTODT["containerNo"] != DBNull.Value)
                        tblPurchaseEnquiryScheduleTONew.ContainerNo = Convert.ToString(tblPurchaseEnquiryScheduleTODT["containerNo"]);

                    if (tblPurchaseEnquiryScheduleTODT["vehicleStateId"] != DBNull.Value)
                        tblPurchaseEnquiryScheduleTONew.VehicleStateId = Convert.ToInt32(tblPurchaseEnquiryScheduleTODT["vehicleStateId"].ToString());

                    if (tblPurchaseEnquiryScheduleTODT["vehicleStateName"] != DBNull.Value)
                        tblPurchaseEnquiryScheduleTONew.VehicleStateName = Convert.ToString(tblPurchaseEnquiryScheduleTODT["vehicleStateName"]);

                    if (tblPurchaseEnquiryScheduleTODT["location"] != DBNull.Value)
                        tblPurchaseEnquiryScheduleTONew.Location = Convert.ToString(tblPurchaseEnquiryScheduleTODT["location"]);

                    tblPurchaseEnquiryScheduleTOList.Add(tblPurchaseEnquiryScheduleTONew);
                }
            }
            return tblPurchaseEnquiryScheduleTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblPurchaseEnquirySchedule(TblPurchaseEnquiryScheduleTO tblPurchaseEnquiryScheduleTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseEnquiryScheduleTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblPurchaseEnquiryScheduleTO tblPurchaseEnquiryScheduleTO, SqlCommand cmdInsert)
        {

            String sqlQuery = @"INSERT INTO [tblPurchaseScheduleSummary] " +
                              " ([purchaseEnquiryId],[supplierId],[vehicleNo],[scheduleDate],[qty],[remark],[statusId],[calculatedMetalCost],[baseMetalCost],[padta],[driverName],[driverContactNo],[transporterName], [vehicleTypeId], [freight], [containerNo], [vehicleStateId], [location], [createdBy],[createdOn],[updatedBy],[updatedOn])" +
            " VALUES (" +
                            "  @PurchaseEnquiryId " +
                            " ,@supplierId " +
                            " ,@vehicleNo " +
                            " ,@ScheduleDate " +
                            " ,@Qty " +
                            " ,@Remark " +
                            " ,@statusId " +
                            " ,@calculatedMetalCost " +
                            " ,@baseMetalCost " +
                            " ,@padta " +
                            " ,@driverName " +
                            " ,@driverContactNo " +
                            " ,@transporterName " +
                            " ,@vehicleTypeId " +
                            " ,@freight " +
                            " ,@containerNo " +
                            " ,@vehicleStateId " +
                            " ,@location " +
                            " ,@CreatedBy " +
                            " ,@CreatedOn " +
                            " ,@UpdatedBy " +
                            " ,@UpdatedOn " +
                            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";
            cmdInsert.Parameters.Add("@PurchaseEnquiryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryScheduleTO.PurchaseEnquiryId);
            cmdInsert.Parameters.Add("@supplierId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryScheduleTO.SupplierId);//(tblPurchaseEnquiryScheduleTO.ProdItemId);
            cmdInsert.Parameters.Add("@vehicleNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryScheduleTO.VehicleNo);
            cmdInsert.Parameters.Add("@ScheduleDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryScheduleTO.ScheduleDate);
            cmdInsert.Parameters.Add("@Qty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryScheduleTO.Qty);
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryScheduleTO.Remark);
            cmdInsert.Parameters.Add("@statusId", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryScheduleTO.StatusId;
            cmdInsert.Parameters.Add("@calculatedMetalCost", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryScheduleTO.CalculatedMetalCost);
            cmdInsert.Parameters.Add("@baseMetalCost", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryScheduleTO.BaseMetalCost);
            cmdInsert.Parameters.Add("@padta", System.Data.SqlDbType.Decimal).Value = (tblPurchaseEnquiryScheduleTO.Padta);

            //Nikhil[2018-05-25] Add
            cmdInsert.Parameters.Add("@driverName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryScheduleTO.DriverName);
            cmdInsert.Parameters.Add("@driverContactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryScheduleTO.DriverContactNo);
            cmdInsert.Parameters.Add("@transporterName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryScheduleTO.TransporterName);
            cmdInsert.Parameters.Add("@vehicleTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryScheduleTO.VehicleTypeId);
            cmdInsert.Parameters.Add("@freight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryScheduleTO.Freight);
            cmdInsert.Parameters.Add("@containerNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryScheduleTO.ContainerNo);
            cmdInsert.Parameters.Add("@vehicleStateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryScheduleTO.VehicleStateId);
            cmdInsert.Parameters.Add("@location", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryScheduleTO.Location);

            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryScheduleTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseEnquiryScheduleTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryScheduleTO.UpdatedBy);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryScheduleTO.UpdatedOn);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblPurchaseEnquiryScheduleTO.IdSchedulePurchase = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Deletion
        //public  int DeleteTblBookingSchedule(Int32 idSchedule)
        //{
        //    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdDelete = new SqlCommand();
        //    try
        //    {
        //        conn.Open();
        //        cmdDelete.Connection = conn;
        //        return ExecuteDeletionCommand(idSchedule, cmdDelete);
        //    }
        //    catch (Exception ex)
        //    {

        //        return 0;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        cmdDelete.Dispose();
        //    }
        //}

        public  int DeleteTblPurchaseEnquirySchedule(Int32 idSchedulePurchase, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idSchedulePurchase, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idSchedulePurchase, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseScheduleSummary]" +
            " WHERE idPurchaseScheduleSummary = " + idSchedulePurchase + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idSchedule", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.IdSchedule;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
