//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Runtime.Serialization;
using System;

namespace Oxite.Model.Extensions
{
    public static class UserBaseExtensions
    {
        public static string ToJson(this UserBase user)
        {
            string serializedUserBase = string.Empty;

            if (user != null)
            {
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(UserBase));

                using (MemoryStream ms = new MemoryStream())
                {
                    jsonSerializer.WriteObject(ms, user);
                    ms.Position = 0;
                    serializedUserBase = new StreamReader(ms).ReadToEnd();
                    ms.Close();
                }
            }

            return serializedUserBase;
        }

        public static UserBase FillFromSerlializedString(this UserBase user, string serializedUserBase)
        {
            UserBase filledUser = null;

            if (!string.IsNullOrEmpty(serializedUserBase))
            {
                DataContractJsonSerializer dcjs = new DataContractJsonSerializer(typeof(UserBase));

                using (MemoryStream ms = new MemoryStream())
                {
                    StreamWriter writer = new StreamWriter(ms, Encoding.Default);
                    writer.Write(serializedUserBase);
                    writer.Flush();

                    ms.Position = 0;

                    try
                    {
                        filledUser = dcjs.ReadObject(ms) as UserBase;
                    }
                    catch (SerializationException)
                    {
                        filledUser = null;
                    }
                    ms.Close();
                }
            }

            return filledUser;
        }

    }
}
