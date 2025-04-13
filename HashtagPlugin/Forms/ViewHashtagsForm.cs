using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static HashtagPlugin.Forms.RemoveHashtagPromptForm;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace HashtagPlugin.Forms
{
    
    public partial class ViewHashtagsForm : Form
    {
        private readonly Outlook.Application outlookApp;

        public ViewHashtagsForm()
        {
            InitializeComponent();
            dgvHashtags.Columns.Add("Hashtag", "Hashtag");
            dgvHashtags.Columns.Add("Count", "Count");
            dgvHashtags.Columns.Add("UsedIn", "Used In");
            dgvHashtags.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvHashtags.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvHashtags.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvHashtags.AllowUserToOrderColumns = true;
            dgvHashtags.SelectionChanged += dgvHashtags_SelectionChanged;
            outlookApp = new Outlook.Application();
        }


        private void loadHashtagsData()
        {
            var hashtags = HashtagStorage.loadHashtags();
            var itemHashtags = HashtagStorage.loadItemHashtags();
            var hashtagUsage = new Dictionary<string, (int count, Dictionary<string, int> itemCounts)>();

            foreach (var item in itemHashtags)
            {
                string itemType = getItemType(item.Key);
                foreach (var hashtag in item.Value)
                {
                    if (!hashtagUsage.ContainsKey(hashtag))
                    {
                        hashtagUsage[hashtag] = (0, new Dictionary<string, int>());
                    }
                    hashtagUsage[hashtag] = (hashtagUsage[hashtag].count + 1, hashtagUsage[hashtag].itemCounts);
                    if (hashtagUsage[hashtag].itemCounts.ContainsKey(itemType))
                    {
                        hashtagUsage[hashtag].itemCounts[itemType]++;
                    }
                    else
                    {
                        hashtagUsage[hashtag].itemCounts[itemType] = 1;
                    }

                }
            }
            foreach (var hashtag in hashtags)
            {
                if (!hashtagUsage.ContainsKey(hashtag))
                {
                    hashtagUsage[hashtag] = (0, new Dictionary<string, int>());
                }
            }
            dgvHashtags.Rows.Clear();

            foreach (var hashtagData in hashtagUsage)
            {
                string usedInText = hashtagData.Value.count == 0
                ? "Not Used" 
                : string.Join(", ", hashtagData.Value.itemCounts.Select(item => $"{item.Value} {item.Key}").ToArray());

                dgvHashtags.Rows.Add(hashtagData.Key, hashtagData.Value.count, usedInText);
            }

        }
        private string getItemType(string itemId)
        {
          object item = outlookApp.Session.GetItemFromID(itemId);
            if (item is Outlook.MailItem)
            {
                return "Email";
            }
            else if (item is Outlook.AppointmentItem)
            {
                return "Appointment";
            }
            else if (item is Outlook.ContactItem)
            {
                return "Contact";
            }
            else
            {
                return "Unknown";
            }
        }
        private void ViewHashtagsForm_Load(object sender, EventArgs e)
        {
            loadHashtagsData();
        }


        private void btnAddHashtag_Click(object sender, EventArgs e)
        {
            string hashtag = txtNewhashtag.Text;
            if (!string.IsNullOrWhiteSpace(hashtag))
            {
                if (!hashtag.StartsWith("#"))
                {
                    hashtag = "#" + hashtag;
                }
                if (HashtagStorage.addHashtag(hashtag))
                {
                    MessageBox.Show("Hashtag added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadHashtagsData();
                }
                else
                {
                    MessageBox.Show("Hashtag already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter a hashtag.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnRemoveHashtag_Click(object sender, EventArgs e)
        {
            if (dgvHashtags.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvHashtags.SelectedRows[0];

                string selectedHashtag = selectedRow.Cells["Hashtag"].Value.ToString();
                int count = int.Parse(selectedRow.Cells["Count"].Value.ToString());
                string usedIn = selectedRow.Cells["UsedIn"].Value.ToString();
                
                if (count > 0)
                {
                    var prompt = new RemoveHashtagPromptForm(selectedHashtag, count, usedIn);
                    var dialogResult = prompt.ShowDialog();
                    if (dialogResult == DialogResult.OK)
                    {
                        var choice = prompt.UserChoice;
                        if(choice == RemoveHashtagChoice.Cancel)
                        {
                            HashtagStorage.removeHashtag(selectedHashtag);
                            var itemHashtags = HashtagStorage.loadItemHashtags();
                            foreach (var item in itemHashtags)
                            {
                                if (item.Value.Contains(selectedHashtag))
                                {
                                    HashtagStorage.removeItemHashtag(item.Key, selectedHashtag);
                                }
                                object itemObj = outlookApp.Session.GetItemFromID(item.Key);
                                if (itemObj is Outlook.MailItem mail)
                                {
                                    mail.Body = RemoveTagFromBody(mail.Body, selectedHashtag);
                                    mail.Save();
                                }
                                else if (itemObj is Outlook.AppointmentItem appointment)
                                {
                                    appointment.Body = RemoveTagFromBody(appointment.Body, selectedHashtag);
                                    appointment.Save();
                                }
                                else if (itemObj is Outlook.ContactItem contact)
                                {
                                    contact.Body = RemoveTagFromBody(contact.Body, selectedHashtag);
                                    contact.Save();
                                }
                            }


                            MessageBox.Show($"Removed '{selectedHashtag}' successfully.");
                        }else if(choice == RemoveHashtagChoice.ViewItems)
                        {
                            var form = new UsedHashtagsForm(selectedHashtag);
                            form.Show();
                        }
                        else
                        {
                            return;
                        }
                        
                    }
                }
                else
                {
                    if (MessageBox.Show("This hashtag is not used. Are you sure you want to remove it?",
                                        "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        HashtagStorage.removeHashtag(selectedHashtag);
                        MessageBox.Show($"Removed '{selectedHashtag}' successfully.");
                    }
                }
                loadHashtagsData();
            }
            else
            {
                MessageBox.Show("Please select a hashtag to remove.");
            }
        }
        private string RemoveTagFromBody(string body, string tagToRemove)
        {
            const string delimiter = "-----";
            int delimiterIndex = body.IndexOf(delimiter);

            if (delimiterIndex != -1)
            {
                string before = body.Substring(0, delimiterIndex + delimiter.Length);
                string after = body.Substring(delimiterIndex + delimiter.Length).Trim();

                var tags = after.Split(' ').Where(t => !t.Equals(tagToRemove, StringComparison.OrdinalIgnoreCase)).ToList();
                string newAfter = string.Join(" ", tags);

                return before + Environment.NewLine + newAfter;
            }
            return body;
        }

        private void dgvHashtags_SelectionChanged(object sender, EventArgs e)
        {
            btnViewItems.Enabled = dgvHashtags.SelectedCells.Count > 0 || dgvHashtags.SelectedRows.Count > 0;
        }

        private void btnViewItems_Click(object sender, EventArgs e)
        {
            if (dgvHashtags.SelectedRows.Count > 0)
            {
                var selectedRow = dgvHashtags.SelectedRows[0];
                string selectedHashtag = selectedRow.Cells["Hashtag"].Value?.ToString();

                if (!string.IsNullOrEmpty(selectedHashtag))
                {
                    var form = new UsedHashtagsForm(selectedHashtag);
                    form.Show();
                }
            }
        }
    }
}
