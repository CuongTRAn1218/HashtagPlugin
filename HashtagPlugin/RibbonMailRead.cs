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
    public partial class RibbonMailRead
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
                }

                else
                {
                    MessageBox.Show("No item selected. Please select a mail item to edit tags.");
                }
            }
        }
    }
}
