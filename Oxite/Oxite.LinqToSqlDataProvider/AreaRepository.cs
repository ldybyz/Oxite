//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Linq;
using Oxite.Model;
using Oxite.Repositories;

namespace Oxite.LinqToSqlDataProvider
{
    public class AreaRepository : IAreaRepository
    {
        private OxiteLinqToSqlDataContext context;
        private Guid siteID;

        public AreaRepository(OxiteLinqToSqlDataContext context, Site site)
        {
            this.context = context;
            this.siteID = site.ID;
        }

        #region IAreaRepository Members

        public Area GetArea(Guid id)
        {
            return (from a in context.oxite_Areas
                    where a.AreaID == id
                    select ProjectArea(a)).FirstOrDefault();
        }

        public Area GetArea(string areaName)
        {
            return (from a in context.oxite_Areas
                    where a.SiteID == siteID && string.Compare(a.AreaName, areaName, true) == 0
                    select ProjectArea(a)).FirstOrDefault();
        }

        public IQueryable<Area> GetAreas()
        {
            return from a in context.oxite_Areas
                   where a.SiteID == siteID
                   select ProjectArea(a);
        }

        public void Save(Area area, Site site)
        {
            oxite_Area dbArea = null;
            Guid areaID = area.ID;

            if (area.ID != Guid.Empty)
            {
                dbArea = (from a in context.oxite_Areas where a.SiteID == siteID && a.AreaID == area.ID select a).FirstOrDefault();
            }

            if (dbArea == null)
            {
                if (area.ID == Guid.Empty)
                {
                    areaID = Guid.NewGuid();
                }

                dbArea = new oxite_Area();

                dbArea.SiteID = site != null && site.ID != Guid.Empty ? site.ID : siteID;
                dbArea.AreaID = areaID;
                if (area.Created.HasValue)
                {
                    dbArea.CreatedDate = area.Created.Value;
                }
                else
                    dbArea.CreatedDate = DateTime.UtcNow;

                context.oxite_Areas.InsertOnSubmit(dbArea);
            }

            dbArea.CommentingDisabled = area.CommentingDisabled;
            dbArea.AreaName = area.Name;
            dbArea.Description = area.Description ?? "";
            dbArea.DisplayName = area.DisplayName ?? "";
            dbArea.ModifiedDate = DateTime.UtcNow;

            context.SubmitChanges();

            area.ID = areaID;
        }

        #endregion

        protected Area ProjectArea(oxite_Area area)
        {
            return new Area()
            {
                ID = area.AreaID,
                Name = area.AreaName,
                DisplayName = area.DisplayName,
                Description = area.Description,
                CommentingDisabled = area.CommentingDisabled,
                Created = area.CreatedDate,
                Modified = area.ModifiedDate
            };
        }
    }
}
