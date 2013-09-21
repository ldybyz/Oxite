using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Oxite.Infrastructure;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakeRegularExpressions : IRegularExpressions
    {
        #region IRegularExpressions Members

        public System.Text.RegularExpressions.Regex GetExpression(string expressionName)
        {
            return new Regex("$^");
        }

        public bool IsMatch(string expressionName, string input)
        {
            throw new NotImplementedException();
        }

        public string Clean(string expressionName, string input)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
