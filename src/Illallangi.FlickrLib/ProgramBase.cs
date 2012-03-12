// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Illallangi Enterprises">
// Copyright (C) 2012 Illallangi Enterprises
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using Ninject;

namespace Illallangi.FlickrLib
{
    public abstract class ProgramBase : IDriver
    {
        #region Fields
        
        private readonly string[] currentArguments;
        private readonly IFlickrWrapper currentFlickrWrapper;
        private readonly WebClient currentWebClient;
        
        #endregion

        #region Constructors

        protected ProgramBase(string[] arguments, IFlickrWrapper flickrWrapper, WebClient webClient)
        {
            this.currentArguments = arguments;
            this.currentFlickrWrapper = flickrWrapper;
            this.currentWebClient = webClient;
        }

        #endregion Constructors 

        #region Properties

        protected string[] Arguments
        {
            get { return this.currentArguments; }
        }

        protected IFlickrWrapper Flickr
        {
            get { return this.currentFlickrWrapper; }
        }

        protected WebClient WebClient
        {
            get { return this.currentWebClient; }
        }

        #endregion Properties 

        #region Methods

        public abstract void Execute();

        #endregion
    }
}