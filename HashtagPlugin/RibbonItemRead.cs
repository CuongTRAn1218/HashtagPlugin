using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HashtagPlugin.Forms;
using Microsoft.Office.Tools.Ribbon;
using Outlook = Microsoft.Office.Interop.Outlook;
namespace HashtagPlugin
{
    public partial class RibbonItemRead
    {
        private void RibbonMailRead_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void btnEditHashtags_Click(object sender, RibbonControlEventArgs e)
        {

            Outlook.Explorer explorer = Globals.ThisAddIn.Application.ActiveExplorer();

            if (explorer.Selection.Count > 0)
            {
                object selectedItem = explorer.Selection[1];
                if (selectedItem is Outlook.MailItem mailItem)
                {
                    var form = new EditHashtagForm(mailItem);
                    form.ShowDialog();
                }
                else if (selectedItem is Outlook.AppointmentItem appointmentItem)
                {
                    var form = new EditHashtagForm(appointmentItem);
                    form.ShowDialog();
                }
                else if (selectedItem is Outlook.ContactItem contactItem)
                {
                    var form = new EditHashtagForm(contactItem);
                    form.ShowDialog();
                }else if(selectedItem is Outlook.TaskItem taskItem)
                {
                    var form = new EditHashtagForm(taskItem);
                    form.ShowDialog();
                }
                else if (selectedItem is Outlook.PostItem postItem)
                {
                    var form = new EditHashtagForm(postItem);
                    form.ShowDialog();
                }
                else if (selectedItem is Outlook.NoteItem noteItem)
                {
                    var form = new EditHashtagForm(noteItem);
                    form.ShowDialog();
                }
                else if (selectedItem is Outlook.MeetingItem meetingItem)
                {
                    var form = new EditHashtagForm(meetingItem);
                    form.ShowDialog();
                }

                else
                {
                    MessageBox.Show("No item selected. Please select an item to edit tags.");
                }
            }
        }

        private void btnAddHashtag_Click(object sender, RibbonControlEventArgs e)
        {
            using (var form = new AddHashtagForm())
            {
                if (form.showForm)
                {
                    form.ShowDialog();
                }

            }
        }
    }
}
