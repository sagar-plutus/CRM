using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{

    /// <summary>
    /// Sanjay [2017-02-10] This Model is used to return the values to caller
    /// when dimensions needs to be shown in DropDown
    /// </summary>
    public class DropDownTO
    {

        #region

        Int32 value;
        String text;
        Object tag;
        
        #endregion

        #region Get Set

        /// <summary>
        /// Sanjay [2017-02-10] To Hold the Id of the Dropdown
        /// </summary>
        public int Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// Sanjay [2017-02-10] To Hold the Text to be shown in dropdown
        /// </summary>
        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
            }
        }

        public object Tag
        {
            get
            {
                return tag;
            }

            set
            {
                tag = value;
            }
        }

        #endregion
    }
}
