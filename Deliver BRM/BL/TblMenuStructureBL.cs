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
    public class TblMenuStructureBL
    {
        #region Selection
        public static List<TblMenuStructureTO> SelectAllTblMenuStructureList()
        {
            return  TblMenuStructureDAO.SelectAllTblMenuStructure();
        }

        public static TblMenuStructureTO SelectTblMenuStructureTO(Int32 idMenu)
        {
            return  TblMenuStructureDAO.SelectTblMenuStructure(idMenu);
        }

       

        #endregion
        
        #region Insertion
        public static int InsertTblMenuStructure(TblMenuStructureTO tblMenuStructureTO)
        {
            return TblMenuStructureDAO.InsertTblMenuStructure(tblMenuStructureTO);
        }

        public static int InsertTblMenuStructure(TblMenuStructureTO tblMenuStructureTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblMenuStructureDAO.InsertTblMenuStructure(tblMenuStructureTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblMenuStructure(TblMenuStructureTO tblMenuStructureTO)
        {
            return TblMenuStructureDAO.UpdateTblMenuStructure(tblMenuStructureTO);
        }

        public static int UpdateTblMenuStructure(TblMenuStructureTO tblMenuStructureTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblMenuStructureDAO.UpdateTblMenuStructure(tblMenuStructureTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblMenuStructure(Int32 idMenu)
        {
            return TblMenuStructureDAO.DeleteTblMenuStructure(idMenu);
        }

        public static int DeleteTblMenuStructure(Int32 idMenu, SqlConnection conn, SqlTransaction tran)
        {
            return TblMenuStructureDAO.DeleteTblMenuStructure(idMenu, conn, tran);
        }

        #endregion
        
    }
}
