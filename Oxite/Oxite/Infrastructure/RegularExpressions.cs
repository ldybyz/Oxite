//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Oxite.Infrastructure
{
    public class RegularExpressions : IRegularExpressions
    {
        private readonly AppSettingsHelper appSettings;
        private Dictionary<string, Regex> expressions;

        public RegularExpressions(AppSettingsHelper appSettings)
        {
            this.appSettings = appSettings;

            loadExpressions();
        }

        #region IRegularExpressions Members

        public Regex GetExpression(string expressionName)
        {
            return expressions[expressionName];
        }

        public bool IsMatch(string expressionName, string input)
        {
            Regex expression = expressions[expressionName];

            return expression.IsMatch(input);
        }

        public string Clean(string expressionName, string input)
        {
            Regex expression = expressions[expressionName];

            return expression.Replace(input, "");
        }

        #endregion

        protected void loadExpressions()
        {
            expressions = new Dictionary<string, Regex>(7)
            {
                {
                    "IsSlug",
                    new Regex(
                        appSettings.GetString(
                            "IsSlug",
                            "^[a-z0-9-_]+$"
                            ),
                        RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline
                        )
                },
                {
                    "SlugReplace",
                    new Regex(
                        appSettings.GetString(
                            "SlugReplace",
                            "([^a-z0-9-_]?)"
                            ),
                        RegexOptions.IgnoreCase | RegexOptions.Compiled
                        )
                },
                {
                    "IsTag",
                    new Regex(
                        appSettings.GetString(
                            "IsTag",
                            "^[a-z0-9-_ ]+$"
                            ),
                        RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline
                        )
                },
                {
                    "TagReplace",
                    new Regex(
                        appSettings.GetString(
                            "TagReplace",
                            "([^a-z0-9]?)"
                            ),
                        RegexOptions.IgnoreCase | RegexOptions.Compiled
                        )
                },
                {
                    "IsEmail",
                    new Regex(
                        appSettings.GetString(
                            "IsEmail",
                            @"^[a-z0-9]+([-+\.]*[a-z0-9]+)*@[a-z0-9]+([-\.][a-z0-9]+)*$"
                            ),
                        RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline
                        )
                },
                {
                    "IsUrl",
                    new Regex(
                        appSettings.GetString(
                            "IsUrl",
                            "^https?://(?:[^./\\s'\"<)\\]]+\\.)+[^./\\s'\"<\")\\]]+(?:/[^'\"<]*)*$"
                            ),
                        RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline
                        )
                },
                {
                    "AreaName",
                    new Regex(
                        appSettings.GetString(
                            "AreaName",
                            "[^a-z0-9]"
                            ),
                        RegexOptions.Compiled
                        )
                },
                {
                    "Username",
                    new Regex(
                        appSettings.GetString(
                            "Username",
                            "[^a-z0-9]"
                            ),
                        RegexOptions.Compiled
                        )
                }
            };
        }
    }
}
