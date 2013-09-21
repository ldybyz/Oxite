//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;

namespace Oxite.Infrastructure
{
    public class InstanceContextDictionary : Dictionary<string, object>, IExtension<InstanceContext>, IInstanceContextInitializer
    {
        #region IExtension<InstanceContext> Members

        public void Attach(InstanceContext owner)
        {
        }

        public void Detach(InstanceContext owner)
        {
        }

        #endregion

        #region IInstanceContextInitializer Members

        public void Initialize(InstanceContext instanceContext, System.ServiceModel.Channels.Message message)
        {
            instanceContext.Extensions.Add(this);
        }

        #endregion
    }
}
