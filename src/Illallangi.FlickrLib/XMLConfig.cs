// -----------------------------------------------------------------------
// <copyright file="XMLConfig.cs" company="Illallangi Enterprises">
// Copyright (C) 2012 Illallangi Enterprises
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using FlickrNet;

namespace Illallangi.FlickrLib
{
    [XmlRoot("config")]
    public sealed class XMLConfig : XMLBackedFile<XMLConfig>, IConfig
    {
        #region Fields

        #region Private Constant Fields

        private const string DEFAULTAPIKEY = "Insert your Flickr API Key here.";
        private const string DEFAULTAPISECRET = "Insert your Flickr API Secret here.";

        #endregion

        #region Private Fields

        private string currentApiKey;
        private string currentApiSecret;
        private Flickr currentFlickr;
        private string currentFrob;
        private string currentToken;

        #endregion

        #endregion

        #region Constructors

        private XMLConfig()
        {
            // NOOP
        }

        #endregion 

        #region Properties 

        #region Public Properties

        [XmlAttribute("key")]
        public string ApiKey
        {
            get { return this.currentApiKey ?? (this.currentApiKey = XMLConfig.DEFAULTAPIKEY); }
            set { this.currentApiKey = value; }
        }

        [XmlAttribute("secret")]
        public string ApiSecret
        {
            get { return this.currentApiSecret ?? (this.currentApiSecret = XMLConfig.DEFAULTAPISECRET); }
            set { this.currentApiSecret = value; }
        }

        [XmlIgnore]
        public Flickr Flickr
        {
            get
            {
                if (null == this.currentFlickr)
                {
                    if (null == this.ApiKey ||
                        null == this.ApiSecret ||
                        string.Empty == this.ApiKey.Trim() ||
                        string.Empty == this.ApiKey.Trim() ||
                        XMLConfig.DEFAULTAPIKEY == this.ApiKey ||
                        XMLConfig.DEFAULTAPISECRET == this.ApiSecret)
                    {
                        throw new Exception("Please ensure Key and Secret are set in the FlickrBack.xml file.");
                    }

                    this.currentFlickr = new Flickr(this.ApiKey, this.ApiSecret, this.currentToken);
                }

                return this.currentFlickr;
            }
        }

        [XmlAttribute("frob")]
        public string Frob
        {
            get { return this.currentFrob ?? (this.currentFrob = this.GetFrob()); }
            set { this.currentFrob = value; }
        }

        [XmlAttribute("token")]
        public string Token
        {
            get { return this.currentToken ?? (this.currentToken = this.GetToken()); }
            set { this.currentToken = value; }
        }

        [XmlAttribute("retries")]
        public int Retries { get; set; }

        #endregion

        #region Private Properties

        // TODO: Fix This
        private static string FileName
        {
            get { return "Flickr.xml"; }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        public static XMLConfig FromFile()
        {
            return (File.Exists(XMLConfig.FileName)
                        ? XMLConfig.FromFile(XMLConfig.FileName)
                        : new XMLConfig()).Save();
        }

        #endregion

        #region Private Methods

        private string GetFrob()
        {
            return this.Flickr.AuthGetFrob();
        }

        private string GetToken()
        {
            Process.Start(this.Flickr.AuthCalcUrl(this.Frob, AuthLevel.Write));
            Console.Write("Press any key to continue...");
            Console.ReadLine();
            return this.Flickr.AuthToken = this.Flickr.AuthGetToken(this.Frob).Token;
        }

        private XMLConfig Save()
        {
            return this.ToFile(XMLConfig.FileName);
        }

        #endregion

        #endregion Methods 
    }
}