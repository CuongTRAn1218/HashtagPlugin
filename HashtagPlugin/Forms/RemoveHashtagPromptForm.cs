using System;
using System.Windows.Forms;

namespace HashtagPlugin.Forms
{
    public partial class RemoveHashtagPromptForm : Form
    {
        public enum RemoveHashtagChoice
        {
            Remove,
            Cancel,
            ViewItems
        }
        private string hashtag;
        private int count;
        private string usedIn;
        public RemoveHashtagChoice UserChoice { get; private set; } = RemoveHashtagChoice.Cancel ;
        public RemoveHashtagPromptForm(string hashtag, int count, string usedIn)
        {
            InitializeComponent();
            this.hashtag = hashtag;
            this.count = count;
            this.usedIn = usedIn;
        }

        private void RemoveHashtagPromptForm_Load(object sender, EventArgs e)
        {
            lbPrompt.Text = $"This hashtag '{hashtag}' is used in {count} items: \n\n" +
                $"{usedIn}.\n\n" +
                "Do you want to remove it?\nRemoving it will also delete it from these items";
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            UserChoice = RemoveHashtagChoice.Remove;
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            UserChoice = RemoveHashtagChoice.Cancel;
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnViewItems_Click(object sender, EventArgs e)
        {
            UserChoice = RemoveHashtagChoice.ViewItems;
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
