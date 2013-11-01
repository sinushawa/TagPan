using DS;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
namespace TagPan
{
    public partial class FastWPFTag : System.Windows.Controls.UserControl, IComponentConnector
	{
		private TagPanel tagPan;
		private System.Collections.Generic.List<TagAndNodeData> _labelsIDPair;
		private bool consoleMode = false;
		private ConsoleContainerElement _consoleRoot;
		private ConsoleContainerElement _currentContainer;
		public Form winParent;
		public FastWPFTag()
		{
			this.InitializeComponent();
		}
		public void CreateAutoCompleteSource(TagPanel _tagPan)
		{
			this.tagPan = _tagPan;
			this._labelsIDPair = (
				from x in this.tagPan.treeDataStructure.GetEnumerable(TreeTraversalType.BreadthFirst, TreeTraversalDirection.TopDown)
				select new TagAndNodeData(x.Value.label, x.Value.ID) into x
				where x.tag != "project" && x.tag != "root"
				select x).ToList<TagAndNodeData>();
			this.FastBox.ItemsSource=((
				from x in this._labelsIDPair
				select x.tag).ToList<string>());
			this._consoleRoot = new ConsoleContainerElement();
			this._currentContainer = this._consoleRoot;
		}
		private SimpleTreeNode<TagNode> RetrieveNodeDataFromTag(string _tag)
		{
			TagAndNodeData _pair = (
				from x in this._labelsIDPair
				where x.tag == _tag
				select x).FirstOrDefault<TagAndNodeData>();
			SimpleTreeNode<TagNode> result;
			if (_pair != null)
			{
				result = (
					from x in this.tagPan.treeDataStructure.GetEnumerable(TreeTraversalType.BreadthFirst, TreeTraversalDirection.TopDown)
					where x.Value.ID == _pair.ID
					select x).FirstOrDefault<SimpleTreeNode<TagNode>>();
			}
			else
			{
				result = null;
			}
			return result;
		}
		private System.Collections.Generic.List<SimpleTreeNode<TagNode>> RetrieveNodeDataContainsTag(string _tag)
		{
			return (
				from x in this.tagPan.treeDataStructure.GetNodeList()
				where x.Value.label.Contains(_tag)
				select x).ToList<SimpleTreeNode<TagNode>>();
		}
		private void FastBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			AutoCompleteBox autoCompleteBox = (AutoCompleteBox)sender;
			if (e.Key == Key.Oem4)
			{
				this.consoleMode = true;
				this.FastPop.IsOpen = true;
				ConsoleContainerElement consoleContainerElement = new ConsoleContainerElement(this._currentContainer);
				this._currentContainer.content.Add(consoleContainerElement);
				this._currentContainer = consoleContainerElement;
				e.Handled = true;
			}
			if (e.Key == Key.Oem6)
			{
				SimpleTreeNode<TagNode> simpleTreeNode = this.RetrieveNodeDataFromTag(autoCompleteBox.Text);
				if (simpleTreeNode != null)
				{
					this._currentContainer.content.Add(new ConsoleStringSelElement(autoCompleteBox.Text, simpleTreeNode.Value.objects));
				}
                autoCompleteBox.Text = "";
				this._currentContainer = this._currentContainer.parent;
				e.Handled = true;
			}
			if (e.Key == Key.Add)
			{
				SimpleTreeNode<TagNode> simpleTreeNode = this.RetrieveNodeDataFromTag(autoCompleteBox.Text);
				if (!this.consoleMode)
				{
					if (simpleTreeNode == null)
					{
						simpleTreeNode = this.tagPan.appendNewNode(autoCompleteBox.Text, this.tagPan.selectedObjects.ToList<int>());
					}
					else
					{
						simpleTreeNode.Value.objects.AddRange(this.tagPan.selectedObjects.ToList<int>());
					}
					if (this.tagPan.autoRename)
					{
						this.tagPan.RenameUsingStructure(null, null);
					}
					this.tagPan.selectedObjects.RaiseCollectionChanged();
				}
				else
				{
					if (simpleTreeNode != null)
					{
						this._currentContainer.content.Add(new ConsoleStringSelElement(autoCompleteBox.Text, simpleTreeNode.Value.objects));
						this._currentContainer.ops.Add(concat.addition);
					}
					else
					{
						this._currentContainer.ops.Add(concat.addition);
					}
				}
				autoCompleteBox.Text="";
				e.Handled = true;
			}
			if (e.Key == Key.Subtract)
			{
				SimpleTreeNode<TagNode> simpleTreeNode = this.RetrieveNodeDataFromTag(autoCompleteBox.Text);
				if (!this.consoleMode)
				{
					if (simpleTreeNode != null)
					{
						using (System.Collections.Generic.IEnumerator<int> enumerator = this.tagPan.selectedObjects.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								int _obj = enumerator.Current;
								if (simpleTreeNode.Value.objects.Any((int x) => x == _obj))
								{
									simpleTreeNode.Value.objects.Remove(_obj);
								}
							}
						}
					}
					if (this.tagPan.autoRename)
					{
						this.tagPan.RenameUsingStructure(null, null);
					}
					this.tagPan.selectedObjects.RaiseCollectionChanged();
					this.winParent.Close();
				}
				else
				{
					if (simpleTreeNode != null)
					{
						this._currentContainer.content.Add(new ConsoleStringSelElement(autoCompleteBox.Text, simpleTreeNode.Value.objects));
						this._currentContainer.ops.Add(concat.substraction);
					}
					else
					{
						this._currentContainer.ops.Add(concat.substraction);
					}
					autoCompleteBox.Text="";
					e.Handled = true;
				}
			}
			if (e.Key == Key.Oem2)
			{
				SimpleTreeNode<TagNode> simpleTreeNode = this.RetrieveNodeDataFromTag(autoCompleteBox.Text);
				if (simpleTreeNode == null)
				{
					simpleTreeNode = this.tagPan.appendNewNode(autoCompleteBox.Text, this.tagPan.selectedObjects.ToList<int>());
				}
				else
				{
					simpleTreeNode.Value.objects.AddRange(this.tagPan.selectedObjects.ToList<int>());
				}
				if (this.tagPan.autoRename)
				{
					this.tagPan.RenameUsingStructure(null, null);
				}
				this.tagPan.selectedObjects.RaiseCollectionChanged();
				this.winParent.Close();
			}
			if (e.Key == Key.Return)
			{
				SimpleTreeNode<TagNode> simpleTreeNode = this.RetrieveNodeDataFromTag(autoCompleteBox.Text);
				if (!this.consoleMode)
				{
					System.Collections.Generic.List<SimpleTreeNode<TagNode>> list = new System.Collections.Generic.List<SimpleTreeNode<TagNode>>();
					list.Add(simpleTreeNode);
					if (this.tagPan.childrenAutoSelect)
					{
						list.AddRange(simpleTreeNode.Children.GetNodeList());
					}
					if (simpleTreeNode != null)
					{
						if (!this.tagPan.additive)
						{
							this.tagPan.selectedObjects.Clear();
						}
						foreach (SimpleTreeNode<TagNode> current in list)
						{
							this.tagPan.selectedObjects.AddRange(current.Value.objects);
						}
						this.tagPan.selectedObjects.RaiseCollectionChanged();
						this.tagPan.RaiseSelectionEvent();
					}
				}
				else
				{
					if (simpleTreeNode != null)
					{
						this._currentContainer.content.Add(new ConsoleStringSelElement(autoCompleteBox.Text, simpleTreeNode.Value.objects));
					}
					if (!this.tagPan.additive)
					{
						this.tagPan.selectedObjects.Clear();
					}
					this.tagPan.selectedObjects.AddRange(this._currentContainer.getCorrespondingSel());
					this.tagPan.RaiseSelectionEvent();
				}
				this.winParent.Close();
			}
			if (e.Key == Key.Multiply)
			{
				SimpleTreeNode<TagNode> simpleTreeNode = this.RetrieveNodeDataFromTag(autoCompleteBox.Text);
				if (this.consoleMode)
				{
					if (simpleTreeNode != null)
					{
						this._currentContainer.content.Add(new ConsoleStringSelElement(autoCompleteBox.Text, simpleTreeNode.Value.objects));
						this._currentContainer.ops.Add(concat.intersection);
					}
					else
					{
						this._currentContainer.ops.Add(concat.intersection);
					}
					autoCompleteBox.Text="";
					e.Handled = true;
				}
			}
			if (e.Key == Key.D5 && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
			{
				if (this.consoleMode)
				{
					System.Collections.Generic.List<int> objects = this.RetrieveNodeDataContainsTag(autoCompleteBox.Text).SelectMany((SimpleTreeNode<TagNode> x) => x.Value.objects).ToList<int>();
					this._currentContainer.content.Add(new ConsoleStringSelElement(autoCompleteBox.Text + "%", objects));
				}
				autoCompleteBox.Text="";
				e.Handled = true;
			}
			if (e.Key == Key.D4 && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
			{
				if (this.consoleMode)
				{
					this._currentContainer.content.Add(new ConsoleStringSelElement("$", this.tagPan.selectedObjects.ToList<int>()));
				}
				autoCompleteBox.Text="";
				e.Handled = true;
			}
			if (e.Key == Key.D3 && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
			{
				SimpleTreeNode<TagNode> simpleTreeNode = this.RetrieveNodeDataFromTag(autoCompleteBox.Text);
				if (this.consoleMode)
				{
					if (simpleTreeNode != null)
					{
						System.Collections.Generic.List<int> objects2 = simpleTreeNode.GetNodeList().SelectMany((SimpleTreeNode<TagNode> x) => x.Value.objects).ToList<int>();
						this._currentContainer.content.Add(new ConsoleStringSelElement(autoCompleteBox.Text + "#", objects2));
					}
				}
				autoCompleteBox.Text="";
				e.Handled = true;
			}
			if (e.Key == Key.Back)
			{
				if (this.consoleMode && autoCompleteBox.Text == "")
				{
					if (this._currentContainer.content.Count > 0 || this._currentContainer.ops.Count > 0)
					{
						if (this._currentContainer.content.Count > this._currentContainer.ops.Count)
						{
							this._currentContainer.content.RemoveAt(this._currentContainer.content.Count - 1);
						}
						else
						{
							this._currentContainer.ops.RemoveAt(this._currentContainer.ops.Count - 1);
						}
					}
					else
					{
						if (this._currentContainer.parent != null)
						{
							this._currentContainer = this._currentContainer.parent;
						}
					}
				}
			}
			if (e.Key == Key.Escape)
			{
				this.winParent.Close();
			}
			this.fastTip.Text = this._consoleRoot.getCorrespondingStr();
		}
		
	}
}
