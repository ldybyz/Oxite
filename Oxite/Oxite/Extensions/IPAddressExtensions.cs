//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Net;

namespace Oxite.Extensions
{
    public static class IPAddressExtensions
    {
        public static long ToLong(this IPAddress address)
        {
            if (address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return 0;
            }

            switch (address.GetAddressBytes().Length)
            {
                case 4:
                    return BitConverter.ToUInt32(address.GetAddressBytes(), 0);
                case 8:
                    return (long)BitConverter.ToUInt64(address.GetAddressBytes(), 0);
                default:
                    throw new Exception("Invalid IPv4 Address passed, length not 4 or 8 bytes");
            }
        }
    }
}