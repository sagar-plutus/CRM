using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class SaveProductImgTO
    {
        #region Declarations

        Int32 idPerson;
        String imgpath;
        public String imgName { get; set; }
        public String imgData { get; set; }
        #endregion

        #region Constructor

        public SaveProductImgTO()
        {

        }

        #endregion

        #region GetSet

        public Int32 IdPerson
        {
            get { return idPerson; }
            set { idPerson = value; }
        }
        public String Imgpath
        {
            get { return imgpath; }
            set { imgpath = value; }
        }

        #endregion
    }
}
