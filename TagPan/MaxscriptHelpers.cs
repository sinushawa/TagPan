using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TagPan
{
    public class ReadWriteKeyPair
    {
        public string key;
        public List<int> objects;

        public ReadWriteKeyPair()
        {
            objects = new List<int>();
        }
    }
}
