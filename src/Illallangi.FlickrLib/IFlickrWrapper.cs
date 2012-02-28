// -----------------------------------------------------------------------
// <copyright file="IFlickrWrapper.cs" company="Illallangi Enterprises">
// Copyright (C) 2012 Illallangi Enterprises
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using FlickrNet;

namespace Illallangi.FlickrLib
{
    public interface IFlickrWrapper
    {
        #region Operations

        PhotoInfo GetPhoto(string photoId);

        Photoset GetPhotoset(string photosetId);

        IEnumerable<string> GetPhotosetIds();

        IEnumerable<string> GetPhotosetPhotoIds(string photosetId);

        AllContexts PhotosGetAllContexts(string photoId);

        #endregion
    }
}