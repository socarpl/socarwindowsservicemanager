namespace SWSM
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            _listBox_ProfileList = new ListBox();
            toolStrip1 = new ToolStrip();
            _button_AddProfile = new ToolStripButton();
            _adgv_ServicesInProfile = new Zuby.ADGV.AdvancedDataGridView();
            panel1 = new Panel();
            advancedDataGridViewSearchToolBar1 = new Zuby.ADGV.AdvancedDataGridViewSearchToolBar();
            splitContainer1 = new SplitContainer();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_adgv_ServicesInProfile).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // _listBox_ProfileList
            // 
            _listBox_ProfileList.Dock = DockStyle.Fill;
            _listBox_ProfileList.FormattingEnabled = true;
            _listBox_ProfileList.Location = new Point(0, 0);
            _listBox_ProfileList.Name = "_listBox_ProfileList";
            _listBox_ProfileList.Size = new Size(296, 558);
            _listBox_ProfileList.TabIndex = 4;
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Items.AddRange(new ToolStripItem[] { _button_AddProfile });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(914, 27);
            toolStrip1.TabIndex = 5;
            toolStrip1.Text = "toolStrip1";
            // 
            // _button_AddProfile
            // 
            _button_AddProfile.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _button_AddProfile.Image = (Image)resources.GetObject("_button_AddProfile.Image");
            _button_AddProfile.ImageTransparentColor = Color.Magenta;
            _button_AddProfile.Name = "_button_AddProfile";
            _button_AddProfile.Size = new Size(29, 24);
            _button_AddProfile.Text = "toolStripButton1";
            // 
            // _adgv_ServicesInProfile
            // 
            _adgv_ServicesInProfile.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _adgv_ServicesInProfile.BackgroundColor = SystemColors.Window;
            _adgv_ServicesInProfile.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _adgv_ServicesInProfile.FilterAndSortEnabled = true;
            _adgv_ServicesInProfile.FilterStringChangedInvokeBeforeDatasourceUpdate = true;
            _adgv_ServicesInProfile.Location = new Point(3, 32);
            _adgv_ServicesInProfile.MaxFilterButtonImageHeight = 23;
            _adgv_ServicesInProfile.Name = "_adgv_ServicesInProfile";
            _adgv_ServicesInProfile.RightToLeft = RightToLeft.No;
            _adgv_ServicesInProfile.RowHeadersWidth = 51;
            _adgv_ServicesInProfile.Size = new Size(587, 526);
            _adgv_ServicesInProfile.SortStringChangedInvokeBeforeDatasourceUpdate = true;
            _adgv_ServicesInProfile.TabIndex = 6;
            // 
            // panel1
            // 
            panel1.Controls.Add(advancedDataGridViewSearchToolBar1);
            panel1.Controls.Add(_adgv_ServicesInProfile);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(590, 558);
            panel1.TabIndex = 7;
            // 
            // advancedDataGridViewSearchToolBar1
            // 
            advancedDataGridViewSearchToolBar1.AllowMerge = false;
            advancedDataGridViewSearchToolBar1.GripStyle = ToolStripGripStyle.Hidden;
            advancedDataGridViewSearchToolBar1.ImageScalingSize = new Size(20, 20);
            advancedDataGridViewSearchToolBar1.Location = new Point(0, 0);
            advancedDataGridViewSearchToolBar1.MaximumSize = new Size(0, 27);
            advancedDataGridViewSearchToolBar1.MinimumSize = new Size(0, 27);
            advancedDataGridViewSearchToolBar1.Name = "advancedDataGridViewSearchToolBar1";
            advancedDataGridViewSearchToolBar1.RenderMode = ToolStripRenderMode.Professional;
            advancedDataGridViewSearchToolBar1.Size = new Size(590, 27);
            advancedDataGridViewSearchToolBar1.TabIndex = 0;
            advancedDataGridViewSearchToolBar1.Text = "advancedDataGridViewSearchToolBar1";
            // 
            // splitContainer1
            // 
            splitContainer1.Location = new Point(12, 30);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(_listBox_ProfileList);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(panel1);
            splitContainer1.Size = new Size(890, 558);
            splitContainer1.SplitterDistance = 296;
            splitContainer1.TabIndex = 8;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 600);
            Controls.Add(splitContainer1);
            Controls.Add(toolStrip1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "MainForm";
            Text = "Form1";
            Load += Form1_Load;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_adgv_ServicesInProfile).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox _listBox_ProfileList;
        private ToolStrip toolStrip1;
        private ToolStripButton _button_AddProfile;
        private Zuby.ADGV.AdvancedDataGridView _adgv_ServicesInProfile;
        private Panel panel1;
        private Zuby.ADGV.AdvancedDataGridViewSearchToolBar advancedDataGridViewSearchToolBar1;
        private SplitContainer splitContainer1;
    }
}
