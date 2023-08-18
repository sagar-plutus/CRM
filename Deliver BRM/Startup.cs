using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.PlatformAbstractions;
using IdentityServer4.AccessTokenValidation;
using System;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.Models;
using System.Collections.Generic;
using SAPbobsCOM;
using SalesTrackerAPI.DAL;


namespace SalesTrackerAPI
{ 
    public class Startup
    {
        public SAPLoginDetails sapLogindtls;
        public static string DeliverUrl { get; private set; }
        public static string ConnectionString { get; private set; }
        public static string NewConnectionString { get; private set; }
        public static string AzureConnectionStr { get; private set; }

        public static Int32 WeighingSrcConfig { get; private set; }
        public static Int32 ModeId { get; private set; }

        public static string IoTBackUpConnectionString { get; private set; }
        public static List<int> AvailableModbusRefList { get;  set; }

        public static string StockUrl { get; private set; }

        public static string GateIotApiURL { get; set; }
        public static string SERVER_DATETIME_QUERY_STRING { get; private set; }
        public static Boolean IsLocalAPI { get; private set; }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //Sanjay[2017-02-11] For Logging Configuration
#if DEBUG
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Error()
            .WriteTo.RollingFile("../logs/error_log-{Date}.txt")
            .WriteTo.Logger(l => l
            .MinimumLevel.Warning()
            .MinimumLevel.Information()
            .WriteTo.RollingFile("../logs/warling_log-{Date}.log")).CreateLogger();
#else
                Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.RollingFile("../logs/log-{Date}.txt").CreateLogger();
#endif
        }

        public IConfiguration Configuration { get; }
        public static SAPbobsCOM.Company CompanyObject { get; private set; }
        public string SapConnectivityErrorCode { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin();
            corsBuilder.AllowCredentials();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", corsBuilder.Build());
            });

            // Sanjay [2017-02-08] All Properties Initials were got automatically converted in lowercase. Following code convert it into proper format
            services.AddMvc().AddJsonOptions(options => { options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented; });
            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            //Sanjay [2018-02-12] To Maintain API Documentation with Versions
            services.AddMvcCore().AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");

            services.AddApiVersioning(option =>
            {
                option.ReportApiVersions = true;
                //option.ApiVersionReader = new HeaderApiVersionReader("api-version");
                option.AssumeDefaultVersionWhenUnspecified = true;
                option.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddSwaggerGen(
       options =>
       {
           var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

           foreach (var description in provider.ApiVersionDescriptions)
           {
               options.SwaggerDoc(
                   description.GroupName,
                   new Info()
                   {
                       Title = $"SalesTrackerAPI {description.ApiVersion}",
                       Version = description.ApiVersion.ToString(),
                       Description = "SalesTrackerAPI V 1.0 Web API Services",
                       Contact = new Contact()
                       { Name = "Vega Innovations & Technoconsultants Pvt Ltd", Email = "sanjay.gunjal@vegainnovations.co.in", Url = "www.vegainnovations.co.in" }

                   });
              // options.IncludeXmlComments(GetXmlCommentsPath());
               options.DescribeAllEnumsAsStrings();
           }
       });

            services.AddMvc();
              
              DeliverUrl = Configuration.GetSection("Data:DeliverUrl").Value.ToString();
              StockUrl = Configuration.GetSection("Data:StockUrl").Value.ToString();

            
            ConnectionString = Configuration.GetSection("Data:DefaultConnection").Value.ToString();
            NewConnectionString = Configuration.GetSection("Data:NewDefaultConnection").Value.ToString();

            //IoTBackUpConnectionString = Configuration.GetSection("Data:IOTBackupDefaultConnection").Value.ToString();

            GateIotApiURL = Configuration.GetSection("Data:GateIotApiURL").Value.ToString();
            //Sudhir[19-July-2018] Added For Live Document and Testing Document

            GetDateTimeQueryString();
            IsLocalApi();
            SAPLoginDetails();
            DOSapLogin();

            TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsValByName(StaticStuff.Constants.CP_AZURE_CONNECTIONSTRING_FOR_DOCUMENTATION);
            if (tblConfigParamsTO != null)
            {
                AzureConnectionStr = tblConfigParamsTO.ConfigParamVal;
            }


            AvailableModbusRefList = DAL.TblLoadingDAO.GeModRefMaxData();

            WeighingSrcConfig = (Int32)SalesTrackerAPI.StaticStuff.Constants.WeighingDataSourceE.DB;
            //TblConfigParamsTO weightSourceConfigTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(StaticStuff.Constants.CP_WEIGHING_MEASURE_SOURCE_ID);
            //if (weightSourceConfigTO != null)
            //{
            //    WeighingSrcConfig = Convert.ToInt32(weightSourceConfigTO.ConfigParamVal);
            //}
            TblModuleTO tblModuleTO = BL.TblModuleBL.SelectTblModuleTO((int)StaticStuff.Constants.DefaultModuleID);
            if (tblModuleTO != null && tblModuleTO.IotconfigSetting != 0)
            {
                WeighingSrcConfig = tblModuleTO.IotconfigSetting;
                ModeId = tblModuleTO.ModeId;
            }

            // Vaibhav [12-Mar-2018] To config authentication server.
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
.AddIdentityServerAuthentication(options =>
{
    options.Authority = "http://localhost:5000"; // Auth Server
    options.RequireHttpsMetadata = false;
});
        }

        private string GetXmlCommentsPath()
        {
            var app = PlatformServices.Default.Application;
            return System.IO.Path.Combine(app.ApplicationBasePath, "SalesTrackerAPI.xml");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApiVersionDescriptionProvider provider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseCors("AllowAll");
            //app.UseApplicationInsightsRequestTelemetry();

            //app.UseApplicationInsightsExceptionTelemetry();

            //Sanjay [2017-02-11] For Logging
            if (env.IsDevelopment())
            {
                loggerFactory.AddDebug(LogLevel.Information).AddSerilog();
            }
            else
            {
                loggerFactory.AddDebug(LogLevel.Error).AddSerilog();
            }

            // Vaibhav [12-Mar-2018] To add Authentication.
            app.UseAuthentication();
            app.UseMvc();

            //Sanjay [2018-02-12] To Implement Swagger Documentation for APIs
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"../swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                });
        }

        public void GetDateTimeQueryString()
        {
            string sqlQuery = "SELECT CURRENT_TIMESTAMP AS ServerDate";

            TblConfigParamsTO tblConfigParamsTO = SalesTrackerAPI.DAL.TblConfigParamsDAO.SelectTblConfigParamsValByName(StaticStuff.Constants.SERVER_DATETIME_QUERY_STRING);
            if (tblConfigParamsTO != null)
            {
                sqlQuery = tblConfigParamsTO.ConfigParamVal;
            }
            SERVER_DATETIME_QUERY_STRING = sqlQuery;

        }

        public void IsLocalApi()
        {
            IsLocalAPI = false;
            TblConfigParamsTO tblConfigParamsTO = SalesTrackerAPI.DAL.TblConfigParamsDAO.SelectTblConfigParamsValByName(StaticStuff.Constants.IS_LOCAL_API);
            if (tblConfigParamsTO != null)
            {
                Int32 isLocalAPI = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                if (isLocalAPI == 1)
                    IsLocalAPI = true;
                else
                    IsLocalAPI = false;
            }
        }

        public void SAPLoginDetails()
        {
            sapLogindtls = new SAPLoginDetails();

            TblConfigParamsTO tblConfigParamsTO = SalesTrackerAPI.DAL.TblConfigParamsDAO.SelectTblConfigParamsValByName(StaticStuff.Constants.SAP_LOGIN_DETAILS);
            if (tblConfigParamsTO != null && tblConfigParamsTO.ConfigParamVal != null)
            {
                sapLogindtls = Newtonsoft.Json.JsonConvert.DeserializeObject<SAPLoginDetails>(tblConfigParamsTO.ConfigParamVal);
            }
        }


        public void DOSapLogin()
        {
            try
            {
                SAPbobsCOM.Company companyObject;
                //SAPbouiCOM.SboGuiApi sboGuiApi;

                companyObject = new SAPbobsCOM.Company();
                //sboGuiApi = new SAPbouiCOM.SboGuiApi();

                companyObject.CompanyDB = sapLogindtls.CompanyDB;

                companyObject.UserName = sapLogindtls.UserName;

                //companyObject.Password = "Sap@1234";
                //companyObject.Password = "Vega@123";
                companyObject.Password = sapLogindtls.Password;
                companyObject.language = SAPbobsCOM.BoSuppLangs.ln_English;
                companyObject.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2017;
                //companyObject.Server = "10.10.110.102";
                companyObject.Server = sapLogindtls.Server;

                companyObject.LicenseServer = sapLogindtls.LicenseServer;

                //companyObject.SLDServer = "52.172.136.203\\SQLEXPRESS,1430";
                companyObject.SLDServer = sapLogindtls.SLDServer;
                companyObject.DbUserName = sapLogindtls.DbUserName;

                companyObject.DbPassword = sapLogindtls.DbPassword;

                //companyObject.LicenseServer = "10.10.110.102:40000";

                //companyObject.UseTrusted = false;
                //string var = companyObject.GetLastErrorDescription();

                //string Error = companyObject.GetLastErrorDescription();

                int checkConnection = -1;

                checkConnection = companyObject.Connect();

                if (checkConnection == 0)
                    CompanyObject = companyObject;
                else
                    SapConnectivityErrorCode = "Connectivity Error Code : " + companyObject.GetLastErrorDescription() + " " + checkConnection.ToString();
            }
            catch (Exception ex)
            {
                SapConnectivityErrorCode = "SAP connectivity Exception " + ex.ToString();
            }
        }
    }

    public class SAPLoginDetails
    {


        string companyDB;
        string userName;
        string password;
        string server;
        string licenseServer;
        string sLDServer;
        string dbUserName;
        string dbPassword;


        public string CompanyDB
        {
            get { return companyDB; }
            set { companyDB = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string Server
        {
            get { return server; }
            set { server = value; }
        }

        public string LicenseServer
        {
            get { return licenseServer; }
            set { licenseServer = value; }
        }

        public string SLDServer
        {
            get { return sLDServer; }
            set { sLDServer = value; }
        }

        public string DbUserName
        {
            get { return dbUserName; }
            set { dbUserName = value; }
        }


        public string DbPassword
        {
            get { return dbPassword; }
            set { dbPassword = value; }
        }


    }
}
