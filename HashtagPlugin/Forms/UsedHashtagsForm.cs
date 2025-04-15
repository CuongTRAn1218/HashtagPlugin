using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace HashtagPlugin.Forms
{
    public partial class UsedHashtagsForm : Form
    {
        private string selectedHashtag;
        public UsedHashtagsForm(string selectedhashtag)
        {
            InitializeComponent();
            this.selectedHashtag = selectedhashtag;
        }

        private void UsedHashtagsForm_Load(object sender, EventArgs e)
        {
            var outlookApp = Globals.ThisAddIn.Application;
            var itemHashtags = HashtagStorage.loadItemHashtags();

            var groupedItems = new Dictionary<string, List<(string id, object item)>>()
            {
                { "Mail", new List<(string, object)>() },
                { "Appointment", new List<(string, object)>() },
                { "Contact", new List<(string, object)>() }
            };

            foreach (var itemId in itemHashtags.Keys)
            {
                if (!itemHashtags[itemId].Contains(selectedHashtag)) continue;

                try
                {
                    var item = outlookApp.Session.GetItemFromID(itemId);
                    if (item is Outlook.MailItem)
                        groupedItems["Mail"].Add((itemId, item));
                    else if (item is Outlook.AppointmentItem)
                        groupedItems["Appointment"].Add((itemId, item));
                    else if (item is Outlook.ContactItem)
                        groupedItems["Contact"].Add((itemId, item));
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

            // Display groups
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
                Width = 400,
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
                displayText = $"\u2709 {mail.Subject}\nFrom: {mail.SenderName}\nReceived: {mail.ReceivedTime}";
            }
            else if (item is Outlook.AppointmentItem appt)
            {
                displayText = $"\u1F4C {appt.Subject}\nStart: {appt.Start:g}\nLocation: {appt.Location}";
            }
            else if (item is Outlook.ContactItem contact)
            {
                displayText = $"\u1F4D {contact.FullName}\nEmail: {contact.Email1Address}\nCompany: {contact.CompanyName}";
            }
            else
            {
                return null;
            }

            lbl.Text = displayText;
            panel.Controls.Add(lbl);

            panel.Click += (s, e) => OpenOutlookItem(entryID);
            foreach (Control ctl in panel.Controls)
            {
                ctl.Click += (s, e) => OpenOutlookItem(entryID);
            }

            return panel;
        }

        private void OpenOutlookItem(string entryID)
        {
            try
            {
                this.WindowState = FormWindowState.Minimized;
                var item = Globals.ThisAddIn.Application.Session.GetItemFromID(entryID);
                item?.Display();
                this.WindowState = FormWindowState.Normal;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open item: " + ex.Message);
            }
        }

    }
}
