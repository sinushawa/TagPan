using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace TagPan
{
    public partial class TagPanel : UserControl
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }


        public event EventHandler<TypedEventArg<List<Int32>>> SelectionEvent;
        public event EventHandler<TypedEventArg<KeyValuePair<int, string>>> RenameObjectEvent;
        public event EventHandler<TypedEventArg<List<KeyValuePair<string, string>>>> RenameTagEvent;
        public event EventHandler<TypedEventArg<KeyValuePair<List<DS.TagNode>,List<SimpleTreeNode<DS.TagNode>>>>> SaveEvent;
        public event EventHandler<TypedEventArg<List<string>>> DeleteTagEvent;
        public event EventHandler ForceRedraw;

        public Stopwatch avoidResizeLoop=new Stopwatch();

        public SimpleTree<DS.TagNode> treeDataStructure = new SimpleTree<DS.TagNode>();
        private Dictionary<TreeNode, Guid> UIToData = new Dictionary<TreeNode, Guid>();
        public ObservableCollection<Int32> selectedObjects = new ObservableCollection<Int32>();
        public ObservableCollection<string> selectedObjectsNames = new ObservableCollection<string>();
        public bool additive = false;
        public bool childrenAutoSelect = false;
        public bool autoRename = true;
        public bool overwriteTag = true;
        public string delimiter = "_";
        private List<RECT> dockable;
        public int snapRadius = 18;
        public int minRadius = 1;

        public TagPanel()
        {
            dockable = new List<RECT>();
            InitializeComponent();
            
            selectedObjects.CollectionChanged += selectedObjects_CollectionChanged;
            treeDataStructure.Value = new DS.TagNode("root");
            TV.LabelEdit = true;
            TV.MouseDown += TV_MouseDown;
            TV.DClick += TV_DClick;
            TV.AfterLabelEdit += TV_AfterLabelEdit;
            TV.DragDrop += TV_DragDrop;
            LoadData(new List<SimpleTreeNode<DS.TagNode>>());
            this.Load += TagPanel_Load;
            
        }
        #region internal
        void TagPanel_Load(object sender, EventArgs e)
        {
            var g = this.Parent;
            ((Form)this.Parent).FormClosing += TagPanel_FormClosing;
            ((Form)this.Parent).LocationChanged += TagPanel_LocationChanged;
            ((Form)this.Parent).ResizeEnd += TagPanel_ResizeEnd;
        }

        void TagPanel_ResizeEnd(object sender, EventArgs e)
        {
            if (!avoidResizeLoop.IsRunning || avoidResizeLoop.ElapsedMilliseconds > 1500)
            {
                Form control = (Form)sender;
                RECT controlRECT = new RECT();
                controlRECT.Left = control.Location.X;
                controlRECT.Right = controlRECT.Left + control.Width;
                controlRECT.Top = control.Location.Y;
                controlRECT.Bottom = controlRECT.Top + control.Height;
                foreach (RECT rec in dockable)
                {
                    if (Math.Abs(controlRECT.Right - rec.Left) < snapRadius && Math.Abs(controlRECT.Right - rec.Left) > minRadius)
                    {
                        control.Width = control.Width - (controlRECT.Right - rec.Left);
                    }
                    if (Math.Abs(controlRECT.Bottom - rec.Top) < snapRadius && Math.Abs(controlRECT.Bottom - rec.Top) > minRadius)
                    {
                        control.Height = control.Height - (controlRECT.Bottom - rec.Top);
                    }
                    if (Math.Abs(controlRECT.Bottom - rec.Bottom) < snapRadius && Math.Abs(controlRECT.Bottom - rec.Bottom) > minRadius)
                    {
                        control.Height = control.Height - (controlRECT.Bottom - rec.Bottom) - 6;
                    }
                }
                if (!avoidResizeLoop.IsRunning)
                {
                    avoidResizeLoop.Start();
                }
                else
                {
                    avoidResizeLoop.Reset();
                }
            }
        }

        void TagPanel_LocationChanged(object sender, EventArgs e)
        {
            if (!avoidResizeLoop.IsRunning || avoidResizeLoop.ElapsedMilliseconds > 1500)
            {
                Form control = (Form)sender;
                RECT controlRECT = new RECT();
                controlRECT.Left = control.Location.X;
                controlRECT.Right = controlRECT.Left + control.Width;
                controlRECT.Top = control.Location.Y;
                controlRECT.Bottom = controlRECT.Top + control.Height;
                foreach (RECT rec in dockable)
                {
                    if (Math.Abs(controlRECT.Left - rec.Right) < snapRadius && Math.Abs(controlRECT.Left - rec.Right) > minRadius)
                    {
                        control.Location = new Point(rec.Right, control.Location.Y);
                    }
                    if (Math.Abs(controlRECT.Right - rec.Left) < snapRadius && Math.Abs(controlRECT.Right - rec.Left) > minRadius)
                    {
                        control.Location = new Point(rec.Left - control.Width, control.Location.Y);
                    }
                    if (Math.Abs(controlRECT.Top - rec.Top) < snapRadius && Math.Abs(controlRECT.Top - rec.Top) > minRadius)
                    {
                        control.Location = new Point(control.Location.X, rec.Top);
                    }
                    if (Math.Abs(controlRECT.Top - rec.Bottom) < snapRadius && Math.Abs(controlRECT.Top - rec.Bottom) > minRadius)
                    {
                        control.Location = new Point(control.Location.X, rec.Bottom);
                    }
                    if (Math.Abs(controlRECT.Bottom - rec.Top) < snapRadius && Math.Abs(controlRECT.Bottom - rec.Top) > minRadius)
                    {
                        control.Location = new Point(control.Location.X, rec.Top - control.Height);
                    }
                    if (Math.Abs(controlRECT.Bottom - rec.Bottom) < snapRadius && Math.Abs(controlRECT.Bottom - rec.Bottom) > minRadius)
                    {
                        control.Location = new Point(control.Location.X, rec.Bottom - control.Height);
                    }
                }
                if (!avoidResizeLoop.IsRunning)
                {
                    avoidResizeLoop.Start();
                }
                else
                {
                    avoidResizeLoop.Reset();
                }
            }
        }
        
        private void TagPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveEvent(null, GetDataForSave());
        }        
        private void TreePopulate(SimpleTreeNode<DS.TagNode> _dnode)
        {
            appendNewNode(_dnode.Value.label, _dnode.Value.objects);
            TreePopulate(_dnode.Children.ToList());
        }
        private void TreePopulate(List<SimpleTreeNode<DS.TagNode>> _dnodes)
        {
            foreach (SimpleTreeNode<DS.TagNode> _child in _dnodes)
            {
                TreePopulate(_child);
            }
        }
        private void TV_DragDrop(object sender, DragEventArgs e)
        {
            // Get the screen point.
            Point pt = new Point(e.X, e.Y);

            // Convert to a point in the TreeView's coordinate system.
            pt = TV.PointToClient(pt);

            // Get the node underneath the mouse.
            TreeNode _targetNode = TV.GetNodeAt(pt);
            TreeNode _dragedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            SimpleTreeNode<DS.TagNode> _dragedData = getNodeData(_dragedNode);
            SimpleTreeNode<DS.TagNode> _targetData = getNodeData(_targetNode);
            if (_targetNode != _dragedNode)
            {
                string _dragedParticular = _dragedData.Value.label.Split(new char[] { '_' }).Last();
                string _targetParticular = _targetData.Value.label.Split(new char[] { '_' }).Last();
                if (_dragedParticular.ToLower() != _targetParticular.ToLower())
                {
                    _dragedData.Parent.Children.Remove(_dragedData);
                    _targetData.Children.Add(_dragedData);
                    _dragedNode.Parent.Nodes.Remove(_dragedNode);
                    _targetNode.Nodes.Add(_dragedNode);
                    List<string> _branchName=_dragedNode.FullPath.Split(new char[] { '\\' }).Where(x => x != "project").ToList();
                    _dragedData.Value.label = joinBranchName(_branchName, delimiter);
                }
                else
                {
                    ConcateneNodes(_dragedData, _targetData);
                    _dragedData.Parent.Children.Remove(_dragedData);
                    _dragedNode.Parent.Nodes.Remove(_dragedNode);
                }
                _targetNode.Expand();
            }
            ForceRedraw(null, null);
        }
        private void TV_MouseDown(object sender, MouseEventArgs e)
        {
            MouseEventArgs eventargs = (MouseEventArgs)e;
            if (eventargs.Button == System.Windows.Forms.MouseButtons.Right)
            {
                TV.SelectedNode.ContextMenuStrip.Show(TV, e.Location);
            }
        }
        private void TV_DClick(object sender, EventArgs e)
        {
            SelectTaggedItem(null, null); ;
        }
        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (TV.SelectedNodes.Count > 1)
            {
                rightClick.Items.Find("addShortcutTagToolStripMenuItem", true).FirstOrDefault().Enabled = true;
            }
            else
            {
                rightClick.Items.Find("addShortcutTagToolStripMenuItem", true).FirstOrDefault().Enabled = false;
            }
        }
        private void TV_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label == "")
            {
                e.CancelEdit = true;
            }
            else
            {
                e.Node.Text = e.Label;
                List < SimpleTreeNode < DS.TagNode >> nodesToRelabel = getNodeData(e.Node).GetNodeList();
                List<KeyValuePair<string, string>> _NotifyMaxContainer = new List<KeyValuePair<string, string>>();
                foreach (SimpleTreeNode<DS.TagNode> _dnode in nodesToRelabel)
                {
                    string _previousTag = _dnode.Value.label;
                    List<string> _branchName = getNodeVisual(_dnode).FullPath.Split(new char[] { '\\' }).Where(x => x != "project").ToList();
                    _dnode.Value.label = joinBranchName(_branchName, delimiter);
                    KeyValuePair<string, string> _keyPair = new KeyValuePair<string, string>(_previousTag, _dnode.Value.label);
                    _NotifyMaxContainer.Add(_keyPair);
                }
                RenameTagEvent(null, new TypedEventArg<List<KeyValuePair<string, string>>>(_NotifyMaxContainer));
                ForceRedraw(null, null);
            }
        }
        private void selectedObjects_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (TreeNode _node in TV.HilightedNodes)
            {
                TV.HighLight(_node, false);

            }
            TV.HilightedNodes.Clear();
            foreach (int obj in selectedObjects)
            {
                Debug.WriteLine(obj);
                List<SimpleTreeNode<DS.TagNode>> nodesContaining = treeDataStructure.GetEnumerable(TreeTraversalType.BreadthFirst, TreeTraversalDirection.TopDown).Where<SimpleTreeNode<DS.TagNode>>(x => x.Value.objects.Contains(obj)).ToList<SimpleTreeNode<DS.TagNode>>();
                foreach (SimpleTreeNode<DS.TagNode> _node in nodesContaining)
                {
                    TV.HighLight(getNodeVisual(_node), true);
                }
            }

        }
        private SimpleTreeNode<DS.TagNode> getNodeData(TreeNode _treeNode)
        {
            SimpleTreeNode<DS.TagNode> node = treeDataStructure.GetEnumerable(TreeTraversalType.BreadthFirst, TreeTraversalDirection.TopDown).Where<SimpleTreeNode<DS.TagNode>>(x => x.Value.ID == UIToData[_treeNode]).FirstOrDefault();
            return node;
        }
        private List<SimpleTreeNode<DS.TagNode>> getSelectedNodesData()
        {
            List<SimpleTreeNode<DS.TagNode>> _selectedNodesData = new List<SimpleTreeNode<DS.TagNode>>();
            foreach (TreeNode _treeNode in TV.SelectedNodes)
            {
                _selectedNodesData.Add(getNodeData(_treeNode));
            }
            return _selectedNodesData;
        }
        private TreeNode getNodeVisual(SimpleTreeNode<DS.TagNode> _DataNode)
        {
            Guid ID = _DataNode.Value.ID;
            KeyValuePair<TreeNode, Guid> _pair = UIToData.Where(x => x.Value == ID).FirstOrDefault();
            TreeNode _visualNode = _pair.Key;
            return _visualNode;
        }
        private KeyValuePair<TreeNode, Guid> getNodeKeyValuePair(SimpleTreeNode<DS.TagNode> _DataNode)
        {
            Guid ID = _DataNode.Value.ID;
            KeyValuePair<TreeNode, Guid> _pair = UIToData.Where(x => x.Value == ID).FirstOrDefault();
            return _pair;
        }
        private List<KeyValuePair<TreeNode, Guid>> getVisualDataPairContainingObject(int _object)
        {
            List<KeyValuePair<TreeNode, Guid>> _returnKeyPair = new List<KeyValuePair<TreeNode, Guid>>();
            List<SimpleTreeNode<DS.TagNode>> _dnodes = treeDataStructure.GetEnumerable(TreeTraversalType.BreadthFirst, TreeTraversalDirection.TopDown).Where<SimpleTreeNode<DS.TagNode>>(x => x.Value.objects.Any(y => y == _object)).ToList();
            foreach (SimpleTreeNode<DS.TagNode> _dnode in _dnodes)
            {
                _returnKeyPair.Add(getNodeKeyValuePair(_dnode));
            }
            return _returnKeyPair;
        }
        private List<int> getCommonObjects()
        {
            List<List<int>> objsInNodes = new List<List<int>>();
            List<TreeNode> _nodes;
            if (childrenAutoSelect == true)
            {
                _nodes = TV.SelectedNodes.GetNodeList();
            }
            else
            {
                _nodes = TV.SelectedNodes;
            }
            foreach (TreeNode _node in _nodes)
            {
                objsInNodes.Add(getNodeData(_node).Value.objects);
            }
            List<int> commonObjects = objsInNodes.IntersectNonEmpty().ToList();
            return commonObjects;
        }
        private string joinBranchName(List<string> _tagsElements, string delimiter)
        {
            _tagsElements = _tagsElements.Where(x => x.ToLower() != "project").ToList();
            string _newName = string.Join(delimiter, _tagsElements);
            return _newName;
        }
        private string tagNameFromFullPath(string _fullPath, string delimiter)
        {
            List<string> _branchName = _fullPath.Split(new char[] { '\\' }).Where(x => x != "project").ToList();
            string _tagName = joinBranchName(_branchName, delimiter);
            return _tagName;
        }
        private TreeNode appendNewNode(string _name, TreeNode _parentNode)
        {
            TreeNode newOne = new TreeNode();
            List<string> _branchName = _parentNode.FullPath.Split(new char[] { '\\' }).Where(x => x != "project").ToList();
            DS.TagNode oData;
            if (_branchName.Count > 0)
            {
                oData = new DS.TagNode(joinBranchName(_branchName, delimiter) + "_" + _name);
            }
            else
            {
                oData = new DS.TagNode(_name);
            }
            SimpleTreeNode<DS.TagNode> newOneData = new SimpleTreeNode<DS.TagNode>();
            newOneData.Value = oData;
            newOne.Text = _name;
            newOne.ContextMenuStrip = rightClick;
            newOne.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            getNodeData(_parentNode).Children.Add(newOneData);
            _parentNode.Nodes.Add(newOne);
            _parentNode.Expand();
            UIToData.Add(newOne, oData.ID);
            return newOne;
        }
        private TreeNode appendNewNode(string _name, TreeNode _parentNode, List<int> _objects)
        {
            TreeNode newOne = new TreeNode();
            List<string> _branchName = _parentNode.FullPath.Split(new char[] { '\\' }).Where(x => x != "project").ToList();
            DS.TagNode oData;
            if (_branchName.Count > 0)
            {
                oData = new DS.TagNode(joinBranchName(_branchName, delimiter) + "_" + _name);
            }
            else
            {
                oData = new DS.TagNode(_name);
            }
            oData.objects.AddRange(_objects);
            SimpleTreeNode<DS.TagNode> newOneData = new SimpleTreeNode<DS.TagNode>();
            newOneData.Value = oData;
            newOne.Text = _name;
            newOne.ContextMenuStrip = rightClick;
            newOne.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            getNodeData(_parentNode).Children.Add(newOneData);
            _parentNode.Nodes.Add(newOne);
            _parentNode.Expand();
            UIToData.Add(newOne, oData.ID);
            selectedObjects.RaiseCollectionChanged();
            ForceRedraw(null, null);
            return newOne;
        }
        private SimpleTreeNode<DS.TagNode> appendNewNode(string _name)
        {
            TreeNode holdSel = TV.SelectedNode;
            SimpleTreeNode<DS.TagNode> _bestnode = treeDataStructure.Children[0];
            List<string> _nameElements = _name.Split(new char[] { '_' }).ToList();
            Queue<string> _queuedElements = new Queue<string>(_nameElements);
            string _lastElement;
            string _reconstructedName = "";
            bool first = true;
            while (true)
            {
                if (!first)
                {
                    _reconstructedName += "_";
                }
                _lastElement = _queuedElements.Dequeue();
                _reconstructedName += _lastElement;
                SimpleTreeNode<DS.TagNode> _testnode = getNodeDataFromBranch(_reconstructedName);
                if (_testnode != null)
                {
                    _bestnode = _testnode;
                }
                else
                {
                    break;
                }
                if (_queuedElements.Count < 1)
                {
                    return _bestnode;
                }
                first = false;
            }
            TV.SelectedNode = getNodeVisual(_bestnode);
            TreeNode _targetNode;
            _targetNode = TV.SelectedNode;
            _targetNode = appendNewNode(_lastElement, _targetNode);
            while (_queuedElements.Count > 0)
            {
                _targetNode = appendNewNode(_queuedElements.Dequeue(), _targetNode);
            }
            TV.SelectedNode = holdSel;
            return getNodeData(_targetNode);
        }
        private void CloneNode(int _originalObject, int _clonedObject)
        {
            List<SimpleTreeNode<DS.TagNode>> originalTags = treeDataStructure.GetEnumerable(TreeTraversalType.BreadthFirst, TreeTraversalDirection.TopDown).Where<SimpleTreeNode<DS.TagNode>>(x => x.Value.objects.Any(y => y == _originalObject)).ToList();
            foreach (SimpleTreeNode<DS.TagNode> _node in originalTags)
            {
                _node.Value.objects.Add(_clonedObject);
            }
            SelectionEvent(this, new TypedEventArg<List<int>>(selectedObjects.ToList()));
            ForceRedraw(null, null);
        }
        private void ConcateneNodes(SimpleTreeNode<DS.TagNode> _dragedNode, SimpleTreeNode<DS.TagNode> _targetNode)
        {
            // find common label
            string toCut = _dragedNode.Parent.Value.label;
            string toAdd = _targetNode.Parent.Value.label;

            List<SimpleTreeNode<DS.TagNode>> nodesToChange = _dragedNode.GetNodeList();
            foreach (SimpleTreeNode<DS.TagNode> _node in nodesToChange)
            {
                string newName = _node.Value.label.Replace(toCut, toAdd);
                UIToData.Remove(getNodeVisual(_node));
                appendNewNode(newName, _node.Value.objects);
            }
        }
        private SimpleTreeNode<DS.TagNode> getNodeDataFromBranch(string _branch)
        {
            SimpleTreeNode<DS.TagNode> _dnode = treeDataStructure.GetEnumerable(TreeTraversalType.BreadthFirst, TreeTraversalDirection.TopDown).First<SimpleTreeNode<DS.TagNode>>(x => x.Value.label == _branch);
            return _dnode;
        }
        public List<string> getObjectTags(int _obj)  //used in fastInfo
        {
            List<string> _tags = treeDataStructure.GetEnumerable(TreeTraversalType.BreadthFirst, TreeTraversalDirection.TopDown).Where<SimpleTreeNode<DS.TagNode>>(x => x.Value.objects.Any(y => y == _obj)).Select(x => x.Value.label).ToList();
            return _tags;
        }
        private List<SimpleTreeNode<DS.TagNode>> getHighestNodesInSelection(int _numberOfSwatches)
        {
            List<SimpleTreeNode<DS.TagNode>> _hnodes = getSelectedNodesData().OrderBy(x => x.Depth).Take(_numberOfSwatches).ToList();
            return _hnodes;
        }
        #endregion
        #region UI methods
        private void ApplyTag(object sender, System.EventArgs e)
        {
            getNodeData(TV.SelectedNode).Value.objects.AddRange(selectedObjects);
            getNodeData(TV.SelectedNode).Value.objects = getNodeData(TV.SelectedNode).Value.objects.Distinct().ToList();
            selectedObjects.RaiseCollectionChanged();
            ForceRedraw(null, null);
        }

        private void SelectTaggedItem(object sender, System.EventArgs e)
        {
            List<TreeNode> _nodes;
            if (childrenAutoSelect == true)
            {
                _nodes = TV.SelectedNodes.GetNodeList();
            }
            else
            {
                _nodes = TV.SelectedNodes;
            }
            List<Int32> ObjectsToSelect = new List<Int32>();
            foreach (TreeNode _node in _nodes)
            {
                var temp = getNodeData(_node);
                ObjectsToSelect.AddRange(getNodeData(_node).Value.objects);
            }
            if (additive == true)
            {
                ObjectsToSelect.AddRange(selectedObjects);
            }
            ObjectsToSelect = ObjectsToSelect.Distinct().ToList();

            selectedObjects.Clear();
            selectedObjects.AddRange(ObjectsToSelect);
            SelectionEvent(this, new TypedEventArg<List<int>>(selectedObjects.ToList()));

        }
        private void SelectCommonItems(object sender, System.EventArgs e)
        {
            List<int> commonObjects = getCommonObjects();
            selectedObjects.Clear();
            selectedObjects.AddRange(commonObjects);
            SelectionEvent(this, new TypedEventArg<List<int>>(selectedObjects.ToList()));
        }
        private void RemoveTaggedItemsFromSelection(object sender, EventArgs e)
        {
            List<int> objsInNodes = new List<int>();
            List<TreeNode> _nodes;
            if (childrenAutoSelect == true)
            {
                _nodes = TV.SelectedNodes.GetNodeList();
            }
            else
            {
                _nodes = TV.SelectedNodes;
            }
            foreach (TreeNode _node in _nodes)
            {
                objsInNodes.AddRange(getNodeData(_node).Value.objects);
            }
            objsInNodes = objsInNodes.Distinct().ToList();
            selectedObjects.RemoveRange(objsInNodes);
            SelectionEvent(this, new TypedEventArg<List<int>>(selectedObjects.ToList()));
            ForceRedraw(null, null);
        }
        private void RemoveTag(object sender, System.EventArgs e)
        {
            List<int> objsInNode = getNodeData(TV.SelectedNode).Value.objects;
            for (int i = 0; i < objsInNode.Count; i++)
            {
                foreach (int obj in selectedObjects)
                {
                    if (obj == objsInNode[i])
                    {
                        getNodeData(TV.SelectedNode).Value.objects.Remove(obj);
                    }
                }
            }
            ForceRedraw(null, null);
        }
        private void ClearSelectionTags()
        {
            DeleteObjects(selectedObjects.ToList());
            ForceRedraw(null, null);
        }

        private void AddTag(object sender, EventArgs e)
        {

            TV.SelectedNode = appendNewNode("untitled", TV.SelectedNode);
            TV.SelectedNode.BeginEdit();
        }
        private void AddShortcutTag(object sender, EventArgs e)
        {
            List<int> objsInNodes = getCommonObjects();
            List<SimpleTreeNode<DS.TagNode>> _dnodes = new List<SimpleTreeNode<DS.TagNode>>();
            foreach (TreeNode _tnode in TV.SelectedNodes)
            {
                _dnodes.Add(getNodeData(_tnode));
            }
            SimpleTreeNode<DS.TagNode> _dnode = _dnodes.First(x => x.Depth == _dnodes.Max(y => y.Depth));
            string shortcutName = _dnodes.Where(x => x != _dnode).First().Value.label;
            TreeNode _vnode = getNodeVisual(_dnode);
            appendNewNode(shortcutName, _vnode, objsInNodes);
        }
        private void CreateStructure(object sender, EventArgs e)
        {
            Dictionary<int, List<string>> _objectsTags = new Dictionary<int, List<string>>();
            int counter = 0;
            foreach (string _oname in selectedObjectsNames)
            {
                List<string> _namesElements = new List<string>();
                _namesElements.AddRange(_oname.Split(new Char[] { ' ', '_', '-' }));
                _namesElements = _namesElements.Distinct().ToList();
                _namesElements = _namesElements.Where(x => (!Regex.IsMatch(x, @"\d+"))).ToList();
                _objectsTags.Add(selectedObjects[counter], _namesElements);
                counter++;
            }
            TreeNode _holdSelectedNode = TV.SelectedNode;
            foreach (KeyValuePair<int, List<string>> _keyPair in _objectsTags)
            {
                TV.SelectedNode = _holdSelectedNode;
                for (int i = 0; i < _keyPair.Value.Count; i++)
                {
                    string _element = _keyPair.Value[i];
                    TreeNode _targetNode;
                    if (TV.SelectedNode.Text.ToLower() == _element.ToLower())
                    {
                        _targetNode = TV.SelectedNode;
                        if (i == _keyPair.Value.Count - 1)
                        {
                            getNodeData(_targetNode).Value.objects.Add(_keyPair.Key);
                        }
                    }
                    else if (TV.SelectedNode.Nodes.Cast<TreeNode>().Any(x => x.Text.ToLower() == _element.ToLower()))
                    {
                        _targetNode = TV.SelectedNode.Nodes.Cast<TreeNode>().Where(x => x.Text.ToLower() == _element.ToLower()).First();
                        if (i == _keyPair.Value.Count - 1)
                        {
                            getNodeData(_targetNode).Value.objects.Add(_keyPair.Key);
                        }
                    }
                    else if (TV.Nodes.Cast<TreeNode>().Any(x => x.Text.ToLower() == _element.ToLower()))
                    {
                        _targetNode = TV.Nodes.Cast<TreeNode>().Where(x => x.Text.ToLower() == _element.ToLower()).First();
                    }
                    else
                    {
                        _targetNode = TV.SelectedNode;
                        if (i < _keyPair.Value.Count - 1)
                        {
                            _targetNode = appendNewNode(_element, _targetNode);
                        }
                        else
                        {
                            _targetNode = appendNewNode(_element, _targetNode, new List<int>() { _keyPair.Key });
                        }
                    }
                    TV.SelectedNode = _targetNode;
                }
            }
            TV.SelectedNode = _holdSelectedNode;
            selectedObjects.RaiseCollectionChanged();
            ForceRedraw(null, null);
        }
        
        private void RenameTag(object sender, System.EventArgs e)
        {
            TV.SelectedNode.BeginEdit();
        }

        private void DeleteTag(object sender, System.EventArgs e)
        {

            SimpleTreeNode<DS.TagNode> _dnode = getNodeData(TV.SelectedNode);
            TreeNode _tnode = TV.SelectedNode;
            List<string> _fullPaths = _tnode.GetNodeList().Select(x => x.FullPath).ToList();
            List<string> _tagNames = new List<string>();
            foreach (string _fullPath in _fullPaths)
            {
                _tagNames.Add(tagNameFromFullPath(_fullPath, delimiter));
            }
            _tnode.Parent.Nodes.Remove(_tnode);
            _dnode.Parent.Children.Remove(_dnode);
            UIToData.Remove(_tnode);
            DeleteTagEvent(null, new TypedEventArg<List<string>>(_tagNames));
            ForceRedraw(null, null);
        }
        private void AddToSelection(object sender, System.EventArgs e)
        {
            additive = !additive;
        }
        private void ChildAutoSelectToggle(object sender, System.EventArgs e)
        {
            childrenAutoSelect = !childrenAutoSelect;
        }
        private void AutoRenameToggle(object sender, System.EventArgs e)
        {
            autoRename = !autoRename;
        }
        private void overwriteTagToggle(object sender, System.EventArgs e)
        {
            overwriteTag = !overwriteTag;
        }
        #endregion
        #region Load & save
        public void LoadData(List<SimpleTreeNode<DS.TagNode>> _data)
        {
            SimpleTreeNode<DS.TagNode> Project = new SimpleTreeNode<DS.TagNode>();
            Project.Value = new DS.TagNode("project");
            treeDataStructure.Children.Add(Project);
            TreeNode _node = new TreeNode(Project.Value.label);
            _node.ContextMenuStrip = rightClick;
            TV.Nodes.Add(_node);
            UIToData.Add(_node, Project.Value.ID);
            TreePopulate(_data);
        }
        public TypedEventArg<KeyValuePair<List<DS.TagNode>, List<SimpleTreeNode<DS.TagNode>>>> GetDataForSave()
        {
            List<DS.TagNode> _nodes = new List<DS.TagNode>();
            List<SimpleTreeNode<DS.TagNode>> _allnodes = treeDataStructure.GetEnumerable(TreeTraversalType.BreadthFirst, TreeTraversalDirection.TopDown).Where(x => x.Value.label != "root" && x.Value.label != "project").ToList();
            foreach (SimpleTreeNode<DS.TagNode> _dnode in _allnodes)
            {
                _nodes.Add(_dnode.Value);
            }
            return new TypedEventArg<KeyValuePair<List<DS.TagNode>, List<SimpleTreeNode<DS.TagNode>>>>(new KeyValuePair<List<DS.TagNode>, List<SimpleTreeNode<DS.TagNode>>>(_nodes, treeDataStructure.Children.ToList()));
        }
        public List<ReadWriteKeyPair> CreateTemplateData()
        {
            return new List<ReadWriteKeyPair>();
        }
        public ReadWriteKeyPair CreateKeyPair()
        {
            return new ReadWriteKeyPair();
        }
        public List<int> CreateObjList()
        {
            return new List<int>();
        }
        public void LoadData(List<ReadWriteKeyPair> _data)
        {
            foreach (ReadWriteKeyPair _pair in _data)
            {
                appendNewNode(_pair.key, _pair.objects);
            }
        }
        #endregion
        #region exposed UI linking
        public void RegisterDockableTo(IntPtr _hwnd) //used in RobMainPanel
        {
            RECT rct = new RECT();
            GetWindowRect(_hwnd, ref rct);
            dockable.Add(rct);
        }
        public void RaiseSelectionEvent() //used in RobMainPanel
        {
            SelectionEvent(null, new TypedEventArg<List<int>>(selectedObjects.ToList()));
        }
        public SimpleTreeNode<DS.TagNode> appendNewNode(string _name, List<int> _objects)  //public for fastTag
        {
            TreeNode holdSel = TV.SelectedNode;
            SimpleTreeNode<DS.TagNode> _bestnode = treeDataStructure.Children[0];
            List<string> _nameElements = _name.Split(new char[] { '_' }).ToList();
            Queue<string> _queuedElements = new Queue<string>(_nameElements);
            string _lastElement;
            string _reconstructedName = "";
            bool first = true;
            while (true)
            {
                if (!first)
                {
                    _reconstructedName += "_";
                }
                _lastElement = _queuedElements.Dequeue();
                _reconstructedName += _lastElement;
                SimpleTreeNode<DS.TagNode> _testnode = getNodeDataFromBranch(_reconstructedName);
                if (_testnode != null)
                {
                    _bestnode = _testnode;
                }
                else
                {
                    break;
                }
                if (_queuedElements.Count < 1)
                {
                    _bestnode.Value.objects.AddRange(_objects);
                    return _bestnode;
                }
                first = false;
            }
            TV.SelectedNode = getNodeVisual(_bestnode);
            TreeNode _targetNode;
            _targetNode = TV.SelectedNode;
            _targetNode = appendNewNode(_lastElement, _targetNode);
            while (_queuedElements.Count > 0)
            {
                _targetNode = appendNewNode(_queuedElements.Dequeue(), _targetNode);
            }
            TV.SelectedNode = holdSel;
            getNodeData(_targetNode).Value.objects.AddRange(_objects);
            return getNodeData(_targetNode);
        }
        public void RenameUsingStructure(object sender, EventArgs e) //used in fastTag
        {

            List<KeyValuePair<int, string>> _objectName = new List<KeyValuePair<int, string>>();

            foreach (int _object in selectedObjects)
            {
                if (!_objectName.Any(x => x.Key == _object))
                {
                    List<KeyValuePair<TreeNode, Guid>> _objectVisualDataNodes = getVisualDataPairContainingObject(_object);
                    List<List<string>> _tagsElements = _objectVisualDataNodes.Select(x => x.Key.FullPath.Split(new char[] { '\\' }).ToList()).ToList();
                    int _lengthTree = _tagsElements.Max(x => x.Count);
                    List<string> _longestBranch = _tagsElements.First(x => x.Count == _lengthTree);
                    _tagsElements.Remove(_longestBranch);
                    foreach (List<string> _leftoverElements in _tagsElements)
                    {
                        _longestBranch.AddRange(_leftoverElements);
                        _longestBranch = _longestBranch.Distinct().ToList();
                    }
                    _longestBranch = _longestBranch.Where(x => x.ToLower() != "project").ToList();
                    string _objectBranchChain = joinBranchName(_longestBranch, delimiter);
                    RenameObjectEvent(null, new TypedEventArg<KeyValuePair<int, string>>(new KeyValuePair<int, string>(_object, _objectBranchChain)));
                }
            }
            ForceRedraw(null, null);
        }
        #endregion
        
        public void ClearTree()
        {
            treeDataStructure.Children[0].Children.Clear();
            TV.Nodes[0].Nodes.Clear();
        }
        public void DeleteObjects(List<Int32> _toDels)
        {
            foreach (Int32 _toDel in _toDels)
            {
                var nodeInDel = treeDataStructure.GetNodeList().Where<SimpleTreeNode<DS.TagNode>>(x => x.Value.objects.Contains(_toDel)).ToList<SimpleTreeNode<DS.TagNode>>();
                foreach (SimpleTreeNode<DS.TagNode> lst in nodeInDel)
                {
                    lst.Value.objects.Remove(_toDel);
                }
            }
        }
        public void PasteTagsToSelection(int _objContainingTagsToPaste)
        {
            if (overwriteTag)
            {
                ClearSelectionTags();
            }
            foreach (int _obj in selectedObjects)
            {
                CloneNode(_objContainingTagsToPaste, _obj);
            }
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            