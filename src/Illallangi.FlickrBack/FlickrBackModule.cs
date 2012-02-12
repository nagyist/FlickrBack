// -----------------------------------------------------------------------
// <copyright file="FlickrBackModule.cs" company="Illallangi Enterprises">
// Copyright (C) 2012 Illallangi Enterprises
// </copyright>
// -----------------------------------------------------------------------

using Ninject.Modules;

namespace Illallangi.FlickrBack
{
    public sealed class FlickrBackModule : NinjectModule
    {
        #region Fields

        private readonly string[] currentArguments;

        #endregion

        #region Constructor

        public FlickrBackModule(string[] arguments)
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
            Bind<IDriver>().To<Program>().WithConstructorArgument("arguments", this.Arguments);
        }

        #endregion
    }
}
