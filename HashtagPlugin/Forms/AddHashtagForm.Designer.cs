namespace HashtagPlugin.Forms
{
    partial class AddHashtagForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbChooseHashtag = new System.Windows.Forms.RadioButton();
            this.rbCreateNew = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNewHashtag = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbHashtags = new System.Windows.Forms.ComboBox();
            this.btnAddHashtag = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.flpSuggesttion = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.lblItemHashtag = new System.Windows.Forms.Label();
            this.lblPlace = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbChooseHashtag);
            this.groupBox1.Controls.Add(this.rbCreateNew);
            this.groupBox1.Location = new System.Drawing.Point(43, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(370, 60);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose method";
            // 
            // rbChooseHashtag
            // 
            this.rbChooseHashtag.AutoSize = true;
            this.rbChooseHashtag.Location = new System.Drawing.Point(154, 21);
            this.rbChooseHashtag.Name = "rbChooseHashtag";
            this.rbChooseHashtag.Size = new System.Drawing.Size(210, 20);
            this.rbChooseHashtag.TabIndex = 1;
            this.rbChooseHashtag.TabStop = true;
            this.rbChooseHashtag.Text = "Choose from existing hashtags";
            this.rbChooseHashtag.UseVisualStyleBackColor = true;
            // 
            // rbCreateNew
            // 
            this.rbCreateNew.AutoSize = true;
            this.rbCreateNew.Location = new System.Drawing.Point(24, 22);
            this.rbCreateNew.Name = "rbCreateNew";
            this.rbCreateNew.Size = new System.Drawing.Size(95, 20);
            this.rbCreateNew.TabIndex = 0;
            this.rbCreateNew.TabStop = true;
            this.rbCreateNew.Text = "Create new";
            this.rbCreateNew.UseVisualStyleBackColor = true;
            this.rbCreateNew.CheckedChanged += new System.EventHandler(this.rbCreateNew_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "New Hashtag";
            // 
            // txtNewHashtag
            // 
            this.txtNewHashtag.Location = new System.Drawing.Point(137, 94);
            this.txtNewHashtag.Name = "txtNewHashtag";
            this.txtNewHashtag.Size = new System.Drawing.Size(276, 22);
            this.txtNewHashtag.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Hashtags";
            // 
            // cbHashtags
            // 
            this.cbHashtags.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbHashtags.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbHashtags.FormattingEnabled = true;
            this.cbHashtags.Location = new System.Drawing.Point(137, 138);
            this.cbHashtags.Name = "cbHashtags";
            this.cbHashtags.Size = new System.Drawing.Size(276, 24);
            this.cbHashtags.TabIndex = 4;
            // 
            // btnAddHashtag
            // 
            this.btnAddHashtag.Location = new System.Drawing.Point(151, 325);
            this.btnAddHashtag.Name = "btnAddHashtag";
            this.btnAddHashtag.Size = new System.Drawing.Size(116, 36);
            this.btnAddHashtag.TabIndex = 5;
            this.btnAddHashtag.Text = "Add Hashtag";
            this.btnAddHashtag.UseVisualStyleBackColor = true;
            this.btnAddHashtag.Click += new System.EventHandler(this.btnAddHashtag_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblPlace);
            this.groupBox2.Controls.Add(this.flpSuggesttion);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.lblItemHashtag);
            this.groupBox2.Location = new System.Drawing.Point(433, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(325, 348);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Hashtags";
            // 
            // flpSuggesttion
            // 
            this.flpSuggesttion.Location = new System.Drawing.Point(10, 125);
            this.flpSuggesttion.Name = "flpSuggesttion";
            this.flpSuggesttion.Size = new System.Drawing.Size(309, 217);
            this.flpSuggesttion.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 18);
            this.label3.TabIndex = 1;
            this.label3.Text = "Suggestions";
            // 
            // lblItemHashtag
            // 
            this.lblItemHashtag.AutoSize = true;
            this.lblItemHashtag.Location = new System.Drawing.Point(7, 24);
            this.lblItemHashtag.Name = "lblItemHashtag";
            this.lblItemHashtag.Size = new System.Drawing.Size(172, 16);
            this.lblItemHashtag.TabIndex = 0;
            this.lblItemHashtag.Text = "Item hashtags placeholders";
            // 
            // lblPlace
            // 
            this.lblPlace.AutoSize = true;
            this.lblPlace.Location = new System.Drawing.Point(113, 99);
            this.lblPlace.Name = "lblPlace";
            this.lblPlace.Size = new System.Drawing.Size(82, 16);
            this.lblPlace.TabIndex = 3;
            this.lblPlace.Text = "Generating...";
            // 
            // AddHashtagForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 373);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnAddHashtag);
            this.Controls.Add(this.cbHashtags);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNewHashtag);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Name = "AddHashtagForm";
            this.Text = "Add Hashtag";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbChooseHashtag;
        private System.Windows.Forms.RadioButton rbCreateNew;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNewHashtag;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbHashtags;
        private System.Windows.Forms.Button btnAddHashtag;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblItemHashtag;
        private System.Windows.Forms.FlowLayoutPanel flpSuggesttion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblPlace;
    }
}