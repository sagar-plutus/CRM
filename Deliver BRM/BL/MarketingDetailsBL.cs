using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesTrackerAPI.StaticStuff;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.BL;
using System.Data.SqlClient;
using OfficeOpenXml;
using System.IO;
using System.Security.Cryptography;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.Text;

using SalesTrackerAPI.DAL;


namespace SalesTrackerAPI.BL
{
    public class MarketingDetailsBL
    {
        #region Selection

        public static MarketingDetailsTO SelectVisitDetailsList(int visitId)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                MarketingDetailsTO marketingDetailsTO = new MarketingDetailsTO();
                // Select Visit Basic Data
                marketingDetailsTO.VisitDetailsTO = TblVisitDetailsBL.SelectTblVisitDetailsTO(visitId);

                // Select Site Requirement Data
                marketingDetailsTO.RequirementTO = TblSiteRequirementsBL.SelectSiteRequirementsTO(visitId);

                // Select Visit Additional Data
                marketingDetailsTO.AdditionalInfoTO = TblVisitAdditionalDetailsBL.SelectVisitAdditionalDetailsTO(visitId);

                // Select Visit Follow Up Information
                marketingDetailsTO.VisitFollowUpInfoTo = TblVisitFollowupInfoBL.SelectVisitFollowupInfoTO(visitId);

                // Select Visit Issue Details Data
                marketingDetailsTO.VisitIssueDetailsTOList = TblVisitIssueDetailsBL.SelectVisitIssueDetailsTOList(visitId);

                // Select Visit Project Data
                marketingDetailsTO.VisitProjectDetailsTOList = TblVisitProjectDetailsBL.SelectProjectDetailsList(visitId);

                return marketingDetailsTO;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectVisitDetailsList");
                return null;
            }
        }

        #endregion

        #region Insertion
        public static ResultMessage SaveMarketingDetails(MarketingDetailsTO marketingDetailsTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            int visitId;

            if (marketingDetailsTO.VisitDetailsTO.IdVisit <= 0)
                visitId = 0;
            else
                visitId = marketingDetailsTO.VisitDetailsTO.IdVisit;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                // Save Visit Basic Details
                if (marketingDetailsTO.VisitDetailsTO != null)
                {
                    // Insertion
                    if (marketingDetailsTO.VisitDetailsTO.IdVisit <= 0)
                    {
                        marketingDetailsTO.VisitDetailsTO.CreatedBy = marketingDetailsTO.CreatedBy;
                        marketingDetailsTO.VisitDetailsTO.CreatedOn = marketingDetailsTO.CreatedOn;

                        resultMessage = TblVisitDetailsBL.SaveNewVisitDetails(marketingDetailsTO.VisitDetailsTO, conn, tran);

                        if (resultMessage.MessageType == ResultMessageE.Information)
                        {
                            //visitId = resultMessage.Result;
                        }
                        else
                        {
                            resultMessage.DefaultBehaviour("Error While SaveNewVisitDetails");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }

                    // Updation
                    if(visitId>0)   // (marketingDetailsTO.VisitDetailsTO.IdVisit > 0)
                    {
                        marketingDetailsTO.VisitDetailsTO.UpdatedBy = marketingDetailsTO.CreatedBy;
                        marketingDetailsTO.VisitDetailsTO.UpdatedOn = marketingDetailsTO.CreatedOn;

                        resultMessage = TblVisitDetailsBL.UpdateVisitDetails(marketingDetailsTO.VisitDetailsTO, conn, tran);

                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            resultMessage.DefaultBehaviour("Error In UpdateVisitDetails While Updation");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }

                    if (resultMessage.MessageType == ResultMessageE.Information)
                    {
                        visitId = resultMessage.Result;
                    }
                }

                // Save Vist Requirements
                if (resultMessage.MessageType == ResultMessageE.Information || marketingDetailsTO.RequirementTO != null)
                {
                    // Insertion
                    if (marketingDetailsTO.RequirementTO != null && marketingDetailsTO.RequirementTO.IdSiteRequirement <= 0)
                    {
                        marketingDetailsTO.RequirementTO.VisitId = visitId;

                        marketingDetailsTO.RequirementTO.CreatedBy = marketingDetailsTO.CreatedBy;
                        marketingDetailsTO.RequirementTO.CreatedOn = marketingDetailsTO.CreatedOn;

                        resultMessage = TblSiteRequirementsBL.SaveSiteRequirements(marketingDetailsTO.RequirementTO, conn, tran);

                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            resultMessage.DefaultBehaviour("Error While SaveSiteRequirements");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }
                    // Updation
                    if (marketingDetailsTO.RequirementTO != null && marketingDetailsTO.RequirementTO.IdSiteRequirement > 0)
                    {
                        marketingDetailsTO.RequirementTO.VisitId = visitId;

                        marketingDetailsTO.RequirementTO.UpdatedBy = marketingDetailsTO.CreatedBy;
                        marketingDetailsTO.RequirementTO.UpdatedOn = marketingDetailsTO.CreatedOn;

                        resultMessage = TblSiteRequirementsBL.UpdateSiteRequirements(marketingDetailsTO.RequirementTO, conn, tran);

                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            resultMessage.DefaultBehaviour("Error In UpdateSiteRequirements While Updation");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }
                }

                // Save Visit Additional Information
                if (resultMessage.MessageType == ResultMessageE.Information || marketingDetailsTO.AdditionalInfoTO != null)
                {
                    // Insertion
                    if (marketingDetailsTO.AdditionalInfoTO != null && marketingDetailsTO.AdditionalInfoTO.IdVisitDetails <= 0)
                    {
                        marketingDetailsTO.AdditionalInfoTO.VisitId = visitId;

                        marketingDetailsTO.AdditionalInfoTO.CreatedBy = marketingDetailsTO.CreatedBy;
                        marketingDetailsTO.AdditionalInfoTO.CreatedOn = marketingDetailsTO.CreatedOn;

                        resultMessage = TblVisitAdditionalDetailsBL.SaveVisitAdditionalInfo(marketingDetailsTO.AdditionalInfoTO, conn, tran);

                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            resultMessage.DefaultBehaviour("Error While SaveVisitAdditionalInfo");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }

                    // Updation
                    if (marketingDetailsTO.AdditionalInfoTO != null && marketingDetailsTO.AdditionalInfoTO.IdVisitDetails > 0)
                    {
                        marketingDetailsTO.AdditionalInfoTO.VisitId = visitId;

                        marketingDetailsTO.AdditionalInfoTO.UpdatedBy = marketingDetailsTO.CreatedBy;
                        marketingDetailsTO.AdditionalInfoTO.UpdatedOn = marketingDetailsTO.CreatedOn;

                        resultMessage = TblVisitAdditionalDetailsBL.UpdateVisitAdditionalInfo(marketingDetailsTO.AdditionalInfoTO, conn, tran);

                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            resultMessage.DefaultBehaviour("Error In SaveVisitAdditionalInfo While Updation");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }
                }


                // Save Visit Follow Up Information
                if (resultMessage.MessageType == ResultMessageE.Information || marketingDetailsTO.VisitFollowUpInfoTo != null)
                {
                    // Insertion
                    if (marketingDetailsTO.VisitFollowUpInfoTo != null && marketingDetailsTO.VisitFollowUpInfoTo.IdProjectFollowUpInfo <= 0)
                    {
                        marketingDetailsTO.VisitFollowUpInfoTo.VisitId = visitId;

                        marketingDetailsTO.VisitFollowUpInfoTo.CreatedBy = marketingDetailsTO.CreatedBy;
                        marketingDetailsTO.VisitFollowUpInfoTo.CreatedOn = marketingDetailsTO.CreatedOn;

                        resultMessage = TblVisitFollowupInfoBL.SaveVisitFollowUpInfo(marketingDetailsTO.VisitFollowUpInfoTo, conn, tran);

                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            resultMessage.DefaultBehaviour("Error While SaveVisitFollowUpInfo");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }

                    // Updation
                    if (marketingDetailsTO.VisitFollowUpInfoTo != null && marketingDetailsTO.VisitFollowUpInfoTo.IdProjectFollowUpInfo > 0)
                    {
                        marketingDetailsTO.VisitFollowUpInfoTo.VisitId = visitId;

                        marketingDetailsTO.VisitFollowUpInfoTo.UpdatedBy = marketingDetailsTO.CreatedBy;
                        marketingDetailsTO.VisitFollowUpInfoTo.UpdatedOn = marketingDetailsTO.CreatedOn;

                        resultMessage = TblVisitFollowupInfoBL.UpdateVisitFollowUpInfo(marketingDetailsTO.VisitFollowUpInfoTo, conn, tran);

                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            resultMessage.DefaultBehaviour("Error In SaveVisitFollowUpInfo While Updation");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }
                }


                // Save Visit Issue Details
                if (resultMessage.MessageType == ResultMessageE.Information || marketingDetailsTO.VisitIssueDetailsTOList != null)
                {
                    // Insertion
                    if (marketingDetailsTO.VisitIssueDetailsTOList != null && marketingDetailsTO.VisitIssueDetailsTOList.Count > 0)
                    {

                        resultMessage = TblVisitIssueDetailsBL.SaveVisitIssueDetails(marketingDetailsTO.VisitIssueDetailsTOList, marketingDetailsTO.CreatedBy, visitId, conn, tran);

                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            resultMessage.DefaultBehaviour("Error While SaveVisitIssueDetails");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }
                }


                // Save Project Details
                if (resultMessage.MessageType == ResultMessageE.Information || marketingDetailsTO.VisitProjectDetailsTOList != null)
                {
                    if (marketingDetailsTO.VisitProjectDetailsTOList != null && marketingDetailsTO.VisitProjectDetailsTOList.Count > 0)
                    {
                        resultMessage = TblVisitProjectDetailsBL.SaveVisitProjectDetails(marketingDetailsTO.VisitProjectDetailsTOList, marketingDetailsTO.CreatedBy, visitId, conn, tran);
                    }
                    if (resultMessage.MessageType != ResultMessageE.Information)
                    {
                        resultMessage.DefaultBehaviour("Error While SaveVisitProjectDetails");
                    }
                }

                resultMessage.DefaultSuccessBehaviour();
                resultMessage.Result = visitId;
                tran.Commit();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveMarketingDetails");
                tran.Rollback();
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region updation
        public static ResultMessage UpdateMarketingDetails(MarketingDetailsTO marketingDetailsTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;

            ResultMessage resultMessage = new ResultMessage();
            int visitId;

            if (marketingDetailsTO.VisitDetailsTO.IdVisit <= 0)
                visitId = 0;
            else
                visitId = marketingDetailsTO.VisitDetailsTO.IdVisit;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                // Update Visit Basic Details
                if (marketingDetailsTO.VisitDetailsTO != null && marketingDetailsTO.VisitDetailsTO.IdVisit > 0)
                {
                    marketingDetailsTO.VisitDetailsTO.UpdatedBy = marketingDetailsTO.UpdatedBy;
                    marketingDetailsTO.VisitDetailsTO.UpdatedOn = marketingDetailsTO.UpdatedOn;

                    resultMessage = TblVisitDetailsBL.UpdateVisitDetails(marketingDetailsTO.VisitDetailsTO,conn,tran);

                    if (resultMessage.MessageType != ResultMessageE.Information)
                    {
                        resultMessage.DefaultBehaviour("Error While UpdateVisitDetails");
                        tran.Rollback();
                        return resultMessage;
                    }
                }

                // Update Vist Requirements
                if (resultMessage.MessageType==ResultMessageE.Information || marketingDetailsTO.RequirementTO != null)
                {
                    if (marketingDetailsTO.RequirementTO != null && marketingDetailsTO.RequirementTO.IdSiteRequirement>0)
                    {
                            marketingDetailsTO.RequirementTO.VisitId = visitId;

                        marketingDetailsTO.RequirementTO.UpdatedBy = marketingDetailsTO.UpdatedBy;
                        marketingDetailsTO.RequirementTO.UpdatedOn = marketingDetailsTO.UpdatedOn;

                        resultMessage = TblSiteRequirementsBL.UpdateSiteRequirements(marketingDetailsTO.RequirementTO,conn,tran);

                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            resultMessage.DefaultBehaviour("Error While UpdateSiteRequirements");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }

                    // Insert requirements details while updation
                    if (marketingDetailsTO.RequirementTO != null && marketingDetailsTO.RequirementTO.IdSiteRequirement <= 0)
                    {
                            marketingDetailsTO.RequirementTO.VisitId = visitId;

                        marketingDetailsTO.RequirementTO.CreatedBy = marketingDetailsTO.UpdatedBy;
                        marketingDetailsTO.RequirementTO.CreatedOn = marketingDetailsTO.UpdatedOn;

                        resultMessage = TblSiteRequirementsBL.SaveSiteRequirements(marketingDetailsTO.RequirementTO, conn, tran);

                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            resultMessage.DefaultBehaviour("Error In SaveSiteRequirements While Updation");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }
                }
                

                // Update Visit Additional Information
                if (resultMessage.MessageType==ResultMessageE.Information || marketingDetailsTO.AdditionalInfoTO != null)
                {
                    if (marketingDetailsTO.AdditionalInfoTO != null && marketingDetailsTO.AdditionalInfoTO.IdVisitDetails > 0)
                    {
                            marketingDetailsTO.AdditionalInfoTO.VisitId = visitId;

                        marketingDetailsTO.AdditionalInfoTO.UpdatedBy = marketingDetailsTO.UpdatedBy;
                        marketingDetailsTO.AdditionalInfoTO.UpdatedOn = marketingDetailsTO.UpdatedOn;

                        resultMessage = TblVisitAdditionalDetailsBL.UpdateVisitAdditionalInfo(marketingDetailsTO.AdditionalInfoTO,conn,tran);

                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            resultMessage.DefaultBehaviour("Error While UpdateVisitAdditionalInfo");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }

                    // Insert visit additional info while updation
                    if (marketingDetailsTO.AdditionalInfoTO != null && marketingDetailsTO.AdditionalInfoTO.IdVisitDetails <= 0)
                    {
                            marketingDetailsTO.AdditionalInfoTO.VisitId = visitId;

                        marketingDetailsTO.AdditionalInfoTO.CreatedBy = marketingDetailsTO.UpdatedBy;
                        marketingDetailsTO.AdditionalInfoTO.CreatedOn = marketingDetailsTO.UpdatedOn;

                        resultMessage = TblVisitAdditionalDetailsBL.SaveVisitAdditionalInfo(marketingDetailsTO.AdditionalInfoTO, conn, tran);

                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            resultMessage.DefaultBehaviour("Error in SaveVisitAdditionalInfo while updation");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }
                }
               
                // Update Visit Follow Up Information
                if (resultMessage.MessageType==ResultMessageE.Information || marketingDetailsTO.VisitFollowUpInfoTo != null)
                {
                    if (marketingDetailsTO.VisitFollowUpInfoTo != null && marketingDetailsTO.VisitFollowUpInfoTo.IdProjectFollowUpInfo > 0)
                    {
                            marketingDetailsTO.VisitFollowUpInfoTo.VisitId = visitId;

                        marketingDetailsTO.VisitFollowUpInfoTo.UpdatedBy = marketingDetailsTO.UpdatedBy;
                        marketingDetailsTO.VisitFollowUpInfoTo.UpdatedOn = marketingDetailsTO.UpdatedOn;

                        resultMessage = TblVisitFollowupInfoBL.UpdateVisitFollowUpInfo(marketingDetailsTO.VisitFollowUpInfoTo,conn,tran);

                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            resultMessage.DefaultBehaviour("Error While UpdateVisitFollowUpInfo");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }

                    if (marketingDetailsTO.VisitFollowUpInfoTo != null && marketingDetailsTO.VisitFollowUpInfoTo.IdProjectFollowUpInfo <= 0)
                    {
                            marketingDetailsTO.VisitFollowUpInfoTo.VisitId = visitId;

                        marketingDetailsTO.VisitFollowUpInfoTo.CreatedBy = marketingDetailsTO.UpdatedBy;
                        marketingDetailsTO.VisitFollowUpInfoTo.CreatedOn = marketingDetailsTO.UpdatedOn;

                        resultMessage = TblVisitFollowupInfoBL.SaveVisitFollowUpInfo(marketingDetailsTO.VisitFollowUpInfoTo, conn, tran);

                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            resultMessage.DefaultBehaviour("Error in SaveVisitFollowUpInfo while updation");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }
                }

                // Update Visit Issue Details
                if (resultMessage.MessageType==ResultMessageE.Information || marketingDetailsTO.VisitIssueDetailsTOList != null)
                {
                    if (marketingDetailsTO.VisitIssueDetailsTOList != null && marketingDetailsTO.VisitIssueDetailsTOList.Count > 0)
                    {

                        //// Added by Vinod Thorat On Dated:22/11/2017 for deletion of the record 
                       int DeleteSuccess = 0;

                        List<TblVisitIssueDetailsTO> visitIssueDetailsTOList = TblVisitIssueDetailsDAO.SelectVisitIssueDetailsList(marketingDetailsTO.VisitDetailsTO.IdVisit);
                        //Sudhir[30-Nov-2017] Added for Deleting Record. Changed in Vinod Code.
                        if (visitIssueDetailsTOList != null)
                        {
                            for (int p = 0; p < visitIssueDetailsTOList.Count; p++)
                            {
                                TblVisitIssueDetailsTO tblVisitIssueDetailsTO = marketingDetailsTO.VisitIssueDetailsTOList.Where(w => w.IdIssue == visitIssueDetailsTOList[p].IdIssue).FirstOrDefault();

                                if (tblVisitIssueDetailsTO == null)
                                {
                                    //delete
                                    DeleteSuccess = TblVisitIssueDetailsBL.DeleteTblVisitIssueDetailsForIssueType(visitIssueDetailsTOList[p].VisitId, visitIssueDetailsTOList[p].IssueTypeId, visitIssueDetailsTOList[p].IssueReasonId);
                                }
                            }
                        }
                        #region Vinod Thorat-Added Delete Records For IssueDetails.
                        //if (visitIssueDetailsTOList!=null)
                        //{
                        //    int LstCount = visitIssueDetailsTOList.Count;

                        //    for (int i = 0; i < LstCount; i++)
                        //    {
                        //        TblVisitIssueDetailsTO tblVisitIssDBQuery = visitIssueDetailsTOList[i];
                        //        TblVisitIssueDetailsTO tblVstIssuGUIList = marketingDetailsTO.VisitIssueDetailsTOList[i];

                        //        if (tblVstIssuGUIList.IssueReasonId == 0 && tblVstIssuGUIList.VisitId == tblVisitIssDBQuery.VisitId && tblVstIssuGUIList.IssueTypeId == tblVisitIssDBQuery.IssueTypeId)
                        //        {
                        //            DeleteSuccess = TblVisitIssueDetailsBL.DeleteTblVisitIssueDetailsForIssueType(tblVisitIssDBQuery.VisitId, tblVisitIssDBQuery.IssueTypeId, tblVisitIssDBQuery.IssueReasonId);
                        //        }

                        //        if (tblVstIssuGUIList.IssueReasonId != 0 && tblVstIssuGUIList.IssueReasonId != Convert.ToInt32(tblVisitIssDBQuery.IssueReasonId) && tblVstIssuGUIList.VisitId == tblVisitIssDBQuery.VisitId && tblVstIssuGUIList.IssueTypeId == tblVisitIssDBQuery.IssueTypeId)
                        //        {
                        //            DeleteSuccess = TblVisitIssueDetailsBL.DeleteTblVisitIssueDetailsForIssueType(tblVisitIssDBQuery.VisitId, tblVisitIssDBQuery.IssueTypeId, tblVisitIssDBQuery.IssueReasonId);
                        //            resultMessage = TblVisitIssueDetailsBL.SaveVisitIssueDetails(marketingDetailsTO.VisitIssueDetailsTOList, marketingDetailsTO.UpdatedBy, visitId, conn, tran);
                        //        }

                        //    }
                        //}
                        #endregion
                        resultMessage = TblVisitIssueDetailsBL.SaveVisitIssueDetails(marketingDetailsTO.VisitIssueDetailsTOList, marketingDetailsTO.UpdatedBy, visitId, conn, tran);

                        //}

                        //resultMessage = TblVisitIssueDetailsBL.UpdateVisitIssueDetails(marketingDetailsTO.VisitIssueDetailsTOList, marketingDetailsTO.UpdatedBy, visitId,conn,tran);
                       // resultMessage = TblVisitIssueDetailsBL.SaveVisitIssueDetails(marketingDetailsTO.VisitIssueDetailsTOList, marketingDetailsTO.UpdatedBy, visitId, conn, tran);
                    }
                    if (resultMessage.MessageType != ResultMessageE.Information)
                    {
                        resultMessage.DefaultBehaviour("Error While SaveVisitIssueDetails");
                        tran.Rollback();
                        return resultMessage;
                    }
                }
                
                // Update Visit Project Details
                if (resultMessage.MessageType==ResultMessageE.Information || marketingDetailsTO.VisitProjectDetailsTOList != null)
                {
                    if (marketingDetailsTO.VisitProjectDetailsTOList != null && marketingDetailsTO.VisitProjectDetailsTOList.Count > 0)
                    {
                        //resultMessage = TblVisitProjectDetailsBL.UpdateVisitProjectDetails(marketingDetailsTO.VisitProjectDetailsTOList, marketingDetailsTO.UpdatedBy, visitId,conn,tran);
                        resultMessage = TblVisitProjectDetailsBL.SaveVisitProjectDetails(marketingDetailsTO.VisitProjectDetailsTOList, marketingDetailsTO.UpdatedBy, visitId, conn, tran);
                    }
                    if (resultMessage.MessageType != ResultMessageE.Information)
                    {
                        resultMessage.DefaultBehaviour("Error While SaveVisitProjectDetails");
                        tran.Rollback();
                        return resultMessage;
                    }
                }
                
                resultMessage.DefaultSuccessBehaviour();
                tran.Commit();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateMarketingDetails");
                tran.Rollback();
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion


        
    }

    
}
