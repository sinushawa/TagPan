using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TagPan
{
    public class TagNode: DDNode
    {
        private Guid _ID;

        public Guid ID
        {
          get { return _ID; }
          set { _ID = value; }
        }
        public TagPan.ObservableCollection<Int32> objects;
        public TagPan.ObservableCollection<string> shortcuts;
        public System.Drawing.Color wireColor;

        public TagNode()
        {
        }
        public TagNode(string _label) : this(Guid.NewGuid(), _label, new List<int>())
        {
        }
        public TagNode(string _label, List<Int32> _objects) : this(Guid.NewGuid(), _label, _objects)
        {
        }
        public TagNode(Guid _ID, string _label, List<Int32> _objects)
        {
            ID = _ID;
            Name = _label;
            objects = new TagPan.ObservableCollection<Int32>();
            objects.AddRange(_objects);
            AllowDrag = true;
            AllowDrop = true;
        }
    }
}
