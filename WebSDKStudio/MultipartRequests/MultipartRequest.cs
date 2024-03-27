using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using WebSDKStudio.Requests;

namespace WebSDKStudio.MultipartRequests
{
    #region Classes

    /// <summary>
    /// Class to show how to do a MultiPartRequest
    /// For more information about MultiPart requests, look for this link : https://code.msdn.microsoft.com/windowsapps/WP8-Post-Multipart-Data-62fbbf72 
    /// </summary>
    internal class MultipartRequest
    {
        #region Constants

        /// <summary>
        /// The encoding in UTF8
        /// </summary>
        internal static readonly Encoding Encoding = Encoding.UTF8;

        /// <summary>
        /// The server uri. Does not contain any query.
        /// </summary>
        internal protected string m_uriServer;

        /// <summary>
        /// The webSdkStudioWindow
        /// </summary>
        internal protected WebSdkStudioWindow m_webSdkStudioWindow;

        /// <summary>
        /// Objects to be sent as part of the Body
        /// </summary>
        internal protected Dictionary<string, object> m_postParameters = new Dictionary<string, object>();

        /// <summary>
        /// Url to send data over
        /// </summary>
        internal protected string m_postUrl = string.Empty;
        
        #endregion

        #region Nested Classes and Structures

        public class FileParameter
        {
            #region Properties

            public string ContentType { get; set; }

            public byte[] File { get; set; }

            public string FileName { get; set; }

            #endregion

            #region Constructors

            public FileParameter(byte[] file, string filename, string contenttype)
            {
                File = file;
                FileName = filename;
                ContentType = contenttype;
            }

            #endregion
        }

        public class PayloadParameter
        {
            #region Properties

            public string ContentType { get; set; }

            public string Payload { get; set; }

            #endregion

            #region Constructors

            public PayloadParameter(string payload, string contenttype)
            {
                Payload = payload;
                ContentType = contenttype;
            }

            #endregion
        }

        #endregion

        #region Private Methods

        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {

            Stream formDataStream = new MemoryStream();
            var needsClrf = false;
            try
            {
                foreach (var param in postParameters)
                {
                    // Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
                    // Skip it on the first parameter, add it to subsequent parameters.
                    if (needsClrf)
                        formDataStream.Write(Encoding.GetBytes(Environment.NewLine), 0, Encoding.GetByteCount(Environment.NewLine));

                    needsClrf = true;

                    if (param.Value is FileParameter value)
                    {
                        var fileToUpload = value;

                        // Add just the first part of this param, since we will write the file data directly to the Stream
                        var header = $"--{new StringBuilder(boundary).Append(Environment.NewLine)}Content-Disposition: form-data; name=\"{param.Key}\"; filename=\"{fileToUpload.FileName ?? param.Key}\"{Environment.NewLine}Content-Type: {new StringBuilder(fileToUpload.ContentType ?? "application/octet-stream").Append(Environment.NewLine).Append(Environment.NewLine)}";

                        formDataStream.Write(Encoding.GetBytes(header), 0, Encoding.GetByteCount(header));

                        // Write the file data directly to the Stream, rather than serializing it to a string.
                        formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                    }
                    else if (param.Value is PayloadParameter payload)
                    {
                        var payloadToUpload = payload;

                        // Add just the first part of this param, since we will write the file data directly to the Stream
                        var header = $"--{new StringBuilder(boundary).Append(Environment.NewLine)}Content-Disposition: form-data; name=\"{param.Key}\"; \"{Environment.NewLine}Content-Type: {new StringBuilder(payloadToUpload.ContentType ?? "application/octet-stream").Append(Environment.NewLine).Append(Environment.NewLine)}";

                        formDataStream.Write(Encoding.GetBytes(header), 0, Encoding.GetByteCount(header));

                        // Write the file data directly to the Stream, rather than serializing it to a string.
                        formDataStream.Write(Encoding.ASCII.GetBytes(payloadToUpload.Payload), 0, payloadToUpload.Payload.Length);
                    }
                    else
                    {
                        var postData = $"--{new StringBuilder(boundary).Append(Environment.NewLine)}Content-Disposition: form-data; name=\"{param.Key}\"{new StringBuilder(Environment.NewLine).Append(Environment.NewLine).Append(param.Value)}";
                        formDataStream.Write(Encoding.GetBytes(postData), 0, Encoding.GetByteCount(postData));
                    }
                }

                // Add the end of the request.  Start with a newline
                var footer = new StringBuilder(Environment.NewLine).Append("--").Append(boundary).Append("--").Append(Environment.NewLine).ToString();
                formDataStream.Write(Encoding.GetBytes(footer), 0, Encoding.GetByteCount(footer));
            }
            catch (Exception ex)
            {
                throw new Exception("Network Issue : ", ex);
            }
            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            var formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();
            return formData;
        }





        internal void MultipartFormDataPost()
        {
            var formDataBoundary = $"----------{Guid.NewGuid():N}";
            var contentType = "multipart/form-data; boundary=" + formDataBoundary;

            var formData = GetMultipartFormData(m_postParameters, formDataBoundary);

            PostForm(contentType, formData);
        }

        private void PostForm(string contentType, byte[] formData)
        {
            var request = new Request("POST", m_postUrl);
            var httpWebRequest = WebRequest.Create(request.Url) as HttpWebRequest;

            if (httpWebRequest == null)
            {
                throw new NullReferenceException("request is not a http request");
            }

            // Set up the request properties.
            httpWebRequest.Method = request.HttpMethod;
            httpWebRequest.ContentType = contentType;
            httpWebRequest.CookieContainer = new CookieContainer();
            httpWebRequest.ContentLength = formData.Length;
            httpWebRequest.Credentials = request.WebRequestCredentials;

            httpWebRequest.BeginGetRequestStream(result =>
            {
                try
                {
                    var webRequest = (HttpWebRequest)result.AsyncState;
                    using (var requestStream = webRequest.EndGetRequestStream(result))
                    {
                        requestStream.Write(formData, 0, formData.Length);
                        requestStream.Close();
                    }
                    webRequest.BeginGetResponse(ar =>
                    {
                        try
                        {
                            var response = webRequest.EndGetResponse(ar);
                            var responseStream = response.GetResponseStream();
                            if (responseStream != null)
                            {
                                using (var sr = new StreamReader(responseStream))
                                {
                                    request.Response = sr.ReadToEnd();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            request.Response = ex.Message;
                        }
                        m_webSdkStudioWindow.OnMultipartResponse(request);
                    }, null);
                }
                catch (Exception ex)
                {
                    request.Response = ex.Message;
                    m_webSdkStudioWindow.OnMultipartResponse(request);
                }
            }, httpWebRequest);

        }

        #endregion
    }

    #endregion
}

