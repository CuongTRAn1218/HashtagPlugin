using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HashtagPlugin.Service
{
    public static class OllamaService
    {
        public static bool IsOllamaInstalled()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c ollama --version",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                process.WaitForExit();
                return process.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }

        public static void InstallOllama(Action<string> onProgress, Action onComplete, Action<string> onError)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string installerUrl = "https://ollama.com/download/OllamaSetup.exe";
                string installerPath = Path.Combine(Path.GetTempPath(), "OllamaSetup.exe");

                using (var client = new WebClient())
                {
                    client.DownloadProgressChanged += (sender, e) =>
                    {
                        onProgress?.Invoke($"Downloading Ollama: {e.ProgressPercentage}% complete...");
                    };

                    client.DownloadFileCompleted += (sender, e) =>
                    {
                        if (e.Error != null)
                        {
                            onError?.Invoke($"Download failed: {e.Error.Message}");
                            return;
                        }

                        try
                        {
                            var installerProcess = Process.Start(installerPath);
                            installerProcess?.WaitForExit();
                            onComplete?.Invoke();
                        }
                        catch (Exception ex)
                        {
                            onError?.Invoke($"Installer failed: {ex.Message}");
                        }
                    };

                    client.DownloadFileAsync(new Uri(installerUrl), installerPath);
                }
            }
            catch (WebException webEx)
            {
                onError?.Invoke($"Network error: {webEx.Message}");
            }
            catch (Exception ex)
            {
                onError?.Invoke($"Unexpected error: {ex.Message}");
            }
        }

        public static async Task PullGemma3ModelAsync(Action<string> onProgress, Action onComplete, Action<string> onError, Action onRefreshUI = null)
        {
            try
            {
                await Task.Run(() =>
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/c ollama pull gemma3",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (var process = Process.Start(startInfo))
                    {
                        process.OutputDataReceived += (sender, e) =>
                        {
                            if (e.Data != null && e.Data.Contains("Downloading"))
                            {
                                string percent = ExtractPercentage(e.Data);
                                if (percent != null)
                                    onProgress?.Invoke($"Pulling Gemma3: {percent}%");
                            }
                        };

                        process.BeginOutputReadLine();
                        process.WaitForExit();
                    }
                });
                onRefreshUI?.Invoke();
                onComplete?.Invoke();
            }
            catch (Exception ex)
            {
                onError?.Invoke($"Failed during Gemma3 pull: {ex.Message}");
            }
        }

        public static async Task EnsureGemma3RunningAsync(Action onStarted = null, Action<string> onError = null)
        {
            try
            {
                if (!IsGemma3Running())
                {
                    await Task.Run(() =>
                    {
                        var runInfo = new ProcessStartInfo
                        {
                            FileName = "ollama",
                            Arguments = "run gemma3",
                            RedirectStandardOutput = false,
                            RedirectStandardError = false,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };
                        Process.Start(runInfo);
                    });

                    onStarted?.Invoke();
                }
            }
            catch (Exception ex)
            {
                onError?.Invoke($"Failed to run Gemma3: {ex.Message}");
            }
        }

        private static bool IsGemma3Running()
        {
            try
            {
                var processes = Process.GetProcessesByName("ollama");
                foreach (var process in processes)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(process.MainWindowTitle) &&
                            process.MainWindowTitle.ToLower().Contains("gemma3"))
                        {
                            return true;
                        }
                    }
                    catch
                    {
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private static string ExtractPercentage(string output)
        {
            int startIndex = output.IndexOf('(') + 1;
            int endIndex = output.IndexOf('%');
            if (startIndex > 0 && endIndex > startIndex)
            {
                return output.Substring(startIndex, endIndex - startIndex);
            }
            return null;
        }
    }
}
