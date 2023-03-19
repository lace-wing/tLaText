using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tLaText.Core
{
    internal struct LaText
    {
        private string[] cache;
        private int version;
        private LaCursor cursor;
    }
}
