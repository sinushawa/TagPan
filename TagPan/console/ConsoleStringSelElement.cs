using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TagPan
{
    class ConsoleStringSelElement : ConsoleElement
    {
        public string name;
        public List<int> objects;

        public ConsoleStringSelElement(string _name, List<int> _objects)
        {
            name = _name;
            objects = _objects;
        }

        public override List<int> getCorrespondingSel()
        {
            return objects;
        }
        public override string getCorrespondingStr()
        {
            return name;
        }

    }
}
