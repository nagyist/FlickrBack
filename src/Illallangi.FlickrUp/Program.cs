// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Illallangi Enterprises">
// Copyright (C) 2012 Illallangi Enterprises
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using FlickrNet;
using Illallangi.FlickrLib;
using Ninject;

namespace Illallangi.FlickrUp
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
            foreach (var folder in this.Folders)
            {
                var collectionName = Path.GetFileName(folder);
                var photoSet = this.Flickr.PhotosetsGetList(true).Where(p => p.Title == collectionName).FirstOrDefault();

                foreach (var file in Directory.GetFiles(folder, "*.jpg"))
                {
                    if (null == photoSet || this.Flickr.GetPhotosetPhotos(photoSet.PhotosetId).Any(p => Path.GetFileNameWithoutExtension(file) == p.Title))
                    {
                        Console.WriteLine("Uploading {0}", file);
                        //var photoId = this.Flickr.Upload(Path.Combine(folder, file), Path.GetFileNameWithoutExtension(file));
                        if (null == photoSet)
                        {
                          //  photoSet = this.Flickr.CreatePhotoset(collectionName, photoId);
                        }
                        else
                        {
                            // Add photo to xphotoset
                        }
                        // Console.WriteLine("Creating {0}", collectionName);
                    }
                }

            }
        }

        private IEnumerable<string> Folders
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var arg in this.Arguments)
                {
                    sb.AppendFormat(" {0}", arg);

                    if (0 == sb.Length || !Directory.Exists(Path.GetFullPath(sb.ToString().Trim()))) continue;

                    yield return Path.GetFullPath(sb.ToString().Trim());
                    sb.Remove(0, sb.Length);
                }
            }
        }

        #endregion
    }
}