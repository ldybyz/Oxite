//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Text.RegularExpressions;

namespace Oxite.Infrastructure
{
    public interface IRegularExpressions
    {
        Regex GetExpression(string expressionName);
        bool IsMatch(string expressionName, string input);
        string Clean(string expressionName, string input);
    }
}
