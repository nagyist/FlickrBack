// -----------------------------------------------------------------------
// <copyright file="FlickrWrapper.cs" company="Illallangi Enterprises">
// Copyright (C) 2012 Illallangi Enterprises
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FlickrNet;

namespace Illallangi.FlickrLib
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
            for (var i = 1; i < this.Config.Retries; i++)
            {
                try
                {
                    return this.Flickr.PhotosGetInfo(photoId);
                }
                catch (FlickrWebException f)
                {
                    Console.WriteLine("{0}, retrying", f.GetType());
                }
                catch (WebException w)
                {
                    Console.WriteLine("{0}, retrying", w.GetType());
                }
            }
            return this.Flickr.PhotosGetInfo(photoId);
        }

        public Photoset GetPhotoset(string photosetId)
        {
            for (var i = 1; i < this.Config.Retries; i++)
            {
                try
                {
                    return this.Flickr.PhotosetsGetInfo(photosetId);
                }
                catch (FlickrWebException f)
                {
                    Console.WriteLine("{0}, retrying", f.GetType());
                }
                catch (WebException w)
                {
                    Console.WriteLine("{0}, retrying", w.GetType());
                }
            }
            return this.Flickr.PhotosetsGetInfo(photosetId);
        }

        public IEnumerable<string> GetPhotosetIds()
        {
            PhotosetCollection collection = null;
            do
            {
                for (var i = 1; i < this.Config.Retries; i++)
                {
                    try
                    {
                        collection = this.Flickr.PhotosetsGetList(null == collection ? 0 : collection.Page + 1, FlickrWrapper.PAGESIZE);
                    }
                    catch (FlickrWebException f)
                    {
                        Console.WriteLine("{0}, retrying", f.GetType());
                    }
                    catch (WebException w)
                    {
                        Console.WriteLine("{0}, retrying", w.GetType());
                    }
                }
                if (null == collection)
                {
                    collection = this.Flickr.PhotosetsGetList(null == collection ? 0 : collection.Page + 1, FlickrWrapper.PAGESIZE);
                }
                foreach (var set in collection)
                {
                    yield return set.PhotosetId;
                }
            }
            while (collection.Page < collection.Pages);
        }

        public AllContexts PhotosGetAllContexts(string photoId)
        {
            AllContexts contexts = null;
            for (var i = 1; i < this.Config.Retries; i++)
            {
                try
                {
                    contexts = this.Flickr.PhotosGetAllContexts(photoId);
                }
                catch (FlickrWebException f)
                {
                    Console.WriteLine("{0}, retrying", f.GetType());
                }
                catch (WebException w)
                {
                    Console.WriteLine("{0}, retrying", w.GetType());
                }
            }
            return contexts ?? this.Flickr.PhotosGetAllContexts(photoId);
        }

        public IEnumerable<string> GetPhotosetPhotoIds(string photosetId)
        {
            return this.GetPhotosetPhotos(photosetId).Select(photo => photo.PhotoId);
        }

        public IEnumerable<Photo> GetPhotosetPhotos(string photosetId)
        {
            PhotosetPhotoCollection collection = null;
            do
            {
                collection = this.Flickr.PhotosetsGetPhotos(photosetId, null == collection ? 0 : collection.Page + 1, FlickrWrapper.PAGESIZE);
                foreach (var photo in collection)
                {
                    yield return photo;
                }
            }
            while (collection.Page < collection.Pages);
        }

        public string Upload(string fileName, string title)
        {
            return this.Flickr.UploadPicture(fileName, title);
        }

        public PhotosetCollection PhotosetsGetList()
        {
            return this.PhotosetsGetList(false);
        }

        public PhotosetCollection PhotosetsGetList(bool clearCache)
        {
            if (clearCache)
            {
                this.Flickr.PhotosetsGetList();
                Flickr.FlushCache(this.Flickr.LastRequest);
            }
            return this.Flickr.PhotosetsGetList();
        }

        public Photoset CreatePhotoset(string collectionName, string photoId)
        {
            return this.Flickr.PhotosetsCreate(collectionName, photoId);
        }

        #endregion
    }
}