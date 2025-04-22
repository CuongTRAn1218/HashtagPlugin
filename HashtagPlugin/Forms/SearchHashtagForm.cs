using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Outlook;
using Outlook = Microsoft.Office.Interop.Outlook;
using HashtagPlugin.Service;
using Exception = System.Exception;
namespace HashtagPlugin.Forms
{
    public partial class SearchHashtagForm : Form
    {
        private readonly List<string> hashtags = HashtagService.loadHashtags();
        private ListBox suggestBox;

        public SearchHashtagForm()
        {
            InitializeComponent();
            suggestBox = new ListBox
            {
                Visible = false,
                Width = txtSearch.Width,
                Location = new Point(txtSearch.Location.X, txtSearch.Location.Y + txtSearch.Height)
            };
            this.Controls.Add(suggestBox);
            suggestBox.Click += SuggestBox_Click;
            suggestBox.KeyDown += SuggestBox_KeyDown;

        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string text = txtSearch.Text;
            var suggestions = hashtags.Where(h => h.Contains(text)).ToList();
            if (suggestions.Count > 0)
            {
                suggestBox.DataSource = suggestions;
                suggestBox.Show();
                suggestBox.BringToFront();
            }
            else
            {
                suggestBox.Hide();
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
            flpHashtags.Controls.Remove(button);
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && suggestBox.Visible)
            {
                suggestBox.Focus();
                if (suggestBox.Items.Count > 0)
                    suggestBox.SelectedIndex = 0;

                e.Handled = true;
            }
        }

        private void SuggestBox_Click(object sender, EventArgs e)
        {
            if (suggestBox.SelectedItem != null)
            {
                txtSearch.Text = suggestBox.SelectedItem.ToString();
                suggestBox.Hide();
                txtSearch.Focus();
            }
        }

        private void SuggestBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && suggestBox.SelectedItem != null)
            {
                txtSearch.Text = suggestBox.SelectedItem.ToString();
                suggestBox.Hide();
                txtSearch.Focus();
                e.Handled = true;
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string tag = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(tag) && tag.StartsWith("#") && !GetCurrentTags().Contains(tag))
            {
                var tagButton = createHashtag(tag);
                flpHashtags.Controls.Add(tagButton);
                txtSearch.Clear();
                suggestBox.Hide();
            }
        }
        private List<string> GetCurrentTags()
        {
            return flpHashtags.Controls
                .OfType<Button>()
                .Select(b => b.Tag.ToString())
                .ToList();
        }
        private void SearchHashtagForm_Load(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var outlookApp = Globals.ThisAddIn.Application;
            List<string> tags = GetCurrentTags();
            List<string> entryIds = HashtagService.SearchItemsByTags(tags);
            Dictionary<string, ItemInfo> search = HashtagService.loadAllItemHashtags(); 
            dgvResult.Rows.Clear();
            foreach (string entryId in entryIds)
            {
                string type = string.Empty;
                string subject = string.Empty;

                try
                {
                    object item = outlookApp.Session.GetItemFromID(entryId);

                    if (search.ContainsKey(entryId))
                    {
                        var itemInfo = search[entryId];
                        type = itemInfo.Type;

                        if (item is Outlook.MailItem mail)
                        {
                            subject = $"{mail.Subject} From: {mail.SenderName} Received: {mail.ReceivedTime:g}";
                        }
                        else if (item is Outlook.ContactItem contact)
                        {
                            subject = $"{contact.FullName} Email: {contact.Email1Address}";
                        }
                        else if (item is Outlook.AppointmentItem appt)
                        {
                            subject = $"{appt.Subject} Start: {appt.Start:g} Location: {appt.Location}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    subject = $"Error loading item: {ex.Message}";
                }
     
                var rowIndex = dgvResult.Rows.Add(type, subject);
                dgvResult.Rows[rowIndex].Tag = entryId;
            }
        }

        private void dgvResult_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                
                string type = dgvResult.Rows[e.RowIndex].Cells[0].Value.ToString();
                string subject = dgvResult.Rows[e.RowIndex].Cells[1].Value.ToString();
                string entryId = dgvResult.Rows[e.RowIndex].Tag.ToString();
                OpenOutlookItem(entryId);
            }
        }
        private void OpenOutlookItem(string entryId)
        {
            try
            {
                var item = Globals.ThisAddIn.Application.Session.GetItemFromID(entryId);
                item?.Display();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open item: " + ex.Message);
            }
        }
    }
}
