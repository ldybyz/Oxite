//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Oxite.Model;
using Oxite.Mvc.Infrastructure;

namespace Oxite.Mvc.ViewModels
{
    public class OxiteModel
    {
        public AntiForgeryToken AntiForgeryToken { get; set; }
        public INamedEntity Container { get; set; }
        public SiteViewModel Site { get; set; }
        public UserViewModel User { get; set; }

        private string skinName;
        public string SkinName
        {
            get 
            {
                if (string.IsNullOrEmpty(skinName))
                    return Site.SkinDefault;

                return skinName; 
            }

            set
            {
                skinName = value;
            }
        }

        private readonly Dictionary<Type, object> modelItems = new Dictionary<Type, object>();

        public void AddModelItem<T>(T modelItem) where T : class
        {
            modelItems[typeof(T)] = modelItem;
        }

        public T GetModelItem<T>() where T : class
        {
            if (modelItems.ContainsKey(typeof(T)))
                return modelItems[typeof(T)] as T;

            return null;
        }

        //TODO: (erikpo) Fix Localize methods. These are temporary stubs
        public string Localize(string key)
        {
            return Localize(key, key);
        }

        public string Localize(string key, string defaultValue)
        {
            ICollection<Phrase> phrases = GetModelItem<ICollection<Phrase>>();

            if (phrases != null)
                return phrases.Where(p => p.Key == key && p.Language == Site.LanguageDefault).Select(p => p.Value).FirstOrDefault() ?? defaultValue;

            return defaultValue;
        }
    }
}
