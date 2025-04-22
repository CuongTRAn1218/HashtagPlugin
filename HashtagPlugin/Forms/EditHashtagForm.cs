using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;
using HashtagPlugin.Service;
namespace HashtagPlugin.Forms
{
    public partial class EditHashtagForm : Form
    {
        private object outlookItem;
        private string originalContent;

        public EditHashtagForm(object item)
        {
            InitializeComponent();
            this.outlookItem = item;

            if (item is Outlook.MailItem mail)
            {
                this.originalContent = mail.Body;
                loadHashtags(mail.EntryID);
            }
            else if (item is Outlook.AppointmentItem appointment)
            {
                this.originalContent = appointment.Body;
                loadHashtags(appointment.EntryID);
            }
            else if (item is Outlook.ContactItem contact)
            {
                this.originalContent = contact.Body;
                loadHashtags(contact.EntryID);
            }
            else
            {
                throw new ArgumentException("Unsupported Outlook item type.");
            }
        }

        private void loadHashtags(string itemId)
        {
            List<string> hashtags = HashtagService.loadItemHashtags(itemId);
            foreach (string hashtag in hashtags)
                {
                    var tagButton = createHashtag(hashtag);
                    flpHashtags.Controls.Add(tagButton);
                }
        }
        private Button createHashtag(string tag)
        {
            var tagButton = new Button();
            tagButton.Text = $"{tag} ✕";
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
            string itemId = null;

            if (MessageBox.Show($"Are you sure you want to remove the tag {tag}?", "Confirm Removal", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                HashtagService.removeItemHashtag(outlookItem, tag);
                flpHashtags.Controls.Remove(button);

            }
        }

        private void ReloadForm()
        {
            Form freshForm = new EditHashtagForm(outlookItem);
            this.Hide();
            freshForm.StartPosition = FormStartPosition.Manual;
            freshForm.Location = this.Location;
            this.Close();
            freshForm.Show();
        }

        private void btnAddHashtag_Click(object sender, EventArgs e)
        {
            originalContent = HashtagService.getItemBody(outlookItem);
            var form = new AddHashtagForm();
            form.ShowDialog();
            ReloadForm();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Save changes?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                HashtagService.saveItem(outlookItem);
                ReloadForm();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Cancel changes made?", "Cancel changes", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (MessageBox.Show("Cancel changes made?", "Cancel changes", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    HashtagService.setItemBody(outlookItem, originalContent);
                    HashtagService.saveItem(outlookItem);
                    ReloadForm();
                }
            }
        }

        private void EditHashtagForm_Load(object sender, EventArgs e)
        {

        }
    }
}
