using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PurchaseTrackerAPI.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace PurchaseTrackerAPI.StaticStuff
{
    public class Constants
    {

        #region Common Methods
        /// <summary>
        /// if it is integer and zero then will return DBNull.Value
        /// if it is double and zero then will return DBNull.Value
        /// if it is datetime and is 1/1/1 then will return DBNull.Value
        /// 
        /// </summary>
        /// <param name="cSharpDataValue"></param>
        /// <returns></returns>
        public static object GetSqlDataValueNullForBaseValue(object cSharpDataValue)
        {
            if (cSharpDataValue == null)
            {
                return DBNull.Value;
            }
            else
            {
                if (cSharpDataValue.GetType() == typeof(DateTime))
                {
                    DateTime dt = (DateTime)cSharpDataValue;
                    if (dt.Year == 1 && dt.Month == 1 && dt.Day == 1)
                    {
                        return DBNull.Value;
                    }
                }
                else if (cSharpDataValue.GetType() == typeof(int))
                {
                    int intValue = (int)cSharpDataValue;
                    if (intValue == 0)
                    {
                        return DBNull.Value;
                    }
                }
                else if (cSharpDataValue.GetType() == typeof(Double))
                {
                    Double douValue = (Double)cSharpDataValue;
                    if (douValue == 0)
                    {
                        return DBNull.Value;
                    }
                }
                else if (cSharpDataValue.GetType() == typeof(Int64))
                {
                    Int64 bigIntValue = (Int64)cSharpDataValue;
                    if (bigIntValue == 0)
                    {
                        return DBNull.Value;
                    }
                }
                return cSharpDataValue;
            }
        }
        #endregion

        #region Enumerations


        public enum OrgTypeE
        {
            C_AND_F_AGENT = 1,
            DEALER = 2,
            COMPETITOR = 3,
            TRANSPOTER = 4,
            INTERNAL = 5,
            OTHER = 0,
            //Vaibhav 
            INFLUENCER = 1006,
            SUPPLIER = 6,
            //Vasp Solution Naresh...
            PURCHASE_COMPETITOR = 7,
            PURCHASE_MANAGER = 1,
        }

        public enum ShowSpotedVehicleE
        {
            SHOW_SPOTED_ENTRIES = 1,
            SHOW_NON_SPOTED_ENTRIES = 0,
            SHOW_ALL_ENTRIES = 3,
        }
        public enum RecycleTransactionTypeE
        {
            SPOT_ENTRY = 1,
            PURCHASE_ENQUIRY = 2,
        }
        public enum dimAddonsFunE
        {
            NOTES = 1,
            FILES = 2,
            PHOTOS = 3
        }
        public enum AddressTypeE
        {
            OFFICE_ADDRESS = 1,
            FACTORY_ADDRESS = 2,
            WORKS_ADDRESS = 3,
            GODOWN_ADDRESS = 4,
            //Vijaymla[01-11-2017] added to save organization new address of type office supply address
            OFFICE_SUPPLY_ADDRESS = 5
        }



        /*[Deepali]@2019/02/11 : Added the enum for the Role Type*/
        public enum SystemRoleTypeE
        {
            SYSTEM_ADMIN = 1,
            DIRECTOR = 2,
            C_AND_F_AGENT = 3,
            LOADING_PERSON = 4,
            MARKETING_FRONTIER = 5,
            MARKETING_BACK_OFFICE = 6,
            FIELD_OFFICER = 7,
            REGIONAL_MANAGER = 8,
            VICE_PRESIDENT_MARKETING = 9,
            ACCOUNTANT = 10,
            SECURITY_OFFICER = 11,
            SUPERWISOR = 12,
            WEIGHING_OFFICER = 13,
            BILLING_OFFICER = 14,
            TRANSPORTER = 15,
            DEALER = 16,
            GRADER = 17,
            RECOVERY_ENGINEER = 18,
            UNLOADING_SUPERVISOR = 19,
            PURCHASE_MANAGER = 20,

            PHOTOGRAPHER = 21,
        }

        //@KKM : Added enum for IOT
        public enum WeighingDataSourceE
        {
            DB = 1,
            IoT = 2,
            BOTH = 3
        }

        public enum ActiveSelectionTypeE
        {
            Both = 1,
            Active = 2,
            NonActive = 3
        }
        public enum TransactionTypeE
        {
            BOOKING = 1,
            LOADING = 2,
            DELIVERY = 3,
            SPECIAL_REQUIREMENT = 4,
            SAUDA = 501
        }

        //public   DateTime ServerDateTime
        //{
        //    get
        //    {
        //        return _iCommonDAO.SelectServerDateTime();
        //    }
        //}

        private static ILogger loggerObj;

        public static ILogger LoggerObj
        {
            get
            {
                return loggerObj;
            }

            set
            {
                loggerObj = value;
            }
        }

        public enum SaudaTypeE
        {
            NO_OF_VEHICLES = 1,
            TONNAGE_QTY = 2,
        }
        public enum CurrencyTypeE
        {
            INR = 1,
            USD = 2,
        }

        public enum SaudaTypeDescE
        {
            NO_OF_VEHICLES,
            TONNAGE_QTY,
        }
        public enum TxnTypeE
        {
            FOR_SUPPLIER = 1,
        }
        public enum TranStatusE
        {
            BOOKING_NEW = 1,
            //BOOKING_APPROVED = 2,
            //BOOKING_APPROVED_DIRECTORS = 3, //Saket [2017-11-10] Commented For SRJ.
            //BOOKING_APPROVED_FINANCE = 3,
            //LOADING_NEW = 4,
            //LOADING_NOT_CONFIRM = 5,
            //LOADING_WAITING = 6,
            //LOADING_CONFIRM = 7,
            //BOOKING_REJECTED_BY_FINANCE = 8,
            //BOOKING_APPROVED_BY_MARKETING = 9,
            //BOOKING_REJECTED_BY_MARKETING = 10,

            //LOADING_REPORTED_FOR_LOADING = 14,
            //LOADING_GATE_IN = 15,
            //LOADING_COMPLETED = 16,
            //LOADING_DELIVERED = 17,
            //LOADING_CANCEL = 18,
            //LOADING_POSTPONED = 19,
            //LOADING_VEHICLE_CLERANCE_TO_SEND_IN = 20, // It will be given by Loading Incharge to Security Officer
            //GJ@20170915 : Added the Unloading Status
            ///UNLOADING_NEW = 21,
            //UNLOADING_CANCELED = 23,

            //VEHICLE_REPORTED_FOR_LOADING=14,
            //VEHICLE_REQUESTED=27,
            //VEHICLE_INSPECTED=28,
            //SEND_IN=29,

            New = 501
    , VEHICLE_REPORTED_FOR_LOADING = 502
    // , VEHICLE_INSPECTED = 503

    , SEND_FOR_INSPECTION = 503
    , VEHICLE_REQUESTED = 504
    , SEND_IN = 505
    , SPOT_ENTRY_VEHICLE_APPROVED = 506
    , SPOT_ENTRY_VEHICLE_REJECTED = 507
    , UNLOADING_IS_IN_PROCESS = 508
    , UNLOADING_COMPLETED = 509
    , VEHICLE_OUT = 510 //Prajakta[04-Oct-2018] Added
    , BOOKING_PENDING_FOR_DIRECTOR_APPROVAL = 511  //Sanjay [2017-12-19] New Status when Finance Forward Booking to Director Approval.
    , PENDING_FOR_PURCHASE_MANAGER_APPROVAL = 512
    , BOOKING_ACCEPTED_BY_PM = 513
    , BOOKING_REJECTED_BY_PM = 514
    , BOOKING_REJECTED_BY_DIRECTOR = 515
    , BOOKING_DELETE = 516
    , BOOKING_ACCEPTED_BY_DIRECTOR = 517
    , COMPLETED = 518
    , VEHICLE_SCHEDULE_PENDING_FOR_APPROVAL = 519
    , VEHICLE_SCHEDULE_APPROVED = 520
    , VEHICLE_SCHEDULE_REJECTED = 521

    , QUALITY_FLAG_COMPLETED = 522
    , FLAG_RAISED = 523
    , SEND_FOR_VERIFICATION = 524               //Priyanka [20-02-2019]
    , CORRECTION_DETAILS_SAVED = 525          //Deepali [25/02/2019]    
    , DELETE_VEHICLE = 526                       //Priyanka [05-03-2019]
   , BOOKING_NEW_B = 527
   , BOOKING_APPROVED = 528

    , VEHICLE_CANCELED = 529            //Prajakta [2019-04-15] Added
    , VEHICLE_PENDING_FOR_YARD_MANAGER = 530

    , VEHICLE_REJECTED_AFTER_WEIGHING = 531
    , VEHICLE_REJECTED_BEFORE_WEIGHING = 532
    , WEIGHING_COMPLETED = 533
    , VEHICLE_REJECTED_AFTER_GROSS_WEIGHT = 534
                , BOOKING_CANCELED = 535
        }


        //    public enum TranStatusE
        //    {
        //        BOOKING_NEW = 1,
        //        BOOKING_APPROVED = 2,
        //        //BOOKING_APPROVED_DIRECTORS = 3, //Saket [2017-11-10] Commented For SRJ.
        //        BOOKING_APPROVED_FINANCE = 3,
        //        LOADING_NEW = 4,
        //        LOADING_NOT_CONFIRM = 5,
        //        LOADING_WAITING = 6,
        //        LOADING_CONFIRM = 7,
        //        BOOKING_REJECTED_BY_FINANCE = 8,
        //        BOOKING_APPROVED_BY_MARKETING = 9,
        //        BOOKING_REJECTED_BY_MARKETING = 10,

        //        BOOKING_DELETE = 50,
        //        //LOADING_REPORTED_FOR_LOADING = 14,
        //        LOADING_GATE_IN = 15,
        //        LOADING_COMPLETED = 16,
        //        LOADING_DELIVERED = 17,
        //        LOADING_CANCEL = 18,
        //        LOADING_POSTPONED = 19,
        //        LOADING_VEHICLE_CLERANCE_TO_SEND_IN = 20, // It will be given by Loading Incharge to Security Officer
        //        //GJ@20170915 : Added the Unloading Status
        //        UNLOADING_NEW = 21,
        //        UNLOADING_COMPLETED = 44,
        //        UNLOADING_CANCELED = 23,
        //        BOOKING_PENDING_FOR_DIRECTOR_APPROVAL = 45,  //Sanjay [2017-12-19] New Status when Finance Forward Booking to Director Approval.
        //        PENDING_FOR_PURCHASE_MANAGER_APPROVAL = 46,
        //        BOOKING_ACCEPTED_BY_PM = 47,
        //        BOOKING_REJECTED_BY_PM = 48,
        //        BOOKING_ACCEPTED_BY_DIRECTOR = 54,
        //        //VEHICLE_REPORTED_FOR_LOADING=14,
        //        //VEHICLE_REQUESTED=27,
        //        //VEHICLE_INSPECTED=28,
        //        //SEND_IN=29,

        //        New = 36
        //, VEHICLE_REPORTED_FOR_LOADING = 37
        //, VEHICLE_INSPECTED = 38
        //, VEHICLE_REQUESTED = 39
        //, SEND_IN = 40
        //, SPOT_ENTRY_VEHICLE_APPROVED = 41
        //, SPOT_ENTRY_VEHICLE_REJECTED = 42
        //, UNLOADING_IS_IN_PROCESS = 51
        //, VEHICLE_OUT = 52 //Prajakta[04-Oct-2018] Added
        //    }

        public enum LoadingLayerE
        {
            BOTTOM = 1,
            MIDDLE1 = 2,
            MIDDLE2 = 3,
            MIDDLE3 = 4,
            TOP = 5
        }

        public enum ShowListE
        {
            ALL = 1,
            UNLOADING = 2,
            GRADING = 3,
            RECOVERY = 4,
            ADD_FREIGHT = 5
        }


        /// <summary>
        /// Sanjay [2017-03-06] To Maintain the historical record for any transactional records
        /// </summary>
        public enum TxnOperationTypeE
        {
            NONE = 0,
            OPENING = 1,
            IN = 2,
            OUT = 3,
            UPDATE = 4
        }
        //ReshmaP[20-01-22] For Internal Transfer
        #region Tranfer Request
        public enum InternalTransferRequestStatusE {
                NEW= 2214,
                IN_PROCESS = 2215,
                CLOSE = 2213,
        }

        public enum InternalTransferRequestVehicalStatusE
        {
            NEW = 2208,
            IN_PROCESS = 2209,
            CLOSE = 2212,
            PENDING_FOR_APPROVAL=2210,
            REJECTED=2211,
            BREAKDOWN = 2314
        }
        public enum TransferVehicalApprovalStatusE
        {
            AUTHORIZE = 2216, 
            PENDING_FOR_APPROVAL = 2218,
            REJECTED = 2217,
        }
        public enum LoadingStatusE
        {
            PENDING_FOR_GATE_IN = 2301,
            LOADING_1ST_WEIGHING = 2302,
            UNDER_LOADING = 2303,
            LOADING_FINAL_WEIGHING = 2304,
            LOADING_GATE_OUT = 2305,
            UNLOADING_GATE_IN = 2306,
            UNLOADING_1ST_WEIGHING = 2307,
            UNDER_UNLOADING = 2308,
            UNDER_FINAL_WEIGHING = 2309,
            UNLOAD_GATE_OUT = 2310,
            PENDING_FOR_HOD_APPROVAL = 2311,
            COMPLETED = 2312,
            REJECTED = 2313
        }
        public enum MasterDimenstion
        {
            Unloading_Point=22
        }
        #endregion
        public enum SystemRolesE
        {
            SYSTEM_ADMIN = 1,
            DIRECTOR = 2,
            C_AND_F_AGENT = 3,
            LOADING_PERSON = 4,
            MARKETING_FRONTIER = 5,
            MARKETING_BACK_OFFICE = 6,
            FIELD_OFFICER = 7,
            REGIONAL_MANAGER = 8,
            VICE_PRESIDENT_MARKETING = 9,
            ACCOUNTANT = 10,
            SECURITY_OFFICER = 11,
            SUPERWISOR = 179,
            PURCHASE_MANAGER = 176,
            INSIDE_GRADER = 178,
        }

        public enum ProductCategoryE
        {
            NONE = 0,
            TMT = 1,
            PLAIN = 2
        }

        public enum ProductSpecE
        {
            NONE = 0,
            STRAIGHT = 1,
            BEND = 2,
            RK_SHORT = 3,
            RK_LONG = 4,
            TUKADA = 5,
            COIL = 6,
        }

        public enum BookingActionE
        {
            OPEN,
            CLOSE
        }

        public enum CommercialLicenseE
        {
            PAN_NO = 1,
            VAT_NO = 2,
            TIN_NO = 3,
            CST_NO = 4,
            EXCISE_REG_NO = 5,
            SGST_NO = 6,  //Prov GSTIN No
            IGST_NO = 7,  //Permenent GSTIN No
            CGST_NO = 8,
            CIN_NO = 9
        }

        public enum TxnDeliveryAddressTypeE
        {
            BILLING_ADDRESS = 1,
            CONSIGNEE_ADDRESS = 2
        }

        public enum AddressSourceTypeE
        {
            FROM_BOOKINGS = 1,
            FROM_DEALER = 2,
            FROM_CNF = 3,
            NEW_ADDRESS = 4
        }

        public enum InvoiceTypeE
        {
            REGULAR_TAX_INVOICE = 1,
            EXPORT_INVOICE = 2,
            DEEMED_EXPORT_INVOICE = 3,
            SEZ_WITH_DUTY = 4,
            SEZ_WITHOUT_DUTY = 5
        }


        public enum InvoiceStatusE
        {
            NEW = 1,
            PENDING_FOR_AUTHORIZATION = 2,
            AUTHORIZED_BY_DIRECTOR = 3,
            REJECTED_BY_DIRECTOR = 4,
            PENDING_FOR_ACCEPTANCE = 5,
            ACCEPTED_BY_DISTRIBUTOR = 6,
            REJECTED_BY_DISTRIBUTOR = 7,
            CANCELLED = 8,
            AUTHORIZED = 9,

            //Priyanka [18-02-2019]
            COMMERCIAL_VERIFIED = 10
        }

        /*GJ@20170913 : Added Enum for Loading Slip Type*/
        public enum LoadingTypeE
        {
            REGULAR = 1,
            OTHER = 2,
        }
        /*GJ@20170913 : Added Enum for Tax Type*/
        public enum TaxTypeE
        {
            IGST = 1,
            CGST = 2,
            SGST = 3,
        }

        /*GJ@20170913 : Added Enum for Other Tax Type*/
        public enum OthrTaxTypeE
        {
            PF = 1,
            FREIGHT = 2,
            CESS = 3,
            AFTER_CESS = 4,
            TCS = 5,
            OTHER_EXPENCES = 6,
            TRANSPORTER_ADVANCE = 7,
        }

        /*GJ@20170913 : Added Enum for Invoice Mod Type*/
        public enum InvoiceModeE
        {
            AUTO_INVOICE = 1,
            AUTO_INVOICE_EDIT = 2,
            MANUAL_INVOICE = 3,
        }

        /*GJ@20171007 : Weighing Measure Type*/
        public enum TransMeasureTypeE
        {
            TARE_WEIGHT = 1,
            INTERMEDIATE_WEIGHT = 2,
            GROSS_WEIGHT = 3,
            NET_WEIGHT = 4
        }
        // Vaibhav [18-Sep-2017] Added to department master

        public enum DepartmentTypeE
        {
            DIVISION = 1,
            DEPARTMENT = 2,
            SUB_DEPARTMENT = 3,
            BOM = 4,
        }

        // Vaibhav [7-Oct-2017] Added to visit persons
        public enum VisitPersonE
        {
            SITE_OWNER = 1,
            SITE_ARCHITECT = 2,
            SITE_STRUCTURAL_ENGG = 3,
            SITE_CONTRACTOR = 4,
            SITE_STEEL_PURCHASE_AUTHORITY = 5,
            DEALER = 6,
            DEALER_MEETING_WITH = 7,
            DEALER_VISIT_ALONG_WITH = 8,
            SITE_COMPLAINT_REFRRED_BY = 9,
            COMMUNICATION_WITH_AT_SITE = 10,
            INFLUENCER_VISITED_BY = 11,
            INFLUENCER_RECOMMANDEDED_BY = 12,
            SITE_EXECUTOR = 13,
        }

        // Vaibhav [7-Oct-2017] Added to visit follow up roles
        public enum VisitFollowUpActionE
        {
            SHARE_INFO_TO = 1,
            CALL_BY_SELF_TO = 2,
            ARRANGE_VISIT_OF = 3,
            ARRANGE_VISIT_TO = 4,
            ARRANGE_FOR = 5,
            POSSIBILITY_OF = 6
        }

        // Vaibhav [10-Oct-2017] added to visit issues 
        public enum VisitIssueTypeE
        {
            DELIVERY_ISSUE = 1,
            Quality_ISSUE = 2,
            PRICE_ISSUE = 3,
            ACCOUNT_ISSUE = 4,
            INFLUENCER_ISSUE = 5
        }

        // Vaibhav [23-Oct-2017] added to visit site type
        public enum VisitSiteTypeE
        {
            SITE_TYPE = 1,
            SITE_CATEGORY = 2,
            SITE_SUBCATEGORY = 3
        }

        // Vaibhav [24-Oct-2017] added to visit project type
        public enum VisitProjectTypeE
        {
            KEY_PROJECT = 1,
            CURRENT_PROJECT = 2
        }


        public enum QualityFormTypeE
        {
            RAISE = 1,
            COMPLETE = 2,
        }

        public enum MasterReportTypeE
        {
            SAUDA_DATE_REPORT = 1,
            UNLOADING_DATE_REPORT = 2,
            UNLOADING_DATE_PADTA_REPORT = 3,
        }

        // Vaibhav [27-Oct-2017] added to follow up roles
        public enum VisitFollowUpRoleE
        {
            SHARE_INFO_TO = 1,
            CALL_BY_SELF_TO_WHOM = 2,
            ARRANGE_VISIT_OF = 3,
            ARRANGE_VISIT_TO = 4,
            ARRANGE_VISIT_FOR = 5,
            POSSIBILITY_OF = 6
        }
        public enum PurchaseVehiclePhasesE
        {
            ORDER_DETAILS = 7,
            OUTSIDE_INSPECTION = 1,
            GRADING = 2,
            RECOVERY = 3,
            CORRECTIONS = 4,
            UNLOADING_COMPLETED = 5,

            CORRECTION_BASE_METAL_COMPARE_DTLS = 6,

            OUTSIDE_INSPECTION_FOR_NC = -1,
            GRADING_FOR_NC = -2,
            RECOVERY_FOR_NC = -3,
            CORRECTIONS_FOR_NC = -4,
            UNLOADING_COMPLETED_FOR_NC = -5,

        }

        public enum TRLoadingFilterE
        {
            Status = 1,
            Date = 2,
            StatusNDate = 3,
        }

        #endregion

        #region Constants Or Static Strings

        public static String Local_URL = "http://localhost:4200";
        //Added by Kiran for set current module id as per tblmodule sequence
        public static Int32 DefaultModuleID = 23;
        public static Boolean Local_API = Startup.IsLocalAPI;
        public static String CONNECTION_STRING = "ConnectionString";
        public static String AZURE_CONNECTION_STRING = "AzureConnectionStr";
        public static String MIGRATION_CONNECTION_STRING = "MIGRATION_CONNECTION_STRING";
        public static String REQUEST_ORIGIN_STRING = "RequestOriginString";
        public static String IdentityColumnQuery = "Select @@Identity";
        public static String DefaultCountry = "India";
        public static Int32 DefaultCountryID = 101;
        public static String DefaultDateFormat = "dd-MM-yyyy HH:mm tt";
        public static String AzureDateFormat = "yyyy-MM-dd HH:mm tt";
        public static String DefaultPassword = "123";
        public static String DefaultErrorMsg = "Error - 00 : Record Could Not Be Saved";
        public static String DefaultSuccessMsg = "Success - Record Saved Successfully";
        //Default Currency Id and Rate is Indain
        public static int DefaultCurrencyID = 1;
        public static int DefaultCurrencyRate = 1;

        // Vaibhav [26-Sep-2017] added to set default company id to Bhagyalaxmi Rolling Mills
        public static int DefaultCompanyId = 19;
        public static int DefaultSalutationId = 1;

        // Vaibhav [17-Dec-2017] Added to file encrypt descrypt and upload to azure
        public static string AzureConnectionStr = "DefaultEndpointsProtocol=https;AccountName=apkupdates;AccountKey=IvC+sc8RLDl3DeH8uZ97A4jX978v78bVFHRQk/qxg2C/J8w/DRslJlLsK7JTF+KhOM0MNUZg443GCVXe3jIanA==";
        //public static string EncryptDescryptKey = "MAKV2SPBNI99212";

        //public static string AzureConnectionStr = "DefaultEndpointsProtocol=https;AccountName=kalika;AccountKey=w9TJ8vgHZV86ksN+Sii21dlNSCELb01JVAqbYq1HSJl/uh7tpC5M7DVnVaSK1i78A4yKDWj0D6uwHPPU+ayTjg==;EndpointSuffix=core.windows.net";
        
        public static string AzureSourceContainerName = "simplitransfer";
        //public static string AzureSourceContainerName = "kalikadataextract";
        //public static string AzureSourceContainerName = "documents";
        //public static string AzureSourceContainerName = "documents";

        public static string AzureTargetContainerName = "simplitransfer";
        public static string ExcelSheetName = "TranData";
        public static string ExcelFileName = "TallyReport ";
        public static string ExcelFileNameTallyTransportEnquiry = "TallyTransportEnquiry ";
        public static string ExcelFileNameCCTransportEnquiry = "CCTransportEnquiry ";
        public static string ExcelFileNameTallyPREnquiry = "TallyPREnquiry ";
        public static string ExcelFileNameTallyCrNoteOrder = "TallyCrNoteOrderCS ";
        public static string ExcelFileNameGradeNoteOrderP = "GradeNoteOrderP ";
        public static string ExcelFileNameGradeNoteEnquiry = "GradeNoteEnquiry ";
        public static string ExcelFileNameWBReport = "WBReport";
        public static int LoadingCountForDataExtraction = 50;
        public static String ENTITY_RANGE_NC_LOADINGSLIP_COUNT = "NC_LOADINGSLIP_COUNT";
        public static int FinYear = 2017;
        public static String STOCK_API_URL = "StockApiURL";

        public static string SERVER_DATETIME_QUERY_STRING = "SERVER_DATETIME_QUERY_STRING";
        public static string IS_LOCAL_API = "IS_LOCAL_API";
        public static string VehicleWiseLoadingReport = "VehicleWiseLoadingReport";
        public static string WBDeviationReport = "WBDeviationReport";
        public static string POVsActualLoadingReport = "POVsActualLoadingReport";
        public static string LoadingUnloadingMasterReport = "LoadingUnloadingMasterReport";
        public static string SLASummaryReport = "SLASummaryReport";
        public static string SLAFurnaceSummaryReport = "SLAFurnaceSummaryReport";
        public static string CP_MIGRATE_TRANSFER_REQUEST_BEFORE_DAYS = "CP_MIGRATE_TRANSFER_REQUEST_BEFORE_DAYS";
        public static string CP_SLA_DOCUMENT_MANDATORY_FOR_FOLLOWING_TO_LOCATION = "CP_SLA_DOCUMENT_MANDATORY_FOR_FOLLOWING_TO_LOCATION";
        public static string TallyStockReport = "TallyStockReport";
        #endregion

        #region Configuration Sections
        public static string REPORT_TEMPLATE_FOLDER_PATH = "REPORT_TEMPLATE_FOLDER_PATH";
        public static string SAUDA_REPORT_TEMPLATE = "saudaRptTemplate";
        public static string MASTER_REPORT_TEMPLATE = "masterRptTemplate";
        public static string RATE_CHART_REPORT_TEMPLATE = "rateChartRptTemplate";

        public static string CP_MAX_ALLOWED_DEL_PERIOD = "MAX_ALLOWED_DEL_PERIOD";
        public static string LOADING_SLIP_DEFAULT_SIZES = "LOADING_SLIP_DEFAULT_SIZES";
        public static string SMS_SUBSCRIPTION_ACTIVATION = "SMS_SUBSCRIPTION_ACTIVATION";
        public static string CP_AUTO_DECLARE_LOADING_QUOTA_ON_STOCK_CONFIRMATION = "AUTO_DECLARE_LOADING_QUOTA_ON_STOCK_CONFIRMATION";
        public static string CP_SYTEM_ADMIN_USER_ID = "SYTEM_ADMIN_USER_ID";
        public static string CP_COMPETITOR_TO_SHOW_IN_HISTORY = "COMPETITOR_TO_SHOW_IN_HISTORY";

        //Vasp Solution Naresh...
        public static string CP_PURCHASE_COMPETITOR_TO_SHOW_IN_HISTORY = "PURCHASE_COMPETITOR_TO_SHOW_IN_HISTORY";
        public static string CP_SCRAP_COMPETITOR_HISTORY_COUNT = "CP_SCRAP_COMPETITOR_HISTORY_COUNT";
        public static string CP_SCRAP_PURCHASE_COMPETITOR_TO_SHOW_IN_HISTORY = "CP_SCRAP_PURCHASE_COMPETITOR_TO_SHOW_IN_HISTORY";

        public static string CP_DELETE_ALERT_BEFORE_DAYS = "DELETE_ALERT_BEFORE_DAYS";
        public static string CP_MIN_AND_MAX_RATE_DEFAULT_VALUES = "MIN_AND_MAX_RATE_DEFAULT_VALUES";

        public static string CP_SCRAP_MIN_AND_MAX_RATE_DEFAULT_VALUES = "CP_SCRAP_MIN_AND_MAX_RATE_DEFAULT_VALUES";
        public static string CP_WEIGHT_TOLERANCE_IN_KGS = "WEIGHT_TOLERANCE_IN_KGS";
        public static string CP_WEIGHING_WEIGHT_TOLERANCE_IN_PERC = "WEIGHING_WEIGHT_TOLERANCE_IN_PERC";
        public static string CP_BOOKING_RATE_MIN_AND_MAX_BAND = "BOOKING_RATE_MIN_AND_MAX_BAND";
        public static string CP_MAX_ALLOWED_CD_STRUCTURE = "MAX_ALLOWED_CD_STRUCTURE";
        public static string CP_LOADING_SLIPS_AUTO_CANCEL_STATUS_IDS = "LOADING_SLIPS_AUTO_CANCEL_STATUS_IDS";
        public static string CP_LOADING_SLIPS_AUTO_POSTPONED_STATUS_ID = "LOADING_SLIPS_AUTO_POSTPONED_STATUS_IDS";
        public static string CP_LOADING_DEFAULT_ALLOWED_UPTO_TIME = "LOADING_DEFAULT_ALLOWED_UPTO_TIME";
        public static string CP_LOADING_SLIPS_AUTO_CYCLE_STATUS_IDS = "LOADING_SLIPS_AUTO_CYCLE_STATUS_IDS";
        public static string CP_DEFAULT_MATE_COMP_ORGID = "DEFAULT_MATE_COMP_ORGID";
        public static string CP_DEFAULT_MATE_SUB_COMP_ORGID = "DEFAULT_MATE_SUB_COMP_ORGID";
        public static string CP_APP_CONFIGURATION_AUTHENTICATION = "APP_CONFIGURATION_AUTHENTICATION";
        public static string CP_FRIEGHT_OTHER_TAX_ID = "FRIEGHT_OTHER_TAX_ID";
        public static string CP_REVERSE_CHARGE_MECHANISM = "REVERSE_CHARGE_MECHANISM";
        public static string CP_DEFAULT_WEIGHING_SCALE = "DEFAULT_WEIGHING_SCALE";

        public static string CP_SCRAP_DEFAULT_WEIGHING_SCALE = "CP_SCRAP_DEFAULT_WEIGHING_SCALE";
        public static string CP_BILLING_NOT_CONFIRM_AUTHENTICATION = "BILLING_NOT_CONFIRM_AUTHENTICATION";
        public static string CONSOLIDATE_STOCK = "CONSOLIDATE_STOCK";
        public static String ENTITY_RANGE_REGULAR_TAX_INVOICE_BMM = "REGULAR_TAX_INVOICE_BMM";
        public static string CP_INTERNALTXFER_INVOICE_ORG_ID = "INTERNALTXFER_INVOICE_ORG_ID";


        // Vaibhav [29-Dec-2017] Added to config days to delete previous stock and quotadeclaration
        public static string CP_DELETE_PREVIOUS_STOCK_AND_PREVIOUS_QUOTADECLARATION_DAYS = "DELETE_PREVIOUS_STOCK_AND_PREVIOUS_QUOTADECLARATION_DAYS";
        public static string CP_MIGRATE_ENQUIRY_DATA = "MIGRATE_ENQUIRY_DATA";
        //Prajakta[27-08-2018] Added to get dashboard enquiry count as per enquiry status
        public static string CP_SCRAP_DASHBOARD_ENQ_QTY_STATUS = "CP_SCRAP_DASHBOARD_ENQ_QTY_STATUS";
        public static string CP_SCRAP_DEAULT_STATE_ID = "CP_SCRAP_DEAULT_STATE_ID";
        public static string CP_SCRAP_DEFAULT_RATE_REC_VARIABLES = "CP_SCRAP_DEFAULT_RATE_REC_VARIABLES";
        public static string CP_SCRAP_IS_SHOW_COMMERCIAL_DETAILS_AT_BOOKING = "CP_SCRAP_IS_SHOW_COMMERCIAL_DETAILS_AT_BOOKING";

        public static string CP_SCRAP_PADTA_CONFIGURATION_LIMIT = "CP_SCRAP_PADTA_CONFIGURATION_LIMIT";

        public static string CP_SCRAP_PADTA_CONFIG_LIMIT_FOR_ENQUIRY_AND_SAUDA_APPROVAL = "CP_SCRAP_PADTA_CONFIG_LIMIT_FOR_ENQUIRY_AND_SAUDA_APPROVAL";

        public static string CP_SCRAP_PADTA_CONFIGURATION_LIMIT_FOR_RECOVERY_CALCULATION = "CP_SCRAP_PADTA_CONFIGURATION_LIMIT_FOR_RECOVERY_CALCULATION";
        public static string CP_SCRAP_PADTA_CONFIGURATION_LIMIT_FOR_UNLOADING_QTY_BOOKING_QTY = "CP_SCRAP_PADTA_CONFIGURATION_LIMIT_FOR_UNLOADING_QTY_BOOKING_QTY";
        public static string CP_SCRAP_IS_CALCULATE_GRADING_DETAILS = "CP_SCRAP_IS_CALCULATE_GRADING_DETAILS";

        public static string CP_SCRAP_MAX_REC_VAL = "CP_SCRAP_MAX_REC_VAL";

        public static string CP_SCRAP_IS_FOR_SRJ = "CP_SCRAP_IS_FOR_SRJ";

        public static string CP_SCRAP_DEFAULT_MATERIAL_TYPE_FOR_SRJ = "CP_SCRAP_DEFAULT_MATERIAL_TYPE_FOR_SRJ";

        public static string CP_SCRAP_DEFAULT_C_OR_NC_ID_TYPE_FOR_SRJ = "CP_SCRAP_DEFAULT_C_OR_NC_ID_TYPE_FOR_SRJ";

        public static string CP_SCRAP_IS_CREATE_AUTO_SAUDA_FOR_SPOT_VEHICLE = "CP_SCRAP_IS_CREATE_AUTO_SAUDA_FOR_SPOT_VEHICLE";
        public static string SAPB1_SERVICES_ENABLE = "SAPB1_SERVICES_ENABLE";
        public static string CP_SCRAP_IS_TAKE_NONCOMMERCIAL_QTY_FOR_WEIGHTED_RATE = "CP_SCRAP_IS_TAKE_NONCOMMERCIAL_QTY_FOR_WEIGHTED_RATE";

        public static string CP_SCRAP_IS_TAKE_NONCOMMERCIAL_QTY_FOR_CALCULATED_METAL_COST = "CP_SCRAP_IS_TAKE_NONCOMMERCIAL_QTY_FOR_CALCULATED_METAL_COST";

        public static string AzureSourceContainerNameForDocument = "kalikadocuments";
        public static string AzureSourceContainerNameForTestingDocument = "testingdocuments";

        //Added by Minal 16 Aug 2021 For Date Back of years
        public static string DATE_BACK_YEARS = "DATE_BACK_YEARS";

        /// <summary>
        /// Sanjay [21 Sept 2018] To Define Purchase Manager Role
        /// </summary>
        public static string CP_PURCHASE_MANAGER_ROLE_ID = "PURCHASE_MANAGER_ROLE_ID";


        public static string CP_SCRAP_SHOW_GRADE_WISE_TRAGET_DETAILS = "CP_SCRAP_SHOW_GRADE_WISE_TRAGET_DETAILS";


        /// <summary>
        /// Sanjay [03 Oct 2018] To Define Scrap Enq Approval For the qty above mentioned of configured qty.
        /// </summary>
        public static string CP_SCRAP_QTY_AUTHORIZATION_AT_ENQUIRY = "SCRAP_QTY_AUTHORIZATION_AT_ENQUIRY";

        /// <summary>
        /// Priyanka [15-01-2019] Added to allow item wise booking rate.
        /// </summary>
        public static string CP_ITEM_WISE_BOOKING_RATE = "CP_ITEM_WISE_BOOKING_RATE";
        public static string CP_SCRAP_IS_FOR_BHAGYALAXMI = "CP_SCRAP_IS_FOR_BHAGYALAXMI";
        public static string CP_SCRAP_DEFAULT_RATE_REFERANCE_VARIABLE = "CP_SCRAP_DEFAULT_RATE_REFERANCE_VARIABLE";
        public static string CP_SCRAP_DEFAULT_RATE_REFERANCE_VARIABLE_C = "CP_SCRAP_DEFAULT_RATE_REFERANCE_VARIABLE_C";
        //public static string SERVER_DATETIME_QUERY_STRING = "SERVER_DATETIME_QUERY_STRING";
        public static string LOCAL_API = "LOCAL_API";


        public static string CP_SCRAP_SPOT_ENTRY_VEHICLE_QTY_TOLERANCE = "CP_SCRAP_SPOT_ENTRY_VEHICLE_QTY_TOLERANCE";
        public static string CP_SCRAP_SEND_SPOT_ENTRY_VEHICLE_FOR_APPROVAL_IF_SCHEDULE_QTY_IS_GREATER_THAN_SAUDA_QTY = "CP_SCRAP_SEND_SPOT_ENTRY_VEHICLE_FOR_APPROVAL_IF_SCHEDULE_QTY_IS_GREATER_THAN_SAUDA_QTY";
        public static string CP_SCRAP_CONFIG_SETTING_FOR_MATERIAL_TYPE_DONT_SEND_TO_APPROVAL = "CP_SCRAP_CONFIG_SETTING_FOR_MATERIAL_TYPE_DONT_SEND_TO_APPROVAL";

        public static string CP_SCRAP_CONFIG_SETTING_FOR_CONTAINER_AND_LOTSIZE = "CP_SCRAP_CONFIG_SETTING_FOR_CONTAINER_AND_LOTSIZE";

        public static string CP_SCRAP_IGNORE_STATUS_FOR_CORRECTION_SCREEN = "CP_SCRAP_IGNORE_STATUS_FOR_CORRECTION_SCREEN";
        public static string CP_SCRAP_OTHER_TAXES_FOR_TCS_IN_GRADE_NOTE = "CP_SCRAP_OTHER_TAXES_FOR_TCS_IN_GRADE_NOTE";
        public static string CP_SCRAP_OTHER_TAXES_FOR_OTHER_IN_PURCHASE_SUMMARY_REPORT = "CP_SCRAP_OTHER_TAXES_FOR_OTHER_IN_PURCHASE_SUMMARY_REPORT";

        //Added by minal [09 June 2021]
        public static string CP_SCRAP_MATERIAL_IDS_FOR_TALLY_CREDIT_NOTE_ORDER_CS_RPT = "CP_SCRAP_MATERIAL_IDS_FOR_TALLY_CREDIT_NOTE_ORDER_CS_RPT";
        public static string CP_SCRAP_MATERIAL_IDS_FOR_TALLY_PR_ENQUIRY_RPT = "CP_SCRAP_MATERIAL_IDS_FOR_TALLY_PR_ENQUIRY_RPT";

        public static string CP_SCRAP_OTHER_TAXES_FOR_TRANSPORTER_ADVANCE_PURCHASE_SUMMARY_REPORT = "CP_SCRAP_OTHER_TAXES_FOR_TRANSPORTER_ADVANCE_PURCHASE_SUMMARY_REPORT";

        //Added by minal [27-04-2021] 
        public static string CP_SCRAP_OTHER_TAXES_FOR_OTHER_EXPENSES_INSURANCE_AMT_PURCHASE_SUMMARY_REPORT = "CP_SCRAP_OTHER_TAXES_FOR_OTHER_EXPENSES_INSURANCE_AMT_PURCHASE_SUMMARY_REPORT";

        //Prajakta[2019-03-05] Added
        public static string CP_SCRAP_DASHBOARD_SAUDA_QTY_STATUS = "CP_SCRAP_DASHBOARD_SAUDA_QTY_STATUS";

        //Prajakta[2019-04-08] Added
        public static string CP_SCRAP_EXTRACTION_DAYS = "CP_SCRAP_EXTRACTION_DAYS";

        public static string CP_SCRAP_TAKE_TARE_WEIGHT = "CP_SCRAP_TAKE_TARE_WEIGHT";

        public static string CP_SCRAP_SUADA_PENDING_QTY_LIMIT = "CP_SCRAP_SUADA_PENDING_QTY_LIMIT";

        public static string CP_AZURE_CONNECTIONSTRING_FOR_DOCUMENTATION = "AZURE_CONNECTIONSTRING_FOR_DOCUMENTATION";

        public static string CP_SCRAP_STATUS_DISPLAYED_ON_PENDING_VEHICLE_SCREEN = "CP_SCRAP_STATUS_DISPLAYED_ON_PENDING_VEHICLE_SCREEN";


        public static string CP_SCRAP_UNLOADING_SUPERVISOR_VEHICLE_ALLOCATED_STATUS = "CP_SCRAP_UNLOADING_SUPERVISOR_VEHICLE_ALLOCATED_STATUS";
        public static string CP_SCRAP_GRADER_VEHICLE_ALLOCATED_STATUS = "CP_SCRAP_GRADER_VEHICLE_ALLOCATED_STATUS";
        public static string CP_SCRAP_RECOVERY_ENGINEER_VEHICLE_ALLOCATED_STATUS = "CP_SCRAP_RECOVERY_ENGINEER_VEHICLE_ALLOCATED_STATUS";

        public static string CP_SCRAP_TOLERANCE_VAL_IN_MT_TO_CLOSE_SAUDA = "CP_SCRAP_TOLERANCE_VAL_IN_MT_TO_CLOSE_SAUDA";

        public static string CP_SCRAP_IS_OPEN_QTY_SAUDA = "CP_SCRAP_IS_OPEN_QTY_SAUDA";

        public static string CP_SCRAP_IS_TAKE_ACTUAL_PENDING_BOOKING_QTY = "CP_SCRAP_IS_TAKE_ACTUAL_PENDING_BOOKING_QTY";

        public static string CP_SCRAP_IGONRE_VEH_STATUS_FOR_CLOSE_SAUDA = "CP_SCRAP_IGONRE_VEH_STATUS_FOR_CLOSE_SAUDA";

        public static string CP_SCRAP_TOLERANCE_VAL_IN_MT_TO_FOR_MANUAL_CLOSE_SAUDA = "CP_SCRAP_TOLERANCE_VAL_IN_MT_TO_FOR_MANUAL_CLOSE_SAUDA";

        public static string CP_SCRAP_IS_SAVE_UNLD_DATE_CORRECTION_DTLS = "CP_SCRAP_IS_SAVE_UNLD_DATE_CORRECTION_DTLS";

        public static string CP_SCRAP_IS_MAKE_RATE_REASON_MANDATORY = "CP_SCRAP_IS_MAKE_RATE_REASON_MANDATORY";

        public static string CP_SCRAP_IS_MAKE_COMMERCIAL_APPROVAL_MANDATORY = "CP_SCRAP_IS_MAKE_COMMERCIAL_APPROVAL_MANDATORY";

        //@KKM [30-7-2019] added for IOT
        public static string CP_WEIGHING_MEASURE_SOURCE_ID = "WEIGHING_MEASURE_SOURCE_ID";

        public static string CP_SCRAP_IS_AUTO_CLOSE_OPEN_SAUDA = "CP_SCRAP_IS_AUTO_CLOSE_OPEN_SAUDA";

        public static string CP_SCRAP_IS_UNLD_COMPLETE_WHILE_WEIGHING_COMPLETE = "CP_SCRAP_IS_UNLD_COMPLETE_WHILE_WEIGHING_COMPLETE";

        public static string CP_SCRAP_IS_PARTY_WT_MANDATORY_WHILE_WEIGHING_COMPLETE = "CP_SCRAP_IS_PARTY_WT_MANDATORY_WHILE_WEIGHING_COMPLETE";

        public static string CP_SCRAP_DEFAULT_IMPURITIES_STR = "CP_SCRAP_DEFAULT_IMPURITIES_STR";

        public static string CP_SCRAP_BIRIM_MACHINE_QTY = "CP_SCRAP_BIRIM_MACHINE_QTY";

        public static string CP_SCRAP_DAILY_RATE_DECLARATION_FOR_ENQUIRY = "CP_SCRAP_DAILY_RATE_DECLARATION_FOR_ENQUIRY";

        public static string CP_SCRAP_PARTY_NET_WT_ACTUAL_NET_WT_DIFF_TOLE_VAL = "CP_SCRAP_PARTY_NET_WT_ACTUAL_NET_WT_DIFF_TOLE_VAL";

        public static string CP_SCRAP_IS_AUTO_SUBMIT_RECOVERY_DTLS = "CP_SCRAP_IS_AUTO_SUBMIT_RECOVERY_DTLS";

        public static string CP_SCRAP_IS_CLOSE_EACH_SAUDA = "CP_SCRAP_IS_CLOSE_EACH_SAUDA";

        public static string CP_SCRAP_AUTO_ROUND_OF_DECIMAL_VAL = "CP_SCRAP_AUTO_ROUND_OF_DECIMAL_VAL";

        public static string CP_MATERIAL_TYPE_FOR_ADDING_SHIPMENT = "CP_MATERIAL_TYPE_FOR_ADDING_SHIPMENT";
        public static string CP_IS_CHECK_CONTAINER_DETAILS_WHILE_SAUDA_LINKING = "CP_IS_CHECK_CONTAINER_DETAILS_WHILE_SAUDA_LINKING";

        //Dhananjay[23-Nov-2021]
        public static string CP_ALWAYS_CREATE_NEW_SAUDA_FOR_EVERY_SPOT_ENTRY = "ALWAYS_CREATE_NEW_SAUDA_FOR_EVERY_SPOT_ENTRY";
        //chetan[21-jan-2020] added
        public static string CP_SCRAP_DB_TO_IOT_STATUS_IDS = "CP_SCRAP_DB_TO_IOT_STATUS_IDS";
        //chetan[09-june-2020]
        public static string CP_SCRAP_PRINT_NC_VEHICAL_REPORT_TEMPLATE_BASIS = "CP_SCRAP_PRINT_NC_VEHICAL_REPORT_TEMPLATE_BASIS";

        public static string CP_SCRAP_OTHER_TAXES_FOR_FREIGHT = "CP_SCRAP_OTHER_TAXES_FOR_FREIGHT";

        public static string CP_SCRAP_OTHER_TAXES_FOR_TCS = "CP_SCRAP_OTHER_TAXES_FOR_TCS";

        public static string CP_SCRAP_COMPLETE_VEH_STATUS_IDS = "CP_SCRAP_COMPLETE_VEH_STATUS_IDS";

        public static string CP_SCRAP_INSIDE_PREMISES_VEH_STATUS_IDS = "CP_SCRAP_INSIDE_PREMISES_VEH_STATUS_IDS";

        public static string CP_SCRAP_OUTSIDE_PREMISES_VEH_STATUS_IDS = "CP_SCRAP_OUTSIDE_PREMISES_VEH_STATUS_IDS";

        public static string CP_SCRAP_SAUDA_AVG_PRICE_OF_MATERIAL = "CP_SCRAP_SAUDA_AVG_PRICE_OF_MATERIAL";

        public static string CP_SCRAP_IS_ADD_VEHICLE_WISE_PROCESS_CHARGE = "CP_SCRAP_IS_ADD_VEHICLE_WISE_PROCESS_CHARGE";

        public static string CP_GRADE_NOTES_ORDER_P_MATERIAL_TYPE_NARRATION_REMARK = "CP_GRADE_NOTES_ORDER_P_MATERIAL_TYPE_NARRATION_REMARK";
        //ReshmaP Added For Internal Transfer
        public static string INTERNAL_TRANSFER_REQUEST = "INTERNAL_TRANSFER_REQUEST";

        public static string LOADING_DISPLAY_NO = "LOADING_DISPLAY_NO";
        public static string SLA_DISPLAY_NO = "SLA_DISPLAY_NO";
        public static string CommonNoteTrasactionType = "TRANSFER_LOADING";
        public static string CommonNotePageElementId = "TRUnLoadingAttachment";
        public static string CommonNoteLoadingPageElementId = "TRLoadingAttachment";
        public static string UNLOADING_SUMMARY_SCREEN = "UNLOADING_SUMMARY_SCREEN";
        public static string LOADING_SLIP_SUMMARY = "LOADING_SLIP_SUMMARY";
        #endregion

        #region Common functions
        public static String GetDateWithFormate(DateTime date)
        {
            return date.ToString("dd/MM/yyyy HH:mm");
        }

        public static Int32 GetDateTimeDiffInMin(DateTime startDate, DateTime endDate)
        {
            if (startDate == new DateTime() || endDate == new DateTime())
            {
                return 0;
            }
            TimeSpan diff = endDate - startDate;
            return diff.Minutes;
        }


        //Gokul [18-03-21] Replace more than one spacecs
        public static string removeUnwantedSpaces(string str)                         //create function
        {

            string pattern = "\\s+";

            string replacement = " ";                       // replacement pattern

            Regex rx = new Regex(pattern);

            string result = rx.Replace(str, replacement); // call to replace space
            return result;
        }

        public static Boolean IsNeedToRemoveFromList(string[] sizeList, Int32 materialId)
        {
            for (int i = 0; i < sizeList.Length; i++)
            {
                int sizeId = Convert.ToInt32(sizeList[i]);
                if (sizeId == materialId)
                {
                    return false;
                }
            }

            return true;
        }

        public static Boolean IsDateTime(String value)
        {
            try
            {
                Convert.ToDateTime(value);
                return true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }

        }

        public static Boolean IsInteger(String value)
        {
            try
            {
                Convert.ToInt32(value);
                return true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }

        }

        public static void SetNullValuesToEmpty(object myObject)
        {
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(myObject);
                    if (string.IsNullOrEmpty(value))
                    {
                        pi.SetValue(myObject, string.Empty);
                    }
                }
            }
        }

        public static DateTime GetStartDateTimeOfYear(DateTime dateTime)
        {
            if (dateTime.Month < 4)
                return GetStartDateTime(new DateTime(dateTime.Year - 1, 4, 1)); //1 Apr
            else
                return GetStartDateTime(new DateTime(dateTime.Year, 4, 1)); //1 Apr
        }

        public static DateTime GetEndDateTimeOfYear(DateTime dateTime)
        {
            if (dateTime.Month > 3)
                return GetEndDateTime(new DateTime(dateTime.Year + 1, 3, 31)); //31 March
            else
                return GetEndDateTime(new DateTime(dateTime.Year, 3, 31)); //31 March

        }

        public static DateTime GetStartDateTime(DateTime dateTime)
        {
            int day = dateTime.Day;
            int month = dateTime.Month;
            int year = dateTime.Year;
            return new DateTime(year, month, day, 0, 0, 0);
        }

        public static DateTime GetEndDateTime(DateTime dateTime)
        {
            int day = dateTime.Day;
            int month = dateTime.Month;
            int year = dateTime.Year;
            return new DateTime(year, month, day, 23, 59, 59);
        }

        public static List<string> GetChangedProperties(Object A, Object B)
        {
            if (A.GetType() != B.GetType())
            {
                throw new System.InvalidOperationException("Objects of different Type");
            }
            List<string> changedProperties = ElaborateChangedProperties(A.GetType().GetProperties(), B.GetType().GetProperties(), A, B);
            return changedProperties;
        }


        public static List<string> ElaborateChangedProperties(PropertyInfo[] pA, PropertyInfo[] pB, Object A, Object B)
        {
            List<string> changedProperties = new List<string>();
            foreach (PropertyInfo info in pA)
            {
                object propValueA = info.GetValue(A, null);
                object propValueB = info.GetValue(B, null);
                if (propValueA != null && propValueB != null)
                {
                    if (propValueA.ToString() != propValueB.ToString())
                    {
                        changedProperties.Add(info.Name);
                    }
                }
                else
                {
                    if (propValueA == null && propValueB != null)
                    {
                        changedProperties.Add(info.Name);
                    }
                    else if (propValueA != null && propValueB == null)
                    {
                        changedProperties.Add(info.Name);
                    }
                }
            }
            return changedProperties;
        }



        #endregion

        /// <summary>
        /// Vijaymala[31-10-2017]Added To Set Details Type for invoice other details
        /// </summary>

        public enum invoiceOtherDetailsTypeE
        {
            DESCRIPTION = 1,
            TERMSANDCONDITION = 2
        }
        public enum InvoiceGenerateModeE
        {
            REGULAR = 0,
            BRMTOBM = 1,
            BMTOCUSTOMER = 2
        }
        //Prajakta[2018-08-13] Added to get scrap material details
        [JsonConverter(typeof(StringEnumConverter))]
        public enum ItemProdCategoryE
        {
            REGULAR_RM = 1,
            FINISHED_GOODS = 2,
            SEMI_FINISHED_GOODS = 3,
            CAPITAL_GOODS = 4,
            SERVICE_CATG_ITEMS = 5,
            SCRAP_OR_WASTE = 6,

        }

        public enum txnTypeEnum
        {
            BOOKING = 1,
            LOADING = 2,
            DELIVERY = 3,
            SPECIAL_REQUIREMENT = 4,
            SCRAP = 7,
            SCRAP_BOOKING = 500,
            SCRAP_VEHICLE_SCHEDULE = 501,
            SCRAP_SPOT_VEHICLE = 502,
        }

        public enum ConfirmTypeE
        {
            CONFIRM = 1,
            NONCONFIRM = 0,
            BOTH = 2

        }

        //Added by minal 16 Aug 2021 Display Process charge only first row 
        public enum DisplayRecordInFirstRowE
        {
            FIRST_ROW = 1
        }

        public enum MaterialTypeE
        {
            LOCAL_AND_INDUSTRIAL = 67,
            IMPORT_SCRAP = 71,
        }

        public enum BRMReportNameE
        {
            TALLY_TRANSPORT_ENQUIRY = 1,
            CC_TRANSPORT_ENQUIRY = 2,
            TALLY_PR_ENQUIRY_REPORT = 3,
            TALLY_CREDIT_NOTE_ORDER_REPORT = 4,
            GRADE_NOTE_ENQUIRY_REPORT = 5,
            WB_REPORT = 6,
            TALLY_REPORT = 7,

        }
        public enum KalikaReportNameE
        {
            TALLY_REPORT = 1,
        }

        public enum VehSchApprovalTypeE
        {
            WT_RATE_APPROVAL = 1,
            SPOT_VEH_QTY_GRAETER_THAN_SAUDA_PEND_QTY = 2,
            PADTA_APPROVAL = 3

        }

        public enum AuthReasonIdsE
        {
            QTY = 1,
            RATE = 2,
            TARGET_QTY = 3,
            PADTA_OUT_OF_BAND = 4,
            EXCEEDS_WEIGHTED_BOOKING_RATE = 5,

        }

        public enum CurrencyE
        {
            INR = 1,
            USD = 2,
        }
       
        public enum ItemGradingFormTypeE
        {
            UNLOADING = 1,
            GRADING = 2,
            RECOVERY = 3,
            OUTSIDE_INSPECTION = 4,
            GRADERS_APPROVAL = 5,
            YARD_MANAGER = 6,
            GATE_IN_OUT = 7,
            SPOT_ENTRY_VEH = 8,
            GRADING_BEFORE_UNLOADING = 9,
        }

        public enum VehicleFilterE
        {
            ALL = 1,
            PENDING = 2,
            COMPLETED = 3,
        }

        public enum BaseMetalCostE
        {
            CURRENT_DATE = 1,
            UNLOADING_DATE = 2,
        }
        //chetan[25-feb-2020] added for generate templ5te report
        public enum ReportE
        {
            NONE = 1,
            EXCEL = 2,
            PDF = 3,
            BOTH = 4,
            PDF_DONT_OPEN = 5,
            EXCEL_DONT_OPEN = 6
        }
        public enum DashboardEntityE
        {
            Inventory=1,
            Punching=2,
            LocalScrap =3,
            Sponge=4,
            BirimMakina=5,
        }

        public enum TRLoadingTypeE
        {
            Manual =1,
            InternalTrasfer = 2,
        }
        public enum DocumentTypeE
        {
            LOADING = 1,
            UNLOADING = 2
        }
        public enum WeighingMeasureTypeIdE
        {
            TARE_WEIGHT = 1,
            INTERMEDIATE_WEIGHT = 2,
            GROSS_WEIGHT = 3,
            NET_WEIGHT = 4
        }
        public enum VehicalTypeE
        {
            REGULAR = 1,
            ONE_TIME = 2
        }
        public enum LoadingTypeIdE
        {
            LOADING = 1,
            UNLOADING = 2
        }
    }
    public static class ListExtensions
    {
        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / chunkSize)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList();
        }
    }
}
