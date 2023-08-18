using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.BL
{
    public class TblVisitIssueDetailsBL
    {
        #region Selection
        public static DataTable SelectAllTblVisitIssueDetails()
        {
            return TblVisitIssueDetailsDAO.SelectAllTblVisitIssueDetails();
        }

        public static List<TblVisitIssueDetailsTO> SelectAllTblVisitIssueDetailsList()
        {
            DataTable tblVisitIssueDetailsTODT = TblVisitIssueDetailsDAO.SelectAllTblVisitIssueDetails();
            return ConvertDTToList(tblVisitIssueDetailsTODT);
        }

        public static TblVisitIssueDetailsTO SelectTblVisitIssueDetailsTO(Int32 idIssue)
        {
            DataTable tblVisitIssueDetailsTODT = TblVisitIssueDetailsDAO.SelectTblVisitIssueDetails(idIssue);
            List<TblVisitIssueDetailsTO> tblVisitIssueDetailsTOList = ConvertDTToList(tblVisitIssueDetailsTODT);
            if (tblVisitIssueDetailsTOList != null && tblVisitIssueDetailsTOList.Count == 1)
                return tblVisitIssueDetailsTOList[0];
            else
                return null;
        }

        public static List<TblVisitIssueDetailsTO> ConvertDTToList(DataTable tblVisitIssueDetailsTODT)
        {
            List<TblVisitIssueDetailsTO> tblVisitIssueDetailsTOList = new List<TblVisitIssueDetailsTO>();
            if (tblVisitIssueDetailsTODT != null)
            {
                
            }
            return tblVisitIssueDetailsTOList;
        }


        public static List<TblVisitIssueDetailsTO> SelectVisitIssueDetailsTOList(Int32 visitId)
        {
            List<TblVisitIssueDetailsTO> visitIssueDetailsTOList = TblVisitIssueDetailsDAO.SelectVisitIssueDetailsList(visitId);
            if (visitIssueDetailsTOList != null )
                return visitIssueDetailsTOList;
            else
                return null;
        }

        #endregion

        #region Insertion
        public static int InsertTblVisitIssueDetails(TblVisitIssueDetailsTO tblVisitIssueDetailsTO)
        {
            return TblVisitIssueDetailsDAO.InsertTblVisitIssueDetails(tblVisitIssueDetailsTO);
        }

        public static int InsertTblVisitIssueDetails(TblVisitIssueDetailsTO tblVisitIssueDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitIssueDetailsDAO.InsertTblVisitIssueDetails(tblVisitIssueDetailsTO, conn, tran);
        }


        // Vaibhav [9-Oct-2017] added to insert visit issue details
        public static ResultMessage SaveVisitIssueDetails(List<TblVisitIssueDetailsTO> tblVisitIssueDetailsTO, Int32 createdBy,Int32 visitId,SqlConnection conn,SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {
                foreach (var visitIssueDetailsTO in tblVisitIssueDetailsTO)
                {
                    // Insertion
                    if (visitIssueDetailsTO.IdIssue <= 0)
                    {
                        if (visitIssueDetailsTO.VisitId <= 0)
                            visitIssueDetailsTO.VisitId = visitId;

                        if (visitIssueDetailsTO.IssueReasonId <= 0)
                        {
                            TblVisitIssueReasonsTO visitIssueReasonsTO = new TblVisitIssueReasonsTO();

                            visitIssueReasonsTO.IssueTypeId = visitIssueDetailsTO.IssueTypeId;
                            visitIssueReasonsTO.VisitIssueReasonName = visitIssueDetailsTO.IssueReason;
                            visitIssueReasonsTO.VisitIssueReasonDesc = visitIssueDetailsTO.IssueReason;
                            visitIssueReasonsTO.IsActive = 1;

                            result = TblVisitIssueReasonsBL.InsertTblVisitIssueReasons(ref visitIssueReasonsTO,conn,tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While InsertTblVisitIssueReasons");
                                tran.Rollback();
                                return resultMessage;
                            }
                            visitIssueDetailsTO.IssueReasonId = visitIssueReasonsTO.IdVisitIssueReasons;
                        }

                            visitIssueDetailsTO.CreatedBy = createdBy;
                        visitIssueDetailsTO.CreatedOn = Constants.ServerDateTime;

                        result = InsertTblVisitIssueDetails(visitIssueDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error While InsertTblVisitIssueDetails");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }

                    // Updation
                    else
                    { 
                        if (visitIssueDetailsTO.VisitId <= 0)
                            visitIssueDetailsTO.VisitId = visitId;

                        if (visitIssueDetailsTO.IssueReasonId <= 0)
                        {
                            TblVisitIssueReasonsTO visitIssueReasonsTO = new TblVisitIssueReasonsTO();

                            visitIssueReasonsTO.IssueTypeId = visitIssueDetailsTO.IssueTypeId;
                            visitIssueReasonsTO.VisitIssueReasonName = visitIssueDetailsTO.IssueReason;
                            visitIssueReasonsTO.VisitIssueReasonDesc = visitIssueDetailsTO.IssueReason;
                            visitIssueReasonsTO.IsActive = 1;

                            result = TblVisitIssueReasonsBL.InsertTblVisitIssueReasons(ref visitIssueReasonsTO, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While InsertTblVisitIssueReasons");
                                tran.Rollback();
                                return resultMessage;
                            }
                            visitIssueDetailsTO.IssueReasonId = visitIssueReasonsTO.IdVisitIssueReasons;
                        }

                        visitIssueDetailsTO.UpdatedBy = createdBy;
                        visitIssueDetailsTO.UpdatedOn = Constants.ServerDateTime;

                        result = UpdateTblVisitIssueDetails(visitIssueDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error While UpdateTblVisitIssueDetails");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }
                }
               
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveVisitIssueDetails");
                tran.Rollback();
                return resultMessage;
            }
        }

        #endregion

        #region Updation
        public static int UpdateTblVisitIssueDetails(TblVisitIssueDetailsTO tblVisitIssueDetailsTO)
        {
            return TblVisitIssueDetailsDAO.UpdateTblVisitIssueDetails(tblVisitIssueDetailsTO);
        }

        public static int UpdateTblVisitIssueDetails(TblVisitIssueDetailsTO tblVisitIssueDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitIssueDetailsDAO.UpdateTblVisitIssueDetails(tblVisitIssueDetailsTO, conn, tran);
        }

        // Vaibhav [1-Nov-2017] added to update visit issue details
        public static ResultMessage UpdateVisitIssueDetails(List<TblVisitIssueDetailsTO> tblVisitIssueDetailsTO, Int32 updatedBy, Int32 visitId,SqlConnection conn,SqlTransaction tran)
        {

            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {

                foreach (var visitIssueDetailsTO in tblVisitIssueDetailsTO)
                {
                    if (visitIssueDetailsTO.IdIssue > 0)
                    {
                        if (visitIssueDetailsTO.VisitId <= 0)
                            visitIssueDetailsTO.VisitId = visitId;

                        if (visitIssueDetailsTO.IssueReasonId <= 0)
                        {
                            TblVisitIssueReasonsTO visitIssueReasonsTO = new TblVisitIssueReasonsTO();

                            visitIssueReasonsTO.IssueTypeId = visitIssueDetailsTO.IssueTypeId;
                            visitIssueReasonsTO.VisitIssueReasonName = visitIssueDetailsTO.IssueReason;
                            visitIssueReasonsTO.VisitIssueReasonDesc = visitIssueDetailsTO.IssueReason;
                            visitIssueReasonsTO.IsActive = 1;

                            result = TblVisitIssueReasonsBL.InsertTblVisitIssueReasons(ref visitIssueReasonsTO, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While InsertTblVisitIssueReasons");
                                tran.Rollback();
                                return resultMessage;
                            }
                            visitIssueDetailsTO.IssueReasonId = visitIssueReasonsTO.IdVisitIssueReasons;
                        }

                        visitIssueDetailsTO.UpdatedBy = updatedBy;
                        visitIssueDetailsTO.UpdatedOn = Constants.ServerDateTime;

                        result = UpdateTblVisitIssueDetails(visitIssueDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error While UpdateTblVisitIssueDetails");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }

                    // Insert visit issue details while updation
                    if (visitIssueDetailsTO.IdIssue <= 0)
                    {
                        if (visitIssueDetailsTO.VisitId <= 0)
                            visitIssueDetailsTO.VisitId = visitId;

                        if (visitIssueDetailsTO.IssueReasonId <= 0)
                        {
                            TblVisitIssueReasonsTO visitIssueReasonsTO = new TblVisitIssueReasonsTO();

                            visitIssueReasonsTO.IssueTypeId = visitIssueDetailsTO.IssueTypeId;
                            visitIssueReasonsTO.VisitIssueReasonName = visitIssueDetailsTO.IssueReason;
                            visitIssueReasonsTO.VisitIssueReasonDesc = visitIssueDetailsTO.IssueReason;
                            visitIssueReasonsTO.IsActive = 1;

                            result = TblVisitIssueReasonsBL.InsertTblVisitIssueReasons(ref visitIssueReasonsTO, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While InsertTblVisitIssueReasons");
                                tran.Rollback();
                                return resultMessage;
                            }
                            visitIssueDetailsTO.IssueReasonId = visitIssueReasonsTO.IdVisitIssueReasons;
                        }

                        visitIssueDetailsTO.CreatedBy = updatedBy;
                        visitIssueDetailsTO.CreatedOn = Constants.ServerDateTime;

                        result = InsertTblVisitIssueDetails(visitIssueDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error in InsertTblVisitIssueDetails while updation");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateVisitIssueDetails");
                tran.Rollback();
                return resultMessage;
            }
        }


        #endregion

        #region Deletion
        public static int DeleteTblVisitIssueDetails(Int32 idIssue)
        {
            return TblVisitIssueDetailsDAO.DeleteTblVisitIssueDetails(idIssue);
        }

        public static int DeleteTblVisitIssueDetails(Int32 idIssue, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitIssueDetailsDAO.DeleteTblVisitIssueDetails(idIssue, conn, tran);
        }

        #region Vinod Thorat[20-NOV-2017] Added for Deleting the Record based on Id's.

        public static int DeleteTblVisitIssueDetailsForIssueType(Int32 VisitId, Int32 issueTypeId, Int32 issueReasonId)
        {
            return TblVisitIssueDetailsDAO.DeleteTblVisitIssueDetailsWithType(VisitId, issueTypeId, issueReasonId);
        }
        #endregion

        #endregion

    }
}
