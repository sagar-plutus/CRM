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
    public class TblSiteRequirementsBL
    {
        #region Selection
        public static DataTable SelectAllTblSiteRequirements()
        {
            return TblSiteRequirementsDAO.SelectAllTblSiteRequirements();
        }

        public static List<TblSiteRequirementsTO> SelectAllTblSiteRequirementsList()
        {
            DataTable tblSiteRequirementsTODT = TblSiteRequirementsDAO.SelectAllTblSiteRequirements();
            return ConvertDTToList(tblSiteRequirementsTODT);
        }

        public static TblSiteRequirementsTO SelectTblSiteRequirementsTO(Int32 idSiteRequirement)
        {
            DataTable tblSiteRequirementsTODT = TblSiteRequirementsDAO.SelectTblSiteRequirements(idSiteRequirement);
            List<TblSiteRequirementsTO> tblSiteRequirementsTOList = ConvertDTToList(tblSiteRequirementsTODT);
            if (tblSiteRequirementsTOList != null && tblSiteRequirementsTOList.Count == 1)
                return tblSiteRequirementsTOList[0];
            else
                return null;
        }

        public static List<TblSiteRequirementsTO> ConvertDTToList(DataTable tblSiteRequirementsTODT)
        {
            List<TblSiteRequirementsTO> tblSiteRequirementsTOList = new List<TblSiteRequirementsTO>();
            if (tblSiteRequirementsTODT != null)
            {

            }
            return tblSiteRequirementsTOList;
        }

        public static TblSiteRequirementsTO SelectSiteRequirementsTO(Int32 visitId)
        {
            TblSiteRequirementsTO tblSiteRequirementsTO = TblSiteRequirementsDAO.SelectSiteRequirements(visitId);
            if (tblSiteRequirementsTO != null)
                return tblSiteRequirementsTO;
            else
                return null;
        }
        #endregion

        #region Insertion
        public static int InsertTblSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO)
        {
            return TblSiteRequirementsDAO.InsertTblSiteRequirements(tblSiteRequirementsTO);
        }

        public static int InsertTblSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblSiteRequirementsDAO.InsertTblSiteRequirements(tblSiteRequirementsTO, conn, tran);
        }

        // Vaibhav [3-Oct-2017] added to insert new site requirements
        public static ResultMessage SaveSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO,SqlConnection conn,SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {
                result = InsertTblSiteRequirements(tblSiteRequirementsTO, conn, tran);

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error  Duplicate Record While Insertion");
                    tran.Rollback();
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveSiteRequirements");
                tran.Rollback();
                return resultMessage;
            }
        }

        #endregion

        #region Updation
        public static int UpdateTblSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO)
        {
            return TblSiteRequirementsDAO.UpdateTblSiteRequirements(tblSiteRequirementsTO);
        }

        public static int UpdateTblSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblSiteRequirementsDAO.UpdateTblSiteRequirements(tblSiteRequirementsTO, conn, tran);
        }

        // Vaibhav [1-Nov-2017] Added to update new site requirements
        public static ResultMessage UpdateSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO,SqlConnection conn,SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {
                
                result = UpdateTblSiteRequirements(tblSiteRequirementsTO, conn, tran);

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While Duplicate Record");
                    tran.Rollback();
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateSiteRequirements");
                tran.Rollback();
                return resultMessage;
            }
        }

        #endregion

        #region Deletion
        public static int DeleteTblSiteRequirements(Int32 idSiteRequirement)
        {
            return TblSiteRequirementsDAO.DeleteTblSiteRequirements(idSiteRequirement);
        }

        public static int DeleteTblSiteRequirements(Int32 idSiteRequirement, SqlConnection conn, SqlTransaction tran)
        {
            return TblSiteRequirementsDAO.DeleteTblSiteRequirements(idSiteRequirement, conn, tran);
        }

        #endregion

    }
}
