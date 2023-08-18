using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblProdClassificationBL
    {
        List<TblProdClassificationTO> SelectAllTblProdClassificationList(string prodClassType = "");
        List<TblProdClassificationTO> SelectAllTblProdClassificationList(SqlConnection conn, SqlTransaction tran, string prodClassType = "");
        List<TblProdClassificationTO> SelectAllTblProdClassification(string parentProdClassId, string prodClassType = "");
        List<DropDownTO> SelectAllProdClassificationForDropDown(Int32 parentClassId);
        TblProdClassificationTO SelectTblProdClassificationTO(Int32 idProdClass);
        List<TblProdClassificationTO> SelectAllProdClassificationListyByItemProdCatgE(PurchaseTrackerAPI.StaticStuff.Constants.ItemProdCategoryE itemProdCategoryE);
        void SetProductClassificationDisplayName(TblProdClassificationTO tblProdClassificationTO, List<TblProdClassificationTO> allProdClassificationList);
        void GetDisplayName(List<TblProdClassificationTO> allProdClassificationList, int parentId, List<TblProdClassificationTO> DisplayNameList);
        int InsertProdClassification(TblProdClassificationTO tblProdClassificationTO);
        int InsertTblProdClassification(TblProdClassificationTO tblProdClassificationTO);
        int InsertTblProdClassification(TblProdClassificationTO tblProdClassificationTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDisplayName(List<TblProdClassificationTO> allProdClassificationList, TblProdClassificationTO ProdClassificationTO, SqlConnection conn, SqlTransaction tran);
          int UpdateProdClassification(TblProdClassificationTO tblProdClassificationTO);
        int UpdateTblProdClassification(TblProdClassificationTO tblProdClassificationTO);
          int UpdateTblProdClassification(TblProdClassificationTO tblProdClassificationTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblProdClassification(Int32 idProdClass);
        int DeleteTblProdClassification(Int32 idProdClass, SqlConnection conn, SqlTransaction tran);

    }
}