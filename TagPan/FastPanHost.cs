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
    public partial class FastPanHost : UserControl
    {

        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Integration.ElementHost FastPanHostElement;
        private FastPan fastPan_obj;
        public event System.EventHandler ForceRedraw;

        public FastPanHost()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.FastPanHostElement = new System.Windows.Forms.Integration.ElementHost();
            this.fastPan_obj = new TagPan.FastPan();
            this.SuspendLayout();
            // 
            // FastPanHostElement
            // 
            this.FastPanHostElement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FastPanHostElement.Location = new System.Drawing.Point(0, 0);
            this.FastPanHostElement.Name = "FastPanHostElement";
            this.FastPanHostElement.Size = new System.Drawing.Size(150, 150);
            this.FastPanHostElement.TabIndex = 0;
            this.FastPanHostElement.Text = "FastPanHost";
            this.FastPanHostElement.Child = this.fastPan_obj;
            // 
            // FastPanHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.FastPanHostElement);
            this.Name = "FastPanHost";
            this.ResumeLayout(false);

        }

        
        
    }
}
