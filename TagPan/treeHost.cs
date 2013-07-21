using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using GongSolutions.Wpf;

namespace TagPan
{
    public class treeHost : UserControl
    {
        private IContainer components = null;
        private ElementHost elementHost;
        private TreeViewX TreeViewX_obj;
        public event System.EventHandler ForceRedraw;

        public treeHost()
        {
            InitializeComponent();
        }
        public void LinkParent()
        {
            this.TreeViewX_obj.winParent = (Form)base.Parent;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.elementHost = new System.Windows.Forms.Integration.ElementHost();
            this.TreeViewX_obj = new TreeViewX();
            base.SuspendLayout();
            // 
            // treeHostX
            // 
            this.elementHost.AllowDrop = true;
            this.elementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost.Location = new System.Drawing.Point(0, 0);
            this.elementHost.Name = "treeHostX";
            this.elementHost.Size = new System.Drawing.Size(150, 150);
            this.elementHost.TabIndex = 0;
            this.elementHost.Text = "treeX";
            this.elementHost.Child = TreeViewX_obj;
            // 
            // treeHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.elementHost);
            this.Name = "WpfPanel";
            this.ResumeLayout(false);

            

        }
    }
}
