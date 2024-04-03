using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Newtonsoft.Json;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace WebSDKStudio.Requests
{
    #region Classes

    /// <summary>
    /// User request class
    /// </summary>
    public class Request
    {
        private static readonly Regex RegexValidStatus = new Regex("(?!=Status.{1,6})OK", RegexOptions.IgnoreCase);

        #region Constants

        private static readonly Uri ErrorImage = new Uri(@"\Pictures\Error.png", UriKind.Relative);

        private static readonly Uri OkImage = new Uri(@"\Pictures\Ok.png", UriKind.Relative);

        /// <summary>
        /// Constant for the Content Length of the header in the request
        /// </summary>
        public const int CONTENT_LENGTH = 0;

        /// <summary>
        /// Constant to make a persistent connection to the internet ressource
        /// </summary>
        public const bool KEEP_ALIVE = false;

        #endregion

        #region Fields

        /// <summary>
        /// The response from the request
        /// </summary>
        private string m_response;

        #endregion

        #region Properties

        /// <summary>
        /// The application Id.
        /// </summary>
        public static string ApplicationId { get; set; }

        /// <summary>
        /// The password to connect.
        /// </summary>
        public static string Password { get; set; }

        /// <summary>
        /// The username to connect.
        /// </summary>
        public static string Username { get; set; }

        /// <summary>
        /// The content type which is going to be sent
        /// </summary>
        public static string Accept { get; set; }

        /// <summary>
        /// The Http Method used. [GET|POST|PUT|DELETE]
        /// </summary>
        public string HttpMethod { get; }

        /// <summary>
        /// Gets or sets the response for the request.
        /// </summary>
        public string Response
        {
            get => m_response;
            set
            {
                ResultIcon = RegexValidStatus.IsMatch(value) 
                    ? OkImage 
                    : ErrorImage;
                m_response = value;
            }
        }

        /// <summary>
        /// Result icon of the request. If it was processed correctly, a green check will show. Otherwise, a red X.
        /// </summary>
        public Uri ResultIcon { get; private set; }

        /// <summary>
        /// The Url of the request.
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// The Web Request Credentials used for the credentials of the request.
        /// </summary>
        public ICredentials WebRequestCredentials { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Public constructor for the class Request.
        /// </summary>
        /// <param name="httpMethod">The Http Method used. [GET|POST|PUT|DELETE]</param>
        /// <param name="url">The Url to use in the request</param>
        public Request(string httpMethod, string url)
        {
            HttpMethod = httpMethod;

            Url = url;

            SetWebRequestCredentials();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the Web Request Credentials.
        /// </summary>
        ///  
        /// <remarks>
        /// IMPORTANT NOTE
        /// The following values are valid for a demo system.
        /// The [Kxs...Nimv] 'sdkCertificateApplicationId' pertains to the demo SDK certificate (part number GSC-SDK).
        /// With this applicationId, this application can connect only to Security Center systems that have that part number (demo systems).
        /// Before going in production with a Web Sdk application, make sure you change
        /// the 'sdkCertificateApplicationId' with the applicationId from the production certificate.
        /// This production certificate is linked to a production part number, which needs to be present and visible in the Security Center 
        /// license options; otherwise this app won't connect (http error 403).
        /// The part number format typically is 'GSC-1SDK-DevelopmentCompany-Application'.
        /// It is also recommended to connect with a user other than Admin.
        /// </remarks>
        private void SetWebRequestCredentials()
        {
            var securityCenterUsername = Username;
            var securityCenterUserPassword = Password;
            var sdkCertificateApplicationId = ApplicationId;
            var webRequestUsername = $"{securityCenterUsername};{sdkCertificateApplicationId}";


            WebRequestCredentials = new NetworkCredential(webRequestUsername, securityCenterUserPassword);
        }

        #endregion
    }

    #endregion
}

