namespace TagPan
{
    partial class FastTag
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
            this.TagBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TagBox
            // 
            this.TagBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TagBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.TagBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.TagBox.Location = new System.Drawing.Point(3, 3);
            this.TagBox.Name = "TagBox";
            this.TagBox.Size = new System.Drawing.Size(294, 20);
            this.TagBox.TabIndex = 0;
            this.TagBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.TagBox.TextChanged += TagBox_TextChanged;
            // 
            // FastTag
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TagBox);
            this.Name = "FastTag";
            this.Size = new System.Drawing.Size(300, 25);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private System.Windows.Forms.TextBox TagBox;
        //private AutoCompleteTextbox TagBox;

    }
}
