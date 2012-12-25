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

        string Upload(string fileName, string title);

        IEnumerable<Photo> GetPhotosetPhotos(string photosetId);
        IEnumerable<Photo> GetPhotosetPhotos(string photosetId, MediaType mediaType);

        IEnumerable<string> GetPhotosetPhotoIds(string photosetId);
        IEnumerable<string> GetPhotosetPhotoIds(string photosetId, MediaType mediaType);

        AllContexts PhotosGetAllContexts(string photoId);

        PhotosetCollection PhotosetsGetList();
        PhotosetCollection PhotosetsGetList(bool clearCache);

        Photoset CreatePhotoset(string collectionName, string photoId);

        #endregion
    }
}