using Genetec.Sdk;
using Genetec.Sdk.Workflows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Web.Mvc;
using TokensDemo;

namespace SampleTokensDemo.Controllers
{
    public class HomeController : Controller
    {
        #region Fields

        // Password of the said User.
        static string Password = "";

        // This is the server of the Directory.
        static string ServerDirectory = "";

        // This is the Username for the connection. Must be a User in the directory.
        static string Username = "admin";

        // This is an hardcoded SDK client certificate.
        static string ClientCertificate = "KxsD11z743Hf5Gq9mv3+5ekxzemlCiUXkTFY5ba1NOGcLCmGstt2n0zYE9NsNimv";

        /// <summary>
        /// The Engine of the Sdk.
        /// </summary>
        static Engine SdkEngine;

        #endregion

        #region Constructors

        public HomeController()
        {
            SetupSdkEngine();
            
            // Register to important events
            //This event will be raised when the Engine is logged on.
            SdkEngine.LoginManager.LoggedOn += OnEngineLoggedOn;
            //This event will be raised when the Engine is logged off.
            SdkEngine.LoginManager.LoggedOff += OnEngineLoggedOff;
            //This event is raised when the Engine logon failed.
            SdkEngine.LoginManager.LogonFailed += OnEngineLogonFailed;
            //This event is raised when there is a status change on the Engine connection.
            SdkEngine.LoginManager.LogonStatusChanged += OnEngineLogonStatusChanged;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Manages all calls to index and ask for first login.
        /// </summary>
        public ActionResult Index()
        {
            SetupFirstLoginAndSession();

            return View();
        }

        /// <summary>
        /// Get Global Token sent to View and manages the errors.
        /// </summary>
        public ActionResult GetGlobalPrivilegeToken()
        {
            VerifyAndMaintainLogon();

            try
            {
                SdkTokenResult sdkGlobalTokenResult = SdkEngine.SecurityTokenManager.EnsureHasGlobalPrivileges();
                SendToViewbagDecodedGlobalToken(sdkGlobalTokenResult);
            }
            catch (Exception exception)
            {
                ViewBag.MessageError = exception.ToString();
            }

            return View("Index");
        }

        /// <summary>
        /// Get Web Token sent to View and manages the errors.
        /// </summary>
        public ActionResult GetWebToken()
        {
            VerifyAndMaintainLogon();

            try
            {
                SdkTokenResult sdkWebTokenResult = SdkEngine.SecurityTokenManager.GetWebTokenResult();
                SendToViewbagDecodedWebToken(sdkWebTokenResult);
            }
            catch (Exception exception)
            {
                ViewBag.MessageError = exception.ToString();
            }

            return View("Index");
        }

        /// <summary>
        /// Get Entity Token sent to View and manages the errors.
        /// </summary>
        public ActionResult GetEntityToken()
        {
            var guid = Request.Form["guid"];
            VerifyAndMaintainLogon();

            try
            {
                if (guid == "")
                {
                    throw new Exception("Guid field can't be empty. Please enter a valid Guid.");
                }

                SdkPrivilegeResults sdkPrivilegeResults = SdkEngine.SecurityTokenManager.EnsureHasPrivileges(new List<Guid> { new Guid(guid) });
                SendToViewbagDecodedEntityToken(sdkPrivilegeResults);
            }
            catch (Exception exception)
            {
                ViewBag.MessageError = exception.ToString();
            }

            return View("Index");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Manage the SdkEngine and its connexion for the purpose of this sample.
        /// </summary>
        private void SetupSdkEngine()
        {
            if (WebApplication.IsSdkEngineCreated == false)
            {
                WebApplication.Engine = new Engine();
                WebApplication.IsSdkEngineCreated = true;
            }

            SdkEngine = WebApplication.Engine;
        }

        /// <summary>
        /// Make a LogOn via the LoginManager of the SdkEngine.
        /// </summary>
        private void Login()
        {
            SdkEngine.ClientCertificate = ClientCertificate;
            SdkEngine.LoginManager.LogOn(ServerDirectory, Username, Password);
        }

        /// <summary>
        /// Calls for a Login is the SdkEngine is not connected.
        /// </summary>
        private void VerifyAndMaintainLogon()
        {
            if (!(SdkEngine.LoginManager.IsConnected))
            {
                Login();
            }
        }

        /// <summary>
        /// Manages session variable creation and first login.
        /// </summary>
        private void SetupFirstLoginAndSession()
        {
            if (Session["SESSION_LOGIN"] == null)
            {
                HttpContext.Session.Add("SESSION_LOGIN", "");
                Login();
            }
            else if (Session["SESSION_LOGIN"].ToString() == "")
            {
                Login();
            }
        }

        /// <summary>
        /// Decode GlobalToken and send Header, Payload and confirmation message to the View.
        /// </summary>
        /// <param name="sdkTokenResult">SdkTokenResult which contains a Header and Payload.</param>
        private void SendToViewbagDecodedGlobalToken(SdkTokenResult sdkTokenResult)
        {
            var jwt = sdkTokenResult.Token;
            var handler = new JwtSecurityTokenHandler();

            var token = handler.ReadJwtToken(jwt);
            ViewBag.TokenGlobaEncoded = sdkTokenResult.Token;

            string headers = GetTokenHeaderDecoded(token.Header);
            ViewBag.TokenGlobalHeader = headers;

            string payloads = GetTokePayloadDecoded(token.Payload);
            ViewBag.TokenGlobalPayload = payloads;

            ViewBag.MessageInfo = "The request fort Global Privilege Token via SDK was made successfully.";
        }

        /// <summary>
        /// Decode WebToken and send Header, Payload and confirmation message to the View.
        /// </summary>
        /// <param name="sdkTokenResult">SdkTokenResult which contains a Header and Payload.</param>
        private void SendToViewbagDecodedWebToken(SdkTokenResult sdkTokenResult)
        {
            var jwt = sdkTokenResult.Token;
            var handler = new JwtSecurityTokenHandler();

            var token = handler.ReadJwtToken(jwt);
            ViewBag.TokenWebEncoded = sdkTokenResult.Token;

            string headers = GetTokenHeaderDecoded(token.Header);
            ViewBag.TokenWebHeader = headers;

            string payloads = GetTokePayloadDecoded(token.Payload);
            ViewBag.TokenWebPayload = payloads;

            ViewBag.MessageInfo = "The request for Web Token via SDK made was successfully.";
        }

        /// <summary>
        /// Decode EntityToken and send Header, Payload and confirmation message to the View.
        /// </summary>
        /// <param name="sdkPrivilegeResults">SdkPrivilegeResults which contains a SdkTokenResult who has a Header and Payload.</param>
        private void SendToViewbagDecodedEntityToken(SdkPrivilegeResults sdkPrivilegeResults)
        {
            SdkTokenResult sdkTokenResult = sdkPrivilegeResults.TokensPerEntity.First().Value;
            var jwt = sdkTokenResult.Token;
            var handler = new JwtSecurityTokenHandler();

            var token = handler.ReadJwtToken(jwt);
            ViewBag.TokenEntityEncoded = sdkTokenResult.Token;

            string headers = GetTokenHeaderDecoded(token.Header);
            ViewBag.TokenEntityHeader = headers;

            string payloads = GetTokePayloadDecoded(token.Payload);
            ViewBag.TokenEntityPayload = payloads;

            ViewBag.MessageInfo = "The request Entity Token via SDK for Guid " + Request.Form["guid"] + " was made successfully.";
        }

        /// <summary>
        /// Decode JWT headers and return the parsed result.
        /// </summary>
        /// <param name="headers">JwtHeader to decode.</param>
        private string GetTokenHeaderDecoded(JwtHeader headers)
        {
            string output = "";
            var jwtDecoded = "{";

            foreach (var h in headers)
            {
                jwtDecoded += '"' + h.Key + "\":\"" + h.Value + "\",";
            }
            jwtDecoded += "}";

            output = JToken.Parse(jwtDecoded).ToString(Formatting.Indented);
            return output;
        }

        /// <summary>
        /// Decode JWT payloads and return the parsed result.
        /// </summary>
        /// <param name="payloads">JwtPayload to decode.</param>
        private string GetTokePayloadDecoded(JwtPayload payloads)
        {
            string output = "";
            var jwtDecoded = "{";

            foreach (var h in payloads)
            {
                jwtDecoded += '"' + h.Key + "\":\"" + h.Value + "\",";
            }
            jwtDecoded += "}";

            output = JToken.Parse(jwtDecoded).ToString(Formatting.Indented);
            return output;
        }

        /// <summary>
        /// Event is raised by the Engine when the logon has succeeded.
        /// </summary>
        /// <param name="sender">The Engine.</param>
        /// <param name="e">The Event.</param>
        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            ViewBag.MessageInfo = "Logged in as " + Username + ".";
            Session["SESSION_LOGIN"] = "loggedOn";
        }

        /// <summary>
        /// Event is raised by the Engine when the logon has failed.
        /// </summary>
        /// <param name="sender">The Engine.</param>
        /// <param name="e">The Event.</param>
        private void OnEngineLogonFailed(object sender, LogonFailedEventArgs e)
        {
            string error = e.FormattedErrorMessage;
            ViewBag.MessageError = error;
        }

        /// <summary>
        /// When the engine is trying to logon, this event will be raised when there is a status change.
        /// </summary>
        /// <param name="sender">The engine.</param>
        /// <param name="e">The event.</param>
        private void OnEngineLogonStatusChanged(object sender, LogonStatusChangedEventArgs e)
        {  
        }

        /// <summary>
        /// Event raised when the Engine Logged off.
        /// </summary>
        /// <param name="sender">The Engine.</param>
        /// <param name="e">The Event.</param>
        private void OnEngineLoggedOff(object sender, LoggedOffEventArgs e)
        {
        }

        #endregion
    }
}