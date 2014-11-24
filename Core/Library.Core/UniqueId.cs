using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core
{
    public sealed class UniqueId
    {
        private static int _id = 100000;

        public static int GetId()
        {
            return _id++;
        }
    }
}
