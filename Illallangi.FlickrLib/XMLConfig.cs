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
    public sealed class XmlConfig : XmlFileBackedObject<XmlConfig>, IConfig
    {
        #region Fields

        #region Private Constant Fields

        private const string DEFAULTAPIKEY = "Insert your Flickr API Key here.";
        private const string DEFAULTAPISECRET = "Insert your Flickr API Secret here.";
        private const int DEFAULTRETRIES = -1;
        private const int DEFAULTMINDELAY = 5;
        private const int DEFAULTMAXDELAY = 60;
        
        #endregion

        #region Private Fields

        private string currentApiKey;
        private string currentApiSecret;
        private Flickr currentFlickr;
        private string currentFrob;
        private string currentToken;
        private int? currentRetries;
        private int? currentMinDelay;
        private int? currentMaxDelay;

        #endregion

        #endregion

        #region Constructors

        private XmlConfig()
        {
            // NOOP
        }

        #endregion 

        #region Properties 

        #region Public Properties

        [XmlAttribute("key")]
        public string ApiKey
        {
            get { return this.currentApiKey ?? (this.currentApiKey = XmlConfig.DEFAULTAPIKEY); }
            set { this.currentApiKey = value; }
        }

        [XmlAttribute("secret")]
        public string ApiSecret
        {
            get { return this.currentApiSecret ?? (this.currentApiSecret = XmlConfig.DEFAULTAPISECRET); }
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
                        XmlConfig.DEFAULTAPIKEY == this.ApiKey ||
                        XmlConfig.DEFAULTAPISECRET == this.ApiSecret)
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
        public int Retries
        {
            get { return this.currentRetries.HasValue ? this.currentRetries.Value : (this.currentRetries = XmlConfig.DEFAULTRETRIES).Value; }
            set { this.currentRetries = value; }
        }

        [XmlAttribute("mindelay")]
        public int MinDelay
        {
            get { return this.currentMinDelay.HasValue ? this.currentMinDelay.Value : (this.currentMinDelay = XmlConfig.DEFAULTMINDELAY).Value; }
            set { this.currentMinDelay = value; }
        }

        [XmlAttribute("maxdelay")]
        public int MaxDelay
        {
            get { return this.currentMaxDelay.HasValue ? this.currentMaxDelay.Value : (this.currentMaxDelay = XmlConfig.DEFAULTMAXDELAY).Value; }
            set { this.currentMaxDelay = value; }
        }

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

        public static XmlConfig FromFile()
        {
            return (File.Exists(XmlConfig.FileName)
                        ? XmlConfig.FromFile(XmlConfig.FileName)
                        : new XmlConfig()).Save();
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

        private XmlConfig Save()
        {
            return this.ToFile(XmlConfig.FileName);
        }

        #endregion

        #endregion Methods 
    }
}