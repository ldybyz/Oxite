//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Security.Cryptography;
using System.Text;

namespace Oxite.Mvc.Infrastructure
{
    public class AntiForgeryToken
    {
        public static readonly string TicksName = "__AntiForgeryTicks";
        public static readonly string TokenName = "__AntiForgeryToken";

        private readonly string salt;

        public AntiForgeryToken(string salt)
        {
            this.salt = salt;
            Ticks = getTicks();
        }

        public long Ticks  { get; set; }

        public string Value
        {
            get
            {
                return GetHash(salt, Ticks.ToString());
            }
        }

        private static long getTicks()
        {
            return DateTime.UtcNow.Ticks;
        }

        public static string GetHash(string salt, string ticks)
        {
            byte[] bytesToHash = Encoding.UTF8.GetBytes(string.Concat(salt, ticks));

            SHA512 hasher = new SHA512Managed();
            byte[] hashed = hasher.ComputeHash(bytesToHash);

            return Convert.ToBase64String(hashed);
        }
    }
}