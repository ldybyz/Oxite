//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Specialized;
using System.IO;
using System.Security.Principal;
using System.Web;
using System.Web.SessionState;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakeHttpContext : HttpContextBase
    {
        private readonly Uri _url;
        private readonly string _appRelativeUrl;
        private readonly IPrincipal _principal;
        private readonly NameValueCollection _formParams;
        private readonly NameValueCollection _queryStringParams;
        private readonly HttpCookieCollection _cookies;
        private readonly SessionStateItemCollection _sessionItems;
        private readonly HttpRequestBase _httpRequest;
        private readonly HttpResponseBase _httpResponse;
        private readonly string _httpMethod;

        private bool _isAuthenticated = false;

        public Stream InputStream { get; set; }

        public FakeHttpContext(string appRelativeUrl)
            : this(null, appRelativeUrl, "GET", null, null, null, null, null)
        {
        }

        public FakeHttpContext(string appRelativeUrl, string httpMethod)
            : this(null, appRelativeUrl, httpMethod, null, null, null, null, null)
        {
        }

        public FakeHttpContext(Uri url, string appRelativeUrl)
            : this(url, appRelativeUrl, "GET", null, null, null, null, null)
        {
        }

        public FakeHttpContext(Uri url, string appRelativeUrl, IPrincipal principal)
            : this(url, appRelativeUrl, "GET", principal, null, null, null, null)
        {
        }

        public FakeHttpContext(Uri url, string appRelativeUrl, string httpMethod, IPrincipal principal, NameValueCollection formParams, NameValueCollection queryStringParams, HttpCookieCollection cookies, SessionStateItemCollection sessionItems )
        {
            _url = url;
            _appRelativeUrl = appRelativeUrl;
            _httpMethod = httpMethod;
            _principal = principal;
            _formParams = formParams;
            _queryStringParams = queryStringParams;
            _cookies = cookies;
            _sessionItems = sessionItems;

            if (principal != null)
                _isAuthenticated = principal.Identity.IsAuthenticated;

            _httpRequest = new FakeHttpRequest(_url, _appRelativeUrl, _httpMethod, _isAuthenticated, _formParams, _queryStringParams, _cookies, InputStream);
            _httpResponse = new FakeHttpResponse();
        }

        public override HttpRequestBase Request
        {
            get
            {
                return _httpRequest;
            }
        }

        public override HttpResponseBase Response
        {
            get
            {
                return _httpResponse;
            }
        }


        public override IPrincipal User
        {
            get
            {
                return _principal;
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public override HttpSessionStateBase Session
        {
            get
            {
                return new FakeHttpSessionState(_sessionItems);
            }
        }
    }
}
