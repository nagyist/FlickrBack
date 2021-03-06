// -----------------------------------------------------------------------
// <copyright file="FlickrWrapper.cs" company="Illallangi Enterprises">
// Copyright (C) 2012 Illallangi Enterprises
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using FlickrNet;

namespace Illallangi.FlickrLib
{
    public sealed class FlickrWrapper : IFlickrWrapper
    {
        #region Fields

        private readonly IConfig currentConfig;
        private readonly IEnumerable<IRetrier> currentRetriers;

        #endregion

        #region Constructors

        public FlickrWrapper(IConfig config, IEnumerable<IRetrier> retriers)
        {
            this.currentConfig = config;
            this.currentRetriers = retriers;
        }

        #endregion

        #region Properties

        private IConfig Config
        {
            get { return this.currentConfig; }
        }

        private IEnumerable<IRetrier> Retriers
        {
            get { return this.currentRetriers; }
        }

        private Flickr Flickr
        {
            get { return this.Config.Flickr; }
        }

        #endregion

        #region Methods

        private T Retry<T>(Func<T> func)
        {
            func = this.Retriers.Aggregate(func, (unknown, retrier) => (() => retrier.Retry(unknown)));
            return func();
        }

        public PhotoInfo GetPhoto(string photoId)
        {
            return this.Retry(() => this.Flickr.PhotosGetInfo(photoId));
        }
        
        public Photoset GetPhotoset(string photosetId)
        {
            return this.Retry(() => this.Flickr.PhotosetsGetInfo(photosetId));
        }

        public IEnumerable<string> GetPhotosetIds()
        {
            PhotosetCollection collection = null;
            do
            {
                collection =
                    this.Retry(
                        () => this.Flickr.PhotosetsGetList(null == collection ? 0 : collection.Page + 1, this.Config.PageSize));
                foreach (var set in collection)
                {
                    yield return set.PhotosetId;
                }
            }
            while (collection.Page < collection.Pages);
        }

        public AllContexts PhotosGetAllContexts(string photoId)
        {
            return this.Retry(() => this.Flickr.PhotosGetAllContexts(photoId));
        }

        public IEnumerable<string> GetPhotosetPhotoIds(string photosetId)
        {
            return this.GetPhotosetPhotos(photosetId).Select(photo => photo.PhotoId);
        }
        public IEnumerable<string> GetPhotosetPhotoIds(string photosetId, MediaType mediaType)
        {
            return this.GetPhotosetPhotos(photosetId, mediaType).Select(photo => photo.PhotoId);
        }

        public IEnumerable<Photo> GetPhotosetPhotos(string photosetId)
        {
            return this.GetPhotosetPhotos(photosetId, MediaType.All);
        }

        public IEnumerable<Photo> GetPhotosetPhotos(string photosetId, MediaType mediaType)
        {
            PhotosetPhotoCollection collection = null;
            do
            {
                collection =
                    this.Retry(
                        () =>
                        this.Flickr.PhotosetsGetPhotos(photosetId, PhotoSearchExtras.None, PrivacyFilter.None, null == collection ? 0 : collection.Page + 1, this.Config.PageSize, mediaType));
                foreach (var photo in collection)
                {
                    yield return photo;
                }
            }
            while (collection.Page < collection.Pages);
        }

        public string Upload(string fileName, string title)
        {
            return this.Retry(() => this.Flickr.UploadPicture(fileName, title));
        }

        public PhotosetCollection PhotosetsGetList()
        {
            return this.PhotosetsGetList(false);
        }

        public PhotosetCollection PhotosetsGetList(bool clearCache)
        {
            if (clearCache)
            {
                this.Retry(() => this.Flickr.PhotosetsGetList());
                Flickr.FlushCache(this.Flickr.LastRequest);
            }
            return this.Retry(() => this.Flickr.PhotosetsGetList());
        }

        public Photoset CreatePhotoset(string collectionName, string photoId)
        {
            return this.Retry(() => this.Flickr.PhotosetsCreate(collectionName, photoId));
        }

        #endregion
    }
}