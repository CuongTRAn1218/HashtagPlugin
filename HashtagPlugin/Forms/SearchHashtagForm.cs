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
using HashtagPlugin.Service;
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
    }
}
