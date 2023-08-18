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
    public class TblVisitAdditionalDetailsBL
    {
        #region Selection
        public static DataTable SelectAllTblVisitAdditionalDetails()
        {
            return TblVisitAdditionalDetailsDAO.SelectAllTblVisitAdditionalDetails();
        }

        public static List<TblVisitAdditionalDetailsTO> SelectAllTblVisitAdditionalDetailsList()
        {
            DataTable tblVisitAdditionalDetailsTODT = TblVisitAdditionalDetailsDAO.SelectAllTblVisitAdditionalDetails();
            return ConvertDTToList(tblVisitAdditionalDetailsTODT);
        }

        public static TblVisitAdditionalDetailsTO SelectTblVisitAdditionalDetailsTO(Int32 idVisitDetails)
        {
            DataTable tblVisitAdditionalDetailsTODT = TblVisitAdditionalDetailsDAO.SelectTblVisitAdditionalDetails(idVisitDetails);
            List<TblVisitAdditionalDetailsTO> tblVisitAdditionalDetailsTOList = ConvertDTToList(tblVisitAdditionalDetailsTODT);
            if (tblVisitAdditionalDetailsTOList != null && tblVisitAdditionalDetailsTOList.Count == 1)
                return tblVisitAdditionalDetailsTOList[0];
            else
                return null;
        }

        public static List<TblVisitAdditionalDetailsTO> ConvertDTToList(DataTable tblVisitAdditionalDetailsTODT)
        {
            List<TblVisitAdditionalDetailsTO> tblVisitAdditionalDetailsTOList = new List<TblVisitAdditionalDetailsTO>();
            if (tblVisitAdditionalDetailsTODT != null)
            {
            }
            return tblVisitAdditionalDetailsTOList;
        }

        public static TblVisitAdditionalDetailsTO SelectVisitAdditionalDetailsTO(Int32 visitId)
        {
            TblVisitAdditionalDetailsTO VisitAdditionalDetailsTO = TblVisitAdditionalDetailsDAO.SelectVisitAdditionalDetails(visitId);
            
            if (VisitAdditionalDetailsTO != null )
                return VisitAdditionalDetailsTO;
            else
                return null;
        }

        #endregion

        #region Insertion
        public static int InsertTblVisitAdditionalDetails(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO)
        {
            return TblVisitAdditionalDetailsDAO.InsertTblVisitAdditionalDetails(tblVisitAdditionalDetailsTO);
        }

        public static int InsertTblVisitAdditionalDetails(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitAdditionalDetailsDAO.InsertTblVisitAdditionalDetails(tblVisitAdditionalDetailsTO, conn, tran);
        }

        public static int InsertTblVisitAdditionalDetailsAbountCompany(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitAdditionalDetailsDAO.InsertTblVisitAdditionalDetails(tblVisitAdditionalDetailsTO, conn, tran);
        }

        // Vaibhav [3-Oct-2017] added to insert new visit additional information
        public static ResultMessage SaveVisitAdditionalInfo(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO,SqlConnection conn,SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {
                List<TblVisitPersonDetailsTO> tblVisitPersonDetailsTOForInsertion = new List<TblVisitPersonDetailsTO>();
                // Visit person mapping
                if (tblVisitAdditionalDetailsTO.VisitPersonDetailsTOList != null && tblVisitAdditionalDetailsTO.VisitPersonDetailsTOList.Count > 0)
                {
                    foreach (var person in tblVisitAdditionalDetailsTO.VisitPersonDetailsTOList)
                    {
                        //TblPersonTO personTO = new TblPersonTO();

                        if (person.IdPerson == 0)
                        {
                            person.SalutationId = Constants.DefaultSalutationId; //set default 1

                            person.CreatedBy = tblVisitAdditionalDetailsTO.CreatedBy;
                            person.CreatedOn = tblVisitAdditionalDetailsTO.CreatedOn;

                            result = BL.TblPersonBL.InsertTblPerson(person, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While InsertTblPerson");
                                tran.Rollback();
                                return resultMessage;
                            }

                            // Visit person insertion
                            TblVisitPersonDetailsTO tblVisitPersonDetailsTO = new TblVisitPersonDetailsTO();

                            //tblVisitPersonDetailsTO.VisitId = TblVisitDetailsBL.SelectLastVisitId(conn,tran);

                            tblVisitPersonDetailsTO.VisitId = tblVisitAdditionalDetailsTO.VisitId;
                            tblVisitPersonDetailsTO.PersonId = person.IdPerson;
                            tblVisitPersonDetailsTO.VisitRoleId = person.VisitRoleId;
                            tblVisitPersonDetailsTOForInsertion.Add(tblVisitPersonDetailsTO);
                          

                            switch (person.VisitRoleId)
                            {
                                case (int)Constants.VisitPersonE.SITE_COMPLAINT_REFRRED_BY:
                                    tblVisitAdditionalDetailsTO.SiteComplaintReferredBy = person.IdPerson;
                                    break;
                                case (int)Constants.VisitPersonE.COMMUNICATION_WITH_AT_SITE:
                                    tblVisitAdditionalDetailsTO.CommunicationPersonId = person.IdPerson;
                                    break;
                            }
                        }
                        else
                        {
                            switch (person.VisitRoleId)
                            {

                                case (int)Constants.VisitPersonE.SITE_COMPLAINT_REFRRED_BY:
                                    tblVisitAdditionalDetailsTO.SiteComplaintReferredBy = person.IdPerson;
                                    break;
                                case (int)Constants.VisitPersonE.COMMUNICATION_WITH_AT_SITE:
                                    tblVisitAdditionalDetailsTO.CommunicationPersonId = person.IdPerson;
                                    break;
                            }

                            result = BL.TblPersonBL.UpdateTblPerson(person, conn, tran);
                            if (result <= 0)
                            {
                                resultMessage.DefaultBehaviour("Error in UpdateTblPerson while updation");
                            }

                            // Visit person mapping insertion
                            TblVisitPersonDetailsTO visitPersonDetailsTO = new TblVisitPersonDetailsTO();

                            visitPersonDetailsTO.VisitId = tblVisitAdditionalDetailsTO.VisitId;
                            visitPersonDetailsTO.PersonId = person.IdPerson;
                            visitPersonDetailsTO.VisitRoleId = person.VisitRoleId;

                            result = TblVisitPersonDetailsBL.UpdateTblPersonVisitDetails(visitPersonDetailsTO, conn, tran);

                            if (result < 0)
                            {
                                resultMessage.DefaultBehaviour("Error in UpdateTblPersonVisitDetails while updation");
                                tran.Rollback();
                                return resultMessage;
                            }
                        }
                    }
                }

                if (tblVisitAdditionalDetailsTO.DesignationTO != null)
                {
                    tblVisitAdditionalDetailsTO.DesignationId = tblVisitAdditionalDetailsTO.DesignationTO.Value;
                }

                result = InsertTblVisitAdditionalDetails(tblVisitAdditionalDetailsTO, conn, tran);

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While InsertTblVisitAdditionalDetails");
                    tran.Rollback();
                    return resultMessage;
                }

                //Sudhir[01-DEC-2017] Added for Insertion of TblVisitPersonDetails.
                for (int i = 0; i < tblVisitPersonDetailsTOForInsertion.Count; i++)
                {
                    result = TblVisitPersonDetailsBL.InsertTblVisitPersonDetails(tblVisitPersonDetailsTOForInsertion[i], conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour(" Error While InsertTblVisitPersonDetails");
                        tran.Rollback();
                        return resultMessage;
                    }
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveVisitAdditionalInfo");
                tran.Rollback();
                return resultMessage;
            }
        }

        #endregion

        #region Updation
        public static int UpdateTblVisitAdditionalDetails(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO)
        {
            return TblVisitAdditionalDetailsDAO.UpdateTblVisitAdditionalDetails(tblVisitAdditionalDetailsTO);
        }

        public static int UpdateTblVisitAdditionalDetails(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitAdditionalDetailsDAO.UpdateTblVisitAdditionalDetails(tblVisitAdditionalDetailsTO, conn, tran);
        }


        // Vaibhav [1-Nov-2017] Added to update visit additional information
        public static ResultMessage UpdateVisitAdditionalInfo(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO,SqlConnection conn,SqlTransaction tran)
        {

            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {
                // Visit person mapping
                if (tblVisitAdditionalDetailsTO.VisitPersonDetailsTOList != null && tblVisitAdditionalDetailsTO.VisitPersonDetailsTOList.Count > 0)
                {
                    foreach (var person in tblVisitAdditionalDetailsTO.VisitPersonDetailsTOList)
                    {
                        //TblPersonTO personTO = new TblPersonTO();

                        if (person.IdPerson == 0)
                        {
                            person.SalutationId = Constants.DefaultSalutationId; //set default 1

                            person.CreatedBy = tblVisitAdditionalDetailsTO.CreatedBy;
                            person.CreatedOn = tblVisitAdditionalDetailsTO.CreatedOn;

                            result = BL.TblPersonBL.InsertTblPerson(person, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While InsertTblPerson");
                                tran.Rollback();
                                return resultMessage;
                            }

                            // Visit person insertion
                            TblVisitPersonDetailsTO tblVisitPersonDetailsTO = new TblVisitPersonDetailsTO();


                            //tblVisitPersonDetailsTO.VisitId = TblVisitDetailsBL.SelectLastVisitId(conn,tran);

                            tblVisitPersonDetailsTO.VisitId = tblVisitAdditionalDetailsTO.VisitId;
                            tblVisitPersonDetailsTO.PersonId = person.IdPerson;
                            tblVisitPersonDetailsTO.VisitRoleId = person.VisitRoleId;

                            result = TblVisitPersonDetailsBL.InsertTblVisitPersonDetails(tblVisitPersonDetailsTO, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour(" Error While InsertTblPersonVisitDetails");
                                tran.Rollback();
                                return resultMessage;
                            }


                            switch (person.VisitRoleId)
                            {
                                case (int)Constants.VisitPersonE.SITE_COMPLAINT_REFRRED_BY:
                                    tblVisitAdditionalDetailsTO.SiteComplaintReferredBy = person.IdPerson;
                                    break;
                                case (int)Constants.VisitPersonE.COMMUNICATION_WITH_AT_SITE:
                                    tblVisitAdditionalDetailsTO.CommunicationPersonId = person.IdPerson;
                                    break;
                            }
                        }
                        else
                        {
                            switch (person.VisitRoleId)
                            {

                                case (int)Constants.VisitPersonE.SITE_COMPLAINT_REFRRED_BY:
                                    tblVisitAdditionalDetailsTO.SiteComplaintReferredBy = person.IdPerson;
                                    break;
                                case (int)Constants.VisitPersonE.COMMUNICATION_WITH_AT_SITE:
                                    tblVisitAdditionalDetailsTO.CommunicationPersonId = person.IdPerson;
                                    break;
                            }

                            result = BL.TblPersonBL.UpdateTblPerson(person, conn, tran);
                            if (result <= 0)
                            {
                                resultMessage.DefaultBehaviour("Error in UpdateTblPerson while updation");
                            }

                            // Visit person mapping insertion
                            TblVisitPersonDetailsTO visitPersonDetailsTO = new TblVisitPersonDetailsTO();

                            visitPersonDetailsTO.VisitId = tblVisitAdditionalDetailsTO.VisitId;
                            visitPersonDetailsTO.PersonId = person.IdPerson;
                            visitPersonDetailsTO.VisitRoleId = person.VisitRoleId;

                            result = TblVisitPersonDetailsBL.UpdateTblPersonVisitDetails(visitPersonDetailsTO, conn, tran);

                            if (result < 0)
                            {
                                resultMessage.DefaultBehaviour("Error in UpdateTblPersonVisitDetails while updation");
                                tran.Rollback();
                                return resultMessage;
                            }
                        }
                    }
                    //Sudhir[20-NOV-2017] Added for Updating VisitPersonIds.
                    if (tblVisitAdditionalDetailsTO.VisitPersonDetailsTOList.Count <= 1)
                    {
                        foreach (var person in tblVisitAdditionalDetailsTO.VisitPersonDetailsTOList)
                        {
                            if (!(person.VisitRoleId == (int)Constants.VisitPersonE.SITE_COMPLAINT_REFRRED_BY))
                            {
                                tblVisitAdditionalDetailsTO.SiteComplaintReferredBy = 0;
                            }
                            if (!(person.VisitRoleId == (int)Constants.VisitPersonE.COMMUNICATION_WITH_AT_SITE))
                            {
                                tblVisitAdditionalDetailsTO.CommunicationPersonId = 0;
                            }
                        }
                    }
                }
                else
                {
                    tblVisitAdditionalDetailsTO.SiteComplaintReferredBy = 0;
                    tblVisitAdditionalDetailsTO.CommunicationPersonId = 0;
                }
                if (tblVisitAdditionalDetailsTO.DesignationTO != null)
                {
                    tblVisitAdditionalDetailsTO.DesignationId = tblVisitAdditionalDetailsTO.DesignationTO.Value;
                }

                result = UpdateTblVisitAdditionalDetails(tblVisitAdditionalDetailsTO, conn, tran);

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While UpdateTblVisitAdditionalDetails");
                    tran.Rollback();
                    return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateVisitAdditionalInfo");
                tran.Rollback();
                return resultMessage;
            }
        }
        #endregion

        #region Deletion
        public static int DeleteTblVisitAdditionalDetails(Int32 idVisitDetails)
        {
            return TblVisitAdditionalDetailsDAO.DeleteTblVisitAdditionalDetails(idVisitDetails);
        }

        public static int DeleteTblVisitAdditionalDetails(Int32 idVisitDetails, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitAdditionalDetailsDAO.DeleteTblVisitAdditionalDetails(idVisitDetails, conn, tran);
        }

        #endregion

    }
}
