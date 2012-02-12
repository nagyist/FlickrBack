// -----------------------------------------------------------------------
// <copyright file="FlickrWrapper.cs" company="Illallangi Enterprises">
// Copyright (C) 2012 Illallangi Enterprises
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using FlickrNet;

namespace Illallangi.FlickrBack
{
    public sealed class FlickrWrapper : IFlickrWrapper
    {
        #region Fields

        private readonly IConfig currentConfig;
        private const int PAGESIZE = 10;

        #endregion

        #region Constructors

        public FlickrWrapper(IConfig config)
        {
            this.currentConfig = config;
        }

        #endregion

        #region Properties

        public IConfig Config
        {
            get { return this.currentConfig; }
        }

        private Flickr Flickr
        {
            get { return this.Config.Flickr; }
        }

        #endregion

        #region Methods

        public PhotoInfo GetPhoto(string photoId)
        {
            return this.Flickr.PhotosGetInfo(photoId);
        }

        public Photoset GetPhotoset(string photosetId)
        {
            return this.Flickr.PhotosetsGetInfo(photosetId);
        }

        public IEnumerable<string> GetPhotosetIds()
        {
            PhotosetCollection collection = null;
            do
            {
                collection = this.Flickr.PhotosetsGetList(null == collection ? 0 : collection.Page + 1, FlickrWrapper.PAGESIZE);
                foreach (var set in collection)
                {
                    yield return set.PhotosetId;
                }
            }
            while (collection.Page < collection.Pages);
        }

        public IEnumerable<string> GetPhotosetPhotoIds(string photosetId)
        {
            PhotosetPhotoCollection collection = null;
            do
            {
                collection = this.Flickr.PhotosetsGetPhotos(photosetId, null == collection ? 0 : collection.Page + 1, FlickrWrapper.PAGESIZE);
                foreach (var photo in collection)
                {
                    yield return photo.PhotoId;
                }
            } 
            while (collection.Page < collection.Pages);
        }

        public AllContexts PhotosGetAllContexts(string photoId)
        {
            return this.Flickr.PhotosGetAllContexts(photoId);
        }

        #endregion
    }
}