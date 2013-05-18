namespace TestApp
{
    partial class frmE3D
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLoad = new System.Windows.Forms.Button();
            this.eluctionViewer1 = new COL.ElutionViewer.EluctionViewer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cboGlycan = new System.Windows.Forms.ComboBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(12, 12);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "LoadData";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // eluctionViewer1
            // 
            this.eluctionViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eluctionViewer1.Location = new System.Drawing.Point(0, 0);
            this.eluctionViewer1.Name = "eluctionViewer1";
            this.eluctionViewer1.Size = new System.Drawing.Size(827, 578);
            this.eluctionViewer1.TabIndex = 1;           
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.cboGlycan);
            this.splitContainer1.Panel1.Controls.Add(this.btnLoad);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.eluctionViewer1);
            this.splitContainer1.Size = new System.Drawing.Size(827, 623);
            this.splitContainer1.SplitterDistance = 41;
            this.splitContainer1.TabIndex = 2;
            // 
            // cboGlycan
            // 
            this.cboGlycan.FormattingEnabled = true;
            this.cboGlycan.Location = new System.Drawing.Point(109, 12);
            this.cboGlycan.Name = "cboGlycan";
            this.cboGlycan.Size = new System.Drawing.Size(174, 21);
            this.cboGlycan.TabIndex = 2;
            this.cboGlycan.SelectedIndexChanged += new System.EventHandler(this.cboGlycan_SelectedIndexChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(584, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // frmE3D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 623);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmE3D";
            this.Text = "frmE3D";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoad;
        private COL.ElutionViewer.EluctionViewer eluctionViewer1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox cboGlycan;
        private System.Windows.Forms.Label label1;
    }
}