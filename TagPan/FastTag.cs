using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

    public partial class FastTag : UserControl
    {
        public event EventHandler ForceRedraw;

        private TagPanel tagPan;
        private List<TagAndNodeData> _labelsIDPair;

        //public event EventHandler<TypedEventArg<string>> TaggedEvent;

        public FastTag()
        {
            InitializeComponent();
            
        }
        public void CreateAutoCompleteSource(TagPanel _tagPan)
        {
            tagPan = _tagPan;
            _labelsIDPair = tagPan.treeDataStructure.GetEnumerable(TreeTraversalType.BreadthFirst, TreeTraversalDirection.TopDown).Select(x => new TagAndNodeData(x.Value.label, x.Value.ID)).Where(x => x.tag != "project" && x.tag != "root").ToList();

            #region textBox
            TagBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection _autoSource = new AutoCompleteStringCollection();
            _autoSource.AddRange(_labelsIDPair.Select(x => x.tag).ToArray());
            TagBox.AutoCompleteCustomSource =_autoSource;
            #endregion

            //TagBox.AutoCompleteList = _labels;
        }

        private SimpleTreeNode<DS.TagNode> RetrieveNodeDataFromTag(string _tag)
        {
            TagAndNodeData _pair = _labelsIDPair.Where(x => x.tag == _tag).FirstOrDefault();
            SimpleTreeNode<DS.TagNode> _dnode;
            if (_pair != null)
            {
                _dnode = tagPan.treeDataStructure.GetEnumerable(TreeTraversalType.BreadthFirst, TreeTraversalDirection.TopDown).Where(x => x.Value.ID == _pair.ID).FirstOrDefault();
            }
            else
            {
                _dnode = null;
            }

            return _dnode;
        }
        private List<SimpleTreeNode<DS.TagNode>> RetrieveNodeDataContainsTag(string _tag)
        {
            List<SimpleTreeNode<DS.TagNode>> _listNodesContaining = tagPan.treeDataStructure.GetEnumerable(TreeTraversalType.BreadthFirst, TreeTraversalDirection.TopDown).Where(x => x.Value.label == _tag).ToList();
            return _listNodesContaining;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Add)
            {
                SimpleTreeNode<DS.TagNode> _dnode = RetrieveNodeDataFromTag(TagBox.Text);
                if (_dnode == null)
                {
                    _dnode = tagPan.appendNewNode(TagBox.Text, tagPan.selectedObjects.ToList());
                }
                else
                {
                    _dnode.Value.objects.AddRange(tagPan.selectedObjects.ToList());
                }
                TagBox.Text = "";
                e.SuppressKeyPress = true;
                if (tagPan.autoRename == true)
                {
                    tagPan.RenameUsingStructure(null, null);
                }
                tagPan.selectedObjects.RaiseCollectionChanged();
                ForceRedraw(null, null);
            }
            if (e.KeyCode == Keys.Subtract)
            {
                SimpleTreeNode<DS.TagNode> _dnode = RetrieveNodeDataFromTag(TagBox.Text);
                if (_dnode != null)
                {
                    foreach (int _obj in tagPan.selectedObjects)
                    {
                        if (_dnode.Value.objects.Any(x => x == _obj))
                        {
                            _dnode.Value.objects.Remove(_obj);
                        }
                    }
                }
                if (tagPan.autoRename == true)
                {
                    tagPan.RenameUsingStructure(null, null);
                }
                tagPan.selectedObjects.RaiseCollectionChanged();
                ForceRedraw(null, null);
                ((Form)this.Parent).Close();
            }
            if (e.KeyCode == Keys.OemQuestion)
            {
                SimpleTreeNode<DS.TagNode> _dnode = RetrieveNodeDataFromTag(TagBox.Text);
                if (_dnode == null)
                {
                    _dnode = tagPan.appendNewNode(TagBox.Text, tagPan.selectedObjects.ToList());
                }
                else
                {
                    _dnode.Value.objects.AddRange(tagPan.selectedObjects.ToList());
                }
                if (tagPan.autoRename == true)
                {
                    tagPan.RenameUsingStructure(null, null);
                }
                tagPan.selectedObjects.RaiseCollectionChanged();
                ((Form)this.Parent).Close();
                
            }
            if (e.KeyCode == Keys.Return)
            {
                List<SimpleTreeNode<DS.TagNode>> _dnodes = new List<SimpleTreeNode<DS.TagNode>>();
                SimpleTreeNode<DS.TagNode> _dnode = RetrieveNodeDataFromTag(TagBox.Text);
                _dnodes.Add(_dnode);
                if (tagPan.childrenAutoSelect == true)
                {
                    _dnodes.AddRange(_dnode.Children.GetNodeList());
                }
                if (_dnode != null)
                {
                    if (tagPan.additive != true)
                    {
                        tagPan.selectedObjects.Clear();
                    }
                    foreach (SimpleTreeNode<DS.TagNode> _dnodeChild in _dnodes)
                    {
                        tagPan.selectedObjects.AddRange(_dnodeChild.Value.objects);
                    }
                    tagPan.selectedObjects.RaiseCollectionChanged();
                    tagPan.RaiseSelectionEvent();
                }
                ((Form)this.Parent).Close();
            }
            if (e.KeyCode == Keys.Multiply)
            {
                List<SimpleTreeNode<DS.TagNode>> _dnodes = new List<SimpleTreeNode<DS.TagNode>>();
                SimpleTreeNode<DS.TagNode> _dnode = RetrieveNodeDataFromTag(TagBox.Text);
                _dnodes.Add(_dnode);
                if (tagPan.childrenAutoSelect == true)
                {
                    _dnodes.AddRange(_dnode.Children.GetNodeList());
                }
                if (_dnode != null)
                {
                    if (tagPan.additive != true)
                    {
                        tagPan.selectedObjects.Clear();
                    }
                    foreach (SimpleTreeNode<DS.TagNode> _dnodeChild in _dnodes)
                    {
                        tagPan.selectedObjects.AddRange(_dnodeChild.Value.objects);
                    }
                    tagPan.selectedObjects.RaiseCollectionChanged();
                    tagPan.RaiseSelectionEvent();
                }
                ((Form)this.Parent).Close();
            }
            if (e.KeyCode == Keys.Escape)
            {
                ((Form)this.Parent).Close();
            }
        }
        private void TagBox_TextChanged(object sender, System.EventArgs e)
        {
            
        }
    }
}
