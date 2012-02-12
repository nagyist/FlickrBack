// -----------------------------------------------------------------------
// <copyright file="Driver.cs" company="Illallangi Enterprises">
// Copyright (C) 2012 Illallangi Enterprises
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using Ninject;

namespace Illallangi.FlickrBack
{
    public sealed class Program : IDriver
    {
        #region Fields
        
        private readonly string[] currentArguments;
        private readonly IFlickrWrapper currentFlickrWrapper;
        private readonly WebClient currentWebClient;
        
        #endregion

        #region Constructors

        public Program(string[] arguments, IFlickrWrapper flickrWrapper, WebClient webClient)
        {
            this.currentArguments = arguments;
            this.currentFlickrWrapper = flickrWrapper;
            this.currentWebClient = webClient;
        }

        #endregion Constructors 

        #region Properties

        private string[] Arguments
        {
            get { return this.currentArguments; }
        }

        private IFlickrWrapper Flickr
        {
            get { return this.currentFlickrWrapper; }
        }

        private WebClient WebClient
        {
            get { return this.currentWebClient; }
        }

        #endregion Properties 

        #region Methods

        public static void Main(string[] args)
        {
            new StandardKernel(new FlickrBackModule(args)).Get<Program>().Execute();
        }

        public void Execute()
        {
            var seenPhoto = new Collection<string>();
            
            foreach (var setId in this.Flickr.GetPhotosetIds())
            {
                foreach (var photoId in this.Flickr.GetPhotosetPhotoIds(setId))
                {
                    if (seenPhoto.Contains(photoId))
                    {
                        continue;
                    }

                    seenPhoto.Add(photoId);

                    var minSize = -1;
                    string targetName = null;

                    var photo = this.Flickr.GetPhoto(photoId);
                    
                    foreach (var contextSet in this.Flickr.PhotosGetAllContexts(photoId).Sets)
                    {
                        var photoset = this.Flickr.GetPhotoset(contextSet.PhotosetId);
                        if (-1 != minSize && photoset.NumberOfPhotos >= minSize)
                        {
                            continue;
                        }

                        minSize = photoset.NumberOfPhotos;
                        targetName = photoset.Title;
                    }
                    
                    Console.WriteLine("{0}\\{1}.{2}", targetName ?? "NoContext", photo.Title, photo.OriginalFormat);
                    
                    var path = Path.GetFullPath(targetName ?? "NoContext");
                    var filename = Path.Combine(path, String.Format("{0}.{1}", photo.Title, photo.OriginalFormat));

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    
                    if (!File.Exists(filename))
                    {
                        this.WebClient.DownloadFile(photo.OriginalUrl, filename);
                    }
                }
            }
        }

        #endregion
    }
}