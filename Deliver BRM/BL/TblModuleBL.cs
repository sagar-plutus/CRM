using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.BL {
    public class TblModuleBL {
        #region Selection
        public static TblModuleTO SelectTblModuleTO (Int32 idModule) {
            return TblModuleDAO.SelectTblModule (idModule);
        }
        public static List<DropDownTO> SelectAllTblModuleList () {
            return TblModuleDAO.SelectAllTblModule ();
        }
        public static List<TblModuleTO> SelectTblModuleList () {
            return TblModuleDAO.SelectTblModuleList ();
        }

        public static TblModuleTO SelectTblModuleTO (Int32 idModule, SqlConnection conn, SqlTransaction tran) {
            return TblModuleDAO.SelectTblModule (idModule, conn, tran);
        }

        #endregion

        #region Insertion
        public static int InsertTblModule (TblModuleTO tblModuleTO) {
            return TblModuleDAO.InsertTblModule (tblModuleTO);
        }

        public static int InsertTblModule (TblModuleTO tblModuleTO, SqlConnection conn, SqlTransaction tran) {
            return TblModuleDAO.InsertTblModule (tblModuleTO, conn, tran);
        }

        #endregion

        #region UserTracking
        public static int InsertTblModuleCommHis (TblModuleCommHisTO tblModuleCommhisTO, SqlConnection conn, SqlTransaction tran) {
            if (conn == null) {

                conn = new SqlConnection (Startup.ConnectionString);
            }
            if (tblModuleCommhisTO.InTime == DateTime.MinValue) {
                tblModuleCommhisTO.InTime = Constants.ServerDateTime;

            }

            return TblModuleDAO.InserttblModuleCommunicationHistory (tblModuleCommhisTO, conn, tran);
        }
        public static int UpdateTblModuleCommHis (TblModuleCommHisTO tblModuleCommhisTO, SqlConnection conn, SqlTransaction tran) {
            if (conn == null) {

                conn = new SqlConnection (Startup.ConnectionString);
            }
            if (tblModuleCommhisTO.OutTime == DateTime.MinValue) {
                tblModuleCommhisTO.OutTime = Constants.ServerDateTime;
            }
            //  tblModuleCommhisTO.OutTime=DateTime.MinValue;

            return TblModuleDAO.UpdatetblModuleCommunicationHistory (tblModuleCommhisTO, conn, tran);
        }

        public static int UpdateTblModuleCommHisBeforeLogin (TblModuleCommHisTO tblModuleCommhisTO, SqlConnection conn, SqlTransaction tran) {
            if (conn == null) {
                conn = new SqlConnection (Startup.ConnectionString);
            }

            tran = null;
            conn.Open ();
            tran = conn.BeginTransaction ();
            if (tblModuleCommhisTO.OutTime == DateTime.MinValue) {
                tblModuleCommhisTO.OutTime = Constants.ServerDateTime;
            }
            int result = TblModuleDAO.UpdatetblModuleCommunicationHistory (tblModuleCommhisTO, conn, tran);
            if (result < 0) {
                tran.Rollback ();
                return 0;
            }
            result = TblModuleDAO.UpdatetblLogin (tblModuleCommhisTO, conn, tran);
            if (result != 1) {
                tran.Rollback ();
                return 0;
            }
            tran.Commit ();
            return 1;
        }
        public static int UpdateTblModuleCommHisBeforeLoginForAPK (TblModuleCommHisTO tblModuleCommhisTO, SqlConnection conn, SqlTransaction tran) {

            if (tblModuleCommhisTO.OutTime == DateTime.MinValue) {
                tblModuleCommhisTO.OutTime = Constants.ServerDateTime;
            }
            int result = TblModuleDAO.UpdatetblModuleCommunicationHistory (tblModuleCommhisTO, conn, tran);
            if (result < 0) {
                tran.Rollback ();
                return 0;
            }
            result = TblModuleDAO.UpdatetblLogin (tblModuleCommhisTO, conn, tran);
            if (result != 1) {
                tran.Rollback ();
                return 0;
            }

            return 1;
        }
        public static int FindLatestLoginIdForLogout (TblModuleCommHisTO tblModuleCommhisTO, SqlConnection conn, SqlTransaction tran) {
            int isImpLogin = 1;
            tblModuleCommhisTO.UserLogin = tblModuleCommhisTO.LoginId.ToString ();
            while (isImpLogin == 1) {

                tblModuleCommhisTO.LoginId = TblModuleDAO.GetPreviousLoginId (tblModuleCommhisTO, conn, tran);
                isImpLogin = TblModuleDAO.CheckIsImpPersonFromLoginId (tblModuleCommhisTO, conn, tran);
                // if(!String.IsNullOrEmpty(tblModuleCommhisTO.UserLogin)) 
                if (isImpLogin == 1) {
                    tblModuleCommhisTO.UserLogin += ",";
                    tblModuleCommhisTO.UserLogin += tblModuleCommhisTO.LoginId.ToString ();
                }
                // tblModuleCommhisTO.UserLogin+= tblModuleCommhisTO.LoginId.ToString();

            }

            if (isImpLogin == 0) {

                int result = UpdateTblModuleCommHisBeforeLogin (tblModuleCommhisTO, conn, tran);
                return result;
            }
            return 0;
        }
        public static int UpdateAllTblModuleCommHis (TblModuleCommHisTO tblModuleCommhisTO, SqlConnection conn, SqlTransaction tran) {
            try {

                if (conn == null) {
                    conn = new SqlConnection (Startup.ConnectionString);
                }
                tran = null;
                conn.Open ();
                tran = conn.BeginTransaction ();
                tblModuleCommhisTO.OutTime = Constants.ServerDateTime;
                int result = TblModuleDAO.UpdateAlltblModuleCommunicationHistory (tblModuleCommhisTO, conn, tran);

                if (result < 0) {
                    tran.Rollback ();
                    return result;
                }
                result = TblModuleDAO.UpdateAlltblLogin (tblModuleCommhisTO, conn, tran);
                if (result == 0) {
                    tran.Rollback ();
                    return result;
                }
                tran.Commit ();
                return 1;
            } catch (Exception ex) {
                return 0;
            } finally {
                conn.Close ();
            }
            return 0;
        }
        public static List<TblModuleCommHisTO> GetAlltblModuleCommHis (int userId) {

            return TblModuleDAO.GetAllTblModuleCommHis (userId);
        }
        public static TblModuleTO GetAllActiveAllowedCnt (int moduleId, int userId, int loginId) {

            return TblModuleDAO.GetAllActiveAllowedCnt (moduleId, userId, loginId);
        }

        public static List<TblModuleCommHisTO> GetActiveCntDetails (int moduleId) {

            return TblModuleDAO.GetActiveUserDetail (moduleId);
        }
        public static int UpdateInsertTblModuleCommHis (TblModuleCommHisTO tblModuleCommhisTO, SqlConnection conn, SqlTransaction tran) {
            try {

                if (conn == null) {
                    conn = new SqlConnection (Startup.ConnectionString);
                }
                tran = null;
                conn.Open ();
                tran = conn.BeginTransaction ();
                tblModuleCommhisTO.OutTime = Constants.ServerDateTime;

                int result = TblModuleDAO.UpdatetblModuleCommunicationHistory (tblModuleCommhisTO, conn, tran);
                if (result < 0) {
                    tran.Rollback ();
                    return result;
                }
                if (tblModuleCommhisTO.InTime == DateTime.MinValue) {
                    tblModuleCommhisTO.InTime = Constants.ServerDateTime;
                    tblModuleCommhisTO.OutTime = DateTime.MinValue;
                }
                result = TblModuleDAO.InserttblModuleCommunicationHistory (tblModuleCommhisTO, conn, tran);
                if (result != 1) {
                    tran.Rollback ();
                    return result;
                }
                tran.Commit ();
                return result;
            } catch (Exception ex) {
                return 0;
            } finally {
                conn.Close ();
            }
            return 0;
        }

        #endregion

        #region Updation
        public static int UpdateTblModule (TblModuleTO tblModuleTO) {
            return TblModuleDAO.UpdateTblModule (tblModuleTO);
        }

        public static int UpdateTblModule (TblModuleTO tblModuleTO, SqlConnection conn, SqlTransaction tran) {
            return TblModuleDAO.UpdateTblModule (tblModuleTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblModule (Int32 idModule) {
            return TblModuleDAO.DeleteTblModule (idModule);
        }

        public static int DeleteTblModule (Int32 idModule, SqlConnection conn, SqlTransaction tran) {
            return TblModuleDAO.DeleteTblModule (idModule, conn, tran);
        }

        #endregion

    }
}