// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Illallangi Enterprises">
// Copyright (C) 2012 Illallangi Enterprises
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using Illallangi.CountEnum;
using Illallangi.FlickrLib;
using Ninject;

namespace Illallangi.FlickrList
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
            var started = DateTime.MinValue;
            foreach (var setId in new CounterEnumerable<string>(this.Flickr.GetPhotosetIds()))
            {
                foreach (var photoId in new CounterEnumerable<string>(this.Flickr.GetPhotosetPhotoIds(setId.Value)))
                {
                    if (DateTime.MinValue == started)
                    {
                        started = DateTime.Now;
                    }
                    var percentage = ((setId.Count - 1 + (((double)photoId.Count - 1) / photoId.Total)) * 100) / setId.Total;
                    var duration = DateTime.Now.Subtract(started);
                    var completion = duration.TotalSeconds / (percentage / 100);
                    var completionTime = (double.IsNaN(completion) || double.IsInfinity(completion) || completion.Equals(0)) ? DateTime.MaxValue : started.AddSeconds(completion);

                    Console.WriteLine("{0:#00.00}% (Set {1}/{2}, Photo {3}/{4}) - ETA {5}).",
                        percentage,
                        setId.Count,
                        setId.Total,
                        photoId.Count,
                        photoId.Total,
                        completionTime);

                    if (seenPhoto.Contains(photoId.Value))
                    {
                        continue;
                    }

                    seenPhoto.Add(photoId.Value);

                    var minSize = -1;
                    string targetName = null;

                    var photo = this.Flickr.GetPhoto(photoId.Value);

                    foreach (var contextSet in this.Flickr.PhotosGetAllContexts(photoId.Value).Sets)
                    {
                        var photoset = this.Flickr.GetPhotoset(contextSet.PhotosetId);
                        if (-1 != minSize && photoset.NumberOfPhotos >= minSize)
                        {
                            continue;
                        }

                        minSize = photoset.NumberOfPhotos;
                        targetName = photoset.Title;
                    }

                    var path = Path.GetFullPath(targetName ?? "NoContext");
                    var filename = Path.Combine(path, string.Format("{0}.{1}", photo.Title, photo.OriginalFormat));

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    if (!File.Exists(filename))
                    {
                        Console.Write("Downloading {0}\\{1}.{2}", targetName ?? "NoContext", photo.Title, photo.OriginalFormat);
                        this.WebClient.DownloadFile(photo.OriginalUrl, filename);
                        Console.WriteLine(".. Done.");
                    }
                }
            }
        }

        #endregion
    }
}
