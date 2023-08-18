using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.StaticStuff
{
    public class ResultMessage
    {
        #region Members
        ResultMessageE messageType;
        String text;
        Exception exception;
        Int32 result;
        Object tag;
        String displayMessage;

        #endregion

        #region Constructor
        public ResultMessage()
        {
            text = "";
        }
        #endregion

        #region GetSet
        public object data { get; set; }
        public Exception Exception
        {
            get { return exception; }
            set { exception = value; }
        }
        public Object Tag
        {
            get { return tag; }
            set { tag = value; }
        }
        public ResultMessageE MessageType
        {
            get { return messageType; }
            set { messageType = value; }
        }
        public String Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// Sanjay [2017-05-30] Property added to Show Error occcurs at server side to User in proper format
        /// </summary>
        public String DisplayMessage
        {
            get { return displayMessage; }
            set { displayMessage = value; }
        }

        public int Result
        {
            get
            {
                return result;
            }

            set
            {
                result = value;
            }
        }

        #endregion

        #region Methods

        public void DefaultBehaviour()
        {
            this.messageType = ResultMessageE.Error;
            this.DisplayMessage = Constants.DefaultErrorMsg;
            this.result = 0;
        }

        public void DefaultBehaviour(String errorTxt)
        {
            this.messageType = ResultMessageE.Error;
            this.DisplayMessage = Constants.DefaultErrorMsg;
            this.Text = errorTxt;
            this.result = 0;
        }

        public void DefaultExceptionBehaviour(Exception ex,String methodName)
        {
            this.messageType = ResultMessageE.Error;
            this.Text = "Exception Error In Method "+ methodName;
            this.Result = -1;
            this.Exception = ex;
            this.DisplayMessage = Constants.DefaultErrorMsg;
        }

        public void DefaultSuccessBehaviour()
        {
            this.messageType = ResultMessageE.Information;
            this.Text = "Record Saved Successfully";
            this.Result = 1;
            this.DisplayMessage = Constants.DefaultSuccessMsg;
        }

        public void DefaultSuccessBehaviour(string displayMsg)
        {
            this.messageType = ResultMessageE.Information;
            this.Text = "Record Saved Successfully";
            this.Result = 1;
            this.DisplayMessage = displayMsg;
        }
        public ResultMessage DeepCopy()
        {
            ResultMessage other = (ResultMessage)this.MemberwiseClone();
            return other;
        }
        #endregion
    }
}
