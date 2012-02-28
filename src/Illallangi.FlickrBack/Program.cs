// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Illallangi Enterprises">
// Copyright (C) 2012 Illallangi Enterprises
// </copyright>
// -----------------------------------------------------------------------

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
            throw new System.NotImplementedException();
        }

        #endregion
    }
}