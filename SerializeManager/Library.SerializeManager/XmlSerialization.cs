﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Library.SerializeManager.ISerializable;
using Library.Core;

namespace Library.SerializeManager
{
    public sealed class XmlSerialization : ISerializableAsync, ISerializable.ISerializable
    {

        #region ISerializable

        public void Serialize(Catalog catalog, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Catalog));
            using (var stream = new FileStream(path, FileMode.Create))
            {
                serializer.Serialize(stream, catalog);
            }
        }

        public Catalog Deserialize(string path)
        {
            Catalog catalog = null;

            if (File.Exists(path))
            {
                XmlSerializer formatter = new XmlSerializer(typeof(Catalog));

                using (var stream = new FileStream(path, FileMode.Open))
                {
                    catalog = (Catalog)formatter.Deserialize(stream);
                }
            }

            return catalog;
        }

        #endregion ISerializable

        #region ISerializableAsync

        public async Task SerializeAsync(Catalog catalog, string path)
        {
            await Task.Run(() => Serialize(catalog, path));
        }

        public async Task<Catalog> DeserializeAsync(string path)
        {
            return await Task.Run(() => Deserialize(path));
        }

        #endregion ISerializableAsync
    }
}
