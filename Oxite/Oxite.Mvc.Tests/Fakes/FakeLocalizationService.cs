using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oxite.Services;
using Oxite.Model;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakeLocalizationService : ILocalizationService
    {
        public List<Phrase> Phrases { get; set; }

        public FakeLocalizationService()
        {
            this.Phrases = new List<Phrase>();
        }

        #region ILocalizationService Members

        public ICollection<Oxite.Model.Phrase> GetTranslations()
        {
            return this.Phrases;
        }

        public ICollection<Oxite.Model.Phrase> GetTranslations(string languageCode)
        {
            return this.Phrases.Where(p => p.Language == languageCode).ToList();
        }

        #endregion
    }
}
