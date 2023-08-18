using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.BL
{
    public class TblCnfDealersBL : ITblCnfDealersBL
    {
        private readonly ITblCnfDealersDAO _iTblCnfDealersDAO;
        private readonly IConnectionString _iConnectionString;
        public TblCnfDealersBL(ITblCnfDealersDAO iTblCnfDealersDAO,
            IConnectionString iConnectionString
            )
        {
            _iConnectionString = iConnectionString;
            _iTblCnfDealersDAO = iTblCnfDealersDAO;
        }
        #region Selection

        public  List<TblCnfDealersTO> SelectAllTblCnfDealersList()
        {
           return  _iTblCnfDealersDAO.SelectAllTblCnfDealers();
        }

        public  TblCnfDealersTO SelectTblCnfDealersTO(Int32 idCnfDealerId)
        {
            return  _iTblCnfDealersDAO.SelectTblCnfDealers(idCnfDealerId);
        }

        public  List<TblCnfDealersTO> SelectAllActiveCnfDealersList(Int32 dealerId,Boolean isSpecialOnly)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblCnfDealersDAO.SelectAllTblCnfDealers(dealerId,isSpecialOnly, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public  List<TblCnfDealersTO> SelectAllActiveCnfDealersList(Int32 dealerId, Boolean isSpecialOnly,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblCnfDealersDAO.SelectAllTblCnfDealers(dealerId,isSpecialOnly, conn,tran);
        }

        #endregion

        #region Insertion
        public  int InsertTblCnfDealers(TblCnfDealersTO tblCnfDealersTO)
        {
            return _iTblCnfDealersDAO.InsertTblCnfDealers(tblCnfDealersTO);
        }

        public  int InsertTblCnfDealers(TblCnfDealersTO tblCnfDealersTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblCnfDealersDAO.InsertTblCnfDealers(tblCnfDealersTO, conn, tran);
        }

        /// <summary>
        /// Sanjay [2017-06-07] This is one time utility function for updating the cnf and dealer relationship in new table
        /// </summary>
        //public  void TransferDealerToCnfDealerReleationship()
        //{
        //    try
        //    {
        //        List<TblOrganizationTO> dealerList = _iTblOrganizationBL.SelectAllTblOrganizationList(StaticStuff.Constants.OrgTypeE.DEALER);
        //        for (int i = 0; i < dealerList.Count; i++)
        //        {
        //            TblCnfDealersTO cndDealerTO = new TblCnfDealersTO();
        //            cndDealerTO.CnfOrgId = dealerList[i].ParentId;
        //            cndDealerTO.DealerOrgId = dealerList[i].IdOrganization;
        //            cndDealerTO.CreatedBy = dealerList[i].CreatedBy;
        //            cndDealerTO.CreatedOn = dealerList[i].CreatedOn;
        //            cndDealerTO.IsActive = 1;
        //            cndDealerTO.Remark = "Primary C&f";

        //            TblCnfDealersTO existCndDealerTO = _iTblCnfDealersDAO.SelectTblCnfDealers(cndDealerTO.CnfOrgId, cndDealerTO.DealerOrgId);
        //            if(existCndDealerTO==null)
        //            {
        //               InsertTblCnfDealers(cndDealerTO);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //    }
        //}
        #endregion
        
        #region Updation
        public  int UpdateTblCnfDealers(TblCnfDealersTO tblCnfDealersTO)
        {
            return _iTblCnfDealersDAO.UpdateTblCnfDealers(tblCnfDealersTO);
        }

        public  int UpdateTblCnfDealers(TblCnfDealersTO tblCnfDealersTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblCnfDealersDAO.UpdateTblCnfDealers(tblCnfDealersTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblCnfDealers(Int32 idCnfDealerId)
        {
            return _iTblCnfDealersDAO.DeleteTblCnfDealers(idCnfDealerId);
        }

        public  int DeleteTblCnfDealers(Int32 idCnfDealerId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblCnfDealersDAO.DeleteTblCnfDealers(idCnfDealerId, conn, tran);
        }

       
        #endregion

    }
}
