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
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblEnquiryDtlBL : ITblEnquiryDtlBL
    {
        private readonly IConnectionString _iConnectionString;
        private readonly Icommondao _iCommonDAO;
        private readonly ITblEnquiryDtlDAO _iTblEnquiryDtlBL;
        public TblEnquiryDtlBL(ITblEnquiryDtlDAO iTblEnquiryDtlDAO, Icommondao icommondao, IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
            _iCommonDAO = icommondao;
            _iTblEnquiryDtlBL = iTblEnquiryDtlDAO;
        }
        #region Selection

        public  List<TblEnquiryDtlTO> SelectAllTblEnquiryDtlList()
        {
            return _iTblEnquiryDtlBL.SelectAllTblEnquiryDtl();
        }

        /// <summary>
        ///  [2017-12-01]Vijaymala:Added to get enquiry detail List  of  organization
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public  List<TblEnquiryDtlTO> SelectAllTblEnquiryDtl(String dealerIds)
        {
            return _iTblEnquiryDtlBL.SelectAllTblEnquiryDtl(dealerIds);
        }


        /// <summary>
        ///  [2017-11-29]Vijaymala:Added to get enquiry detail of particular organization
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public  List<TblEnquiryDtlTO> SelectEnquiryDtlList(Int32 dealerId)
        {
            return _iTblEnquiryDtlBL.SelectEnquiryDtlList(dealerId);
        }

        public  TblEnquiryDtlTO SelectTblEnquiryDtl(Int32 idEnquiryDtl)
        {
            return _iTblEnquiryDtlBL.SelectTblEnquiryDtl(idEnquiryDtl);
        }



        #endregion

        #region Insertion
        public  int InsertTblEnquiryDtl(TblEnquiryDtlTO tblEnquiryDtlTO)
        {
            return _iTblEnquiryDtlBL.InsertTblEnquiryDtl(tblEnquiryDtlTO);
        }

        public  int InsertTblEnquiryDtl(TblEnquiryDtlTO tblEnquiryDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEnquiryDtlBL.InsertTblEnquiryDtl(tblEnquiryDtlTO, conn, tran);
        }

        /// <summary>
        /// [04-12-2017]Vijaymala :Added to save  enquiry detail of organization which exports from excel
        /// </summary>
        /// <param name="tblEnquiryDtlTO"></param>
        /// <returns></returns>

        public  ResultMessage SaveOrgEnquiryDtl(List<TblEnquiryDtlTO> tblEnquiryDtlTOList,Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region validations

                //for (int i = 0; i < tblEnquiryDtlTOList.Count; i++)
                //{
                //    TblEnquiryDtlTO tblEnquiryDtlTO = tblEnquiryDtlTOList[i];
                //    TblOrganizationTO tblOrganizationTO = BL.TblOrganizationBL.SelectTblOrganizationTOByEnqRefId(tblEnquiryDtlTO.EnqRefId);
                //    if(tblOrganizationTO !=null)
                //    {
                //        tblEnquiryDtlTO.IsMatch = 1;
                //    }
                //    else
                //    {
                //        tblEnquiryDtlTO.IsMatch = 0;
                //    }
                //    // tblEnquiryDtlTO.CreatedBy = Convert.ToInt32(loginUserId);
                //    // tblEnquiryDtlTO.CreatedOn =  _iCommonDAO.ServerDateTime;
                //    // tblEnquiryDtlTO.IsActive = 1;

                //    //#region 1. Deactivate All Previous Organization Details
                //    //TblEnquiryDtlTO activeEnquiryDtlTO = _iTblEnquiryDtlBL.SelectOrganizationEnquiryDtl(tblEnquiryDtlTO.EnqRefId);
                //    //if (activeEnquiryDtlTO != null)
                //    //{
                //    //    tblEnquiryDtlTO.OrganizationId = activeEnquiryDtlTO.OrganizationId;
                //    //    result = DAL._iTblEnquiryDtlBL.DeactivateOrgEnqDeatls(activeEnquiryDtlTO.IdEnquiryDtl, conn, tran);
                //    //    if (result < 0)
                //    //    {
                //    //        tran.Rollback();
                //    //        resultMessage.DefaultBehaviour();
                //    //        resultMessage.Text = "Error While Deactivating Organization Enquiry Details";
                //    //        return resultMessage;
                //    //    }
                //    //}

                //    //#endregion

                //    //#region 2. Save Organization Enquiry Details
                //    //result = InsertTblEnquiryDtl(tblEnquiryDtlTO, conn, tran);
                //    //if (result < 0)
                //    //{
                //    //    tran.Rollback();
                //    //    resultMessage.DefaultBehaviour();
                //    //    resultMessage.Text = "Error While Inserting Organization Enquiry Details";
                //    //    return resultMessage;
                //    //}
                //}

                ////List<TblEnquiryDtlTO> listTemp = tblEnquiryDtlTOList.Where(w => w.IsMatch == 0).ToList();
                ////if (listTemp != null && listTemp.Count > 0)
                ////{
                ////    //return tblEnquiryDtlTOList
                ////}


                #endregion

                #region Delete previous records

                result = DeleteTblEnquiryDtl(conn, tran);
                if(result == -1)
                {
                    tran.Rollback();
                    resultMessage.Text = "Exception Error While Delete TblEnquiryDtl";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = -1;
                    return resultMessage;

                }

                #endregion


                #region Insert New Records

                DateTime createdOn =  _iCommonDAO.ServerDateTime;

                for (int i = 0; i < tblEnquiryDtlTOList.Count; i++)
                {
                    TblEnquiryDtlTO tblEnquiryDtlTO = tblEnquiryDtlTOList[i];
                    //TblOrganizationTO tblOrganizationTO = BL.TblOrganizationBL.SelectTblOrganizationTOByEnqRefId(tblEnquiryDtlTO.EnqRefId);
                    //if (tblOrganizationTO != null)
                    //    tblEnquiryDtlTO.OrganizationId = tblOrganizationTO.IdOrganization;
                    //else
                    //    tblEnquiryDtlTO.OrganizationId = 0;

                    tblEnquiryDtlTO.CreatedBy = Convert.ToInt32(loginUserId);
                    tblEnquiryDtlTO.CreatedOn = createdOn;

                    result = InsertTblEnquiryDtl(tblEnquiryDtlTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.Text = "Error While Inserting Organization Enquiry Details";
                        return resultMessage;
                    }

                }
                #endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                resultMessage.Text = "Enquiry Details Of Organization Updated Successfully.";
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.Text = "Exception Error While Record Save in BL : SaveOrgEnquiryDtl";
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

        #region Updation
        public  int UpdateTblEnquiryDtl(TblEnquiryDtlTO tblEnquiryDtlTO)
        {
            return _iTblEnquiryDtlBL.UpdateTblEnquiryDtl(tblEnquiryDtlTO);
        }

        public  int UpdateTblEnquiryDtl(TblEnquiryDtlTO tblEnquiryDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEnquiryDtlBL.UpdateTblEnquiryDtl(tblEnquiryDtlTO, conn, tran);
        }

        #endregion

        #region Deletion
        public  int DeleteTblEnquiryDtl(Int32 idEnquiryDtl)
        {
            return _iTblEnquiryDtlBL.DeleteTblEnquiryDtl(idEnquiryDtl);
        }

        public  int DeleteTblEnquiryDtl(Int32 idEnquiryDtl, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEnquiryDtlBL.DeleteTblEnquiryDtl(idEnquiryDtl, conn, tran);
        }

        public  int DeleteTblEnquiryDtl(SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEnquiryDtlBL.DeleteTblEnquiryDtl(conn, tran);
        }


        #endregion
    }
}



