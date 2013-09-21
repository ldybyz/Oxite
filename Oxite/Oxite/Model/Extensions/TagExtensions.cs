//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using Oxite.Routing;

namespace Oxite.Model.Extensions
{
    public static class TagExtensions
    {
        public static string GetUrl(this Tag tag, RequestContext context, RouteCollection routes)
        {
            return routes.GetUrl(context, "PostsByTag", new { tagName = tag.Name });
        }

        public static int GetTagWeight(this Tag tag, IEnumerable<KeyValuePair<Tag, int>> tagsWithPostCount, ref double? averagePostCount, ref double? standardDeviationOfPostCount)
        {
            return tag.GetTagWeight(tagsWithPostCount, ref averagePostCount, ref standardDeviationOfPostCount, 1, 7);
        }

        public static int GetTagWeight(this Tag tag, IEnumerable<KeyValuePair<Tag, int>> tagsWithPostCount, ref double? averagePostCount, ref double? standardDeviationOfPostCount, int? minWeight, int? maxWeight)
        {
            if (!averagePostCount.HasValue)
            {
                averagePostCount = tagsWithPostCount.Select(t => (double)t.Value).Average();
            }
            if (!standardDeviationOfPostCount.HasValue)
            {
                standardDeviationOfPostCount = tagsWithPostCount.Select(t => (double)t.Value).GetStandardDeviation();
            }

            double factor = ((tagsWithPostCount.Where(t => t.Key.ID == tag.ID).First().Value - averagePostCount.Value) /
                             standardDeviationOfPostCount.Value) +
                            (averagePostCount.Value / standardDeviationOfPostCount.Value);
            int weight;

            if (factor < 0.2)
            {
                weight = 1;
            }
            else if (factor < 0.4)
            {
                weight = 2;
            }
            else if (factor < 0.8)
            {
                weight = 3;
            }
            else if (factor < 1.6)
            {
                weight = 4;
            }
            else if (factor < 3.2)
            {
                weight = 5;
            }
            else if (factor < 6.4)
            {
                weight = 6;
            }
            else
            {
                weight = 7;
            }

            if (maxWeight.HasValue && weight > maxWeight.Value)
            {
                weight = maxWeight.Value;
            }
            else if (minWeight.HasValue && weight < minWeight.Value)
            {
                weight = minWeight.Value;
            }

            return weight;
        }

        public static double GetStandardDeviation(this IEnumerable<double> values)
        {
            return values.GetStandardDeviation(false);
        }

        public static double GetStandardDeviationP(this IEnumerable<double> values)
        {
            return values.GetStandardDeviation(true);
        }

        public static double GetStandardDeviation(this IEnumerable<double> values, bool entirePopulation)
        {
            int count = 0;
            double dSum = 0;
            double sqrSum = 0;
            int adjustment = 1;

            if (entirePopulation)
            {
                adjustment = 0;
            }

            foreach (double val in values)
            {
                dSum += val;
                sqrSum += val * val;
                count += 1;
            }

            if (count > 1)
            {
                double var = count * sqrSum - (dSum * dSum);
                double prec = var / (dSum * dSum);

                //Double is only guaranteed for 15 digits. A difference
                //with a result less than 0.000000000000001 will be considered zero.
                if (prec < 0.000000000000001 || var < 0)
                {
                    var = 0;
                }
                else
                {
                    var = var / (count * (count - adjustment));
                }

                return Math.Sqrt(var);
            }

            return 0;
        }
    }
}
