using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;
using HashtagPlugin.Service;

namespace HashtagPlugin.Forms
{
    public partial class UsedHashtagsForm : Form
    {
        private string selectedHashtag;

        public UsedHashtagsForm(string selectedhashtag)
        {
            InitializeComponent();
            this.selectedHashtag = selectedhashtag;
            this.TopMost = true;
            flpItems.AutoScroll = true;
            flpItems.WrapContents = false;
            flpItems.FlowDirection = FlowDirection.TopDown;
        }

        private void UsedHashtagsForm_Load(object sender, EventArgs e)
        {
            var outlookApp = Globals.ThisAddIn.Application;
            var itemHashtags = HashtagService.loadAllItemHashtags();

            var groupedItems = new Dictionary<string, List<(string id, object item)>>()
            {
                { "Mail", new List<(string, object)>() },
                { "Appointment", new List<(string, object)>() },
                { "Contact", new List<(string, object)>() },
                { "Task", new List<(string, object)>() },
                { "Post", new List<(string, object)>() },
                { "Note", new List<(string, object)>() },

            };

            foreach (var kvp in itemHashtags)
            {
                string itemId = kvp.Key;
                var info = kvp.Value;

                if (info.Hashtags == null || !info.Hashtags.Contains(selectedHashtag))
                    continue;

                try
                {
                    var item = outlookApp.Session.GetItemFromID(itemId);
                    if (item is Outlook.MailItem)
                        groupedItems["Mail"].Add((itemId, item));
                    else if (item is Outlook.AppointmentItem)
                        groupedItems["Appointment"].Add((itemId, item));
                    else if (item is Outlook.ContactItem)
                        groupedItems["Contact"].Add((itemId, item));
                    else if (item is Outlook.TaskItem)
                        groupedItems["Task"].Add((itemId, item));
                    else if (item is Outlook.PostItem)
                        groupedItems["Post"].Add((itemId, item));
                    else if (item is Outlook.NoteItem)
                        groupedItems["Note"].Add((itemId, item));

                }
                catch (Exception ex)
                {
                    flpItems.Controls.Add(new Label
                    {
                        Text = $"Unable to load item: {ex.Message}",
                        ForeColor = Color.Red,
                        AutoSize = true
                    });
                }
            }

            // Display grouped items
            foreach (var group in groupedItems)
            {
                if (group.Value.Count == 0) continue;

                var header = new Label
                {
                    Text = $"{group.Key}:",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    AutoSize = true,
                    Padding = new Padding(5)
                };
                flpItems.Controls.Add(header);

                foreach (var (id, item) in group.Value)
                {
                    var panel = CreateItemPanel(item, id);
                    if (panel != null)
                        flpItems.Controls.Add(panel);
                }
            }
        }

        private Control CreateItemPanel(object item, string entryID)
        {
            Panel panel = new Panel
            {
                Width = flpItems.Width-2,
                Height = 80,
                BorderStyle = BorderStyle.FixedSingle,
                Tag = entryID,
                Margin = new Padding(5),
                Cursor = Cursors.Hand
                
            };

            Label lbl = new Label
            {
                AutoSize = true,
                Location = new Point(10, 10),
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };

            string displayText = "";

            if (item is Outlook.MailItem mail)
            {
                displayText = $"\u2709 {mail.Subject}\nFrom: {mail.SenderName}\nReceived: {mail.ReceivedTime:g}";
            }
            else if (item is Outlook.AppointmentItem appt)
            {
                displayText = $"\U0001F4C5 {appt.Subject}\nStart: {appt.Start:g}\nLocation: {appt.Location}";
            }
            else if (item is Outlook.ContactItem contact)
            {
                displayText = $"\U0001F464 {contact.FullName}\nEmail: {contact.Email1Address}";
            }
            else if (item is Outlook.TaskItem task)
            {
                displayText = $"\U0001F4CB {task.Subject}\nDue: {task.DueDate:g}";
            }
            else if (item is Outlook.PostItem post)
            {
                displayText = $"\U0001F4AC {post.Subject}\nPosted: {post.CreationTime:g}";
            }
            else if (item is Outlook.NoteItem note)
            {
                displayText = $"\U0001F4DD {note.Subject}\nCreated: {note.CreationTime:g}";
            }
            else
            {
                return null;
            }

            lbl.Text = displayText;
            panel.Controls.Add(lbl);

            // Attach click handler
            panel.Click += (s, e) => OpenOutlookItem(entryID);
            foreach (Control ctl in panel.Controls)
                ctl.Click += (s, e) => OpenOutlookItem(entryID);

            return panel;
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
