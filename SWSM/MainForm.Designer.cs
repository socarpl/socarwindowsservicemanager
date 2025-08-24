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
            button1 = new Button();
            _check_GetCommandlines = new CheckBox();
            label1 = new Label();
            advancedDataGridView1 = new Zuby.ADGV.AdvancedDataGridView();
            ((System.ComponentModel.ISupportInitialize)advancedDataGridView1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(133, 23);
            button1.TabIndex = 0;
            button1.Text = "Load services";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // _check_GetCommandlines
            // 
            _check_GetCommandlines.AutoSize = true;
            _check_GetCommandlines.Location = new Point(151, 16);
            _check_GetCommandlines.Name = "_check_GetCommandlines";
            _check_GetCommandlines.Size = new Size(123, 19);
            _check_GetCommandlines.TabIndex = 1;
            _check_GetCommandlines.Text = "Get Commandline";
            _check_GetCommandlines.UseVisualStyleBackColor = true;
            _check_GetCommandlines.CheckedChanged += _check_GetCommandlines_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(269, 130);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 2;
            label1.Text = "label1";
            // 
            // advancedDataGridView1
            // 
            advancedDataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            advancedDataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            advancedDataGridView1.FilterAndSortEnabled = true;
            advancedDataGridView1.FilterStringChangedInvokeBeforeDatasourceUpdate = true;
            advancedDataGridView1.Location = new Point(12, 157);
            advancedDataGridView1.MaxFilterButtonImageHeight = 23;
            advancedDataGridView1.Name = "advancedDataGridView1";
            advancedDataGridView1.RightToLeft = RightToLeft.No;
            advancedDataGridView1.Size = new Size(776, 281);
            advancedDataGridView1.SortStringChangedInvokeBeforeDatasourceUpdate = true;
            advancedDataGridView1.TabIndex = 3;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(advancedDataGridView1);
            Controls.Add(label1);
            Controls.Add(_check_GetCommandlines);
            Controls.Add(button1);
            Name = "MainForm";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)advancedDataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private CheckBox _check_GetCommandlines;
        private Label label1;
        private Zuby.ADGV.AdvancedDataGridView advancedDataGridView1;
    }
}
