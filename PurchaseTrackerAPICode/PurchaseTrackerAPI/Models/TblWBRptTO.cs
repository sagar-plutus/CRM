using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblWBRptTO
    {
        #region Declarations

        String wBID;
        String userID;
        String orignalRSTNo;
        String additionalRSTNo;
        String date;
        String time;
        String materialType;
        String materialSubType;
        Decimal grossWeight;
        Decimal firstWeight;
        Decimal secondWeight;
        Decimal thirdWeight;
        Decimal forthWeight;
        Decimal fifthWeight;
        Decimal sixthWeight;
        Decimal seventhWeight;
        Decimal tareWeight;
        Decimal netWeight;
        String loadOrUnload;
        String fromLocation;
        String toLocation;
        String transactionType;
        String vehicleNumber;
        String vehicleStatus;
        String billType;
        String vehicleID;
        Int64 id;
        #endregion

        #region Constructor


        #endregion

        #region GetSet

        public String WBID
        {
            get { return wBID; }
            set { wBID = value; }
        }

        public String UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        public String OrignalRSTNo
        {
            get { return orignalRSTNo; }
            set { orignalRSTNo = value; }
        }
        public String AdditionalRSTNo
        {
            get { return additionalRSTNo; }
            set { additionalRSTNo = value; }
        }
        public String Date
        {
            get { return date; }
            set { date = value; }
        }
        public String Time
        {
            get { return time; }
            set { time = value; }
        }
        public String MaterialType
        {
            get { return materialType; }
            set { materialType = value; }
        }
        public String MaterialSubType
        {
            get { return materialSubType; }
            set { materialSubType = value; }
        }
        public Decimal GrossWeight
        {
            get { return grossWeight; }
            set { grossWeight = value; }
        }
        public Decimal FirstWeight
        {
            get { return firstWeight; }
            set { firstWeight = value; }
        }
        public Decimal SecondWeight
        {
            get { return secondWeight; }
            set { secondWeight = value; }
        }
        public Decimal ThirdWeight
        {
            get { return thirdWeight; }
            set { thirdWeight = value; }
        }
        public Decimal ForthWeight
        {
            get { return forthWeight; }
            set { forthWeight = value; }
        }
        public Decimal FifthWeight
        {
            get { return fifthWeight; }
            set { fifthWeight = value; }
        }
        public Decimal SixthWeight
        {
            get { return sixthWeight; }
            set { sixthWeight = value; }
        }
        public Decimal SeventhWeight
        {
            get { return seventhWeight; }
            set { seventhWeight = value; }
        }
        public Decimal TareWeight
        {
            get { return tareWeight; }
            set { tareWeight = value; }
        }
        public Decimal NetWeight
        {
            get { return netWeight; }
            set { netWeight = value; }
        }
        public String LoadOrUnload
        {
            get { return loadOrUnload; }
            set { loadOrUnload = value; }
        }
        public String FromLocation
        {
            get { return fromLocation; }
            set { fromLocation = value; }
        }
        public String ToLocation
        {
            get { return toLocation; }
            set { toLocation = value; }
        }
        public String TransactionType
        {
            get { return transactionType; }
            set { transactionType = value; }
        }
        public String VehicleNumber
        {
            get { return vehicleNumber; }
            set { vehicleNumber = value; }
        }
        public String VehicleStatus
        {
            get { return vehicleStatus; }
            set { vehicleStatus = value; }
        }
        public String BillType
        {
            get { return billType; }
            set { billType = value; }
        }
        public String VehicleID
        {
            get { return vehicleID; }
            set { vehicleID = value; }
        }       

        public Int64 Id
        {
            get { return id; }
            set { id = value; }
        }

        #endregion
    }
}
