using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class DimProdSpecDescTO
    {
        #region Declarations
        Int32 idProductSpecDesc;
        string prodSpecDesc;
        Int32 isActive;
        Int32 displaySequence;
        Int32 createdBy;
        DateTime createdOn;
        Int32 updatedBy;
        DateTime updatedOn;
        Int32 isVisible;        
        #endregion

        #region Constructor
        public DimProdSpecDescTO()
        {
        }
        #endregion

        #region GetSet

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

        public Int32 IdProductSpecDesc
        {
            get { return idProductSpecDesc; }
            set { idProductSpecDesc = value; }
        }
        public string ProdSpecDesc
        {
            get { return prodSpecDesc; }
            set { prodSpecDesc = value; }
        }

        public int IsActive { get => isActive; set => isActive = value; }

        public Int32 DisplaySequence
        {
            get { return displaySequence; }
            set { displaySequence = value; }
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

        public Int32 IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }

        #endregion
    }
}
