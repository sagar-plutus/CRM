using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System.Linq;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblAlertUsersBL : ITblAlertUsersBL
    {
        private readonly ITblAlertUsersDAO _ITblAlertUsersDAO;
        private readonly ITblAlertActionDtlBL _iTblAlertActionDtlBL;
        public TblAlertUsersBL(ITblAlertUsersDAO itblAlertUsersDAO, ITblAlertActionDtlBL iTblAlertActionDtlBL)

        {
            _iTblAlertActionDtlBL = iTblAlertActionDtlBL;
            _ITblAlertUsersDAO = itblAlertUsersDAO;
        }
        #region Selection
        public  List<TblAlertUsersTO> SelectAllTblAlertUsersList()
        {
            return _ITblAlertUsersDAO.SelectAllTblAlertUsers();
        }

        public  TblAlertUsersTO SelectTblAlertUsersTO(Int32 idAlertUser)
        {
            return _ITblAlertUsersDAO.SelectTblAlertUsers(idAlertUser);
        }

        public  List<TblAlertUsersTO> SelectAllActiveNotAKAlertList(Int32 userId, Int32 roleId)
        {
            return _ITblAlertUsersDAO.SelectAllActiveNotAKAlertList(userId, roleId);
        }

        public  List<TblAlertUsersTO> SelectAllActiveAlertList(Int32 userId, Int32 roleId)
        {
            List<TblAlertUsersTO> list = _ITblAlertUsersDAO.SelectAllActiveAlertList(userId, roleId);
            List<TblAlertActionDtlTO> alertActionDtlTOList = _iTblAlertActionDtlBL.SelectAllTblAlertActionDtlList(userId);
            if (alertActionDtlTOList != null)
            {
                if (list != null && list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        var isAck = alertActionDtlTOList.Where(a => a.AlertInstanceId == list[i].AlertInstanceId).LastOrDefault();
                        if (isAck != null)
                        {
                            if (isAck.ResetDate != DateTime.MinValue)
                            {
                                list.RemoveAt(i);
                                i--;
                            }
                            else
                                list[i].IsAcknowledged = 1;
                        }
                    }
                }

                if (list != null && list.Count > 0)
                    list = list.OrderByDescending(a => a.IsAcknowledged == 0).ThenBy(a => a.AlertInstanceId).ToList();

            }
            return list;
        }

        #endregion

        #region Insertion
        public  int InsertTblAlertUsers(TblAlertUsersTO tblAlertUsersTO)
        {
            return _ITblAlertUsersDAO.InsertTblAlertUsers(tblAlertUsersTO);
        }

        public  int InsertTblAlertUsers(TblAlertUsersTO tblAlertUsersTO, SqlConnection conn, SqlTransaction tran)
        {
            return _ITblAlertUsersDAO.InsertTblAlertUsers(tblAlertUsersTO, conn, tran);
        }

        #endregion

        #region Updation
        public  int UpdateTblAlertUsers(TblAlertUsersTO tblAlertUsersTO)
        {
            return _ITblAlertUsersDAO.UpdateTblAlertUsers(tblAlertUsersTO);
        }

        public  int UpdateTblAlertUsers(TblAlertUsersTO tblAlertUsersTO, SqlConnection conn, SqlTransaction tran)
        {
            return _ITblAlertUsersDAO.UpdateTblAlertUsers(tblAlertUsersTO, conn, tran);
        }

        #endregion

        #region Deletion
        public  int DeleteTblAlertUsers(Int32 idAlertUser)
        {
            return _ITblAlertUsersDAO.DeleteTblAlertUsers(idAlertUser);
        }

        public  int DeleteTblAlertUsers(Int32 idAlertUser, SqlConnection conn, SqlTransaction tran)
        {
            return _ITblAlertUsersDAO.DeleteTblAlertUsers(idAlertUser, conn, tran);
        }

        #endregion

    }
}
