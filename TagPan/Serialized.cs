using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DS
{
    public class TagNode
    {
        public Guid ID;
        public string label;
        public bool isShortcut;
        public List<Int32> objects;

        public TagNode()
        {
        }
        public TagNode(string _label) : this(Guid.NewGuid(), _label, false, new List<int>())
        {
        }
        public TagNode(string _label, bool _isShortcut) : this(Guid.NewGuid(), _label, _isShortcut, new List<int>())
        {
        }
        public TagNode(string _label, List<Int32> _objects) : this(Guid.NewGuid(), _label, false, _objects)
        {
        }
        public TagNode(Guid _ID, string _label, bool _isShortcut, List<Int32> _objects)
        {
            ID = _ID;
            label = _label;
            isShortcut = _isShortcut;
            objects = _objects;
        }
    }
}
