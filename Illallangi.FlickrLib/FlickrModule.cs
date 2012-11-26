// -----------------------------------------------------------------------
// <copyright file="FlickrBackModule.cs" company="Illallangi Enterprises">
// Copyright (C) 2012 Illallangi Enterprises
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FlickrNet;
using Ninject;
using Ninject.Modules;

namespace Illallangi.FlickrLib
{
    public sealed class FlickrModule<T> : NinjectModule where T: IDriver
    {
        #region Fields

        private readonly string[] currentArguments;

        #endregion

        #region Constructor

        public FlickrModule(string[] arguments)
        {
            this.currentArguments = arguments;
        }

        #endregion

        #region Properties

        private string[] Arguments
        {
            get { return this.currentArguments; }
        }

        #endregion

        #region Methods

        public override void Load()
        {
            Bind<IConfig>()
                .ToMethod(c => XmlConfig.FromFile()).InSingletonScope();

            Bind<IDelayer>()
                .To<RandomDelayer>()
                .InSingletonScope();

            Bind<IRetrier>()
                .To<Retrier<FlickrException>>()
                .InSingletonScope();

            Bind<IRetrier>()
                .To<Retrier<WebException>>()
                .InSingletonScope();
            
            Bind<IFlickrWrapper>()
                .To<FlickrWrapper>()
                .InSingletonScope();

            Bind<IDriver>()
                .To<T>()
                .InSingletonScope()
                .WithConstructorArgument("arguments", this.Arguments);
        }

        #endregion
    }
}
