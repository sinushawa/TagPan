using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Form=System.Windows.Forms.Form;
using GongSolutions.Wpf;
using DragDrop = GongSolutions.Wpf.DragDrop;
using System.Windows.Data;

namespace TagPan
{
    public interface ISelectable
    {
        bool IsSelected { get; set; }
    }
public partial class TreeViewX : System.Windows.Controls.UserControl
{
    #region Attributes

    private TreeViewItem _lastItemSelected; // Used in shift selections
    private TreeViewItem _itemToCheck; // Used when clicking on a selected item to check if we want to deselect it or to drag the current selection
    private bool _isDragEnabled;
    private bool _isDropEnabled;
    public Form winParent;

    #endregion

    #region Dependency Properties

    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable<ISelectable>), typeof(TreeViewX));

    public IEnumerable<ISelectable> ItemsSource
    {
        get
        {
            return (IEnumerable<ISelectable>)this.GetValue(TreeViewX.ItemsSourceProperty);
        }
        set
        {
            this.SetValue(TreeViewX.ItemsSourceProperty, value);
        }
    }

    public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(TreeViewX));

    public DataTemplate ItemTemplate
    {
        get
        {
            return (DataTemplate)GetValue(TreeViewX.ItemTemplateProperty);
        }
        set
        {
            SetValue(TreeViewX.ItemTemplateProperty, value);
        }
    }

    public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(TreeViewX));

    public Style ItemContainerStyle
    {
        get
        {
            return (Style)GetValue(TreeViewX.ItemContainerStyleProperty);
        }
        set
        {
            SetValue(TreeViewX.ItemContainerStyleProperty, value);
        }
    }

    public static readonly DependencyProperty DropHandlerProperty = DependencyProperty.Register("DropHandler", typeof(IDropTarget), typeof(TreeViewX));

    public IDropTarget DropHandler
    {
        get
        {
            return (IDropTarget)GetValue(TreeViewX.DropHandlerProperty);
        }
        set
        {
            SetValue(TreeViewX.DropHandlerProperty, value);
        }
    }

    #endregion

    #region Properties

    public bool IsDragEnabled
    {
        get
        {
            return _isDragEnabled;
        }
        set
        {
            if (_isDragEnabled != value)
            {
                _isDragEnabled = value;
                DragDrop.SetIsDragSource(this.InnerTreeView, _isDragEnabled);
            }
        }
    }

    public bool IsDropEnabled
    {
        get
        {
            return _isDropEnabled;
        }
        set
        {
            if (_isDropEnabled != value)
            {
                _isDropEnabled = value;
                DragDrop.SetIsDropTarget(this.InnerTreeView, _isDropEnabled);
            }
        }
    }

    #endregion

    #region Public Methods

    public TreeViewX()
    {
        InitializeComponent();
        HierarchicalDataTemplate treeViewDataTemplate = new HierarchicalDataTemplate(typeof(SimpleTree<DS.TagNode>));
        treeViewDataTemplate.ItemsSource = new Binding("Children");
        FrameworkElementFactory fEF = new FrameworkElementFactory(typeof(TextBlock));
        fEF.SetBinding(TextBlock.TextProperty, new Binding("Value.label"));
        treeViewDataTemplate.VisualTree = fEF;
        InnerTreeView.ItemTemplate = treeViewDataTemplate;
    }

    #endregion

    #region Event Handlers

    private void TreeViewOnPreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.OriginalSource is Shape || e.OriginalSource is Grid || e.OriginalSource is Border) // If clicking on the + of the tree
            return;

        TreeViewItem item = this.GetTreeViewItemClicked((FrameworkElement)e.OriginalSource);

        if (item != null && item.Header != null)
        {
            this.SelectedItemChangedHandler(item);
        }
    }

    // Check done to avoid deselecting everything when clicking to drag
    private void TreeViewOnPreviewMouseUp(object sender, MouseButtonEventArgs e)
    {
        if (_itemToCheck != null)
        {
            TreeViewItem item = this.GetTreeViewItemClicked((FrameworkElement)e.OriginalSource);

            if (item != null && item.Header != null)
            {
                if (!TreeViewX.IsCtrlPressed)
                {
                    GetTreeViewItems(true).Select(t => t.Header).Cast<ISelectable>().ToList().ForEach(f => f.IsSelected = false);
                    ((ISelectable)_itemToCheck.Header).IsSelected = true;
                    _lastItemSelected = _itemToCheck;
                }
                else
                {
                    ((ISelectable)_itemToCheck.Header).IsSelected = false;
                    _lastItemSelected = null;
                }
            }
        }
    }

    #endregion

    #region Private Methods

    private void SelectedItemChangedHandler(TreeViewItem item)
    {
        ISelectable content = (ISelectable)item.Header;

        _itemToCheck = null;

        if (content.IsSelected)
        {
            // Check it at the mouse up event to avoid deselecting everything when clicking to drag
            _itemToCheck = item;
        }
        else
        {
            if (!TreeViewX.IsCtrlPressed)
            {
                GetTreeViewItems(true).Select(t => t.Header).Cast<ISelectable>().ToList().ForEach(f => f.IsSelected = false);
            }

            if (TreeViewX.IsShiftPressed && _lastItemSelected != null)
            {
                foreach (TreeViewItem tempItem in GetTreeViewItemsBetween(_lastItemSelected, item))
                {
                    ((ISelectable)tempItem.Header).IsSelected = true;
                    _lastItemSelected = tempItem;
                }
            }
            else
            {
                content.IsSelected = true;
                _lastItemSelected = item;
            }
        }
    }

    private static bool IsCtrlPressed
    {
        get
        {
            return Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
        }
    }

    private static bool IsShiftPressed
    {
        get
        {
            return Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
        }
    }

    private TreeViewItem GetTreeViewItemClicked(UIElement sender)
    {
        Point point = sender.TranslatePoint(new Point(0, 0), this.InnerTreeView);
        DependencyObject visualItem = this.InnerTreeView.InputHitTest(point) as DependencyObject;
        while (visualItem != null && !(visualItem is TreeViewItem))
        {
            visualItem = VisualTreeHelper.GetParent(visualItem);
        }

        return visualItem as TreeViewItem;
    }

    private IEnumerable<TreeViewItem> GetTreeViewItemsBetween(TreeViewItem start, TreeViewItem end)
    {
        List<TreeViewItem> items = this.GetTreeViewItems(false);

        int startIndex = items.IndexOf(start);
        int endIndex = items.IndexOf(end);

        // It's possible that the start element has been removed after it was selected,
        // I don't find a way to happen on the end but I add the code to handle the situation just in case
        if (startIndex == -1 && endIndex == -1)
        {
            return new List<TreeViewItem>();
        }
        else if (startIndex == -1)
        {
            return new List<TreeViewItem>() {end};
        }
        else if (endIndex == -1)
        {
            return new List<TreeViewItem>() { start };
        }
        else
        {
            return startIndex > endIndex ? items.GetRange(endIndex, startIndex - endIndex + 1) : items.GetRange(startIndex, endIndex - startIndex + 1); 
        }
    }

    private List<TreeViewItem> GetTreeViewItems(bool includeCollapsedItems)
    {
        List<TreeViewItem> returnItems = new List<TreeViewItem>();

        for (int index = 0; index < this.InnerTreeView.Items.Count; index++)
        {
            TreeViewItem item = (TreeViewItem)this.InnerTreeView.ItemContainerGenerator.ContainerFromIndex(index);
            returnItems.Add(item);
            if (includeCollapsedItems || item.IsExpanded)
            {
                returnItems.AddRange(GetTreeViewItemItems(item, includeCollapsedItems));                    
            }
        }

        return returnItems;
    }

    private static IEnumerable<TreeViewItem> GetTreeViewItemItems(TreeViewItem treeViewItem, bool includeCollapsedItems)
    {
        List<TreeViewItem> returnItems = new List<TreeViewItem>();

        for (int index = 0; index < treeViewItem.Items.Count; index++)
        {
            TreeViewItem item = (TreeViewItem)treeViewItem.ItemContainerGenerator.ContainerFromIndex(index);
            if (item != null)
            {
                returnItems.Add(item);
                if (includeCollapsedItems || item.IsExpanded)
                {
                    returnItems.AddRange(GetTreeViewItemItems(item, includeCollapsedItems));
                }
            }
        }

        return returnItems;
    }

    #endregion
}
}
