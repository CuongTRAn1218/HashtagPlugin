using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using HashtagPlugin.Service;
using Microsoft.Office.Interop.Outlook;
using Image = System.Drawing.Image;

namespace HashtagPlugin.Forms
{
    public partial class StartForm : Form
    {
        private Image img;
        private Point imageLocation = new Point(0, -99);

        public StartForm()
        {
            InitializeComponent();
            UpdateInstallationStatus();
            img = Image.FromFile("images/email.png");
            panel1.Paint += Panel1_Paint;
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            if (img != null)
            {
                e.Graphics.DrawImage(img, imageLocation);
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;

            await OllamaService.EnsureGemma3RunningAsync(
                onStarted: () =>
                {
                    lblGstatus.Invoke((System.Action)(() =>
                    {
                        lblGstatus.Text = "Gemma3 is now running.";
                        lblGstatus.ForeColor = Color.Green;
                    }));

                    // Only close after successful start
                    this.Invoke((System.Action)(() => this.Close()));
                },
                onError: (error) =>
                {
                    btnStart.Invoke((System.Action)(() => btnStart.Enabled = true));
                    lblGstatus.Invoke((System.Action)(() =>
                    {
                        lblGstatus.Text = "Failed to start Gemma3.";
                        lblGstatus.ForeColor = Color.Red;
                    }));
                    MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            );
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            OllamaService.InstallOllama(
                onProgress: (progress) =>
                {
                    lblDownload.Invoke((System.Action)(() =>
                    {
                        lblDownload.Text = progress;
                        lblDownload.ForeColor = Color.Green;
                        lblDownload.Visible = true;
                    }));
                },
                onComplete: async () =>
                {
                    MessageBox.Show("Ollama installed. Now pulling and running Gemma3 model...");
                    await PullGemmaModelAsync();
                },
                onError: (error) =>
                {
                    MessageBox.Show(error, "Installation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            );
        }

        private async Task PullGemmaModelAsync()
        {
            await OllamaService.PullGemma3ModelAsync(
                onProgress: (progress) =>
                {
                    lblGstatus.Invoke((System.Action)(() =>
                    {
                        lblGstatus.Text = progress;
                        lblGstatus.ForeColor = Color.Green;
                        lblGstatus.Visible = true;
                    }));
                },
                onComplete: () =>
                {
                    MessageBox.Show("Gemma3 model pulled and run successfully.");
                },
                onError: (error) =>
                {
                    MessageBox.Show(error, "Gemma3 Model Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                },
                onRefreshUI: () =>
                {
                    this.Invoke((System.Action)(() => UpdateInstallationStatus()));
                }
            );
        }

        private void UpdateInstallationStatus()
        {
            if (OllamaService.IsOllamaInstalled())
            {
                lblPlace.Text = "Installed";
                lblPlace.ForeColor = Color.Green;
                btnInstall.Enabled = false;
                lblDownload.Visible = true;
                lblDownload.Text = "Ollama Gemma3 model is installed";
                lblDownload.ForeColor = Color.Green;
                lblGstatus.Visible = false;
            }
            else
            {
                lblPlace.Text = "Not Installed";
                lblPlace.ForeColor = Color.Red;
                btnInstall.Enabled = true;
                lblDownload.Visible = false;
                lblGstatus.Visible = false;
            }
        }
    }
}
