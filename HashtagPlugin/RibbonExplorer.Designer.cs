namespace HashtagPlugin
{
    partial class RibbonExplorer : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public RibbonExplorer()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.btnAddHashtag = this.Factory.CreateRibbonButton();
            this.btnSearch = this.Factory.CreateRibbonButton();
            this.btnViewHashtags = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.group1);
            this.tab1.Label = "TabAddIns";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.btnAddHashtag);
            this.group1.Items.Add(this.btnSearch);
            this.group1.Items.Add(this.btnViewHashtags);
            this.group1.Label = "Hashtag Tools";
            this.group1.Name = "group1";
            // 
            // btnAddHashtag
            // 
            this.btnAddHashtag.Label = "Add Hashtag";
            this.btnAddHashtag.Name = "btnAddHashtag";
            this.btnAddHashtag.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnAddHashtag_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Label = "Search by Hashtags";
            this.btnSearch.Name = "btnSearch";
            // 
            // btnViewHashtags
            // 
            this.btnViewHashtags.Label = "View Hashtags";
            this.btnViewHashtags.Name = "btnViewHashtags";
            this.btnViewHashtags.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnViewHashtags_Click);
            // 
            // RibbonExplorer
            // 
            this.Name = "RibbonExplorer";
            this.RibbonType = "Microsoft.Outlook.Explorer";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.RibbonMailExplorer_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnSearch;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnViewHashtags;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnAddHashtag;
    }

    partial class ThisRibbonCollection
    {
        internal RibbonExplorer RibbonMailExplorer
        {
            get { return this.GetRibbon<RibbonExplorer>(); }
        }
    }
}
