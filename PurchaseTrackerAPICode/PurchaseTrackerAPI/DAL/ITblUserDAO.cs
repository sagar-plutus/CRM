using PurchaseTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PurchaseTrackerAPI.DAL
{
    public interface ITblUserDAO
    {

        String SqlSelectQuery();


        #region Selection
        List<TblUserTO> SelectAllTblUser(Boolean onlyActiveYn);

        TblUserTO SelectTblUser(Int32 idUser, SqlConnection conn, SqlTransaction tran);

        TblUserTO SelectTblUser(String userID, String password);

        Boolean IsThisUserExists(String userID, SqlConnection conn, SqlTransaction tran);

        Dictionary<int, string> SelectUserMobileNoDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran);

        Dictionary<int, string> SelectUserDeviceRegNoDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran);

        List<TblUserTO> SelectAllTblUser(Int32 orgId, SqlConnection conn, SqlTransaction tran);


         List<DropDownTO> SelectAllActiveUsersForDropDown();


         List<TblUserTO> ConvertDTToList(SqlDataReader tblUserTODT);
        
         TblUserTO SelectUserByImeiNumber(string imeiNumber, SqlConnection conn, SqlTransaction tran);
        
        List<DropDownTO> GetUnloadingPersonListForDropDown(string roleId);
        
         List<DropDownTO> GetUnloadingPersonListForDropDown(Int32 roleId, SqlConnection conn, SqlTransaction tran);
       
        #endregion

        #region Insertion
         int InsertTblUser(TblUserTO tblUserTO);
        
        int InsertTblUser(TblUserTO tblUserTO, SqlConnection conn, SqlTransaction tran);
        
        int ExecuteInsertionCommand(TblUserTO tblUserTO, SqlCommand cmdInsert);
        
        #endregion

        #region Updation
        int UpdateTblUser(TblUserTO tblUserTO);
        
        int UpdateTblUser(TblUserTO tblUserTO, SqlConnection conn, SqlTransaction tran);

        int ExecuteUpdationCommand(TblUserTO tblUserTO, SqlCommand cmdUpdate);
        #endregion
        
        int DeleteTblUser(Int32 idUser);
        
        int DeleteTblUser(Int32 idUser, SqlConnection conn, SqlTransaction tran);
        

         int ExecuteDeletionCommand(Int32 idUser, SqlCommand cmdDelete);
    }
}