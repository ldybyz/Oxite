
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Configuration;
using System.Configuration;

namespace Microsoft.ServiceModel.Samples.XmlRpc
{
    public class XmlRpcHttpBindingCollectionElement : StandardBindingCollectionElement<XmlRpcHttpBinding, XmlRpcHttpBindingElement>
    {
        // Methods
        public static XmlRpcHttpBindingCollectionElement GetBindingCollectionElement()
        {
            return (XmlRpcHttpBindingCollectionElement)GetBindingCollectionElement("xmlRpcHttpBinding");
        }

        private static BindingCollectionElement GetBindingCollectionElement(string bindingCollectionName)
        {
            BindingCollectionElement element = null;
            BindingsSection section = (BindingsSection)ConfigurationManager.GetSection("system.serviceModel/bindings");
            try
            {
                element = section.BindingCollections.Find(new Predicate<BindingCollectionElement>(delegate(BindingCollectionElement target) { return target.BindingName == "bindingCollectionName"; }));
            }
            catch (KeyNotFoundException)
            {
                throw new ConfigurationErrorsException();
            }
            return element;
        }

 

 

    }
}
