// -----------------------------------------------------------------------
// <copyright file="FlickrBackModule.cs" company="Illallangi Enterprises">
// Copyright (C) 2012 Illallangi Enterprises
// </copyright>
// -----------------------------------------------------------------------

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
            Bind<IConfig>().ToMethod(c => XMLConfig.FromFile()).InSingletonScope();
            Bind<IFlickrWrapper>().To<FlickrWrapper>().InSingletonScope();
            Bind<IDriver>().To<T>().WithConstructorArgument("arguments", this.Arguments);
        }

        #endregion
    }
}
