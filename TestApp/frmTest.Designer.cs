namespace TestApp
{
    partial class frmTest
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
            this.btnReader = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtScanNo = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnDecreaseScanNo = new System.Windows.Forms.Button();
            this.btnIncreaseScanNo = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnReader
            // 
            this.btnReader.Location = new System.Drawing.Point(439, 47);
            this.btnReader.Name = "btnReader";
            this.btnReader.Size = new System.Drawing.Size(75, 23);
            this.btnReader.TabIndex = 0;
            this.btnReader.Text = "Read Scan";
            this.btnReader.UseVisualStyleBackColor = true;
            this.btnReader.Click += new System.EventHandler(this.btnReader_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(13, 12);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(470, 20);
            this.txtFileName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "ScanNo";
            // 
            // txtScanNo
            // 
            this.txtScanNo.Location = new System.Drawing.Point(65, 49);
            this.txtScanNo.Name = "txtScanNo";
            this.txtScanNo.Size = new System.Drawing.Size(53, 20);
            this.txtScanNo.TabIndex = 3;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(489, 10);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(25, 23);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(885, 686);
            this.dataGridView1.TabIndex = 5;
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
            this.splitContainer1.Panel1.Controls.Add(this.btnIncreaseScanNo);
            this.splitContainer1.Panel1.Controls.Add(this.btnDecreaseScanNo);
            this.splitContainer1.Panel1.Controls.Add(this.txtFileName);
            this.splitContainer1.Panel1.Controls.Add(this.btnBrowse);
            this.splitContainer1.Panel1.Controls.Add(this.btnReader);
            this.splitContainer1.Panel1.Controls.Add(this.txtScanNo);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(885, 774);
            this.splitContainer1.SplitterDistance = 84;
            this.splitContainer1.TabIndex = 6;
            // 
            // btnDecreaseScanNo
            // 
            this.btnDecreaseScanNo.Location = new System.Drawing.Point(124, 47);
            this.btnDecreaseScanNo.Name = "btnDecreaseScanNo";
            this.btnDecreaseScanNo.Size = new System.Drawing.Size(34, 23);
            this.btnDecreaseScanNo.TabIndex = 5;
            this.btnDecreaseScanNo.Text = "-";
            this.btnDecreaseScanNo.UseVisualStyleBackColor = true;
            this.btnDecreaseScanNo.Click += new System.EventHandler(this.btnDecreaseScanNo_Click);
            // 
            // btnIncreaseScanNo
            // 
            this.btnIncreaseScanNo.Location = new System.Drawing.Point(164, 47);
            this.btnIncreaseScanNo.Name = "btnIncreaseScanNo";
            this.btnIncreaseScanNo.Size = new System.Drawing.Size(43, 23);
            this.btnIncreaseScanNo.TabIndex = 6;
            this.btnIncreaseScanNo.Text = "+";
            this.btnIncreaseScanNo.UseVisualStyleBackColor = true;
            this.btnIncreaseScanNo.Click += new System.EventHandler(this.btnIncreaseScanNo_Click);
            // 
            // frmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 774);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmTest";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnReader;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtScanNo;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnIncreaseScanNo;
        private System.Windows.Forms.Button btnDecreaseScanNo;
    }
}

