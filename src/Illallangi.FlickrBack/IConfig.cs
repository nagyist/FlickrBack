// -----------------------------------------------------------------------
// <copyright file="IConfig.cs" company="Illallangi Enterprises">
// Copyright (C) 2012 Illallangi Enterprises
// </copyright>
// -----------------------------------------------------------------------

using FlickrNet;

namespace Illallangi.FlickrBack
{
    public interface IConfig
    {
        #region Data Members

        string ApiKey { get; }

        string ApiSecret { get; }

        Flickr Flickr { get; }

        string Frob { get; }

        string Token { get; }

        #endregion
    }
}