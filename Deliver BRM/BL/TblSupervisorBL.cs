using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.BL
{
    public class TblSupervisorBL
    {
        #region Selection
        public static List<TblSupervisorTO> SelectAllTblSupervisorList()
        {
           return  TblSupervisorDAO.SelectAllTblSupervisor();
        }

        public static TblSupervisorTO SelectTblSupervisorTO(Int32 idSupervisor)
        {
            return TblSupervisorDAO.SelectTblSupervisor(idSupervisor);
        }



        #endregion

        #region Insertion

        internal static ResultMessage SaveNewSuperwisor(TblSupervisorTO supervisorTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                supervisorTO.PersonTO.CreatedBy = supervisorTO.CreatedBy;
                supervisorTO.PersonTO.CreatedOn = supervisorTO.CreatedOn;
                if (string.IsNullOrEmpty(supervisorTO.PersonTO.Comments))
                    supervisorTO.PersonTO.Comments = "Superwisor";

                if (supervisorTO.PersonTO.DobDay > 0 && supervisorTO.PersonTO.DobMonth > 0 && supervisorTO.PersonTO.DobYear > 0)
                {
                    supervisorTO.PersonTO.DateOfBirth = new DateTime(supervisorTO.PersonTO.DobYear, supervisorTO.PersonTO.DobMonth, supervisorTO.PersonTO.DobDay);
                }
                else
                {
                    supervisorTO.PersonTO.DateOfBirth = DateTime.MinValue;
                }

                int result = BL.TblPersonBL.InsertTblPerson(supervisorTO.PersonTO, conn, tran);
                if(result!=1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "Error In Method SaveNewSuperwisor";
                    resultMessage.DisplayMessage = "Error... Record could not be saved";
                    return resultMessage;
                }

                supervisorTO.PersonId = supervisorTO.PersonTO.IdPerson;
                result = InsertTblSupervisor(supervisorTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "Error In Method InsertTblSupervisor";
                    resultMessage.DisplayMessage = "Error... Record could not be saved";
                    return resultMessage;
                }

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                resultMessage.Text = "Success... Record saved";
                resultMessage.DisplayMessage = "Success... Record saved";
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error In Method SaveNewSuperwisor";
                resultMessage.DisplayMessage = "Exception Error... Record could not be saved";
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public static int InsertTblSupervisor(TblSupervisorTO tblSupervisorTO)
        {
            return TblSupervisorDAO.InsertTblSupervisor(tblSupervisorTO);
        }

        public static int InsertTblSupervisor(TblSupervisorTO tblSupervisorTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblSupervisorDAO.InsertTblSupervisor(tblSupervisorTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblSupervisor(TblSupervisorTO tblSupervisorTO)
        {
            return TblSupervisorDAO.UpdateTblSupervisor(tblSupervisorTO);
        }

        public static int UpdateTblSupervisor(TblSupervisorTO tblSupervisorTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblSupervisorDAO.UpdateTblSupervisor(tblSupervisorTO, conn, tran);
        }

        internal static ResultMessage UpdateSuperwisor(TblSupervisorTO supervisorTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                if (supervisorTO.PersonTO.DobDay > 0 && supervisorTO.PersonTO.DobMonth > 0 && supervisorTO.PersonTO.DobYear > 0)
                {
                    supervisorTO.PersonTO.DateOfBirth = new DateTime(supervisorTO.PersonTO.DobYear, supervisorTO.PersonTO.DobMonth, supervisorTO.PersonTO.DobDay);
                }
                else
                {
                    supervisorTO.PersonTO.DateOfBirth = DateTime.MinValue;
                }

                int result = BL.TblPersonBL.UpdateTblPerson(supervisorTO.PersonTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "Error In Method SaveNewSuperwisor";
                    resultMessage.DisplayMessage = "Error... Record could not be saved";
                    return resultMessage;
                }

                supervisorTO.SupervisorName = supervisorTO.PersonTO.FirstName + " " + supervisorTO.PersonTO.LastName;
                result = UpdateTblSupervisor(supervisorTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "Error In Method UpdateTblSupervisor";
                    resultMessage.DisplayMessage = "Error... Record could not be saved";
                    return resultMessage;
                }

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                resultMessage.Text = "Success... Record saved";
                resultMessage.DisplayMessage = "Success... Record saved";
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error In Method UpdateSuperwisor";
                resultMessage.DisplayMessage = "Exception Error... Record could not be saved";
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region Deletion
        public static int DeleteTblSupervisor(Int32 idSupervisor)
        {
            return TblSupervisorDAO.DeleteTblSupervisor(idSupervisor);
        }

        public static int DeleteTblSupervisor(Int32 idSupervisor, SqlConnection conn, SqlTransaction tran)
        {
            return TblSupervisorDAO.DeleteTblSupervisor(idSupervisor, conn, tran);
        }

       

        #endregion

    }
}
