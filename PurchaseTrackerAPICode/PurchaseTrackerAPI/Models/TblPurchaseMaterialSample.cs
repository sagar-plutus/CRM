using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseMaterialSampleTO
    {
        #region Declarations
        //for tblPurchaseMaterialSample table
        Int32 idPurchaseMaterialSample;
        Int32 purchaseScheduleSummaryId;
        Int32 userId;
        Int32 phaseId;
        Int32 purchaseMaterialSampleTypeId;
        Int32 purchaseMaterialSampleCategoryId;         
        Boolean isDone;
        String vehicleNo;
        Int32 createdBy;
        DateTime createdOn;
        Int32 updatedBy;
        DateTime updatedOn;
        String comments;
        bool testTypeGalvanize;
        Boolean testTypeRustAndRust  ;
        Boolean testPhysical   ;
        Boolean test5KG   ;
        Boolean testSpectro  ;

        #endregion

        #region Constructor
        public TblPurchaseMaterialSampleTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPurchaseMaterialSample
        {
            get { return idPurchaseMaterialSample; }
            set { idPurchaseMaterialSample = value; }
        }

        public Int32 PurchaseScheduleSummaryId
        {
            get { return purchaseScheduleSummaryId; }
            set { purchaseScheduleSummaryId = value; }
        }
        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public Int32 PhaseId
        {
            get { return phaseId; }
            set { phaseId = value; }
        }
        public Int32 PurchaseMaterialSampleTypeId
        {
            get { return purchaseMaterialSampleTypeId; }
            set { purchaseMaterialSampleTypeId = value; }
        }
        public Int32 PurchaseMaterialSampleCategoryId
        {
            get { return purchaseMaterialSampleCategoryId; }
            set { purchaseMaterialSampleCategoryId = value; }
        }
        public Boolean IsDone
        {
            get { return isDone; }
            set { isDone = value; }
        }
        
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        public String VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }

         
        

        public bool TestTypeGalvanize
        {
            get { return testTypeGalvanize; }
            set { testTypeGalvanize = value; }
        }
        public bool TestTypeRustAndRust
        {
            get { return testTypeRustAndRust; }
            set { testTypeRustAndRust = value; }
        }
        public bool TestPhysical
        {
            get { return testPhysical; }
            set { testPhysical = value; }
        }
        public bool Test5KG
        {
            get { return test5KG; }
            set { test5KG = value; }
        }
        public bool TestSpectro
        {
            get { return testSpectro; }
            set { testSpectro = value; }
        }

        #endregion

        #region Methods


        #endregion
    }

    public class TblPurchaseMaterialSampleTypeTo
    {
        #region Declarations
        //for [tblPurchaseMaterialSampleType] table
        Int32 idPurchaseMaterialSampleType;
        String typeName;        
        Int32 createdBy;
        DateTime createdOn;
        Int32 updatedBy;
        DateTime updatedOn;
        


        #endregion

        #region Constructor
        public TblPurchaseMaterialSampleTypeTo()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPurchaseMaterialSampleType
        {
            get { return idPurchaseMaterialSampleType; }
            set { idPurchaseMaterialSampleType = value; }

        }
        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }
        

        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        } 

        #endregion

        #region Methods


        #endregion
    }

    public class TblPurchaseMaterialSampleCategoryTo
    {
        #region Declarations
        //for [tblPurchaseMaterialSampleType] table
        Int32 idPurchaseMaterialSampleCategory;
        String typeName;
        Int32 createdBy;
        DateTime createdOn;
        Int32 updatedBy;
        DateTime updatedOn;



        #endregion

        #region Constructor
        public TblPurchaseMaterialSampleCategoryTo()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPurchaseMaterialSampleCategory
        {
            get { return idPurchaseMaterialSampleCategory; }
            set { idPurchaseMaterialSampleCategory = value; }

        }
        public string CategoryName
        {
            get { return typeName; }
            set { typeName = value; }
        }


        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }


    

        #endregion

        #region Methods


        #endregion
    }
}
