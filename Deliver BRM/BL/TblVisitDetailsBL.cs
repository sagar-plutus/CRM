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
    public class TblVisitDetailsBL
    {
        #region Selection
        public static List<TblVisitDetailsTO> SelectAllTblVisitDetails()
        {
            return TblVisitDetailsDAO.SelectAllTblVisitDetails();
        }

        public static List<TblVisitDetailsTO> SelectAllTblVisitDetailsList()
        {
            List<TblVisitDetailsTO> tblVisitDetailsTODT = TblVisitDetailsDAO.SelectAllTblVisitDetails();
            //return ConvertDTToList(tblVisitDetailsTODT);
            return null;
        }

        public static TblVisitDetailsTO SelectTblVisitDetailsTO(int idVisit)
        {
            TblVisitDetailsTO tblVisitDetailsTO = TblVisitDetailsDAO.SelectTblVisitDetails(idVisit);
            if (tblVisitDetailsTO != null)
                return tblVisitDetailsTO;
            else
                return null;
        }

        public static List<TblVisitDetailsTO> ConvertDTToList(DataTable tblVisitDetailsTODT)
        {
            List<TblVisitDetailsTO> tblVisitDetailsTOList = new List<TblVisitDetailsTO>();
            if (tblVisitDetailsTODT != null)
            {

            }
            return tblVisitDetailsTOList;
        }


        // Vaibhav [28-Oct-2017] added to select max visit id 
        public static Int32 SelectLastVisitId(SqlConnection conn, SqlTransaction tran)
        {
            Int32 result = TblVisitDetailsDAO.SelectLastVisitId(conn, tran);
            if (result > 0)
                return result;
            else
                return -1;
        }

        // Vaibhav [28-Oct-2017] added to select all visit details list
        public static List<TblVisitDetailsTO> SelectAllVisitDetailsList()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<TblVisitDetailsTO> visitDetailsTOList = TblVisitDetailsDAO.SelectAllTblVisitDetails();
                if (visitDetailsTOList != null)
                    return visitDetailsTOList;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllVisitDetails");
                return null;
            }
        }

        #endregion

        #region Insertion
        public static int InsertTblVisitDetails(TblVisitDetailsTO tblVisitDetailsTO)
        {
            return TblVisitDetailsDAO.InsertTblVisitDetails(tblVisitDetailsTO);
        }

        public static int InsertTblVisitDetails(ref TblVisitDetailsTO tblVisitDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitDetailsDAO.InsertTblVisitDetails(ref tblVisitDetailsTO, conn, tran);
        }

        public static ResultMessage SaveNewVisitDetails(TblVisitDetailsTO tblVisitDetailsTO, SqlConnection conn, SqlTransaction tran)
        {

            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            // Visit person mapping insertion
            List<TblVisitPersonDetailsTO> tblVisitPersonDetailsTOForInsertion = new List<TblVisitPersonDetailsTO>();
           
            try
            {

                // Insert new visit purpose
                if (tblVisitDetailsTO.VisitPurposeTO != null)
                {
                    TblVisitPurposeTO tblVisitPurposeTO = new TblVisitPurposeTO();

                    if (tblVisitDetailsTO.VisitPurposeTO.Value == 0)
                    {
                        tblVisitPurposeTO.VisitPurposeDesc = tblVisitDetailsTO.VisitPurposeTO.Text;
                        tblVisitPurposeTO.CreatedBy = tblVisitDetailsTO.CreatedBy;
                        tblVisitPurposeTO.CreatedOn = tblVisitDetailsTO.CreatedOn;
                        tblVisitPurposeTO.VisitTypeId = tblVisitDetailsTO.VisitTypeId;
                        tblVisitPurposeTO.IsActive = 1;

                        result = TblVisitPurposeBL.InsertTblVisitPurpose(ref tblVisitPurposeTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error While InsertTblVisitPurpose");
                            tran.Rollback();
                            return resultMessage;
                        }
                        else
                            tblVisitDetailsTO.VisitPurposeId = tblVisitPurposeTO.IdVisitPurpose;
                    }
                    else
                    {
                        tblVisitDetailsTO.VisitPurposeId = tblVisitDetailsTO.VisitPurposeTO.Value;
                    }
                }

                // Insert new payment term
                if (tblVisitDetailsTO.PaymentTermTO != null)
                {
                    TblPaymentTermTO tblPaymentTermTO = new TblPaymentTermTO();

                    if (tblVisitDetailsTO.PaymentTermTO.Value == 0)
                    {
                        tblPaymentTermTO.PaymentTermDisplayName = tblVisitDetailsTO.PaymentTermTO.Text;
                        tblPaymentTermTO.PaymentTermDesc = tblVisitDetailsTO.PaymentTermTO.Text;
                        tblPaymentTermTO.CreatedBy = tblVisitDetailsTO.CreatedBy;
                        tblPaymentTermTO.CreatedOn = tblVisitDetailsTO.CreatedOn;
                        tblPaymentTermTO.IsActive = 1;

                        result = TblPaymentTermBL.InsertTblPaymentTerm(ref tblPaymentTermTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error While InsertTblPaymentTerm");
                            tran.Rollback();
                            return resultMessage;
                        }
                        else
                            tblVisitDetailsTO.PaymentTermId = tblPaymentTermTO.IdPaymentTerm;
                    }
                    else
                    {
                        tblVisitDetailsTO.PaymentTermId = tblVisitDetailsTO.PaymentTermTO.Value;
                    }
                }

                // Insert new site status

                if (tblVisitDetailsTO.SiteStatusTO != null)
                {
                    if (tblVisitDetailsTO.SiteStatusTO.Value == 0)
                    {
                        TblSiteStatusTO tblSiteStatusTO = new TblSiteStatusTO();
                        tblSiteStatusTO.SiteStatusDisplayName = tblVisitDetailsTO.SiteStatusTO.Text;
                        tblSiteStatusTO.SiteStatusDesc = tblVisitDetailsTO.SiteStatusTO.Text;
                        tblSiteStatusTO.CreatedBy = tblVisitDetailsTO.CreatedBy;
                        tblSiteStatusTO.CreatedOn = tblVisitDetailsTO.CreatedOn;
                        tblSiteStatusTO.IsActive = 1;

                        result = TblSiteStatusBL.InsertTblSiteStatus(ref tblSiteStatusTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error While InsertTblSiteStatus");
                            tran.Rollback();
                            return resultMessage;
                        }
                        else
                            tblVisitDetailsTO.SiteStatusId = tblSiteStatusTO.IdSiteStatus;
                    }
                    else
                    {
                        tblVisitDetailsTO.SiteStatusId = tblVisitDetailsTO.SiteStatusTO.Value;
                    }
                }

                // Insert visit site type
                if (tblVisitDetailsTO.SiteTypeTOList != null && tblVisitDetailsTO.SiteTypeTOList.Count > 0)
                {
                    List<TblSiteTypeTO> siteTypeList = tblVisitDetailsTO.SiteTypeTOList.OrderBy(ele => ele.DimSiteTypeId).ToList();

                    foreach (var sitetype in siteTypeList)
                    {
                        if (sitetype.DimSiteTypeId == (int)Constants.VisitSiteTypeE.SITE_CATEGORY)
                        {
                            if (sitetype.IdSiteType == 0)
                            {
                                TblSiteTypeTO tblSiteTypeTO = new TblSiteTypeTO();
                                tblSiteTypeTO.SiteTypeDisplayName = sitetype.SiteTypeDisplayName;
                                tblSiteTypeTO.SiteTypeDesc = sitetype.SiteTypeDisplayName;
                                tblSiteTypeTO.ParentSiteTypeId = sitetype.ParentSiteTypeId;
                                tblSiteTypeTO.DimSiteTypeId = sitetype.DimSiteTypeId;
                                tblSiteTypeTO.CreatedBy = tblVisitDetailsTO.CreatedBy;
                                tblSiteTypeTO.CreatedOn = tblVisitDetailsTO.CreatedOn;
                                tblSiteTypeTO.IsActive = 1;

                                result = TblSiteTypeBL.InsertTblSiteType(ref tblSiteTypeTO, conn, tran);
                                if (result != 1)
                                {
                                    resultMessage.DefaultBehaviour("Error While InsertTblSiteType");
                                    tran.Rollback();
                                    return resultMessage;
                                }
                                else
                                    tblVisitDetailsTO.SiteTypeId = tblSiteTypeTO.IdSiteType;
                            }
                            else
                            {
                                tblVisitDetailsTO.SiteTypeId = sitetype.IdSiteType;
                            }
                        }
                        if (sitetype.DimSiteTypeId == (int)Constants.VisitSiteTypeE.SITE_SUBCATEGORY)
                        {
                            if (sitetype.IdSiteType == 0)
                            {
                                TblSiteTypeTO tblSiteTypeTO = new TblSiteTypeTO();
                                tblSiteTypeTO.SiteTypeDisplayName = sitetype.SiteTypeDisplayName;
                                tblSiteTypeTO.SiteTypeDesc = sitetype.SiteTypeDisplayName;

                                if (sitetype.ParentSiteTypeId <= 0)
                                    tblSiteTypeTO.ParentSiteTypeId = tblVisitDetailsTO.SiteTypeId;
                                else
                                    tblSiteTypeTO.ParentSiteTypeId = sitetype.ParentSiteTypeId;

                                tblSiteTypeTO.CreatedBy = tblVisitDetailsTO.CreatedBy;
                                tblSiteTypeTO.CreatedOn = tblVisitDetailsTO.CreatedOn;
                                tblSiteTypeTO.DimSiteTypeId = sitetype.DimSiteTypeId;
                                tblSiteTypeTO.IsActive = 1;

                                result = TblSiteTypeBL.InsertTblSiteType(ref tblSiteTypeTO, conn, tran);
                                if (result != 1)
                                {
                                    resultMessage.DefaultBehaviour("Error While InsertTblSiteType");
                                    tran.Rollback();
                                    return resultMessage;
                                }
                                else
                                    tblVisitDetailsTO.SiteTypeId = tblSiteTypeTO.IdSiteType;
                            }
                            else
                            {
                                tblVisitDetailsTO.SiteTypeId = sitetype.IdSiteType;
                            }
                        }
                    }
                }

                // Visit person mapping
                if (tblVisitDetailsTO.VisitPersonDetailsTOList != null && tblVisitDetailsTO.VisitPersonDetailsTOList.Count > 0)
                {
                    foreach (var person in tblVisitDetailsTO.VisitPersonDetailsTOList)
                    {
                        //TblPersonTO personTO = new TblPersonTO();

                        int userId = tblVisitDetailsTO.CreatedBy;

                        if (person.IdPerson == 0)
                        {
                            person.SalutationId = Constants.DefaultCompanyId; //set default 1

                            person.CreatedBy = tblVisitDetailsTO.CreatedBy;
                            person.CreatedOn = tblVisitDetailsTO.CreatedOn;

                            result = BL.TblPersonBL.InsertTblPerson(person, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While InsertTblPerson");
                                tran.Rollback();
                                return resultMessage;
                            }
                            TblVisitPersonDetailsTO tblVisitPersonDetailsTO = new TblVisitPersonDetailsTO();
                            // Defination of visit person details
                            //tblVisitPersonDetailsTO.VisitId = SelectLastVisitId(conn, tran);
                            tblVisitPersonDetailsTO.PersonId = person.IdPerson;
                            tblVisitPersonDetailsTO.VisitRoleId = person.VisitRoleId;
                            tblVisitPersonDetailsTOForInsertion.Add(tblVisitPersonDetailsTO);
                            // For site owner 
                            if (person.IsSiteOwner == 1)
                            {
                                tblVisitDetailsTO.SiteOwnerId = person.IdPerson;
                            }

                            switch (person.VisitRoleId)
                            {

                                case (int)Constants.VisitPersonE.SITE_ARCHITECT:
                                    tblVisitDetailsTO.SiteArchitectId = person.IdPerson;
                                    break;
                                case (int)Constants.VisitPersonE.SITE_STRUCTURAL_ENGG:
                                    tblVisitDetailsTO.SiteStructuralEnggId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.SITE_CONTRACTOR:
                                    if (person.IsSiteOwner == 0)
                                        tblVisitDetailsTO.ContractorId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.SITE_STEEL_PURCHASE_AUTHORITY:
                                    tblVisitDetailsTO.PurchaseAuthorityPersonId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.DEALER_MEETING_WITH:
                                    tblVisitDetailsTO.DealerMeetingWithId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.DEALER_VISIT_ALONG_WITH:
                                    tblVisitDetailsTO.DealerVisitAlongWithId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.INFLUENCER_VISITED_BY:
                                    tblVisitDetailsTO.InfluencerVisitedBy = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.INFLUENCER_RECOMMANDEDED_BY:
                                    tblVisitDetailsTO.InfluencerRecommandedBy = person.IdPerson;
                                    break;
                            }
                        }
                        else
                        {
                            switch (person.VisitRoleId)
                            {
                                case (int)Constants.VisitPersonE.SITE_ARCHITECT:
                                    tblVisitDetailsTO.SiteArchitectId = person.IdPerson;
                                    break;
                                case (int)Constants.VisitPersonE.SITE_STRUCTURAL_ENGG:
                                    tblVisitDetailsTO.SiteStructuralEnggId = person.IdPerson;
                                    break;
                                case (int)Constants.VisitPersonE.SITE_CONTRACTOR:
                                    if (person.IsSiteOwner == 0)
                                    {
                                        tblVisitDetailsTO.ContractorId = person.IdPerson;
                                    }
                                    break;

                                case (int)Constants.VisitPersonE.SITE_STEEL_PURCHASE_AUTHORITY:
                                    tblVisitDetailsTO.PurchaseAuthorityPersonId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.DEALER:
                                    tblVisitDetailsTO.DealerId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.DEALER_MEETING_WITH:
                                    tblVisitDetailsTO.DealerMeetingWithId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.DEALER_VISIT_ALONG_WITH:
                                    tblVisitDetailsTO.DealerVisitAlongWithId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.INFLUENCER_VISITED_BY:
                                    tblVisitDetailsTO.InfluencerVisitedBy = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.INFLUENCER_RECOMMANDEDED_BY:
                                    tblVisitDetailsTO.InfluencerRecommandedBy = person.IdPerson;
                                    break;
                            }

                            result = BL.TblPersonBL.UpdateTblPerson(person, conn, tran);
                            if (result <= 0)
                            {
                                resultMessage.DefaultBehaviour("Error in UpdateTblPerson while updation");
                            }

                            // Visit person mapping insertion
                            TblVisitPersonDetailsTO visitPersonDetailsTO = new TblVisitPersonDetailsTO();

                           // visitPersonDetailsTO.VisitId = SelectLastVisitId(conn, tran) + 1;
                            visitPersonDetailsTO.PersonId = person.IdPerson;
                            visitPersonDetailsTO.VisitRoleId = person.VisitRoleId;
                            tblVisitPersonDetailsTOForInsertion.Add(visitPersonDetailsTO);
                            //int personCount = TblVisitPersonDetailsBL.SelectVisitPersonCount(visitPersonDetailsTO.VisitId, visitPersonDetailsTO.PersonId, visitPersonDetailsTO.VisitRoleId, conn, tran);
                            //if (personCount <= 0)
                            //    result = TblVisitPersonDetailsBL.InsertTblVisitPersonDetails(visitPersonDetailsTO, conn, tran);
                            //else
                            //    result = TblVisitPersonDetailsBL.UpdateTblPersonVisitDetails(visitPersonDetailsTO, conn, tran);

                            if (result < 0)
                            {
                                resultMessage.DefaultBehaviour("Error in UpdateTblPersonVisitDetails while updation");
                                tran.Rollback();
                                return resultMessage;
                            }
                        }
                    }
                }

                result = InsertTblVisitDetails(ref tblVisitDetailsTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While InsertTblVisitDetails");
                    tran.Rollback();
                    return resultMessage;
                }
                //Sudhir[01-DEC-2017] Added for Insertion of Records in tblVisitpersonDetails.
                for (int i = 0; i < tblVisitPersonDetailsTOForInsertion.Count; i++)
                {
                    tblVisitPersonDetailsTOForInsertion[i].VisitId = tblVisitDetailsTO.IdVisit;

                    int personCount = TblVisitPersonDetailsBL.SelectVisitPersonCount(tblVisitPersonDetailsTOForInsertion[i].VisitId, tblVisitPersonDetailsTOForInsertion[i].PersonId, tblVisitPersonDetailsTOForInsertion[i].VisitRoleId, conn, tran);
                    if (personCount <= 0)
                        result = TblVisitPersonDetailsBL.InsertTblVisitPersonDetails(tblVisitPersonDetailsTOForInsertion[i], conn, tran);
                    else
                        result = TblVisitPersonDetailsBL.UpdateTblPersonVisitDetails(tblVisitPersonDetailsTOForInsertion[i], conn, tran);
                    //result = TblVisitPersonDetailsBL.InsertTblVisitPersonDetails(tblVisitPersonDetailsTOForInsertion[i], conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour(" Error While InsertTblPersonVisitDetails");
                        tran.Rollback();
                        return resultMessage;
                    }
                }
                resultMessage.DefaultSuccessBehaviour();
                resultMessage.Result = tblVisitDetailsTO.IdVisit;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveNewVisitBasicDetails");
                tran.Rollback();
                return resultMessage;
            }
        }

        #endregion

        #region Updation
        //public static int UpdateTblVisitDetails(TblVisitDetailsTO tblVisitDetailsTO)
        //{
        //    return TblVisitDetailsDAO.UpdateTblVisitDetails(tblVisitDetailsTO);
        //}

        public static int UpdateTblVisitDetails(TblVisitDetailsTO tblVisitDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitDetailsDAO.UpdateTblVisitDetails(tblVisitDetailsTO, conn, tran);
        }

        // Vaibhav [1-Nov-2017] Added to update visit basic details
        public static ResultMessage UpdateVisitDetails(TblVisitDetailsTO tblVisitDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {
                // Insert new visit purpose
                if (tblVisitDetailsTO.VisitPurposeTO != null)
                {
                    TblVisitPurposeTO tblVisitPurposeTO = new TblVisitPurposeTO();

                    if (tblVisitDetailsTO.VisitPurposeTO.Value == 0)
                    {
                        tblVisitPurposeTO.VisitPurposeDesc = tblVisitDetailsTO.VisitPurposeTO.Text;
                        tblVisitPurposeTO.CreatedBy = tblVisitDetailsTO.CreatedBy;
                        tblVisitPurposeTO.CreatedOn = tblVisitDetailsTO.CreatedOn;
                        tblVisitPurposeTO.VisitTypeId = tblVisitDetailsTO.VisitTypeId;
                        tblVisitPurposeTO.IsActive = 1;

                        result = TblVisitPurposeBL.InsertTblVisitPurpose(ref tblVisitPurposeTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error While InsertTblVisitPurpose");
                            tran.Rollback();
                            return resultMessage;
                        }
                        else
                            tblVisitDetailsTO.VisitPurposeId = tblVisitPurposeTO.IdVisitPurpose;
                    }
                    else
                    {
                        tblVisitDetailsTO.VisitPurposeId = tblVisitDetailsTO.VisitPurposeTO.Value;
                    }
                }
                else
                {
                    tblVisitDetailsTO.VisitPurposeId = 0;
                }

                // Insert new payment term
                if (tblVisitDetailsTO.PaymentTermTO != null)
                {
                    TblPaymentTermTO tblPaymentTermTO = new TblPaymentTermTO();

                    if (tblVisitDetailsTO.PaymentTermTO.Value == 0)
                    {
                        tblPaymentTermTO.PaymentTermDisplayName = tblVisitDetailsTO.PaymentTermTO.Text;
                        tblPaymentTermTO.PaymentTermDesc = tblVisitDetailsTO.PaymentTermTO.Text;
                        tblPaymentTermTO.CreatedBy = tblVisitDetailsTO.CreatedBy;
                        tblPaymentTermTO.CreatedOn = tblVisitDetailsTO.CreatedOn;
                        tblPaymentTermTO.IsActive = 1;

                        result = TblPaymentTermBL.InsertTblPaymentTerm(ref tblPaymentTermTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error While InsertTblPaymentTerm");
                            tran.Rollback();
                            return resultMessage;
                        }
                        else
                            tblVisitDetailsTO.PaymentTermId = tblPaymentTermTO.IdPaymentTerm;
                    }
                    else
                    {
                        tblVisitDetailsTO.PaymentTermId = tblVisitDetailsTO.PaymentTermTO.Value;
                    }
                }
                else
                {
                    tblVisitDetailsTO.PaymentTermId = 0;
                }

                // Insert new site status

                if (tblVisitDetailsTO.SiteStatusTO != null)
                {
                    if (tblVisitDetailsTO.SiteStatusTO.Value == 0)
                    {
                        TblSiteStatusTO tblSiteStatusTO = new TblSiteStatusTO();
                        tblSiteStatusTO.SiteStatusDisplayName = tblVisitDetailsTO.SiteStatusTO.Text;
                        tblSiteStatusTO.SiteStatusDesc = tblVisitDetailsTO.SiteStatusTO.Text;
                        tblSiteStatusTO.CreatedBy = tblVisitDetailsTO.CreatedBy;
                        tblSiteStatusTO.CreatedOn = tblVisitDetailsTO.CreatedOn;
                        tblSiteStatusTO.IsActive = 1;

                        result = TblSiteStatusBL.InsertTblSiteStatus(ref tblSiteStatusTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error While InsertTblSiteStatus");
                            tran.Rollback();
                            return resultMessage;
                        }
                        else
                            tblVisitDetailsTO.SiteStatusId = tblSiteStatusTO.IdSiteStatus;
                    }
                    else
                    {
                        tblVisitDetailsTO.SiteStatusId = tblVisitDetailsTO.SiteStatusTO.Value;
                    }
                }
                else
                {
                    tblVisitDetailsTO.SiteStatusId = 0;
                }

                // Insert visit site type
                if (tblVisitDetailsTO.SiteTypeTOList != null && tblVisitDetailsTO.SiteTypeTOList.Count > 0)
                {
                    List<TblSiteTypeTO> siteTypeList = tblVisitDetailsTO.SiteTypeTOList.OrderBy(ele => ele.DimSiteTypeId).ToList();

                    foreach (var sitetype in siteTypeList)
                    {
                        if (sitetype.DimSiteTypeId == (int)Constants.VisitSiteTypeE.SITE_CATEGORY)
                        {
                            if (sitetype.IdSiteType == 0)
                            {
                                TblSiteTypeTO tblSiteTypeTO = new TblSiteTypeTO();
                                tblSiteTypeTO.SiteTypeDisplayName = sitetype.SiteTypeDisplayName;
                                tblSiteTypeTO.SiteTypeDesc = sitetype.SiteTypeDisplayName;
                                tblSiteTypeTO.ParentSiteTypeId = sitetype.ParentSiteTypeId;
                                tblSiteTypeTO.DimSiteTypeId = sitetype.DimSiteTypeId;
                                tblSiteTypeTO.CreatedBy = tblVisitDetailsTO.CreatedBy;
                                tblSiteTypeTO.CreatedOn = tblVisitDetailsTO.CreatedOn;
                                tblSiteTypeTO.IsActive = 1;

                                result = TblSiteTypeBL.InsertTblSiteType(ref tblSiteTypeTO, conn, tran);
                                if (result != 1)
                                {
                                    resultMessage.DefaultBehaviour("Error While InsertTblSiteType");
                                    tran.Rollback();
                                    return resultMessage;
                                }
                                else
                                    tblVisitDetailsTO.SiteTypeId = tblSiteTypeTO.IdSiteType;
                            }
                            else
                            {
                                tblVisitDetailsTO.SiteTypeId = sitetype.IdSiteType;
                            }
                        }
                        if (sitetype.DimSiteTypeId == (int)Constants.VisitSiteTypeE.SITE_SUBCATEGORY)
                        {
                            if (sitetype.IdSiteType == 0)
                            {
                                TblSiteTypeTO tblSiteTypeTO = new TblSiteTypeTO();
                                tblSiteTypeTO.SiteTypeDisplayName = sitetype.SiteTypeDisplayName;
                                tblSiteTypeTO.SiteTypeDesc = sitetype.SiteTypeDisplayName;

                                if (sitetype.ParentSiteTypeId <= 0)
                                    tblSiteTypeTO.ParentSiteTypeId = tblVisitDetailsTO.SiteTypeId;
                                else
                                    tblSiteTypeTO.ParentSiteTypeId = sitetype.ParentSiteTypeId;

                                tblSiteTypeTO.CreatedBy = tblVisitDetailsTO.CreatedBy;
                                tblSiteTypeTO.CreatedOn = tblVisitDetailsTO.CreatedOn;
                                tblSiteTypeTO.DimSiteTypeId = sitetype.DimSiteTypeId;
                                tblSiteTypeTO.IsActive = 1;

                                result = TblSiteTypeBL.InsertTblSiteType(ref tblSiteTypeTO, conn, tran);
                                if (result != 1)
                                {
                                    resultMessage.DefaultBehaviour("Error While InsertTblSiteType");
                                    tran.Rollback();
                                    return resultMessage;
                                }
                                else
                                    tblVisitDetailsTO.SiteTypeId = tblSiteTypeTO.IdSiteType;
                            }
                            else
                            {
                                tblVisitDetailsTO.SiteTypeId = sitetype.IdSiteType;
                            }
                        }
                        if (sitetype.DimSiteTypeId == (int)Constants.VisitSiteTypeE.SITE_TYPE && tblVisitDetailsTO.SiteTypeTOList.Count == 1)
                        {
                            tblVisitDetailsTO.SiteTypeId = sitetype.IdSiteType;
                        }
                    }
                }
                else
                {
                    tblVisitDetailsTO.SiteTypeId = 0;
                }

                // Visit person mapping
                if (tblVisitDetailsTO.VisitPersonDetailsTOList != null && tblVisitDetailsTO.VisitPersonDetailsTOList.Count > 0)
                {
                    foreach (var person in tblVisitDetailsTO.VisitPersonDetailsTOList)
                    {
                        //TblPersonTO personTO = new TblPersonTO();

                        int userId = tblVisitDetailsTO.CreatedBy;

                        if (person.IdPerson == 0)
                        {
                            person.SalutationId = Constants.DefaultSalutationId; //set default 1

                            person.CreatedBy = tblVisitDetailsTO.UpdatedBy;
                            person.CreatedOn = tblVisitDetailsTO.UpdatedOn;

                            result = BL.TblPersonBL.InsertTblPerson(person, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While InsertTblPerson");
                                tran.Rollback();
                                return resultMessage;
                            }

                            // Visit person mapping insertion
                            TblVisitPersonDetailsTO tblVisitPersonDetailsTO = new TblVisitPersonDetailsTO();


                            //tblVisitPersonDetailsTO.VisitId = SelectLastVisitId(conn, tran);

                            tblVisitPersonDetailsTO.VisitId = tblVisitDetailsTO.IdVisit;
                            tblVisitPersonDetailsTO.PersonId = person.IdPerson;
                            tblVisitPersonDetailsTO.VisitRoleId = person.VisitRoleId;

                            result = TblVisitPersonDetailsBL.InsertTblVisitPersonDetails(tblVisitPersonDetailsTO, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour(" Error While InsertTblPersonVisitDetails");
                                tran.Rollback();
                                return resultMessage;
                            }

                            // For site owner 
                            if (person.IsSiteOwner == 1)
                            {
                                tblVisitDetailsTO.SiteOwnerId = person.IdPerson;
                            }

                            switch (person.VisitRoleId)
                            {

                                case (int)Constants.VisitPersonE.SITE_ARCHITECT:
                                    tblVisitDetailsTO.SiteArchitectId = person.IdPerson;
                                    break;
                                case (int)Constants.VisitPersonE.SITE_STRUCTURAL_ENGG:
                                    tblVisitDetailsTO.SiteStructuralEnggId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.SITE_CONTRACTOR:
                                    if (person.IsSiteOwner == 0)
                                        tblVisitDetailsTO.ContractorId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.SITE_STEEL_PURCHASE_AUTHORITY:
                                    tblVisitDetailsTO.PurchaseAuthorityPersonId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.DEALER_MEETING_WITH:
                                    tblVisitDetailsTO.DealerMeetingWithId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.DEALER_VISIT_ALONG_WITH:
                                    tblVisitDetailsTO.DealerVisitAlongWithId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.INFLUENCER_VISITED_BY:
                                    tblVisitDetailsTO.InfluencerVisitedBy = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.INFLUENCER_RECOMMANDEDED_BY:
                                    tblVisitDetailsTO.InfluencerRecommandedBy = person.IdPerson;
                                    break;
                            }
                        }
                        else
                        {
                            switch (person.VisitRoleId)
                            {
                                case (int)Constants.VisitPersonE.SITE_ARCHITECT:
                                    tblVisitDetailsTO.SiteArchitectId = person.IdPerson;
                                    break;
                                case (int)Constants.VisitPersonE.SITE_STRUCTURAL_ENGG:
                                    tblVisitDetailsTO.SiteStructuralEnggId = person.IdPerson;
                                    break;
                                case (int)Constants.VisitPersonE.SITE_CONTRACTOR:
                                    if (person.IsSiteOwner == 0)
                                    {
                                        tblVisitDetailsTO.ContractorId = person.IdPerson;
                                    }
                                    break;

                                case (int)Constants.VisitPersonE.SITE_STEEL_PURCHASE_AUTHORITY:
                                    tblVisitDetailsTO.PurchaseAuthorityPersonId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.DEALER:
                                    tblVisitDetailsTO.DealerId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.DEALER_MEETING_WITH:
                                    tblVisitDetailsTO.DealerMeetingWithId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.DEALER_VISIT_ALONG_WITH:
                                    tblVisitDetailsTO.DealerVisitAlongWithId = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.INFLUENCER_VISITED_BY:
                                    tblVisitDetailsTO.InfluencerVisitedBy = person.IdPerson;
                                    break;

                                case (int)Constants.VisitPersonE.INFLUENCER_RECOMMANDEDED_BY:
                                    tblVisitDetailsTO.InfluencerRecommandedBy = person.IdPerson;
                                    break;
                            }


                            result = BL.TblPersonBL.UpdateTblPerson(person, conn, tran);
                            if (result <= 0)
                            {
                                resultMessage.DefaultBehaviour("Error in UpdateTblPerson while updation");
                            }

                            // Visit person mapping insertion
                            TblVisitPersonDetailsTO visitPersonDetailsTO = new TblVisitPersonDetailsTO();

                            visitPersonDetailsTO.VisitId = tblVisitDetailsTO.IdVisit;
                            visitPersonDetailsTO.PersonId = person.IdPerson;
                            visitPersonDetailsTO.VisitRoleId = person.VisitRoleId;

                            int personCount = TblVisitPersonDetailsBL.SelectVisitPersonCount(visitPersonDetailsTO.VisitId, visitPersonDetailsTO.PersonId, visitPersonDetailsTO.VisitRoleId, conn, tran);

                            if (personCount <= 0)
                                result = TblVisitPersonDetailsBL.InsertTblVisitPersonDetails(visitPersonDetailsTO, conn, tran);
                            else
                                result = TblVisitPersonDetailsBL.UpdateTblPersonVisitDetails(visitPersonDetailsTO, conn, tran);

                            if (result < 0)
                            {
                                resultMessage.DefaultBehaviour("Error in UpdateTblPersonVisitDetails while updation");
                                tran.Rollback();
                                return resultMessage;
                            }
                        }
                    }

                    // Update site owner ids
                    foreach (var persondetails in tblVisitDetailsTO.VisitPersonDetailsTOList)
                    {
                        if (persondetails.IsSiteOwner == 1 && !(persondetails.VisitRoleId == (int)Constants.VisitPersonE.SITE_OWNER || persondetails.VisitRoleId == (int)Constants.VisitPersonE.SITE_CONTRACTOR || persondetails.VisitRoleId == (int)Constants.VisitPersonE.SITE_EXECUTOR))
                        {
                            tblVisitDetailsTO.SiteOwnerTypeId = 0;
                            tblVisitDetailsTO.SiteOwnerId = 0;
                            //if (!(persondetails.VisitRoleId == (int)Constants.VisitPersonE.SITE_OWNER || persondetails.VisitRoleId == (int)Constants.VisitPersonE.SITE_CONTRACTOR || persondetails.VisitRoleId == (int)Constants.VisitPersonE.SITE_EXECUTOR))
                            //{

                            //}
                        }
                    }

                    // Update person ids
                    foreach (int persontype in Enum.GetValues(typeof(Constants.VisitPersonE)))
                    {
                        if (persontype == (int)Constants.VisitPersonE.SITE_ARCHITECT)
                            if (!tblVisitDetailsTO.VisitPersonDetailsTOList.Exists(ele => ele.VisitRoleId == persontype))
                            {
                                tblVisitDetailsTO.SiteArchitectId = 0;
                            }

                        if (persontype == (int)Constants.VisitPersonE.SITE_CONTRACTOR)
                        {
                            var contractorCnt = tblVisitDetailsTO.VisitPersonDetailsTOList.Count(n => n.VisitRoleId == persontype);
                            if (contractorCnt != 0)
                            {
                                List<TblVisitPersonDetailsTO> tempVisitPersonDetailsTOList = new List<TblVisitPersonDetailsTO>();
                                tempVisitPersonDetailsTOList = tblVisitDetailsTO.VisitPersonDetailsTOList.Where(n => n.VisitRoleId == persontype).ToList();
                                if (contractorCnt == 1 && tempVisitPersonDetailsTOList.Exists(ele => ele.IsSiteOwner == 1))
                                {
                                    tblVisitDetailsTO.ContractorId = 0;
                                }

                                //if (contractorCnt == 1 && tblVisitDetailsTO.VisitPersonDetailsTOList.Exists(ele => ele.IsSiteOwner == 1))
                                //{
                                //    tblVisitDetailsTO.ContractorId = 0;
                                //}
                            }
                            else
                            {
                                tblVisitDetailsTO.ContractorId = 0;
                            }
                        }

                        if (persontype == (int)Constants.VisitPersonE.SITE_STRUCTURAL_ENGG)
                            if (!tblVisitDetailsTO.VisitPersonDetailsTOList.Exists(ele => ele.VisitRoleId == persontype))
                            {
                                tblVisitDetailsTO.SiteStructuralEnggId = 0;
                            }

                        if (persontype == (int)Constants.VisitPersonE.SITE_STEEL_PURCHASE_AUTHORITY)
                            if (!tblVisitDetailsTO.VisitPersonDetailsTOList.Exists(ele => ele.VisitRoleId == persontype))
                            {
                                tblVisitDetailsTO.PurchaseAuthorityPersonId = 0;
                            }
                        if (persontype == (int)Constants.VisitPersonE.INFLUENCER_VISITED_BY)
                            if (!tblVisitDetailsTO.VisitPersonDetailsTOList.Exists(ele => ele.VisitRoleId == persontype))
                            {
                                tblVisitDetailsTO.InfluencerVisitedBy = 0;
                            }
                        if (persontype == (int)Constants.VisitPersonE.INFLUENCER_RECOMMANDEDED_BY)
                            if (!tblVisitDetailsTO.VisitPersonDetailsTOList.Exists(ele => ele.VisitRoleId == persontype))
                            {
                                tblVisitDetailsTO.InfluencerRecommandedBy = 0;
                            }
                    }
                }
                else
                {
                    tblVisitDetailsTO.SiteOwnerTypeId = 0;
                    tblVisitDetailsTO.SiteOwnerId = 0;
                    tblVisitDetailsTO.ContractorId = 0;
                    //tblVisitDetailsTO.DealerMeetingWithId = 0;
                    //tblVisitDetailsTO.DealerId = 0;
                    //tblVisitDetailsTO.DealerVisitAlongWithDesignationId = 0;
                    //tblVisitDetailsTO.DealerVisitAlongWithId = 0;
                    tblVisitDetailsTO.InfluencerRecommandedBy = 0;
                    tblVisitDetailsTO.InfluencerVisitedBy = 0;
                    tblVisitDetailsTO.PurchaseAuthorityPersonId = 0;
                    tblVisitDetailsTO.SiteArchitectId = 0;
                    tblVisitDetailsTO.SiteStructuralEnggId = 0;

                }

                result = UpdateTblVisitDetails(tblVisitDetailsTO, conn, tran);
                if (result < 0)
                {
                    resultMessage.DefaultBehaviour("Error While UpdateTblVisitDetails");
                    tran.Rollback();
                    return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour();
                resultMessage.Result = tblVisitDetailsTO.IdVisit;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateVisitDetails");
                tran.Rollback();
                return resultMessage;
            }
        }

        #endregion

        #region Deletion
        public static int DeleteTblVisitDetails(int idVisit)
        {
            return TblVisitDetailsDAO.DeleteTblVisitDetails(idVisit);
        }

        public static int DeleteTblVisitDetails(int idVisit, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitDetailsDAO.DeleteTblVisitDetails(idVisit, conn, tran);
        }

        #endregion

    }
}
