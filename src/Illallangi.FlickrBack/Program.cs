// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Illallangi Enterprises">
// Copyright (C) 2012 Illallangi Enterprises
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using Illallangi.FlickrLib;
using Ninject;

namespace Illallangi.FlickrBack
{
    public sealed class Program : ProgramBase
    {
        #region Constructors

        public Program(string[] arguments, IFlickrWrapper flickrWrapper, WebClient webClient)
            : base(arguments, flickrWrapper, webClient)
        {
        }

        #endregion

        #region Methods

        public static void Main(string[] arguments)
        {
            new StandardKernel(new FlickrModule<Program>(arguments)).Get<IDriver>().Execute();
        }

        public override void Execute()
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
                    var filename = Path.Combine(path, string.Format("{0}.{1}", photo.Title, photo.OriginalFormat));

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