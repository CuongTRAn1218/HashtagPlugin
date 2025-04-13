using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace HashtagPlugin.Forms
{
    public partial class AddHashtagForm : Form
    {
        public bool showForm = true;
        public AddHashtagForm()
        {
            InitializeComponent();
            loadExistingHashtags();
            try
            {
                var explorer = Globals.ThisAddIn.Application.ActiveExplorer();
                object item = explorer.Selection[1];
                if (item is Outlook.MailItem mail)
                {
                    this.Text = "Add Hashtag for Mail";
                }
                else if (item is Outlook.AppointmentItem appointment)
                {
                    this.Text = "Add Hashtag for Appointment";
                }
                else if (item is Outlook.ContactItem contact)
                {
                    this.Text = "Add Hashtag for Contact";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to determine item type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                showForm = false;
            }
            
        }
        private void loadExistingHashtags()
        {
            List<string> hashtags = HashtagStorage.loagHashtags();
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
                if (item is Outlook.MailItem mail)
                {
                    mail.Body = AppendHashtag(mail.Body, hashtag);
                    mail.Save();
                   

                }
                else if (item is Outlook.AppointmentItem appointment)
                {
                    appointment.Body = AppendHashtag(appointment.Body, hashtag);
                    appointment.Save();
                }
                else if (item is Outlook.ContactItem contact)
                {
                    
                    contact.Body = AppendHashtag(contact.Body, hashtag);
                    contact.Save();
                }
                HashtagStorage.addHashtag(hashtag);
                MessageBox.Show("Added hashtag: " + hashtag);
                this.Close();
            }
        }
        private string AppendHashtag(string body, string newTag)
        {
            const string tagSeparator = "-----";
            var lines = body.TrimEnd().Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

            int tagLineIndex = lines.FindLastIndex(line => line.StartsWith(tagSeparator));
            List<string> tags;

            if (tagLineIndex >= 0 && tagLineIndex < lines.Count - 1)
            {
                string tagsLine = lines[tagLineIndex + 1];
                tags = tagsLine.Split(' ')
                              .Where(t => !string.IsNullOrWhiteSpace(t))
                              .ToList();

                if (!tags.Contains(newTag))
                {
                    tags.Add(newTag);
                    lines[tagLineIndex + 1] = string.Join(" ", tags);
                }
            }

            else if (tagLineIndex < 0)
            {
                lines.Add(tagSeparator);
                lines.Add(newTag);
            }
            else
            {
                lines.Add(newTag);
            }

            return string.Join("\r\n", lines);
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
    }
}
