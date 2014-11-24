using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Library.Core.CatalogItems
{
    [Serializable]
    public abstract class CatalogItem : IComparable
    {
        [XmlAttribute]
        public int Id { get; set; }
        [XmlAttribute]
        public string Title { get; set; }
        [XmlAttribute]
        public int Year { get; set; }

        public CatalogItem()
        {
            Id = UniqueId.GetId();
        }

        public override string ToString()
        {
            return string.Format("Id - {0}, Title - {1}, Year - {2}", Id, Title, Year);
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            CatalogItem otherCatalogItem = obj as CatalogItem;
            if (otherCatalogItem != null)
            {
                return this.Id.CompareTo(otherCatalogItem.Id);
            }
            else 
                throw new ArgumentException();
        }
    }
}