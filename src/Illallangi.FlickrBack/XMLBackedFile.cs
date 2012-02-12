// -----------------------------------------------------------------------
// <copyright file="XMLBackedFile.cs" company="Illallangi Enterprises">
// Copyright (C) 2012 Illallangi Enterprises
// </copyright>
// -----------------------------------------------------------------------

using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Illallangi.FlickrBack
{
    /// <summary>
    /// An class that allows serializing to and deserializing from a XML file.
    /// </summary>
    /// <typeparam name="T">The type to back.</typeparam>
    public abstract class XMLBackedFile<T> where T : XMLBackedFile<T>
    {
        #region Methods

        #region Static Methods

        /// <summary>
        /// Deserializes the specified XML file to a T.
        /// </summary>
        /// <param name="fileName">The file to deserialize.</param>
        /// <returns>A T deserialized from the specified XML file.</returns>
        public static T FromFile(string fileName)
        {
            return FromString(File.ReadAllText(fileName));
        }

        /// <summary>
        /// Deserializes a string to a T.
        /// </summary>
        /// <param name="input">The string to deserialize.</param>
        /// <returns>A T deserialized from the specified string.</returns>
        public static T FromString(string input)
        {
            var ser = new XmlSerializer(typeof(T));
            var sr = new StringReader(input);
            var ret = (T)ser.Deserialize(sr);
            return ret;
        }

        #endregion

        #region Non-Static Methods

        /// <summary>
        /// Serializes this T to a file.
        /// </summary>
        /// <param name="fileName">The filename to serialize to.</param>
        public virtual T ToFile(string fileName)
        {
            File.WriteAllText(fileName, this.ToString());
            return (T)this;
        }

        /// <summary>
        /// Serializes this T to a string.
        /// </summary>
        /// <returns>A string serialization of this T.</returns>
        public override string ToString()
        {
            var stringBuilder = new System.Text.StringBuilder();

            var xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add(string.Empty, string.Empty);

            var xmlSerializer = new XmlSerializer(typeof(T));

            var xmlWriterSettings = new XmlWriterSettings
                                        {
                                            OmitXmlDeclaration = true,
                                            Indent = true
                                        };

            var xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSettings);

            xmlSerializer.Serialize(xmlWriter, this, xmlSerializerNamespaces);
            xmlWriter.Close();

            return stringBuilder.ToString();
        }

        #endregion
        
        #endregion
    }
}
