using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;

namespace SalesTrackerAPI.BL
{
    public class TblCnfDealersBL
    {
        #region Selection

        public static List<TblCnfDealersTO> SelectAllTblCnfDealersList()
        {
           return  TblCnfDealersDAO.SelectAllTblCnfDealers();
        }

        public static TblCnfDealersTO SelectTblCnfDealersTO(Int32 idCnfDealerId)
        {
            return  TblCnfDealersDAO.SelectTblCnfDealers(idCnfDealerId);
        }

        public static List<TblCnfDealersTO> SelectAllActiveCnfDealersList(Int32 dealerId,Boolean isSpecialOnly)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblCnfDealersDAO.SelectAllTblCnfDealers(dealerId,isSpecialOnly, conn, tran);
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

        public static List<TblCnfDealersTO> SelectAllActiveCnfDealersList(Int32 dealerId, Boolean isSpecialOnly,SqlConnection conn,SqlTransaction tran)
        {
            return TblCnfDealersDAO.SelectAllTblCnfDealers(dealerId,isSpecialOnly, conn,tran);
        }

        #endregion

        #region Insertion
        public static int InsertTblCnfDealers(TblCnfDealersTO tblCnfDealersTO)
        {
            return TblCnfDealersDAO.InsertTblCnfDealers(tblCnfDealersTO);
        }

        public static int InsertTblCnfDealers(TblCnfDealersTO tblCnfDealersTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblCnfDealersDAO.InsertTblCnfDealers(tblCnfDealersTO, conn, tran);
        }

        /// <summary>
        /// Sanjay [2017-06-07] This is one time utility function for updating the cnf and dealer relationship in new table
        /// </summary>
        public static void TransferDealerToCnfDealerReleationship()
        {
            try
            {
                List<TblOrganizationTO> dealerList = BL.TblOrganizationBL.SelectAllTblOrganizationList(StaticStuff.Constants.OrgTypeE.DEALER);
                for (int i = 0; i < dealerList.Count; i++)
                {
                    TblCnfDealersTO cndDealerTO = new TblCnfDealersTO();
                    cndDealerTO.CnfOrgId = dealerList[i].ParentId;
                    cndDealerTO.DealerOrgId = dealerList[i].IdOrganization;
                    cndDealerTO.CreatedBy = dealerList[i].CreatedBy;
                    cndDealerTO.CreatedOn = dealerList[i].CreatedOn;
                    cndDealerTO.IsActive = 1;
                    cndDealerTO.Remark = "Primary C&f";

                    TblCnfDealersTO existCndDealerTO = TblCnfDealersDAO.SelectTblCnfDealers(cndDealerTO.CnfOrgId, cndDealerTO.DealerOrgId);
                    if(existCndDealerTO==null)
                    {
                        BL.TblCnfDealersBL.InsertTblCnfDealers(cndDealerTO);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
        }
        #endregion
        
        #region Updation
        public static int UpdateTblCnfDealers(TblCnfDealersTO tblCnfDealersTO)
        {
            return TblCnfDealersDAO.UpdateTblCnfDealers(tblCnfDealersTO);
        }

        public static int UpdateTblCnfDealers(TblCnfDealersTO tblCnfDealersTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblCnfDealersDAO.UpdateTblCnfDealers(tblCnfDealersTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblCnfDealers(Int32 idCnfDealerId)
        {
            return TblCnfDealersDAO.DeleteTblCnfDealers(idCnfDealerId);
        }

        public static int DeleteTblCnfDealers(Int32 idCnfDealerId, SqlConnection conn, SqlTransaction tran)
        {
            return TblCnfDealersDAO.DeleteTblCnfDealers(idCnfDealerId, conn, tran);
        }

       
        #endregion

    }
}
