using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HashtagPlugin.Service;
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
                HashtagService.addItemHashtag(item, hashtag);
                MessageBox.Show("Added hashtag: " + hashtag);
                
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
    }
}
