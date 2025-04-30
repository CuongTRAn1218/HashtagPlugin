using HashtagPlugin.Forms;
using Microsoft.Office.Tools.Ribbon;

namespace HashtagPlugin
{
    public partial class RibbonExplorer
    {
        private void RibbonMailExplorer_Load(object sender, RibbonUIEventArgs e)
        {
            btnAddHashtag.Image = Properties.Resources.add;
            btnAddHashtag.ShowImage = true;
            btnAddHashtag.ShowLabel = true;

            btnViewHashtags.Image = Properties.Resources.view;
            btnViewHashtags.ShowImage = true;
            btnViewHashtags.ShowLabel = true;

            btnSearch.Image = Properties.Resources.search;
            btnSearch.ShowImage = true;
            btnSearch.ShowLabel = true;
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
