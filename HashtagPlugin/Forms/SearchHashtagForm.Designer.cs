namespace HashtagPlugin.Forms
{
    partial class SearchHashtagForm
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
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.flpHashtags = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvResult = new System.Windows.Forms.DataGridView();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Subject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(289, 14);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(360, 22);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(665, 14);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // flpHashtags
            // 
            this.flpHashtags.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpHashtags.Location = new System.Drawing.Point(192, 63);
            this.flpHashtags.Name = "flpHashtags";
            this.flpHashtags.Size = new System.Drawing.Size(627, 145);
            this.flpHashtags.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Search with hashtags";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(825, 121);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dgvResult
            // 
            this.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Type,
            this.Subject});
            this.dgvResult.Location = new System.Drawing.Point(16, 227);
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.RowHeadersWidth = 51;
            this.dgvResult.RowTemplate.Height = 24;
            this.dgvResult.Size = new System.Drawing.Size(947, 446);
            this.dgvResult.TabIndex = 6;
            this.dgvResult.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvResult_CellDoubleClick);
            // 
            // Type
            // 
            this.Type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Type.HeaderText = "Type";
            this.Type.MinimumWidth = 6;
            this.Type.Name = "Type";
            this.Type.Width = 68;
            // 
            // Subject
            // 
            this.Subject.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Subject.HeaderText = "Subject";
            this.Subject.MinimumWidth = 6;
            this.Subject.Name = "Subject";
            // 
            // SearchHashtagForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(975, 685);
            this.Controls.Add(this.dgvResult);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.flpHashtags);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtSearch);
            this.Name = "SearchHashtagForm";
            this.Text = "SearchHashtagForm";
            this.Load += new System.EventHandler(this.SearchHashtagForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.FlowLayoutPanel flpHashtags;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgvResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Subject;
    }
}