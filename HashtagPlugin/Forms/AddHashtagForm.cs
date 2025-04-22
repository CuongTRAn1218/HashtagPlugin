using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HashtagPlugin.Service;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Drawing;

namespace HashtagPlugin.Forms
{
    public partial class AddHashtagForm : Form
    {
        public bool showForm = true;
        private static string entryId = null;
        public AddHashtagForm()
        {
            InitializeComponent();
            loadExistingHashtags();
            try
            {
                var explorer = Globals.ThisAddIn.Application.ActiveExplorer();
                object item = explorer.Selection[1];
                string itemSubject = null;
                string itemBody = null;
                if (item is Outlook.MailItem mail)
                {
                    this.Text = "Add Hashtag for Mail";
                    entryId = mail.EntryID;
                    itemSubject = mail.Subject;
                    itemBody = mail.Body;
                }
                else if (item is Outlook.AppointmentItem appointment)
                {
                    this.Text = "Add Hashtag for Appointment";
                    entryId = appointment.EntryID;
                    itemSubject = appointment.Subject;
                    itemBody = appointment.Body;
                }
                else if (item is Outlook.ContactItem contact)
                {
                    this.Text = "Add Hashtag for Contact";
                    entryId = contact.EntryID;
                    itemSubject = contact.FullName;
                    itemBody = contact.Body;
                }
                this.lblItemHashtag.Text = string.Join(" ",HashtagService.loadItemHashtags(entryId));
                getSuggestions(itemSubject+"\n"+itemBody);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to determine item type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                showForm = false;
            }
            
        }
        private void loadExistingHashtags()
        {
            List<string> hashtags = HashtagService.loadHashtags();
            cbHashtags.Items.Clear();
            cbHashtags.Items.AddRange(hashtags.ToArray());

        }


        private void btnAddHashtag_Click(object sender, EventArgs e)
        {
            string hashtag = null;
            if (rbCreateNew.Checked)
            {
                hashtag = txtNewHashtag.Text;
                if (!hashtag.StartsWith("#"))
                {
                    hashtag = "#"+hashtag;
                    if(HashtagService.checkHashtag(hashtag))
                    {
                        MessageBox.Show("Hashtag already exists", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                }
            }
            else if(rbChooseHashtag.Checked) { 
                hashtag = cbHashtags.SelectedItem.ToString();
            }
            string checkValid = hashtag.Substring(1);
            if (string.IsNullOrWhiteSpace(checkValid))
            {
                MessageBox.Show("Please enter new hashtag or choose existing hashtag", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var explorer = Globals.ThisAddIn.Application.ActiveExplorer();
            if (explorer.Selection.Count > 0)
            {
                object item = explorer.Selection[1];
                HashtagService.addHashtag(hashtag);
                HashtagService.addItemHashtag(item, hashtag);
                MessageBox.Show("Added hashtag: " + hashtag);
                ReloadForm();
            }
        }



        private void rbCreateNew_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCreateNew.Checked)
            {
                txtNewHashtag.Enabled = true;
                cbHashtags.Text = "";
                cbHashtags.Enabled = false;
            }
            else
            {
                txtNewHashtag.Clear();
                txtNewHashtag.Enabled = false;
                cbHashtags.Enabled = true;
                cbHashtags.Text = "#";
            }
        }


        private Button createHashtag(string tag)
        {
            var tagButton = new Button();
            tagButton.Text = $"{tag}";
            tagButton.Font = new Font("Arial", 10);
            tagButton.Tag = tag;
            tagButton.BackColor = Color.LightGray;
            tagButton.FlatStyle = FlatStyle.Flat;
            tagButton.FlatAppearance.BorderSize = 0;
            tagButton.Padding = new Padding(5);
            tagButton.Margin = new Padding(5);
            tagButton.AutoSize = true;
            tagButton.Click += TagButton_Click;
            return tagButton;
        }
        private void TagButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            var tag = button?.Tag.ToString();
            if(MessageBox.Show($"Add tag {tag}?", "Confirm Adding", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var explorer = Globals.ThisAddIn.Application.ActiveExplorer();
                if (explorer.Selection.Count > 0)
                {
                    object item = explorer.Selection[1];
                    HashtagService.addHashtag(tag);
                    HashtagService.addItemHashtag(item, tag);
                    MessageBox.Show("Added hashtag: " + tag);
                    flpSuggesttion.Controls.Remove(button);
                    ReloadForm();
                }
            }
        }
        private async void getSuggestions(string body)
        {

            try
            {
                List<string> hashtags = await HashtagService.GenerateHashtagsFromOllamac(body,HashtagService.loadItemHashtags(entryId));
                flpSuggesttion.Controls.Clear();
                foreach (string hashtag in hashtags)
                {
                    var tagButton = createHashtag(hashtag);
                    flpSuggesttion.Controls.Add(tagButton);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating hashtags:\n" + ex.Message);
            }
        }

        private void ReloadForm()
        {
            Form freshForm = new AddHashtagForm();
            this.Hide();
            freshForm.StartPosition = FormStartPosition.Manual;
            freshForm.Location = this.Location;
            this.Close();
            freshForm.Show();
        }
    }
}
