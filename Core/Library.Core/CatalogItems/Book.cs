using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Library.Core.CatalogItems
{
    [Serializable]
    public sealed class Book : CatalogItem
    {
        [XmlAttribute]
        public string Author { get; set; }

        public Book()
            : base()
        {
        }

        public override string ToString()
        {
            return base.ToString() + string.Format(", Author - {0}", Author);
        }
    }
}
