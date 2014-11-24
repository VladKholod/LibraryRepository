using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Library.Core;

namespace Library.SerializeManager.ISerializable
{
    public interface ISerializable
    {
        void Serialize(Catalog catalog, string path);
        Catalog Deserialize(string path);
    }
}
