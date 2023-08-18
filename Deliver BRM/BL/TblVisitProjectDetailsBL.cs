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
    public class TblVisitProjectDetailsBL
    {
        #region Selection
        public static List<TblVisitProjectDetailsTO> SelectProjectDetailsList(Int32 visitId)
        {
            return TblVisitProjectDetailsDAO.SelectAllTblVisitProjectDetails(visitId);
        }

        public static List<TblVisitProjectDetailsTO> SelectAllTblVisitProjectDetailsList()
        {
            List<TblVisitProjectDetailsTO> tblProjectDetailsTOList = TblVisitProjectDetailsDAO.SelectAllTblVisitProjectDetails(0);
            return tblProjectDetailsTOList;
        }

        public static TblVisitProjectDetailsTO SelectTblProjectDetailsTO(Int32 idProject)
        {
            DataTable tblProjectDetailsTODT = TblVisitProjectDetailsDAO.SelectTblVisitProjectDetails(idProject);
            List<TblVisitProjectDetailsTO> tblProjectDetailsTOList = ConvertDTToList(tblProjectDetailsTODT);
            if (tblProjectDetailsTOList != null && tblProjectDetailsTOList.Count == 1)
                return tblProjectDetailsTOList[0];
            else
                return null;
        }

        public static List<TblVisitProjectDetailsTO> ConvertDTToList(DataTable tblProjectDetailsTODT)
        {
            List<TblVisitProjectDetailsTO> tblProjectDetailsTOList = new List<TblVisitProjectDetailsTO>();
            if (tblProjectDetailsTODT != null)
            {

            }
            return tblProjectDetailsTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblProjectDetails(TblVisitProjectDetailsTO tblProjectDetailsTO)
        {
            return TblVisitProjectDetailsDAO.InsertTblVisitProjectDetails(tblProjectDetailsTO);
        }

        public static int InsertTblProjectDetails(TblVisitProjectDetailsTO tblProjectDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitProjectDetailsDAO.InsertTblVisitProjectDetails(tblProjectDetailsTO, conn, tran);
        }

        // Vaibhav [25-Oct-2017] added to insert visit project information
        public static ResultMessage SaveVisitProjectDetails(List<TblVisitProjectDetailsTO> tblVisitProjectDetailsTOList, Int32 createdBy,Int32 visitId,SqlConnection conn,SqlTransaction tran)
        {

            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                foreach (var visitProjectDetailsTO in tblVisitProjectDetailsTOList)
                {
                    // Insertion
                    if (visitProjectDetailsTO.IdProject <= 0)
                    {
                        if (visitProjectDetailsTO.VisitId <= 0)
                            visitProjectDetailsTO.VisitId = visitId;

                        visitProjectDetailsTO.CreatedBy = createdBy;
                        visitProjectDetailsTO.CreatedOn = Constants.ServerDateTime;

                        if (visitProjectDetailsTO.ContactPersonId == 0)
                        {
                            TblPersonTO personTO = new TblPersonTO();

                            personTO.SalutationId = Constants.DefaultSalutationId; //set default 1

                            
                            String contactPersonName= visitProjectDetailsTO.ContactPersonName;

                            if (contactPersonName.Contains(' '))
                            {
                                String[] contactPersonNames = contactPersonName.Split(' ');

                                if (contactPersonNames.Length >= 0)
                                    personTO.FirstName = contactPersonNames[0] != null ? contactPersonNames[0].ToString() : null;
                                if (contactPersonNames.Length > 1)
                                    personTO.LastName = contactPersonNames[1] != null ? contactPersonNames[1].ToString() : null;
                                if (contactPersonNames.Length > 2)
                                    personTO.LastName = contactPersonNames[2] != null ? contactPersonNames[2].ToString() : null;
                            }
                            else
                            {
                                personTO.FirstName = contactPersonName;
                                personTO.LastName = "-";
                            }
                            personTO.MobileNo = visitProjectDetailsTO.ContactNo;
                            personTO.PrimaryEmail = visitProjectDetailsTO.EmailId;
                            personTO.CreatedBy = visitProjectDetailsTO.CreatedBy;
                            personTO.CreatedOn = visitProjectDetailsTO.CreatedOn;

                            result = BL.TblPersonBL.InsertTblPerson(personTO, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While InsertTblSiteStatus");
                                tran.Rollback();
                                return resultMessage;
                            }
                            else
                                visitProjectDetailsTO.ContactPersonId = personTO.IdPerson;
                        }

                        result = InsertTblProjectDetails(visitProjectDetailsTO, conn, tran);

                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error While InsertTblProjectDetails");
                            tran.Rollback();
                            return resultMessage;
                        }
                        else
                        {
                            resultMessage.Tag = visitProjectDetailsTO.IdProject;
                        }
                    }

                    // Updation
                    else
                    { 
                        if (visitProjectDetailsTO.VisitId <= 0)
                            visitProjectDetailsTO.VisitId = visitId;

                        visitProjectDetailsTO.UpdatedBy = createdBy;
                        visitProjectDetailsTO.UpdatedOn = Constants.ServerDateTime;

                        TblPersonTO personTO = new TblPersonTO();

                        personTO.SalutationId = Constants.DefaultSalutationId; //set default 1

                        String contactPersonName = visitProjectDetailsTO.ContactPersonName;

                        if (contactPersonName.Contains(' '))
                        {
                            String[] contactPersonNames = visitProjectDetailsTO.ContactPersonName.Split(' ');

                            if (contactPersonNames.Length >= 0)
                                personTO.FirstName = contactPersonNames[0] != null ? contactPersonNames[0].ToString() : null;
                            if (contactPersonNames.Length > 1)
                                personTO.LastName = contactPersonNames[1] != null ? contactPersonNames[1].ToString() : null;
                            if (contactPersonNames.Length > 2)
                                personTO.LastName = contactPersonNames[2] != null ? contactPersonNames[2].ToString() : null;
                        }
                        else
                        {
                            personTO.FirstName = contactPersonName;
                            personTO.LastName = "-";
                        }

                        personTO.IdPerson = visitProjectDetailsTO.ContactPersonId;
                        personTO.MobileNo = visitProjectDetailsTO.ContactNo;
                        personTO.PrimaryEmail = visitProjectDetailsTO.EmailId;
                        personTO.CreatedBy = createdBy;
                        personTO.CreatedOn = Constants.ServerDateTime;

                        if (visitProjectDetailsTO.ContactPersonId == 0)
                        {

                            result = BL.TblPersonBL.InsertTblPerson(personTO, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While InsertTblSiteStatus");
                                tran.Rollback();
                                return resultMessage;
                            }
                            else
                                visitProjectDetailsTO.ContactPersonId = personTO.IdPerson;
                        }
                        else
                        {
                            result = BL.TblPersonBL.UpdateTblPerson(personTO, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While UpdateTblPerson");
                                tran.Rollback();
                                return resultMessage;
                            }
                            else
                                visitProjectDetailsTO.ContactPersonId = personTO.IdPerson;
                        }

                        result = UpdateTblProjectDetails(visitProjectDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error While UpdateTblProjectDetails");
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
                resultMessage.DefaultExceptionBehaviour(ex, "SaveVisitProjectDetails");
                tran.Rollback();
                return resultMessage;
            }
        }

        #endregion

        #region Updation
        public static int UpdateTblProjectDetails(TblVisitProjectDetailsTO tblProjectDetailsTO)
        {
            return TblVisitProjectDetailsDAO.UpdateTblVisitProjectDetails(tblProjectDetailsTO);
        }

        public static int UpdateTblProjectDetails(TblVisitProjectDetailsTO tblProjectDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitProjectDetailsDAO.UpdateTblVisitProjectDetails(tblProjectDetailsTO, conn, tran);
        }

        // Vaibhav [1-Nov-2017] added to update visit project information
        public static ResultMessage UpdateVisitProjectDetails(List<TblVisitProjectDetailsTO> tblVisitProjectDetailsTOList, Int32 updatedBy, Int32 visitId,SqlConnection conn,SqlTransaction tran)
        {

            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                foreach (var visitProjectDetailsTO in tblVisitProjectDetailsTOList)
                {
                    if (visitProjectDetailsTO.IdProject > 0)
                    {
                        if (visitProjectDetailsTO.VisitId <= 0)
                            visitProjectDetailsTO.VisitId = visitId;

                        visitProjectDetailsTO.UpdatedBy = updatedBy;
                        visitProjectDetailsTO.UpdatedOn = Constants.ServerDateTime;

                        if (visitProjectDetailsTO.ContactPersonId == 0)
                        {
                            TblPersonTO personTO = new TblPersonTO();

                            personTO.SalutationId = Constants.DefaultSalutationId; //set default 1

                            String contactPersonName= visitProjectDetailsTO.ContactPersonName;

                            if (contactPersonName.Contains(' '))
                            {
                                String[] contactPersonNames = visitProjectDetailsTO.ContactPersonName.Split(' ');

                                if (contactPersonNames.Length >= 0)
                                    personTO.FirstName = contactPersonNames[0] != null ? contactPersonNames[0].ToString() : null;
                                if (contactPersonNames.Length > 1)
                                    personTO.LastName = contactPersonNames[1] != null ? contactPersonNames[1].ToString() : null;
                                if (contactPersonNames.Length > 2)
                                    personTO.LastName = contactPersonNames[2] != null ? contactPersonNames[2].ToString() : null;
                            }
                            else
                            {
                                personTO.FirstName = contactPersonName;
                                personTO.LastName = "-";
                            }

                            personTO.MobileNo = visitProjectDetailsTO.ContactNo;
                            personTO.PrimaryEmail = visitProjectDetailsTO.EmailId;
                            personTO.CreatedBy = updatedBy;
                            personTO.CreatedOn = Constants.ServerDateTime;

                            result = BL.TblPersonBL.InsertTblPerson(personTO, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While InsertTblSiteStatus");
                                tran.Rollback();
                                return resultMessage;
                            }
                            else
                                visitProjectDetailsTO.ContactPersonId = personTO.IdPerson;
                        }

                        result = UpdateTblProjectDetails(visitProjectDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error While UpdateTblProjectDetails");
                            tran.Rollback();
                            return resultMessage;
                        }
                    }

                    // Insert project details while updation
                    if (visitProjectDetailsTO.IdProject <= 0)
                    {
                        if (visitProjectDetailsTO.VisitId <= 0)
                            visitProjectDetailsTO.VisitId = visitId;

                        visitProjectDetailsTO.CreatedBy = updatedBy;
                        visitProjectDetailsTO.CreatedOn = Constants.ServerDateTime;

                        if (visitProjectDetailsTO.ContactPersonId == 0)
                        {
                            TblPersonTO personTO = new TblPersonTO();

                            personTO.SalutationId = Constants.DefaultSalutationId; //set default 1

                            String contactPersonName = visitProjectDetailsTO.ContactPersonName;

                            if (contactPersonName.Contains(' '))
                            {
                                String[] contactPersonNames = visitProjectDetailsTO.ContactPersonName.Split(' ');

                                if (contactPersonNames.Length >= 0)
                                    personTO.FirstName = contactPersonNames[0] != null ? contactPersonNames[0].ToString() : null;
                                if (contactPersonNames.Length > 1)
                                    personTO.LastName = contactPersonNames[1] != null ? contactPersonNames[1].ToString() : null;
                                if (contactPersonNames.Length > 2)
                                    personTO.LastName = contactPersonNames[2] != null ? contactPersonNames[2].ToString() : null;
                            }
                            else
                            {
                                personTO.FirstName = contactPersonName;
                                personTO.LastName = "-";
                            }

                            personTO.MobileNo = visitProjectDetailsTO.ContactNo;
                            personTO.PrimaryEmail = visitProjectDetailsTO.EmailId;
                            personTO.CreatedBy = updatedBy;
                            personTO.CreatedOn = Constants.ServerDateTime;

                            result = BL.TblPersonBL.InsertTblPerson(personTO, conn, tran);

                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While InsertTblSiteStatus");
                                tran.Rollback();
                                return resultMessage;
                            }
                            else
                                visitProjectDetailsTO.ContactPersonId = personTO.IdPerson;
                        }

                        result = InsertTblProjectDetails(visitProjectDetailsTO, conn, tran);

                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error in InsertTblProjectDetails while updation");
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
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateVisitProjectDetails");
                tran.Rollback();
                return resultMessage;
            }
        }

        #endregion

        #region Deletion
        public static int DeleteTblProjectDetails(Int32 idProject)
        {
            return TblVisitProjectDetailsDAO.DeleteTblVisitProjectDetails(idProject);
        }

        public static int DeleteTblProjectDetails(Int32 idProject, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitProjectDetailsDAO.DeleteTblVisitProjectDetails(idProject, conn, tran);
        }

        #endregion

    }
}
