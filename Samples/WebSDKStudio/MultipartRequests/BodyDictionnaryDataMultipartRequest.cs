using System.Collections.Generic;

namespace WebSDKStudio.MultipartRequests
{
    internal class BodyDictionnaryDataMultipartRequest : MultipartRequest
    {

        public BodyDictionnaryDataMultipartRequest(string uriServer, WebSdkStudioWindow webSdkStudioWindow)
        {
            m_uriServer = uriServer;
            m_webSdkStudioWindow = webSdkStudioWindow;
        }

        public void UploadData(string posturl, Dictionary<string, string> postparameters)
        {
            foreach (var item in postparameters)
            {
                m_postParameters.Add(item.Key, new PayloadParameter(item.Value, "payload/txt"));
            }
            m_postUrl = posturl;

            MultipartFormDataPost();
        }
    }
}
