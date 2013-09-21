//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.IO;
using System.Text;
using System.Web;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakeHttpResponse : HttpResponseBase
    {
        private StringBuilder _sb;
        private StringWriter _sw;

        public FakeHttpResponse()
        {
            _sb = new StringBuilder();
            _sw = new StringWriter(_sb);
        }

        public override void Write(string s)
        {
            _sb.Append(s);
        }

        public override TextWriter Output
        {
            get
            {
                return _sw;
            }
        }

        public override string ToString()
        {
            return _sb.ToString();
        }

        public override string ApplyAppPathModifier(string virtualPath)
        {
            return virtualPath;
        }
    }
}
