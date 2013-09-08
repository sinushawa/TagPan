using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Controls.DragNDrop;
using System.Linq;

namespace TagPan
{
    public class DDNode
    {
        #region Constructors and Destructors

        public DDNode()
        {
            Children = new ObservableCollection<DDNode>();
        }

        public void OnInsert(int index, object obj)
        {
            DragContent content = obj as DragContent;
            if (content != null)
            {
                foreach (var item in content.Items.Reverse())
                {
                    DDNode oldNode = (DDNode)item;
                    DDNode newNode = new DDNode();
                    newNode.Name = string.Format("Copy of {0}", oldNode.Name.Replace(" (Drag Allowed)", string.Empty));
                    Children.Insert(index, newNode);
                }
            }
            else
            {
                Children.Insert(index, new DDNode() { Name = "New node" });
            }
        }

        public bool CanInsertFormat(int index, string format)
        {
            return true;
        }

        public bool CanInsert(int index, object obj)
        {
            return AllowInsert;
        }

        public bool CanDropFormat(string arg)
        {
            return true;
        }

        public bool AllowDrop { get; set; }

        public bool AllowDrag { get; set; }

        public bool AllowInsert { get; set; }

        public bool CanDrop(object obj)
        {
            return AllowDrop;
        }

        public void OnDrop(object obj)
        {
            DragContent content = obj as DragContent;
            if (content != null)
            {
                foreach (var item in content.Items.Reverse())
                {
                    DDNode oldNode = (DDNode)item;
                    DDNode newNode = new DDNode();
                    newNode.Name = string.Format("Copy of {0}", oldNode.Name.Replace(" (Drag Allowed)", string.Empty));
                    Children.Add(newNode);
                }
            }
            else
            {
                Children.Add(new DDNode() { Name = "New node" });
            }
        }

        public bool CanDrag()
        {
            return AllowDrag;
        }

        public object OnDrag()
        {
            return this;
        }

        #endregion

        #region Public Properties

        public ObservableCollection<DDNode> Children { get; set; }

        public string Name { get; set; }
        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
