using System.Windows.Forms;

namespace TagPan
{
    partial class TagPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TagPanel));
            this.applyTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectTaggedItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectCommonItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeTaggedItemsFromSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.addTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addShortcutTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createStructureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameUsingStructureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.addToSelectiontoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.childrenAutoSelecttoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoRenametoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RCBase = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.rightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TV = new TagPan.MSTreeview();
            this.rightClick.SuspendLayout();
            this.SuspendLayout();
            // 
            // applyTagToolStripMenuItem
            // 
            this.applyTagToolStripMenuItem.Name = "applyTagToolStripMenuItem";
            this.applyTagToolStripMenuItem.ShortcutKeyDisplayString = "Shift+num1";
            this.applyTagToolStripMenuItem.Size = new System.Drawing.Size(295, 22);
            this.applyTagToolStripMenuItem.Text = "Apply Tag";
            this.applyTagToolStripMenuItem.ToolTipText = "Shift+num1";
            this.applyTagToolStripMenuItem.Click+=ApplyTag;
            // 
            // selectTaggedItemToolStripMenuItem
            // 
            this.selectTaggedItemToolStripMenuItem.Name = "selectTaggedItemToolStripMenuItem";
            this.selectTaggedItemToolStripMenuItem.ShortcutKeyDisplayString = "Shift+num2";
            this.selectTaggedItemToolStripMenuItem.Size = new System.Drawing.Size(295, 22);
            this.selectTaggedItemToolStripMenuItem.Text = "Select Tagged Item";
            this.selectTaggedItemToolStripMenuItem.ToolTipText = "Shift+num2";
            this.selectTaggedItemToolStripMenuItem.Click += SelectTaggedItem;
            // 
            // selectCommonItemsToolStripMenuItem
            // 
            this.selectCommonItemsToolStripMenuItem.Name = "selectCommonItemsToolStripMenuItem";
            this.selectCommonItemsToolStripMenuItem.ShortcutKeyDisplayString = "Shift+num3";
            this.selectCommonItemsToolStripMenuItem.Size = new System.Drawing.Size(295, 22);
            this.selectCommonItemsToolStripMenuItem.Text = "Select Common Items";
            this.selectCommonItemsToolStripMenuItem.ToolTipText = "Shift+num3";
            this.selectCommonItemsToolStripMenuItem.Click+=SelectCommonItems;
            // 
            // removeTaggedItemsFromSelectionToolStripMenuItem
            // 
            this.removeTaggedItemsFromSelectionToolStripMenuItem.Name = "removeTaggedItemsFromSelectionToolStripMenuItem";
            this.removeTaggedItemsFromSelectionToolStripMenuItem.ShortcutKeyDisplayString = "Shift+num4";
            this.removeTaggedItemsFromSelectionToolStripMenuItem.Size = new System.Drawing.Size(295, 22);
            this.removeTaggedItemsFromSelectionToolStripMenuItem.Text = "Remove Items from selection";
            this.removeTaggedItemsFromSelectionToolStripMenuItem.ToolTipText = "Shift+num4";
            this.removeTaggedItemsFromSelectionToolStripMenuItem.Click += RemoveTaggedItemsFromSelection;
            // 
            // removeTagToolStripMenuItem
            // 
            this.removeTagToolStripMenuItem.Name = "removeTagToolStripMenuItem";
            this.removeTagToolStripMenuItem.ShortcutKeyDisplayString = "Shift+num5";
            this.removeTagToolStripMenuItem.Size = new System.Drawing.Size(295, 22);
            this.removeTagToolStripMenuItem.Text = "Remove Tag";
            this.removeTagToolStripMenuItem.ToolTipText = "Shift+num5";
            this.removeTagToolStripMenuItem.Click += RemoveTag;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(292, 6);
            // 
            // addTagToolStripMenuItem
            // 
            this.addTagToolStripMenuItem.Name = "addTagToolStripMenuItem";
            this.addTagToolStripMenuItem.ShortcutKeyDisplayString = "Crtl+num1";
            this.addTagToolStripMenuItem.Size = new System.Drawing.Size(295, 22);
            this.addTagToolStripMenuItem.Text = "Add Tag";
            this.addTagToolStripMenuItem.ToolTipText = "Crtl+num1";
            this.addTagToolStripMenuItem.Click += AddTag;
            // 
            // addShortcutTagToolStripMenuItem
            // 
            this.addShortcutTagToolStripMenuItem.Name = "addShortcutTagToolStripMenuItem";
            this.addShortcutTagToolStripMenuItem.Size = new System.Drawing.Size(295, 22);
            this.addShortcutTagToolStripMenuItem.Text = "Add shortcut";
            this.addShortcutTagToolStripMenuItem.Click += AddShortcutTag;
            // 
            // createStructureToolStripMenuItem
            // 
            this.createStructureToolStripMenuItem.Name = "createStructureToolStripMenuItem";
            this.createStructureToolStripMenuItem.Size = new System.Drawing.Size(295, 22);
            this.createStructureToolStripMenuItem.Text = "Create tags from name";
            this.createStructureToolStripMenuItem.Click += CreateStructure;
            // 
            // renameUsingStructureToolStripMenuItem
            // 
            this.renameUsingStructureToolStripMenuItem.Name = "renameUsingStructureToolStripMenuItem";
            this.renameUsingStructureToolStripMenuItem.Size = new System.Drawing.Size(295, 22);
            this.renameUsingStructureToolStripMenuItem.Text = "Rename from structure";
            this.renameUsingStructureToolStripMenuItem.Click += RenameUsingStructure;
            // 
            // renameTagToolStripMenuItem
            // 
            this.renameTagToolStripMenuItem.Name = "renameTagToolStripMenuItem";
            this.renameTagToolStripMenuItem.ShortcutKeyDisplayString = "Crtl+num2";
            this.renameTagToolStripMenuItem.Size = new System.Drawing.Size(295, 22);
            this.renameTagToolStripMenuItem.Text = "Rename Tag";
            this.renameTagToolStripMenuItem.ToolTipText = "Crtl+num2";
            this.renameTagToolStripMenuItem.Click += RenameTag;
            // 
            // deleteTagToolStripMenuItem
            // 
            this.deleteTagToolStripMenuItem.Name = "deleteTagToolStripMenuItem";
            this.deleteTagToolStripMenuItem.ShortcutKeyDisplayString = "Crtl+num3";
            this.deleteTagToolStripMenuItem.Size = new System.Drawing.Size(295, 22);
            this.deleteTagToolStripMenuItem.Text = "Delete Tag";
            this.deleteTagToolStripMenuItem.ToolTipText = "Crtl+num3";
            this.deleteTagToolStripMenuItem.Click+=DeleteTag;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(292, 6);
            // 
            // AddToSelectiontoolStripMenuItem
            // 
            this.addToSelectiontoolStripMenuItem.CheckOnClick = true;
            this.addToSelectiontoolStripMenuItem.Name = "AddToSelectiontoolStripMenuItem";
            this.addToSelectiontoolStripMenuItem.ShortcutKeyDisplayString = "Crtl+num4";
            this.addToSelectiontoolStripMenuItem.Size = new System.Drawing.Size(295, 22);
            this.addToSelectiontoolStripMenuItem.Text = "Add to current selection";
            this.addToSelectiontoolStripMenuItem.ToolTipText = "Crtl+num4";
            this.addToSelectiontoolStripMenuItem.Click += AddToSelection;
            // 
            // ChildrenAutoSelecttoolStripMenuItem
            // 
            this.childrenAutoSelecttoolStripMenuItem.CheckOnClick = true;
            this.childrenAutoSelecttoolStripMenuItem.Name = "ChildrenAutoSelecttoolStripMenuItem";
            this.childrenAutoSelecttoolStripMenuItem.ShortcutKeyDisplayString = "Crtl+num5";
            this.childrenAutoSelecttoolStripMenuItem.Size = new System.Drawing.Size(295, 22);
            this.childrenAutoSelecttoolStripMenuItem.Text = "Select objects in children tags";
            this.childrenAutoSelecttoolStripMenuItem.ToolTipText = "Crtl+num5";
            this.childrenAutoSelecttoolStripMenuItem.Click += ChildAutoSelectToggle;
            // 
            // autoRenametoolStripMenuItem
            // 
            this.autoRenametoolStripMenuItem.CheckOnClick = true;
            this.autoRenametoolStripMenuItem.Name = "AutoRenametoolStripMenuItem";
            this.autoRenametoolStripMenuItem.Size = new System.Drawing.Size(295, 22);
            this.autoRenametoolStripMenuItem.Text = "Auto rename using FastTag";
            this.autoRenametoolStripMenuItem.Click += AutoRenameToggle;
            // 
            // RCBase
            // 
            this.RCBase.Name = "RCBase";
            this.RCBase.Size = new System.Drawing.Size(61, 4);
            // 
            // rightClick
            // 
            this.rightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.applyTagToolStripMenuItem,
            this.selectTaggedItemToolStripMenuItem,
            this.selectCommonItemsToolStripMenuItem,
            this.removeTagToolStripMenuItem,
            this.toolStripSeparator1,
            this.addTagToolStripMenuItem,
            this.addShortcutTagToolStripMenuItem,
            this.createStructureToolStripMenuItem,
            this.renameUsingStructureToolStripMenuItem,
            this.renameTagToolStripMenuItem,
            this.deleteTagToolStripMenuItem,
            this.toolStripSeparator2,
            this.addToSelectiontoolStripMenuItem,
            this.childrenAutoSelecttoolStripMenuItem,
            this.autoRenametoolStripMenuItem});
            this.rightClick.Name = "rightClick";
            this.rightClick.Size = new System.Drawing.Size(296, 214);
            // 
            // TV
            // 
            this.TV.AllowDrop = true;
            this.TV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.TV.Location = new System.Drawing.Point(0, 0);
            this.TV.Name = "TV";
            this.TV.SelectedNodes = ((System.Collections.Generic.List<System.Windows.Forms.TreeNode>)(resources.GetObject("TV.SelectedNodes")));
            this.TV.Size = new System.Drawing.Size(197, 250);
            this.TV.TabIndex = 2;
            // 
            // TagPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TV);
            this.Name = "TagPanel";
            this.Size = new System.Drawing.Size(200, 250);
            this.rightClick.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        void TV_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        void TV_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        #endregion

        public System.Windows.Forms.ContextMenuStrip rightClick;
        private System.Windows.Forms.ToolStripMenuItem applyTagToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectTaggedItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectCommonItemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeTaggedItemsFromSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeTagToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem addTagToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addShortcutTagToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createStructureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameUsingStructureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameTagToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteTagToolStripMenuItem;
        public System.Windows.Forms.ContextMenuStrip RCBase;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem addToSelectiontoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem childrenAutoSelecttoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoRenametoolStripMenuItem;
        private MSTreeview TV;
    }
}
