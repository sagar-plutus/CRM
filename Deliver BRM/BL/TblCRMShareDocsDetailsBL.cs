using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.BL
{
    public class TblCRMShareDocsDetailsBL
    {
        #region Selection
        public static List<TblCRMShareDocsDetailsTO> SelectAllTblCRMShareDocsDetails()
        {
            return TblCRMShareDocsDetailsDAO.SelectAllTblCRMShareDocsDetails();
        }

        public static List<TblCRMShareDocsDetailsTO> SelectAllTblCRMShareDocsDetailsList()
        {
            return TblCRMShareDocsDetailsDAO.SelectAllTblCRMShareDocsDetails();
             //ConvertDTToList(tblCRMShareDocsDetailsTODT);
        }

        public static TblCRMShareDocsDetailsTO SelectTblCRMShareDocsDetailsTO(Int32 idShareDoc)
        {
            TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTODT = TblCRMShareDocsDetailsDAO.SelectTblCRMShareDocsDetails(idShareDoc);
            if (tblCRMShareDocsDetailsTODT != null)
                return tblCRMShareDocsDetailsTODT;
            else
                return null;
        }

        #endregion

        #region Insertion
        public static int InsertTblCRMShareDocsDetails(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO)
        {
            return TblCRMShareDocsDetailsDAO.InsertTblCRMShareDocsDetails(tblCRMShareDocsDetailsTO);
        }

        public static int InsertTblCRMShareDocsDetails(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblCRMShareDocsDetailsDAO.InsertTblCRMShareDocsDetails(tblCRMShareDocsDetailsTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateTblCRMShareDocsDetails(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO)
        {
            return TblCRMShareDocsDetailsDAO.UpdateTblCRMShareDocsDetails(tblCRMShareDocsDetailsTO);
        }

        public static int UpdateTblCRMShareDocsDetails(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblCRMShareDocsDetailsDAO.UpdateTblCRMShareDocsDetails(tblCRMShareDocsDetailsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblCRMShareDocsDetails(Int32 idShareDoc)
        {
            return TblCRMShareDocsDetailsDAO.DeleteTblCRMShareDocsDetails(idShareDoc);
        }

        public static int DeleteTblCRMShareDocsDetails(Int32 idShareDoc, SqlConnection conn, SqlTransaction tran)
        {
            return TblCRMShareDocsDetailsDAO.DeleteTblCRMShareDocsDetails(idShareDoc, conn, tran);
        }

        #endregion

        public static async Task<ResultMessage> ShareDetialsAsync(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                 Task<ResultMessage> resultMessag = SendEmailToSelectedPersonsAsync(tblCRMShareDocsDetailsTO);
                resultMessage = await resultMessag;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "ShareDetials");
                resultMessage.DisplayMessage = ex.Message;
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }
        }

        //Sudhir[10-MAY-2018] Added for Sending Mail. And Saving Record to ShareDetails
        public static async Task<ResultMessage> SendEmailToSelectedPersonsAsync(TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (tblCRMShareDocsDetailsTO != null)
                {
                    List<PersonShareData> personShareDataList = tblCRMShareDocsDetailsTO.PersonShareData;
                    if (personShareDataList != null && personShareDataList.Count > 0)
                    {
                        foreach (PersonShareData personShareData in personShareDataList)
                        {
                            if (personShareData.EmailId != null )
                            {
                                int result = 0;
                                SendMail sendMail = new SendMail();
                                sendMail.To = personShareData.EmailId;
                                //sendMail.BodyContent = personShareData.DocBase64;
                                sendMail.Message = personShareData.DocBase64;
                                string[] mailSubject =personShareData.FileName.Split('.'); ;
                                if(mailSubject.Length > 0)
                                {
                                    sendMail.Subject = mailSubject[0];
                                }
                                else
                                {
                                    sendMail.Subject = "Regards New Visit Details ";
                                }
                                if (personShareData.FileName == null )
                                {
                                    personShareData.FileName = "VisitDetails.pdf";
                                }
                                Task<ResultMessage> resultMessag = SendMailBL.SendEmailAsync(sendMail, personShareData.FileName);
                                resultMessage = await resultMessag;

                                if (resultMessage.MessageType != ResultMessageE.Error)
                                {
                                    //Insert Record to tblCRMShareDoc Details.
                                    TblCRMShareDocsDetailsTO tblCRMShareDocsDetails = tblCRMShareDocsDetailsTO;
                                    tblCRMShareDocsDetails.VisitId = tblCRMShareDocsDetailsTO.VisitId;
                                    tblCRMShareDocsDetails.DocumentId = tblCRMShareDocsDetailsTO.DocumentId;
                                    if(personShareData.RoleId==-1)
                                    {
                                        tblCRMShareDocsDetailsTO.EntityTypeId = 2;
                                        tblCRMShareDocsDetailsTO.UserId = personShareData.PersonId;
                                    }
                                    else
                                    {
                                        tblCRMShareDocsDetailsTO.EntityTypeId = 1;
                                        tblCRMShareDocsDetailsTO.UserId = personShareData.PersonId;
                                    }

                                    if(personShareData.RoleId == -1)
                                    {
                                        tblCRMShareDocsDetails.RoleId = 0;
                                        personShareData.RoleId = 0;
                                    }
                                    tblCRMShareDocsDetails.RoleId = personShareData.RoleId;
                                    tblCRMShareDocsDetails.CreatedOn = Constants.ServerDateTime;
                                    tblCRMShareDocsDetails.CreatedBy = tblCRMShareDocsDetailsTO.CreatedBy;
                                    result = TblCRMShareDocsDetailsBL.InsertTblCRMShareDocsDetails(tblCRMShareDocsDetails);
                                    if(result !=1)
                                    {
                                        resultMessage.DefaultBehaviour("Error While Inserting Data into TblCRMShareDocsDetails");
                                        resultMessage.DisplayMessage = "Error While Sending Email";
                                        resultMessage.MessageType = ResultMessageE.Error;
                                        return resultMessage;
                                    }
                                }
                                else
                                {
                                    return resultMessage;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }


                    }
                    else
                    {
                        //personShareDataList    return -1;
                        resultMessage.DefaultBehaviour("personShareDataList found Null");
                        resultMessage.DisplayMessage = "Error While Sending Email";
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }
                }
                else
                {
                    //tblCRMShareDocsDetailsTO 
                    resultMessage.DefaultBehaviour("tblCRMShareDocsDetailsTO found Null");
                    resultMessage.DisplayMessage = "Error While Sending Email";
                    resultMessage.MessageType = ResultMessageE.Error;
                    return resultMessage;
                }
                //return 1;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SendEmailToSelectedPersons");
                resultMessage.DisplayMessage = "Error While Sending Email";
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }
        }
    }
}
