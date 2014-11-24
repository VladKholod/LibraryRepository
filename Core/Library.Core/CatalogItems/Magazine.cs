using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Library.Core.CatalogItems
{
    [Serializable]
    public sealed class Magazine : CatalogItem
    {
        [XmlAttribute]
        public int IssueNumber { get; set; }

        public Magazine()
            : base()
        {
        }

        public override string ToString()
        {
            return base.ToString() + string.Format(", IssueNumber - {0}", IssueNumber);
        }
    }
}