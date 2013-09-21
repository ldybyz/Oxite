//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using Oxite.Model;
using Oxite.Services;

namespace Oxite.BackgroundServices
{
    public class SendTrackbacks : BackgroundServiceBase, IBackgroundService
    {
        private readonly ITrackbackOutboundService trackbackOutboundService;

        public SendTrackbacks(IPluginService pluginService, ITrackbackOutboundService trackbackOutboundService)
            : base(pluginService)
        {
            this.trackbackOutboundService = trackbackOutboundService;

            ID = new Guid("{67E3E02C-1522-4814-8693-2FBBD37F71B8}");
            Name = "Oxite Trackback Sender";
            Category = "Background Services";
        }

        #region IBackgroundService Members

        public bool ExecuteOnAll
        {
            get
            {
                return bool.Parse(GetSetting("ExecuteOnAll"));
            }
            set
            {
                SaveSetting("ExecuteOnAll", value.ToString());
            }
        }

        public TimeSpan Interval
        {
            get
            {
                return new TimeSpan(long.Parse(GetSetting("Interval")));
            }
            set
            {
                SaveSetting("Interval", value.Ticks.ToString());
            }
        }

        public void Run()
        {
            foreach (TrackbackOutbound trackback in trackbackOutboundService.GetNextOutbound(ExecuteOnAll, Interval))
            {
                try
                {
                    sendTrackback(trackback);

                    trackback.MarkAsCompleted();
                }
                catch
                {
                    trackback.MarkAsFailed();
                }
                finally
                {
                    trackbackOutboundService.Save(trackback);
                }
            }
        }

        #endregion

        #region Methods

        protected override void OnInitializeSettings()
        {
            base.OnInitializeSettings();

            Settings["ExecuteOnAll"] = true.ToString();
            Settings["Interval"] = TimeSpan.FromMinutes(10).Ticks.ToString();
            Settings["RetryInterval"] = TimeSpan.FromHours(6).Ticks.ToString();
            Settings["RetryCount"] = 28.ToString();
        }

        private static void sendTrackback(TrackbackOutbound trackback)
        {
            WebClient wc = new WebClient();
            string pageText = wc.DownloadString(trackback.TargetUrl);
            string trackBackItem = getTrackBackText(pageText, trackback.TargetUrl, trackback.PostUrl);

            if (trackBackItem != null)
            {
                if (!trackBackItem.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
                {
                    trackBackItem = "http://" + trackBackItem;
                }

                sendPing(
                    trackBackItem,
                    string.Format(
                        "title={0}&url={1}&blog_name={2}&excerpt={3}",
                        HttpUtility.HtmlEncode(trackback.PostTitle),
                        HttpUtility.HtmlEncode(trackback.PostUrl),
                        HttpUtility.HtmlEncode(trackback.PostAreaTitle),
                        HttpUtility.HtmlEncode(trackback.PostBody)
                        )
                    );
            }
        }

        private static string getTrackBackText(string pageText, string url, string postUrl)
        {
            if (!Regex.IsMatch(pageText, postUrl, RegexOptions.IgnoreCase | RegexOptions.Singleline))
            {
                const string sPattern = @"<rdf:\w+\s[^>]*?>(</rdf:rdf>)?";
                Regex r = new Regex(sPattern, RegexOptions.IgnoreCase);

                for (Match m = r.Match(pageText); m.Success; m = m.NextMatch())
                {
                    if (m.Groups.ToString().Length > 0)
                    {
                        string text = m.Groups[0].ToString();

                        if (text.IndexOf(url, StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            Regex reg = new Regex("trackback:ping=\"([^\"]+)\"", RegexOptions.IgnoreCase);
                            Match m2 = reg.Match(text);

                            if (m2.Success)
                                return m2.Result("$1");

                            return text;
                        }
                    }
                }
            }

            return null;
        }

        private static void sendPing(string trackBackItem, string parameters)
        {
            StreamWriter myWriter = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(trackBackItem);

            request.Method = "POST";
            request.ContentLength = parameters.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            request.KeepAlive = false;

            //TODO: (erikpo) Log the response or error returned

            try
            {
                myWriter = new StreamWriter(request.GetRequestStream());
                myWriter.Write(parameters);

                myWriter.Flush();

                WebResponse response = request.GetResponse();
            }
            catch { }
            finally
            {
                if (myWriter != null) myWriter.Close();
            }
        }

        #endregion
    }
}
