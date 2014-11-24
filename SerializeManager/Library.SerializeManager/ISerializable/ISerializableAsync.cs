using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Library.Core;

namespace Library.SerializeManager.ISerializable
{
    public interface ISerializableAsync : ISerializable
    {
        Task SerializeAsync(Catalog catalog, string path);
        Task<Catalog> DeserializeAsync(string path);
    }
}
