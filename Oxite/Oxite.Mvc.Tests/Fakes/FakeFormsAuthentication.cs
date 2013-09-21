using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oxite.Infrastructure;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakeFormsAuthentication : IFormsAuthentication
    {
        public string LastUserName { get; set; }
        public bool LastPersistCookie { get; set; }
        public bool SignedOut { get; set; }

        #region IFormsAuthentication Members

        public void SetAuthCookie(string userName, bool createPersistentCookie)
        {
            this.LastUserName = userName;
            this.LastPersistCookie = createPersistentCookie;
        }

        public void SignOut()
        {
            this.SignedOut = true;
        }

        #endregion
    }
}
