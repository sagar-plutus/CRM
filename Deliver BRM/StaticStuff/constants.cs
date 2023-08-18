using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SalesTrackerAPI.StaticStuff
{
    public class Constants
    {

        //private static ILogger loggerObj;

        public Constants(ILogger<Constants> logger)
        {
          loggerObj = logger;
//Constants.LoggerObj = logger;
        }

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

        public enum DeliverReportNameE
        {
            TALLY_STOCK_TRANSFER_REPORT = 1,
            CC_STOCK_TRANSFER_REPORT = 2,
            TALLY_SALES_CHART_ENQUIRY_REPORT=3,
            WB_REPORT=4,
        }

        public enum ConfirmedTypeE
        {
            CONFIRM = 1,
            NONCONFIRM = 0,
        }

        public enum ApplicationModeTypeE
        {
            NORMAL_MODE = 1,
            BASIC_MODE = 2,
        }
        //vipul[06/05/2019] user subscription active count setting
        public enum UsersSubscriptionActiveCntCalSetting
        {
            ByTab = 1,
            WithOutTab=2,
        }
        public static void writeLog(string data)
        {
            loggerObj.LogError(data + " " + Constants.ServerDateTime.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
        }
        public enum OrgTypeE
        {
            C_AND_F_AGENT = 1,
            DEALER = 2,
            COMPETITOR = 3,
            TRANSPOTER = 4,
            INTERNAL = 5,
            SUPPLIER=6,
            OTHER = 0,
            //Vaibhav 
            //INFLUENCER = 1006,
            PURCHASE_COMPETITOR=7,
            INFLUENCER=8,
            PROJECT_ORGANIZATIONS = 9,
            USERS=15
        }
        public enum UsersConfigration
        {
            USER_CONFIG = 1
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

        public enum TransactionTypeE
        {
            BOOKING = 1,
            LOADING = 2,
            DELIVERY = 3,
            SPECIAL_REQUIREMENT = 4
        }
        public enum TxnAddressTypeE
        {
            BILLING = 1,
            CONSIGNEE = 2,
            SHIPPING = 3,
            BILLING_FROM_ADDRESS = 4        //Priyanka [28-05-2019]
        }
        public static DateTime ServerDateTime
        {
            get
            {
                return DAL.CommonDAO.SelectServerDateTime();
            }
        }

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

        public enum ReportE
        {
            NONE = 1,
            EXCEL = 2,
            PDF = 3,
            BOTH = 4,
            PDF_DONT_OPEN = 5,
            EXCEL_DONT_OPEN = 6
        }

        public enum TranStatusE
        {
            BOOKING_NEW = 1,
            BOOKING_APPROVED = 2,
            BOOKING_APPROVED_DIRECTORS = 3,
            LOADING_NEW = 4,
            LOADING_NOT_CONFIRM = 5,
            LOADING_WAITING = 6,
            LOADING_CONFIRM = 7,
            BOOKING_REJECTED_BY_DIRECTORS = 8,
            BOOKING_APPROVED_BY_MARKETING = 9,
            BOOKING_REJECTED_BY_MARKETING = 10,
            BOOKING_ACCEPTED_BY_CANDF = 11,
            BOOKING_REJECTED_BY_CANDF = 12,
            BOOKING_DELETE = 13,
            LOADING_REPORTED_FOR_LOADING = 14,
            LOADING_GATE_IN = 15,
            LOADING_COMPLETED = 16,
            LOADING_DELIVERED = 17,
            LOADING_CANCEL = 18,
            LOADING_POSTPONED = 19,
            LOADING_VEHICLE_CLERANCE_TO_SEND_IN = 20, // It will be given by Loading Incharge to Security Officer
            //GJ@20170915 : Added the Unloading Status
            UNLOADING_NEW = 21,
            UNLOADING_COMPLETED = 22,
            UNLOADING_CANCELED = 23,
            LOADING_IN_PROGRESS = 24,
            INVOICE_GENERATED_AND_READY_FOR_DISPACH = 25,
            PENDING_BOOKING_FOR_CNF_APPROVAL = 26,
            PENDING_DELIVERY_INFO_FOR_CNF_APPROVAL = 27,
            DELIVERY_INFO_APPROVED_BY_CANDF = 28,

            CANCEL_DELIVERY_INFO =29

        }
        public enum BookingTypeE
        {
            Normal=1,
            Bulk=2
        }
        public enum LoadingLayerE
        {
            BOTTOM = 1,
            MIDDLE1 = 2,
            MIDDLE2 = 3,
            MIDDLE3 = 4,
            TOP = 5
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
            Weighing_Officer = 13,
            Billing_Officer = 14,
            Transporter = 15,
            Dealer = 16
        }

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
            SUPERWISOR = 12,
            Weighing_Officer = 13,
            Billing_Officer = 14,
            Transporter = 15,
            Dealer = 16
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
            SEZ_WITHOUT_DUTY = 5,
            SERVICES = 6                        //Priyanka [23-05-2018]
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
        }

        /*GJ@20170913 : Added Enum for Loading Slip Type*/
        public enum LoadingTypeE
        {
            REGULAR = 1,
            OTHER = 2,
            DELIVERY_INFO = 3
        }
        /*GJ@20170913 : Added Enum for Tax Type*/
        public enum TaxTypeE
        {
            IGST = 1,
            CGST = 2,
            SGST = 3,
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
            DUPLICATE = 1,
            CHANGEFROM = 2
        }



        /// <summary>
        /// Sanjay [2018-02-19] To Define Item Product Categories
        /// Was Required to distiguish between Finished Good and Scrap
        /// </summary>
        /// <remarks> Enum for Item Product Categories Of The System</remarks>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum ItemProdCategoryE
        {
            REGULAR_RM = 1,
            FINISHED_GOODS = 2,
            SEMI_FINISHED_GOODS = 3,
            CAPITAL_GOODS = 4,
            SERVICE_CATG_ITEMS = 5,
            SCRAP_OR_WASTE = 6,
            GIFT_ITEMS=7,

        }
        
        //Priyanka [21-03-2018] : Added for Select Type of list in view booking summary list
        public enum SelectTypeE
        {
            DISTRICT = 1,
            STATE = 2,
            CNF = 3,
            DEALER = 4

        }

        public enum SupportPageTypE
        {
            BILLING = 16,
            LOADING_SLIP = 5
        }

        //Sudhir[20-APR-2018] Added for Person_Type.
        public enum PersonType
        {
            FIRST_OWNER = 15,
            SECOND_OWNER = 16
        }

        public enum FileTypesE
        {
            IMAGE=1,
            PDF=2,
            AUDIO=3,
            VIDEO=4
        }

        public enum ReportingTypeE
        {
            ADMINISTRATIVE = 1,
            TECHNICAL = 2
        }

        public enum UnitCategoryE
        {
            MATERIAL=1,
            COST=2,
            DISTANCE=3
        }

        public enum RouteTypeE
        {
            ACTUAL=1,
            SUGGESTED=2
        }

        //Priyanka [06-12-2018] : Added enum for modBusTCP.
        public enum WeighingDataSourceE
        {
            DB =1,
            IoT =2,
            BOTH = 3
        }

        public enum ActiveSelectionTypeE
        {
            Both = 1,
            Active = 2,
            NonActive = 3
        }

        /// <summary>
        /// Dhananjay [2020-11-19] For EInvoice API
        /// </summary>
        public enum EInvoiceAPIE
        {
            OAUTH_TOKEN = 1,
            EINVOICE_AUTHENTICATE = 2,
            GENERATE_EINVOICE = 3,
            CANCEL_EINVOICE = 4,
            GET_EINVOICE = 5,
            GENERATE_EWAYBILL = 6,
            CANCEL_EWAYBILL = 7
        }

        ///<summary>
        ///Aditee [04-01-2021] for generate invoice flag for Address Update
        ///</summary>
        public enum EGenerateEInvoiceCreationType
        {
            UPDATE_ONLY_ADDRESS = 1,
            GENERATE_INVOICE_ONLY = 2,
            INVOICE_WITH_EWAY_BILL = 3,
        }

        #endregion

        #region Constants Or Static Strings
        //Aditee
        public static String CP_EXISTING_ADDRESS_COUNT_FOR_BOOKING = "EXISTING_ADDRESS_COUNT_FOR_BOOKING";
        public static String IdentityColumnQuery = "Select @@Identity";
        //Added by Kiran for set current module id as per tblmodule sequence
        public static Int32 DefaultModuleID = 1;
        public static String DefaultCountry = "India";
        public static Int32 DefaultCountryID = 101;
        public static String DefaultDateFormat = "dd-MM-yyyy HH:mm tt";
        public static String DefaultDateOnlyFormat = "dd/MM/yyyy";
        public static String AzureDateFormat = "yyyy-MM-dd HH:mm tt";
        public static String DefaultPassword = "polaad@123";
        public static String DefaultErrorMsg = "Error - 00 : Record Could Not Be Saved";
        public static String DefaultSuccessMsg = "Success - Record Saved Successfully";
        //Default Currency Id and Rate is Indain
        public static int DefaultCurrencyID = 1;
        public static int DefaultCurrencyRate = 1;

        // Vaibhav [26-Sep-2017] added to set default company id to Bhagyalaxmi Rolling Mills
        public static int DefaultCompanyId = 19;
        public static int DefaultSalutationId = 1;

        // Vaibhav [17-Dec-2017] Added to file encrypt descrypt and upload to azure
        public static string AzureConnectionStr = "DefaultEndpointsProtocol=https;AccountName=apkupdates;AccountKey=IvC+sc8RLDl3DeH8uZ97A4jX978v78bVFHRQk/qxg2C/J8w/DRslJlLsK7JTF+KhOM0MNUZg443GCVXe3jIanA==;EndpointSuffix=core.windows.net";
        //public static string EncryptDescryptKey = "MAKV2SPBNI99212";
        //public static string AzureSourceContainerName = "documents";//"documents";
       // public static string AzureConnectionStr = "DefaultEndpointsProtocol=https;AccountName=testingdoc;AccountKey=VYfTkWd+MVtDgsjLk33o2KkX7CM0+vRXh5SjcPDNwBJU0+VjmxWyw20tUwhZzmrjWDXwCxeoH59FsGrNoWo8sQ==;EndpointSuffix=core.windows.net";
        //public static string AzureConnectionStr = "DefaultEndpointsProtocol=https;AccountName=testingdoc;AccountKey=VYfTkWd+MVtDgsjLk33o2KkX7CM0+vRXh5SjcPDNwBJU0+VjmxWyw20tUwhZzmrjWDXwCxeoH59FsGrNoWo8sQ==;EndpointSuffix=core.windows.net";
        // public static string AzureSourceContainerName = "simplirecycle";
        public static string AzureSourceContainerName = "documents";//"documents";
        public static string AzureTargetContainerName = "newdocuments";
        
        public static string ExcelSheetName = "TranData";
        public static string ExcelFileName = "Tran";
        public static string ExlFlnmCCStockTransfer = "CC_Stock_Transfer_Report ";
        public static string ExlFlnmTallyStockTransfer = "Tally_Stock_Transfer_Report";

        public static string ExlFlnmSCE = "Sales_Chart_Enquiry";
        public static string ExlFlnmSCEBars = "Tally_Sales_Enquiry";
        public static string ExlFlnmSCEInternal = "Sales_Internal_Transfer_Enquiry";


        public static string ExlFlnmWBReport = "WB_Report";
        public static int LoadingCountForDataExtraction = 50;
        public static String ENTITY_RANGE_NC_LOADINGSLIP_COUNT = "NC_LOADINGSLIP_COUNT";
        public static int FinYear = 2019;
        public static String ENTITY_RANGE_C_LOADINGSLIP_COUNT = "C_LOADINGSLIP_COUNT";
        public static String ENTITY_RANGE_LOADING_COUNT = "LOADING_COUNT";
        public static String ENTITY_RANGE_DELIVERYINFO_COUNT = "DELIVERYINFO_COUNT";
        public static String REGULAR_BOOKING = "REGULAR_BOOKING";
        public static String BULK_BOOKING = "BULK_BOOKING";

        // public static String ENTITY_RANGE_REGULAR_TAX_INVOICE_BMM = "REGULAR_TAX_INVOICE_BMM";

        public static string ENTITY_RANGE__BMM = "_BMM";
        //Sudhir[24-04-2018] Added for CRM Documents.
        //public static string AzureSourceContainerNameForDocument = "documentation";
        ////Sudhir[19-07-2018] Added for CRM Testing Documents.
        //public static string AzureSourceContainerNameForTestingDocument = "testingdocuments";

        public static int PreviousRecordDeletionDays = -2; //Use for Delete Previous 2 Days Records.

        //Hrushikesh added 
        //Permissions 
        public static String ReadWrite = "RW";
        public static String NotApplicable = "NA";
        public static String SAPB1_SERVICES_ENABLE = "SAPB1_SERVICES_ENABLE";

        /// <summary>
        /// Dhananjay [2021-01-02] For EInvoice API
        /// </summary>
        public static string EINVOICE_AUTHENTICATE = "einvoice/Authenticate";

        public static string SERVER_DATETIME_QUERY_STRING = "SERVER_DATETIME_QUERY_STRING";
        public static string IS_LOCAL_API = "IS_LOCAL_API";
        public static string SAP_LOGIN_DETAILS = "SAP_LOGIN_DETAILS";



        /// <summary>
        /// Dhananjay [2020-11-19] For EInvoice API
        /// </summary>
        public static Int32 secsToBeDeductededFromTokenExpTime = 120;
        public static String IS_MAP_MY_INDIA = "IS_MAP_MY_INDIA";
        public static string MAP_MY_INDIA_URL_FOR_myLocationAddress = "MAP_MY_INDIA_URL_FOR_myLocationAddress";
        public static string GOOGLE_MAP_API_URL_FOR_LAT_LONG = "GOOGLE_MAP_API_URL_FOR_LAT_LONG";

        #endregion

        #region Configuration Sections

        public static string CP_MAX_ALLOWED_DEL_PERIOD = "MAX_ALLOWED_DEL_PERIOD";
        public static string LOADING_SLIP_DEFAULT_SIZES = "LOADING_SLIP_DEFAULT_SIZES";
        public static string SMS_SUBSCRIPTION_ACTIVATION = "SMS_SUBSCRIPTION_ACTIVATION";
        public static string CP_AUTO_DECLARE_LOADING_QUOTA_ON_STOCK_CONFIRMATION = "AUTO_DECLARE_LOADING_QUOTA_ON_STOCK_CONFIRMATION";
        public static string CP_SYTEM_ADMIN_USER_ID = "SYTEM_ADMIN_USER_ID";
        public static string CP_COMPETITOR_TO_SHOW_IN_HISTORY = "COMPETITOR_TO_SHOW_IN_HISTORY";
        public static string CP_DELETE_ALERT_BEFORE_DAYS = "DELETE_ALERT_BEFORE_DAYS";
        public static string CP_MIN_AND_MAX_RATE_DEFAULT_VALUES = "MIN_AND_MAX_RATE_DEFAULT_VALUES";
        public static string CP_WEIGHT_TOLERANCE_IN_KGS = "WEIGHT_TOLERANCE_IN_KGS";
        public static string CP_WEIGHING_WEIGHT_TOLERANCE_IN_PERC = "WEIGHING_WEIGHT_TOLERANCE_IN_PERC";
        public static string CP_BOOKING_RATE_MIN_AND_MAX_BAND = "BOOKING_RATE_MIN_AND_MAX_BAND";
        public static string CP_MAX_ALLOWED_CD_STRUCTURE = "MAX_ALLOWED_CD_STRUCTURE";
        public static string CP_LOADING_SLIPS_AUTO_CANCEL_STATUS_IDS = "LOADING_SLIPS_AUTO_CANCEL_STATUS_IDS";
        public static string CP_LOADING_SLIPS_AUTO_POSTPONED_STATUS_ID = "LOADING_SLIPS_AUTO_POSTPONED_STATUS_IDS";
        public static string CP_LOADING_DEFAULT_ALLOWED_UPTO_TIME = "LOADING_DEFAULT_ALLOWED_UPTO_TIME";
        public static string CP_LOADING_SLIPS_AUTO_CYCLE_STATUS_IDS = "LOADING_SLIPS_AUTO_CYCLE_STATUS_IDS";
        public static string CP_DEFAULT_MATE_COMP_ORGID = "DEFAULT_MATE_COMP_ORGID";
        public static String ENTITY_RANGE_REGULAR_TAX_INTERNALORG = "REGULAR_TAX_INVOICE_ORG_";
        public static string CP_DEFAULT_MATE_SUB_COMP_ORGID = "DEFAULT_MATE_SUB_COMP_ORGID";
        public static string CP_APP_CONFIGURATION_AUTHENTICATION = "APP_CONFIGURATION_AUTHENTICATION";
        public static string CP_FRIEGHT_OTHER_TAX_ID = "FRIEGHT_OTHER_TAX_ID";
        public static string CP_REVERSE_CHARGE_MECHANISM = "REVERSE_CHARGE_MECHANISM";
        public static string CP_DEFAULT_WEIGHING_SCALE = "DEFAULT_WEIGHING_SCALE";
        public static string CP_DEFAULT_WEIGHING_SCALE_INTERNAL_TRANSFER = "DEFAULT_WEIGHING_SCALE_INTERNAL_TRANSFER";
        public static string CP_BILLING_NOT_CONFIRM_AUTHENTICATION = "BILLING_NOT_CONFIRM_AUTHENTICATION";
        public static string CP_INTERNALTXFER_INVOICE_ORG_ID = "INTERNALTXFER_INVOICE_ORG_ID";

        // Vaibhav [29-Dec-2017] Added to config days to delete previous stock and quotadeclaration
        public static string CP_DELETE_PREVIOUS_STOCK_AND_PREVIOUS_QUOTADECLARATION_DAYS = "DELETE_PREVIOUS_STOCK_AND_PREVIOUS_QUOTADECLARATION_DAYS";
        public static string CP_MIGRATE_ENQUIRY_DATA = "MIGRATE_ENQUIRY_DATA";
        public static string CP_MIGRATE_BEFORE_DAYS = "MIGRATE_BEFORE_DAYS";

        //Priyanka [22-08-2018] Added to send sms to selected roles
        public static string CP_ROLES_TO_SEND_SMS_ABOUT_RATE_AND_QUOTA = "ROLES_TO_SEND_SMS_ABOUT_RATE_AND_QUOTA";

        public static string CP_AZURE_CONNECTIONSTRING_FOR_DOCUMENTATION = "AZURE_CONNECTIONSTRING_FOR_DOCUMENTATION";
        public static string CP_AZURE_CONNECTIONSTRING_FOR_DOCUMENTATION_TESTING = "AZURE_CONNECTIONSTRING_FOR_DOCUMENTATION_TESTING";

        public static string DEFAULT_FETCH_SUCCESS_MSG = "Record Fetch Succesfully";

        public static string DEFAULT_NOTFOUND_MSG = " Record Could not be found";

        // @ModBusTCP : Priyanka [05-12-2018] Added to check nodeJS API configuration.
        public static string CP_WEIGHING_MEASURE_SOURCE_ID = "WEIGHING_MEASURE_SOURCE_ID";

        // @ModBusTCP : Priyanka [08-12-2018] Added to check Maximum loading sizes limit for creating loading slip.
        public static string CP_MAXIMUM_LOADING_SIZE_LIMIT = "MAXIMUM_LOADING_SIZE_LIMIT";

        public static string IS_SHOW_BOOKING_PAGE_SIZE_AND_ADDRESS_DTLS = "IS_SHOW_BOOKING_PAGE_SIZE_AND_ADDRESS_DTLS";
        public static string BOOKING_STATUS_ID_FOR_VIEW_SCHEDULE = "BOOKING_STATUS_ID_FOR_VIEW_SCHEDULE";

        public static string CP_POST_SALES_INVOICE_TO_SAP_DIRECTLY_AFTER_INVOICE_GENERATION = "CP_POST_SALES_INVOICE_TO_SAP_DIRECTLY_AFTER_INVOICE_GENERATION";

        //Harshala added
        public static string CP_TCS_OTHER_TAX_ID = "CP_TCS_OTHER_TAX_ID";

        public static string DEFAULT_TCS_PERCENT_IF_PAN_PRESENT = "DEFAULT_TCS_PERCENT_IF_PAN_PRESENT";

        public static string DEFAULT_TCS_PERCENT_IF_PAN_NOT_PRESENT = "DEFAULT_TCS_PERCENT_IF_PAN_NOT_PRESENT";

        public static string CP_IS_INCLUDE_TCS_TO_AUTO_INVOICE = "CP_IS_INCLUDE_TCS_TO_AUTO_INVOICE";

        //[2021-02-26] Dhananjay added
        public static string CP_EINVOICE_SHIPPING_ADDRESS = "CP_EINVOICE_SHIPPING_ADDRESS";
        //[2021-05-12] Dhananjay added
        public static string CP_EINVOICE_EWAY_BILL = "CP_EINVOICE_EWAY_BILL";
        public static string CP_DELIVER_INVOICE_TDS_TAX_PCT = "CP_DELIVER_INVOICE_TDS_TAX_PCT";
        
        #endregion

        #region Common functions

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
        //@Kiran 12/12/2018 for get setting of WEIGHING MEASURE SOURCE ID
        public static int getweightSourceConfigTO()
        {
            //Int32 weightSourceConfigId = (Int32)Constants.WeighingDataSourceE.DB;
            //TblConfigParamsTO weightSourceConfigTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_WEIGHING_MEASURE_SOURCE_ID);
            //if (weightSourceConfigTO != null)
            //{
            //    weightSourceConfigId = Convert.ToInt32(weightSourceConfigTO.ConfigParamVal);
            //}
            //return weightSourceConfigId;

            return Startup.WeighingSrcConfig;

        }
        public static int getModeIdConfigTO()
        {
            Int32 modeId = 1;
            TblModuleTO tblModuleTo = BL.TblModuleBL.SelectTblModuleTO(Constants.DefaultModuleID);
            if (tblModuleTo != null)
            {
                modeId = Convert.ToInt32(tblModuleTo.ModeId);
            }
            return modeId;
        }

        #endregion

    }
}
