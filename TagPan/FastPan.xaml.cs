using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Form=System.Windows.Forms;

namespace TagPan
{
    public partial class FastPan : System.Windows.Controls.UserControl
    {
        private TagNode root;
        private Point dragStartPoint;

        public TagNode Root
        {
            get { return root; }
            set { root = value; }
        }
        public Form.Form winParent;

        public FastPan()
        {
            InitializeComponent();
            
            LoadSource();
        }
        public void LoadSource()
        {
            root = new TagNode("root");
            root.Objects.Add(0);
            TagNode firstchild = new TagNode("project");
            firstchild.Objects.Add(2);
            TagNode leafOne = new TagNode("leaf");
            leafOne.Objects.Add(165);
            TagNode leafTwo = new TagNode("two");
            leafTwo.Objects.Add(5);
            firstchild.Children.Add(leafOne);
            firstchild.Children.Add(leafTwo);
            root.Children.Add(firstchild);
            DataContext = root;
        }
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            dragStartPoint = e.GetPosition(null);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = dragStartPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed
                && (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                var frameworkElem = ((FrameworkElement)e.OriginalSource);
                DragDrop.DoDragDrop(frameworkElem, new DataObject("Node", frameworkElem.DataContext), DragDropEffects.Move);
            }
        }
    }
}
