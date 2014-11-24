using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


using Library.Core;
using Library.SerializeManager;
using Library.SerializeManager.ISerializable;

namespace Library.FileManager
{
    public sealed class LibraryFileManager
    {
        private readonly ISerializableAsync _serializer;

        public LibraryFileManager(ISerializableAsync serializer)
        {
            this._serializer = serializer;
        }

        public void SerializeFile(Catalog catalog, string path)
        {
            _serializer.Serialize(catalog, path);
        }

        public Catalog DeserializeFile(string path)
        {
            return _serializer.Deserialize(path);
        }

        public async Task SerializeFileAsync(Catalog catalog, string path)
        {
            await _serializer.SerializeAsync(catalog, path);
        }

        public async Task<Catalog> DeserializeFileAsync(string path)
        {
            return await _serializer.DeserializeAsync(path);
        }

        public List<string> GetAvaliableFiles() 
        {
            List<string> files = new List<string>();

            string path = Parameters.CatalogSerializedDirectoryPath;

            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return files;
            }

            return GetFiles(path, "*.xml|*.bin", SearchOption.TopDirectoryOnly);
        }

        private List<string> GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            string[] searchPatterns = searchPattern.Split('|');
            List<string> files = new List<string>();
            foreach (string pattern in searchPatterns)
                files.AddRange(System.IO.Directory.GetFiles(path, pattern, searchOption));
            files.Sort();
            return files;
        }
    }
}
