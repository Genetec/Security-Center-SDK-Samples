// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using WebSDKStudio.Events;
using WebSDKStudio.Requests;

namespace WebSDKStudio
{

    /// <summary>
    /// Interaction logic for EventsMonitor.xaml
    /// </summary>
    public partial class EventsMonitor
    {

        private string ConnectionId = "ConnectionId:";

        #region Public Fields

        public static readonly DependencyProperty EventsReceivedSelectedItemProperty = DependencyProperty.Register(
            "EventsReceivedSelectedItem", typeof(Event), typeof(EventsMonitor), new PropertyMetadata(default(Event)));

        #endregion Public Fields

        #region Private Fields

        private readonly string m_uri;

        /// <summary>
        /// Last event received
        /// </summary>
        private Event m_lastEvent;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Observable collection for the Events received
        /// </summary>
        public ObservableCollection<Event> EventsReceivedItems { get; }

        /// <summary>
        /// Depedency property for the selected Event
        /// </summary>
        public Event EventsReceivedSelectedItem
        {
            get => (Event)GetValue(EventsReceivedSelectedItemProperty);
            set => SetValue(EventsReceivedSelectedItemProperty, value);
        }

        #endregion Public Properties

        #region Private Properties

        private Guid m_connectionId;

        private StreamReader m_streamReader;

        private HttpWebResponse m_webResponse;

        #endregion Private Properties

        #region Public Constructors

        /// <summary>
        /// Constructor of the class EventMonitor
        /// </summary>
        /// <param name="uri">The base uri for the connection</param>
        public EventsMonitor(string uri)
        {
            InitializeComponent();
            EventsReceivedItems = new ObservableCollection<Event>();
            m_uri = uri;
            StartMonitoring();
            DataContext = this;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Method to start the monitoring with the server. This has to be done before calling a subscribe with the sdk.
        /// We get the Uri that the server sends us and listen on it.
        /// </summary>
        public void StartMonitoring()
        {
            try
            {
                // Register to the event stream using the provided URI. We will do this asynchronously.
                var eventWebRequest = CreateHttpWebRequest(new Request("GET", m_uri + "events"));
                eventWebRequest.BeginGetResponse(OnEventStreamWebResponseReceived, eventWebRequest);
            }
            catch (WebException ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnClosed(EventArgs e)
        {
            var request = CreateHttpWebRequest(new Request("POST", $"{m_uri}events/closeconnection/{m_connectionId}"));
            request.BeginGetResponse(null, null);
            m_webResponse?.Dispose();
            m_streamReader?.Dispose();
            base.OnClosed(e);
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Method to create the HttpWebRequest
        /// </summary>
        /// <param name="request">The <see cref="Request"/> to be sent.</param>
        /// <returns></returns>
        private HttpWebRequest CreateHttpWebRequest(Request request)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(request.Url);
            webRequest.Method = request.HttpMethod;
            webRequest.Credentials = request.WebRequestCredentials;
            webRequest.Accept = Request.Accept;
            webRequest.ContentLength = Request.CONTENT_LENGTH;

            return webRequest;
        }

        /// <summary>
        /// When there is an event fired in the sdk, we receive it and show it in the sample.
        /// </summary>
        private void OnEventFired(string source, string eventName, DateTime eventTimeStamp)
        {
            // CheckAccess returns true if you're on the dispatcher thread
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new Action(() => { OnEventFired(source, eventName, eventTimeStamp); }), DispatcherPriority.Send, null);
                return;
            }

            //Create a new event to place it in the Observable Collection
            var newEvent = new Event
            {
                Source = source,
                EventName = eventName,
                EventTimeStamp = eventTimeStamp
            };

            EventsReceivedItems.Add(newEvent);

            //Scroll to the new event if the user did not select another one
            if (EventsReceivedItems.Count == 1 || m_lastEvent == EventsReceivedSelectedItem)
            {
                EventsReceivedSelectedItem = newEvent;
                m_eventsReceived.ScrollIntoView(EventsReceivedSelectedItem);
            }
            m_lastEvent = newEvent;
        }
        /// <summary>
        /// Event to receive all the events.
        /// </summary>
        /// <param name="ar">IAsyncResult</param>
        private void OnEventStreamWebResponseReceived(IAsyncResult ar)
        {
            const string boundary = "--GENETECBOUNDARY";
            const string keepAlive = "KeepAlive";
            var extractContentType = new Regex(@"(?!Content-type: )(?<Mime>\w*\/\w*)", RegexOptions.IgnoreCase);

            try
            {
                var asyncState = (HttpWebRequest) ar.AsyncState;
                m_webResponse = (HttpWebResponse)(asyncState).EndGetResponse(ar);
                m_streamReader = new StreamReader(m_webResponse.GetResponseStream() ?? throw new InvalidOperationException());
                var eventBuilder = new StringBuilder();
                var newEvent = false;
                var currentMime = string.Empty;
                while (true)
                {
                    var response = m_streamReader.ReadLine();

                    // We just don't care about those.
                    if (string.IsNullOrEmpty(response))
                        continue;

                    if (response.StartsWith(ConnectionId))
                    {
                        m_connectionId = Guid.Parse(response.Replace(ConnectionId, string.Empty));
                        continue;
                    }

                    // Alright, When it starts with alright you know it's complex ahah.
                    // Here since we support the 4 serialization, we gonna do some magic.
                    // Since you will probably only use 1 serialization, just look at the one you need.
                    if (response.Equals(boundary, StringComparison.CurrentCultureIgnoreCase))
                    {
                        newEvent = true;
                        continue;
                    }

                    // It's only to keep the connection alive.
                    if (response.Equals(keepAlive, StringComparison.CurrentCultureIgnoreCase))
                        continue;

                    if (newEvent)
                    {
                        var tempMime = extractContentType.Match(response)?.Groups["Mime"].Value.ToLower();
                        if (tempMime != "text/plain")
                        {
                            currentMime = tempMime;
                            newEvent = false;
                            eventBuilder.Clear();
                        }
                        continue;
                    }

                    if (string.IsNullOrEmpty(currentMime))
                        continue;

                    string eventName;
                    DateTime eventTimeStamp;
                    string source;
                    dynamic d;

                    switch (currentMime)
                    {
                        case "text/json":
                            d = JObject.Parse(response);

                            // In this scope we don't care of those events.
                            var jsonResult = d["Rsp"]["Result"];
                            if (jsonResult["AlarmTriggered"] != null
                                || jsonResult["AlarmAcknowledge"] != null)
                                continue;

                            eventName = jsonResult["EventType"].ToString();
                            eventTimeStamp = DateTime.Parse(jsonResult["Timestamp"].ToString());
                            source = jsonResult["SourceGuid"].ToString();

                            OnEventFired(source, eventName, eventTimeStamp);
                            eventBuilder.Clear();
                            break;
                        // Old JSON Serialization
                        case "application/jsonrequest":

                            // Honestly, do no try to use this in production.
                            // We do show how to do it, but it takes a LONG time to actually
                            // get the event. Many many many streams will be sent and it takes forever
                            // to get all of them. Can't change this because of the backward, hence the new Mimes.
                            // Use any of the other 3 Mimes and you should be Ok.
                            // I do insist on using the new Mimes "text/json" and "text/xml" instead of the old ones.

                            eventBuilder.Append(response);
                            try
                            {
                                // JSON is incorrect, so we fix it.
                                // This is really unstable.
                                // Again, DO NOT USE THIS MIME IN Production Environment.
                                var text = eventBuilder.ToString()
                                    .Replace("--GENETECBOUNDARY", string.Empty)
                                    .Replace("Content-type: text/plain", string.Empty)
                                    .Replace("Content-type: text/plai", string.Empty);
                                d = JObject.Parse(text);
                            }
                            catch (JsonException)
                            {
                                // Ignored because we did not receive everything yet.
                                // There are \r\n in the request.
                                continue;
                            }
                            catch (Exception)
                            {
                                continue;
                            }

                            var oldJsonResult = d["event"];
                            if (oldJsonResult == null)
                                continue;
                            eventName = oldJsonResult["EventType"].ToString();
                            eventTimeStamp = DateTime.Parse(oldJsonResult["Timestamp"].ToString());
                            source = oldJsonResult["SourceGuid"].ToString();

                            OnEventFired(source, eventName, eventTimeStamp);
                            eventBuilder.Clear();
                            break;

                        case "text/xml":
                            eventBuilder.Append(response);
                            XDocument doc;
                            try
                            {
                                doc = XDocument.Parse(eventBuilder.ToString());
                            }
                            catch (XmlException)
                            {
                                // Ignored because we did not receive everything yet.
                                // There are \r\n in the request.
                                continue;
                            }

                            var xmlResult = doc.Element("WebSdk").Element("Rsp").Element("Result");
                            if (xmlResult.Element("AlarmTriggered") != null
                                || xmlResult.Element("AlarmAcknowledge") != null)
                                continue;

                            eventName = xmlResult.Element("EventType").Value;
                            eventTimeStamp = DateTime.Parse(xmlResult.Element("Timestamp").Value);
                            source = xmlResult.Element("SourceGuid").Value;

                            OnEventFired(source, eventName, eventTimeStamp);
                            eventBuilder.Clear();
                            break;
                        // Old XML Serialization
                        case "application/xml":
                            eventBuilder.Append(response).Append(Environment.NewLine);
                            XDocument oldDoc;
                            try
                            {
                                oldDoc = XDocument.Parse(eventBuilder.ToString());
                            }
                            catch (XmlException)
                            {
                                continue;
                            }

                            // This means the event finished streaming.
                            var eventElement = oldDoc.Element("event");
                            if (eventElement == null)
                                continue;
                            eventName = eventElement.Element("EventType").Value;
                            eventTimeStamp = DateTime.Parse(eventElement.Element("Timestamp").Value);
                            source = eventElement.Element("SourceGuid").Value;

                            OnEventFired(source, eventName, eventTimeStamp);
                            eventBuilder.Clear();

                            break;

                        default:
                            continue;
                    }
                }
            }
            catch (Exception exception)
            {
                Dispatcher.Invoke(Close);
                if (exception.Message != "Unable to read data from the transport connection: A blocking operation was interrupted by a call to WSACancelBlockingCall.")
                {
                    Console.WriteLine("Stopped monitoring events because something went wrong. Connection Lost?");
                    Console.WriteLine("ERROR: " + exception.Message + " at " + DateTime.Now);
                    MessageBox.Show("Stopped monitoring events because something went wrong. Connection Lost?");
                }
            }
        }

        #endregion Private Methods
    }

}