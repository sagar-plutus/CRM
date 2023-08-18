using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblPersonBL : ITblPersonBL
    {
        private readonly ITblPersonDAO _iTblPersonDAO;
        public TblPersonBL(ITblPersonDAO itblPersonDAO)
        {
            _iTblPersonDAO = itblPersonDAO;
        }
        #region Selection
        public  List<TblPersonTO> SelectAllTblPersonList()
        {
            return _iTblPersonDAO.SelectAllTblPerson();
        }

        public  TblPersonTO SelectTblPersonTO(Int32 idPerson)
        {
            return _iTblPersonDAO.SelectTblPerson(idPerson);
        }

        public  List<TblPersonTO> SelectAllPersonListByOrganization(int organizationId)
        {
            return _iTblPersonDAO.SelectAllTblPersonByOrganization(organizationId);
        }
        /// <summary>
        /// swati pisal
        /// To Get person details using userId
        /// </summary>
        /// <param name="tblPersonTO"></param>
        /// <returns></returns>
        public  TblPersonTO SelectAllPersonListByUser(int userId)
        {
            return _iTblPersonDAO.SelectAllTblPersonByUser(userId);
        }
        #endregion

        #region Insertion
        public  int InsertTblPerson(TblPersonTO tblPersonTO)
        {
            return _iTblPersonDAO.InsertTblPerson(tblPersonTO);
        }

        public  int InsertTblPerson(TblPersonTO tblPersonTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPersonDAO.InsertTblPerson(tblPersonTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblPerson(TblPersonTO tblPersonTO)
        {
            return _iTblPersonDAO.UpdateTblPerson(tblPersonTO);
        }

        public  int UpdateTblPerson(TblPersonTO tblPersonTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPersonDAO.UpdateTblPerson(tblPersonTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblPerson(Int32 idPerson)
        {
            return _iTblPersonDAO.DeleteTblPerson(idPerson);
        }

        public  int DeleteTblPerson(Int32 idPerson, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPersonDAO.DeleteTblPerson(idPerson, conn, tran);
        }

        #endregion
        
    }
}
