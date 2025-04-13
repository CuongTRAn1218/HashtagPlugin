namespace HashtagPlugin.Forms
{
    partial class ViewHashtagsForm
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
            this.dgvHashtags = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNewhashtag = new System.Windows.Forms.TextBox();
            this.btnAddHashtag = new System.Windows.Forms.Button();
            this.btnRemoveHashtag = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHashtags)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvHashtags
            // 
            this.dgvHashtags.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHashtags.Location = new System.Drawing.Point(13, 13);
            this.dgvHashtags.Name = "dgvHashtags";
            this.dgvHashtags.RowHeadersWidth = 51;
            this.dgvHashtags.RowTemplate.Height = 24;
            this.dgvHashtags.Size = new System.Drawing.Size(775, 340);
            this.dgvHashtags.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 367);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Create new hashtag";
            // 
            // txtNewhashtag
            // 
            this.txtNewhashtag.Location = new System.Drawing.Point(143, 364);
            this.txtNewhashtag.Name = "txtNewhashtag";
            this.txtNewhashtag.Size = new System.Drawing.Size(195, 22);
            this.txtNewhashtag.TabIndex = 2;
            // 
            // btnAddHashtag
            // 
            this.btnAddHashtag.Location = new System.Drawing.Point(344, 363);
            this.btnAddHashtag.Name = "btnAddHashtag";
            this.btnAddHashtag.Size = new System.Drawing.Size(75, 23);
            this.btnAddHashtag.TabIndex = 3;
            this.btnAddHashtag.Text = "Add";
            this.btnAddHashtag.UseVisualStyleBackColor = true;
            this.btnAddHashtag.Click += new System.EventHandler(this.btnAddHashtag_Click);
            // 
            // btnRemoveHashtag
            // 
            this.btnRemoveHashtag.Location = new System.Drawing.Point(713, 363);
            this.btnRemoveHashtag.Name = "btnRemoveHashtag";
            this.btnRemoveHashtag.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveHashtag.TabIndex = 4;
            this.btnRemoveHashtag.Text = "Remove";
            this.btnRemoveHashtag.UseVisualStyleBackColor = true;
            this.btnRemoveHashtag.Click += new System.EventHandler(this.btnRemoveHashtag_Click);
            // 
            // ViewHashtagsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 399);
            this.Controls.Add(this.btnRemoveHashtag);
            this.Controls.Add(this.btnAddHashtag);
            this.Controls.Add(this.txtNewhashtag);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvHashtags);
            this.Name = "ViewHashtagsForm";
            this.Text = "ViewHashtagsForm";
            this.Load += new System.EventHandler(this.ViewHashtagsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHashtags)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvHashtags;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNewhashtag;
        private System.Windows.Forms.Button btnAddHashtag;
        private System.Windows.Forms.Button btnRemoveHashtag;
    }
}