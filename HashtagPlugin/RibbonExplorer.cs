using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HashtagPlugin.Forms;
using Microsoft.Office.Tools.Ribbon;

namespace HashtagPlugin
{
    public partial class RibbonExplorer
    {
        private void RibbonMailExplorer_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void btnAddHashtag_Click(object sender, RibbonControlEventArgs e)
        {
            using(var form = new AddHashtagForm())
            {
                if (form.showForm)
                {
                    form.ShowDialog();
                }

            }
        }

        private void btnViewHashtags_Click(object sender, RibbonControlEventArgs e)
        {
            var form = new ViewHashtagsForm();
            form.Show();
        }

        private void btnSearch_Click(object sender, RibbonControlEventArgs e)
        {
            var form = new SearchHashtagForm();
            form.Show();
        }
    }
}
