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
            this.SuspendLayout();
            // 
            // elementHost1
            // 
            this.FastPanHostElement.Location = new System.Drawing.Point(3, 3);
            this.FastPanHostElement.Name = "FastPanHost";
            this.FastPanHostElement.Size = new System.Drawing.Size(144, 100);
            this.FastPanHostElement.TabIndex = 0;
            this.FastPanHostElement.Text = "FastPanHost";
            this.FastPanHostElement.Child = null;
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
