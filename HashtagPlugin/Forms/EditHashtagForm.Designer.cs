namespace HashtagPlugin.Forms
{
    partial class EditHashtagForm
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
            this.flpHashtags = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddHashtag = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // flpHashtags
            // 
            this.flpHashtags.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpHashtags.Location = new System.Drawing.Point(12, 39);
            this.flpHashtags.Name = "flpHashtags";
            this.flpHashtags.Size = new System.Drawing.Size(369, 474);
            this.flpHashtags.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "Hashtags";
            // 
            // btnAddHashtag
            // 
            this.btnAddHashtag.Location = new System.Drawing.Point(405, 354);
            this.btnAddHashtag.Name = "btnAddHashtag";
            this.btnAddHashtag.Size = new System.Drawing.Size(128, 41);
            this.btnAddHashtag.TabIndex = 2;
            this.btnAddHashtag.Text = "Add Hashtag";
            this.btnAddHashtag.UseVisualStyleBackColor = true;
            this.btnAddHashtag.Click += new System.EventHandler(this.btnAddHashtag_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(405, 472);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(128, 41);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(405, 412);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(128, 41);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // EditHashtagForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 525);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnAddHashtag);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.flpHashtags);
            this.Name = "EditHashtagForm";
            this.Text = "Edit Hashtag";
            this.Load += new System.EventHandler(this.EditHashtagForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpHashtags;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddHashtag;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}