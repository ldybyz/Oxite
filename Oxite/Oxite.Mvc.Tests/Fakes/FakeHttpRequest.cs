//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakeHttpRequest : HttpRequestBase
    {
        private readonly Uri _url;
        private readonly string _appRelativeUrl;
        private readonly NameValueCollection _formParams;
        private readonly NameValueCollection _queryStringParams;
        private readonly HttpCookieCollection _cookies;
        private readonly string _httpMethod;

        public FakeHttpRequest(Uri url, string appRelativeUrl, string httpMethod, bool isAuthenticated, NameValueCollection formParams, NameValueCollection queryStringParams, HttpCookieCollection cookies, Stream inputStream)
        {
            if (!appRelativeUrl.StartsWith("~"))
                throw new Exception("appRelativeUrl must start with ~");

            _url = url;
            _appRelativeUrl = appRelativeUrl;
            _httpMethod = httpMethod;
            this.RequestIsAuthenticated = isAuthenticated;
            _formParams = formParams;
            _queryStringParams = queryStringParams;
            _cookies = cookies;
            RequestInputStream = inputStream;

            this.RequestHeaders = new NameValueCollection();
        }

        public NameValueCollection RequestHeaders { get; set; }
        public Stream RequestInputStream { get; set; }
        public bool RequestIsAuthenticated { get; set; }

        public override NameValueCollection Form
        {
            get
            {
                return _formParams;
            }
        }

        public override NameValueCollection QueryString
        {
            get
            {
                return _queryStringParams;
            }
        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return _cookies;
            }
        }

        public override string AppRelativeCurrentExecutionFilePath
        {
            get { return _appRelativeUrl; }
        }


        public override string HttpMethod
        {
            get
            {
                return _httpMethod;
            }
        }

        public override Uri Url
        {
            get
            {
                return _url;
            }
        }

        public override string PathInfo
        {
            get
            {
                return String.Empty;
            }
        }


        public override bool IsLocal
        {
            get
            {
                return (Url.Host.ToLower() ==  "localhost" || Url.Host == "127.0.01");
            }
        }

        public override bool IsAuthenticated
        {
            get
            {
                return RequestIsAuthenticated;
            }
        }

        public override Stream InputStream
        {
            get
            {
                return this.RequestInputStream;
            }
        }

        public override NameValueCollection Headers
        {
            get
            {
                return this.RequestHeaders;
            }
        }

        public override string ApplicationPath
        {
            get
            {
                int charactersToTrim = this._appRelativeUrl.Length - 2;
                if (charactersToTrim == 0)
                    return this.Url.AbsolutePath;

                int trimStart = this.Url.AbsolutePath.Length - charactersToTrim;

                if (trimStart < this.Url.AbsolutePath.Length)
                    return this.Url.AbsolutePath.Remove(this.Url.AbsolutePath.Length - charactersToTrim);
                else
                    return "/";
            }
        }

        public override string RequestType
        {
            get
            {
                return this.HttpMethod;
            }
            set
            {
                base.RequestType = value;
            }
        }
    }
}
