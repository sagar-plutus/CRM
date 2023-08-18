using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblUserBL
    {
        List<TblUserTO> SelectAllTblUserList(Boolean onlyActiveYn);
        TblUserTO SelectTblUserTO(Int32 idUser);
        int SelectUserByImeiNumber(string idDevice);
        TblUserTO SelectTblUserTO(Int32 idUser, SqlConnection conn, SqlTransaction tran);
        TblUserTO SelectTblUserTO(String userID, String password);
        Boolean IsThisUserExists(String userId);
        Boolean IsThisUserExists(String userId, SqlConnection conn, SqlTransaction tran);
        Dictionary<int, string> SelectUserMobileNoDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran);
        Dictionary<int, string> SelectUserDeviceRegNoDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran);
        List<TblUserTO> SelectAllTblUserList(Int32 orgId, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> SelectAllActiveUsersForDropDown();
        List<DropDownTO> GetUnloadingPersonListForDropDown(string roleId);
        List<DropDownTO> GetUnloadingPersonListForDropDown(Int32 roleId, SqlConnection conn, SqlTransaction tran);
        int InsertTblUser(TblUserTO tblUserTO);
        int InsertTblUser(TblUserTO tblUserTO, SqlConnection conn, SqlTransaction tran);
        String CreateUserName(string firstName, string lastName, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveNewUser(TblUserTO tblUserTO, Int32 loginUserId);
        int UpdateTblUser(TblUserTO tblUserTO);
        int UpdateTblUser(TblUserTO tblUserTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateUser(TblUserTO tblUserTO, Int32 loginUserId);
        int DeleteTblUser(Int32 idUser);
        int DeleteTblUser(Int32 idUser, SqlConnection conn, SqlTransaction tran);

    }
}