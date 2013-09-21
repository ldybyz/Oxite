//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;

namespace Oxite.Model
{
    public class PageOfList<T> : List<T>, IPageOfList<T>
    {
        public PageOfList(IEnumerable<T> items, int pageIndex, int pageSize, int totalItemCount)
        {
            AddRange(items);
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalItemCount = totalItemCount;
            TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
        }

        #region IPageOfList<T> Members

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalItemCount { get; set; }
        public int TotalPageCount { get; private set; }

        #endregion
    }
}
