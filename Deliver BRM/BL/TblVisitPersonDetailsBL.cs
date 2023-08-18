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
    public class TblVisitPersonDetailsBL
    {
        #region Selection

        // Vaibhav [2-oct-2017] added to select all person visit details list
        public static List<TblVisitPersonDetailsTO> SelectAllTblVisitPersonDetailsList(int visitTypeId)
        {
            List<TblVisitPersonDetailsTO> tblVisitPersonDetailsTOList = TblVisitPersonDetailsDAO.SelectAllTblVisitPersonDetails(visitTypeId);
            if (tblVisitPersonDetailsTOList != null)
                return tblVisitPersonDetailsTOList;
            else
                return null;
        }

        //Hrushikesh[11/01/2019]
        public static List<TblVisitPersonDetailsTO> SelectPersonDetailsForOffline(String personTypes)
        {
            //return TblVisitPersonDetailsDAO.SelectPersonDetailsForOffline(personTypes); ////Saket[2021-09-02] commented due to offline query taking more DTU.
            return new List<TblVisitPersonDetailsTO>();
        }
          
        public static TblVisitPersonDetailsTO SelectTblPersonVisitDetailsTO(Int32 personId)
        {
            DataTable tblPersonVisitDetailsTODT = TblVisitPersonDetailsDAO.SelectTblPersonVisitDetails(personId);
            List<TblVisitPersonDetailsTO> tblVisitPersonDetailsTOList = ConvertDTToList(tblPersonVisitDetailsTODT);
            if (tblVisitPersonDetailsTOList != null && tblVisitPersonDetailsTOList.Count == 1)
                return tblVisitPersonDetailsTOList[0];
            else
                return null;
        }

        public static List<TblVisitPersonDetailsTO> ConvertDTToList(DataTable tblVisitPersonDetailsTODT)
        {
            List<TblVisitPersonDetailsTO> tblVisitPersonDetailsTOList = new List<TblVisitPersonDetailsTO>();
            if (tblVisitPersonDetailsTODT != null)
            {

            }
            return tblVisitPersonDetailsTOList;
        }

        // Vaibhav [8-Nov-2017] added to select all person visit details list
        public static int SelectVisitPersonCount(int visitId,int personId,int personTypeId,SqlConnection conn,SqlTransaction tran)
        {
            int personCount = TblVisitPersonDetailsDAO.SelectVisitPersonCount(visitId,personId,personTypeId,conn,tran);
            return personCount;
        }

        // Vaibhav [19-Nov-2017] added to select visit person role list
        public static List<DropDownTO> SelectVisitPersonRoleListForDropDown()
        {
            List<DropDownTO> visitPersonRoleList =TblVisitPersonDetailsDAO.SelectVisitPersonRoleListForDropDown();
            if (visitPersonRoleList != null)
                return visitPersonRoleList;
            else
                return null;
        }

        //Sudhir- Added for Get All Visit Person Type List.
        public static List<DropDownTO> SelectAllVisitPersonTypeList()
        {
            return TblVisitPersonDetailsDAO.SelectAllVisitPersonTypeList();
        }

        //GetVisitPersonDropDownListOnPersonType
        public static List<DropDownTO> SelectVisitPersonDropDownListOnPersonType(Int32 personType)
        {
            return TblVisitPersonDetailsDAO.SelectVisitPersonDropDownListOnPersonType(personType);
        }

        //Sudhir[17-July-2018] Added For More Filtering Data --GetVisitPersonDropDownListOnPersonType
        public static List<DropDownTO> SelectVisitPersonDropDownListOnPersonType(Int32 personType,int? organizationId)
        {
            return TblVisitPersonDetailsDAO.SelectVisitPersonDropDownListOnPersonType(personType, organizationId);
        }

        #endregion

        #region Insertion
        public static int InsertTblPersonVisitDetails(TblVisitPersonDetailsTO tblPersonVisitDetailsTO)
        {
            return TblVisitPersonDetailsDAO.InsertTblPersonVisitDetails(tblPersonVisitDetailsTO);
        }

        public static int InsertTblVisitPersonDetails(TblVisitPersonDetailsTO tblVisitPersonDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitPersonDetailsDAO.InsertTblVisitPersonDetails(tblVisitPersonDetailsTO, conn, tran);
        }

        // Vaibhav [3-Oct-2017] added to insert new visit person details
        public static ResultMessage SaveNewVisitPersonDetail(TblVisitPersonDetailsTO tblVisitPersonDetailsTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                result = BL.TblPersonBL.InsertTblPerson(tblVisitPersonDetailsTO, conn, tran);

                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error while InsertTblPerson");
                    return resultMessage;
                }
                result = InsertTblVisitPersonDetails(tblVisitPersonDetailsTO, conn, tran);

                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error while InsertTblPersonVisitDetails");
                    return resultMessage;
                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveNewVisitPersonDetail");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region Updation
        public static int UpdateTblPersonVisitDetails(TblVisitPersonDetailsTO tblPersonVisitDetailsTO)
        {
            return TblVisitPersonDetailsDAO.UpdateTblPersonVisitDetails(tblPersonVisitDetailsTO);
        }

        public static int UpdateTblPersonVisitDetails(TblVisitPersonDetailsTO tblPersonVisitDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitPersonDetailsDAO.UpdateTblPersonVisitDetails(tblPersonVisitDetailsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblPersonVisitDetails(Int32 personId)
        {
            return TblVisitPersonDetailsDAO.DeleteTblPersonVisitDetails(personId);
        }

        public static int DeleteTblPersonVisitDetails(Int32 personId, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitPersonDetailsDAO.DeleteTblPersonVisitDetails(personId, conn, tran);
        }

        #endregion

    }
}
