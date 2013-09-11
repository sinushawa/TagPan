using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TagPan
{
    public class TagAndNodeData
    {
        public string tag;
        public Guid ID;

        public TagAndNodeData(string _tag, Guid _ID)
        {
            tag = _tag;
            ID = _ID;
        }
    }
}
