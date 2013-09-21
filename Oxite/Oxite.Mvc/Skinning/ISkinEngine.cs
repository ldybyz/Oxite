//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

namespace Oxite.Mvc.Skinning
{
    public interface ISkinEngine
    {
        string FindMasterPath(string name, string folderName, out string[] locationsSearched);
        string FindViewPath(string name, string folderName, out string[] locationsSearched);
        string FindPartialViewPath(string name, string folderName, out string[] locationsSearched);
    }
}
