using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL
{
    public class TblPurchaseVehicleOtherEntryBL : ITblPurchaseVehicleOtherEntryBL
    {
        private readonly ITblPurchaseVehicleOtherEntryDAO _iTblPurchaseVehicleOtherEntryDAO;
        private readonly ITblPurchaseVehicleSpotEntryDAO _iTblPurchaseVehicleSpotEntryDAO;
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseVehicleOtherEntryBL( ITblPurchaseVehicleSpotEntryDAO iTblPurchaseVehicleSpotEntryDAO, IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
            _iTblPurchaseVehicleSpotEntryDAO = iTblPurchaseVehicleSpotEntryDAO;
          
        }
        #region Selection
        public  List<TblPurchaseVehicleSpotEntryTO> SelectAllTblPurchaseVehicleSpotEntry()
        {
            return _iTblPurchaseVehicleSpotEntryDAO.SelectAllTblPurchaseVehicleSpotEntry();
        }

        // public  List<TblPurchaseVehicleSpotEntryTO> SelectAllTblPurchaseVehicleSpotEntryList()
        // {
        //     return  _iTblPurchaseVehicleSpotEntryDAO.SelectAllTblPurchaseVehicleSpotEntryList();
        // }

        public  List<VehicleNumber> SelectAllVehicles()
        {
            return _iTblPurchaseVehicleSpotEntryDAO.SelectAllVehicles();
        }
        public  TblPurchaseVehicleSpotEntryTO SelectTblPurchaseVehicleSpotEntryTO(Int32 idVehicleSpotEntry)
        {
            List<TblPurchaseVehicleSpotEntryTO> tblPurchaseVehicleSpotEntryTOList = _iTblPurchaseVehicleSpotEntryDAO.SelectTblPurchaseVehicleSpotEntry(idVehicleSpotEntry);
            if (tblPurchaseVehicleSpotEntryTOList != null && tblPurchaseVehicleSpotEntryTOList.Count == 1)
                return tblPurchaseVehicleSpotEntryTOList[0];
            else
                return null;
        }

       
        public  ResultMessage SaveVehicleOtherEntry(TblPurchaseVehicleOthertEntryTO tblPurchaseVehicleSpotEntryTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;

            //return TblPurchaseVehicleDetailsDAO.SaveVehicleSpotEntry(tblPurchaseVehicleDetailsTO, conn, tran);
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                result = InsertTblPurchaseVehicleOtherEntry(tblPurchaseVehicleSpotEntryTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.Text = "Error While InsertTblPurchaseVehicleSpotEntry : SaveVehicleSpotEntry";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                else
                {
                    //tran.Commit();

                    #region Notifications & SMSs

                    // if (tblPurchaseVehicleDetailsTO.SystemRolesE == Constants.SystemRolesE.PURCHASE_MANAGER)
                    // {

                    //     TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                    //     List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();
                    //     List<TblUserTO> cnfUserList = BL.TblUserBL.SelectAllTblUserList(tblPurchaseVehicleDetailsTO.RoleId, conn, tran);
                    //     TblUserTO userTO = TblUserBL.SelectTblUserTO(tblPurchaseVehicleDetailsTO.CreatedBy, conn, tran);
                    //     if (userTO != null)
                    //     {
                    //         TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                    //         tblAlertUsersTO.UserId = userTO.IdUser;
                    //         tblAlertUsersTO.DeviceId = userTO.RegisteredDeviceId;
                    //         tblAlertUsersTOList.Add(tblAlertUsersTO);
                    //     }

                    //     if (cnfUserList != null && cnfUserList.Count > 0)
                    //     {
                    //         for (int a = 0; a < cnfUserList.Count; a++)
                    //         {
                    //             TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                    //             tblAlertUsersTO.UserId = cnfUserList[a].IdUser;
                    //             tblAlertUsersTO.DeviceId = cnfUserList[a].RegisteredDeviceId;
                    //             if (tblAlertUsersTOList != null && tblAlertUsersTOList.Count > 0)
                    //             {
                    //                 var isExistTO = tblAlertUsersTOList.Where(x => x.UserId == tblAlertUsersTO.UserId).FirstOrDefault();
                    //                 if (isExistTO == null)
                    //                     tblAlertUsersTOList.Add(tblAlertUsersTO);
                    //             }
                    //             else
                    //                 tblAlertUsersTOList.Add(tblAlertUsersTO);
                    //         }
                    //     }

                    //     if (tblPurchaseVehicleDetailsTO.SystemRolesE == Constants.SystemRolesE.PURCHASE_MANAGER)
                    //     {
                    //         tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.PURCHASE_VEHICLESPOT_ENTRY;
                    //         tblAlertInstanceTO.AlertAction = "NEW_VEHICLE_ARRIVAL_ENTRY";
                    //         tblAlertInstanceTO.AlertComment = "New Vehicle Number #" + tblPurchaseVehicleDetailsTO.VehicleNo + " Arrival.";
                    //         tblAlertInstanceTO.SourceDisplayId = "NEW_VEHICLE_ARRIVAL_ENTRY";
                    //         // SMS to Dealer Need to check...
                    //         Dictionary<Int32, String> orgMobileNoDCT = BL.TblOrganizationBL.SelectRegisteredMobileNoDCT(tblPurchaseVehicleDetailsTO.RoleId.ToString(), conn, tran);
                    //         if (orgMobileNoDCT != null && orgMobileNoDCT.Count == 1)
                    //         {
                    //             tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                    //             TblSmsTO smsTO = new TblSmsTO();
                    //             smsTO.MobileNo = orgMobileNoDCT[Convert.ToInt32(tblPurchaseVehicleDetailsTO.RoleId)];
                    //             smsTO.SourceTxnDesc = "NEW_VEHICLE_ARRIVAL_ENTRY";
                    //             string confirmMsg = string.Empty;
                    //             smsTO.SmsTxt = "New Vehicle arrival " + tblPurchaseVehicleDetailsTO.VehicleNo + " Arrival.";
                    //             tblAlertInstanceTO.SmsTOList.Add(smsTO);
                    //         }
                    //         //Need To conform
                    //         result = BL.TblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.PURCHASE_VEHICLESPOT_ENTRY, Convert.ToInt32(tblPurchaseVehicleDetailsTO.RoleId), conn, tran);

                    //         if (result < 0)
                    //         {
                    //             tran.Rollback();
                    //             resultMessage.MessageType = ResultMessageE.Error;
                    //             resultMessage.Text = "Error While Reseting Prev Alert";
                    //             return resultMessage;
                    //         }
                    //     }

                    //     tblAlertInstanceTO.EffectiveFromDate = tblPurchaseVehicleDetailsTO.CreatedOn;//tblPurchaseVehicleDetailsTO.UpdatedOn;
                    //     tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                    //     tblAlertInstanceTO.IsActive = 1;
                    //     tblAlertInstanceTO.SourceEntityId = Convert.ToInt32(tblPurchaseVehicleDetailsTO.RoleId);//Need to check....
                    //     tblAlertInstanceTO.RaisedOn = tblPurchaseVehicleDetailsTO.CreatedOn;// tblPurchaseVehicleDetailsTO.UpdatedOn;
                    //     tblAlertInstanceTO.RaisedBy = tblPurchaseVehicleDetailsTO.CreatedBy;// tblPurchaseVehicleDetailsTO.UpdatedBy;
                    //     tblAlertInstanceTO.IsAutoReset = 1;

                    //     ResultMessage rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                    //     if (rMessage.MessageType != ResultMessageE.Information)
                    //     {
                    //         tran.Rollback();
                    //         resultMessage.DefaultBehaviour();
                    //         resultMessage.Text = "Error While Generating Notification";
                    //         //return resultMessage;
                    //     }
                    // }

                    #endregion

                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = "Record Saved Sucessfully";
                    resultMessage.Result = 1;
                    tran.Commit();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save : SaveVehicleSpotEntry";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }

        }


        #endregion

        #region Insertion
        public  int InsertTblPurchaseVehicleSpotEntry(TblPurchaseVehicleOthertEntryTO tblPurchaseVehicleOtherEntryTO)
        {
            return _iTblPurchaseVehicleOtherEntryDAO.InsertTblPurchaseVehicleOtherEntry(tblPurchaseVehicleOtherEntryTO);
        }

        public  int InsertTblPurchaseVehicleOtherEntry(TblPurchaseVehicleOthertEntryTO tblPurchaseVehicleOtherEntryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleOtherEntryDAO.InsertTblPurchaseVehicleOtherEntry(tblPurchaseVehicleOtherEntryTO, conn, tran);
        }

        #endregion

        #region Updation
        public  int UpdateTblPurchaseVehicleSpotEntry(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO)
        {
            return _iTblPurchaseVehicleSpotEntryDAO.UpdateTblPurchaseVehicleSpotEntry(tblPurchaseVehicleSpotEntryTO);
        }

        public  int UpdateTblPurchaseVehicleSpotEntry(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleSpotEntryDAO.UpdateTblPurchaseVehicleSpotEntry(tblPurchaseVehicleSpotEntryTO, conn, tran);
        }

        #endregion

        #region Deletion
        public  int DeleteTblPurchaseVehicleSpotEntry(Int32 idVehicleSpotEntry)
        {
            return _iTblPurchaseVehicleSpotEntryDAO.DeleteTblPurchaseVehicleSpotEntry(idVehicleSpotEntry);
        }

        public  int DeleteTblPurchaseVehicleSpotEntry(Int32 idVehicleSpotEntry, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleSpotEntryDAO.DeleteTblPurchaseVehicleSpotEntry(idVehicleSpotEntry, conn, tran);
        }

        #endregion

    }
}
