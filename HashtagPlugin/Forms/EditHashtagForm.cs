using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Office.Interop.Outlook;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace HashtagPlugin.Forms
{
    public partial class EditHashtagForm : Form
    {
        private Outlook.MailItem mailItem;
        private Outlook.AppointmentItem appointmentItem;
        private Outlook.ContactItem contactItem;
        private string originalContent;

        public EditHashtagForm(Outlook.MailItem mailItem)
        {
            InitializeComponent();
            this.mailItem = mailItem;
            this.originalContent = mailItem.Body;
            loadHashtags(mailItem.Body);
        }

        public EditHashtagForm(Outlook.AppointmentItem appointmentItem)
        {
            InitializeComponent();
            this.appointmentItem = appointmentItem;
            this.originalContent = appointmentItem.Body;
            loadHashtags(appointmentItem.Body);
        }

        public EditHashtagForm(Outlook.ContactItem contactItem)
        {
            InitializeComponent();
            this.contactItem = contactItem;
            this.originalContent = contactItem.Body;  // Contact's notes field
            loadHashtags(contactItem.Body);
        }

        private void loadHashtags(string content)
        {
            List<string> hashtags = extractHashtags(content);
            foreach (string hashtag in hashtags)
            {
                var tagButton = createHashtag(hashtag);
                flpHashtags.Controls.Add(tagButton);
            }
        }

        private List<string> extractHashtags(string content)
        {
            Regex regex = new Regex(@"#\w+");
            MatchCollection matches = regex.Matches(content);
            List<string> hashtags = new List<string>();
            foreach (Match match in matches)
            {
                hashtags.Add(match.Value);
            }
            return hashtags;
        }

        private Button createHashtag(string tag)
        {
            var tagButton = new Button();
            tagButton.Text = $"{tag} X";
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

            if (MessageBox.Show($"Are you sure you want to remove the tag {tag}?", "Confirm Removal", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (mailItem != null)
                    this.mailItem.Body = RemoveTagFromBody(this.mailItem.Body, tag);
                else if (appointmentItem != null)
                    this.appointmentItem.Body = RemoveTagFromBody(this.appointmentItem.Body, tag);
                else if (contactItem != null)
                    this.contactItem.Body = RemoveTagFromBody(this.contactItem.Body, tag);

                flpHashtags.Controls.Remove(button);
            }
        }

        private string RemoveTagFromBody(string body, string tagToRemove)
        {
            var tags = body.Split(' ').Where(t => t != tagToRemove).ToList();
            return string.Join(" ", tags);
        }

        private void ReloadForm()
        {
            Form freshForm = null;

            if (mailItem != null)
                freshForm = new EditHashtagForm(mailItem);
            else if (appointmentItem != null)
                freshForm = new EditHashtagForm(appointmentItem);
            else if (contactItem != null)
                freshForm = new EditHashtagForm(contactItem);

            this.Hide();
            freshForm.StartPosition = FormStartPosition.Manual;
            freshForm.Location = this.Location;
            this.Close();
            freshForm.Show();
        }

        private void btnAddHashtag_Click(object sender, EventArgs e)
        {
            this.originalContent = this.mailItem?.Body ?? this.appointmentItem?.Body ?? this.contactItem?.Body;
            var form = new AddHashtagForm();
            form.ShowDialog();
            ReloadForm();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Save changes?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (mailItem != null)
                    this.mailItem.Save();
                else if (appointmentItem != null)
                    this.appointmentItem.Save();
                else if (contactItem != null)
                    this.contactItem.Save();

                ReloadForm();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Cancel changes made?", "Cancel changes", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (mailItem != null)
                    this.mailItem.Body = originalContent;
                else if (appointmentItem != null)
                    this.appointmentItem.Body = originalContent;
                else if (contactItem != null)
                    this.contactItem.Body = originalContent;

                if (mailItem != null)
                    this.mailItem.Save();
                else if (appointmentItem != null)
                    this.appointmentItem.Save();
                else if (contactItem != null)
                    this.contactItem.Save();

                ReloadForm();
            }
        }
    }
}
