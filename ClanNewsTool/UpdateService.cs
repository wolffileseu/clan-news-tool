using System.Diagnostics;
using System.Reflection;

namespace ClanNewsTool
{
    public class UpdateService
    {
        public static string CurrentVersion =>
            Assembly.GetExecutingAssembly()
                .GetName().Version?.ToString(3) ?? "1.0.0";

        public static async Task CheckAndUpdateAsync(bool silent = false)
        {
            try
            {
                var versionInfo = await ApiService.GetVersionAsync();
                if (versionInfo == null) return;

                if (!IsNewerVersion(versionInfo.Version, CurrentVersion))
                {
                    if (!silent)
                        MessageBox.Show("Du hast bereits die neueste Version!",
                            "Kein Update", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    return;
                }

                var msg = $"Neue Version verfügbar: {versionInfo.Version}\n" +
                          $"Aktuelle Version: {CurrentVersion}\n\n" +
                          $"Änderungen:\n{versionInfo.Changelog}\n\n" +
                          $"Jetzt aktualisieren?";

                var result = versionInfo.ForceUpdate
                    ? DialogResult.Yes
                    : MessageBox.Show(msg, "Update verfügbar",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes && versionInfo.DownloadUrl != null)
                {
                    await DownloadAndInstallAsync(versionInfo.DownloadUrl);
                }
            }
            catch
            {
                // Kein Update verfügbar oder kein Internet – still ignorieren
            }
        }

        private static bool IsNewerVersion(string remote, string local)
        {
            if (!Version.TryParse(remote, out var r)) return false;
            if (!Version.TryParse(local, out var l)) return false;
            return r > l;
        }

        private static async Task DownloadAndInstallAsync(string downloadUrl)
        {
            var tempPath = Path.Combine(Path.GetTempPath(), "ClanNewsTool_update.exe");

            using var progress = new ProgressForm("Update wird heruntergeladen...");
            progress.Show();
            Application.DoEvents();

            try
            {
                using var client = new HttpClient();
                var bytes = await client.GetByteArrayAsync(downloadUrl);
                await File.WriteAllBytesAsync(tempPath, bytes);
                progress.Close();

                // Updater-Script erstellen und ausführen
                var currentExe = Process.GetCurrentProcess().MainModule?.FileName ?? "";
                var scriptPath = Path.Combine(Path.GetTempPath(), "update.bat");
                await File.WriteAllTextAsync(scriptPath,
                    $"@echo off\n" +
                    $"timeout /t 2 /nobreak >nul\n" +
                    $"copy /Y \"{tempPath}\" \"{currentExe}\"\n" +
                    $"start \"\" \"{currentExe}\"\n" +
                    $"del \"{tempPath}\"\n" +
                    $"del \"%~f0\"\n");

                Process.Start(new ProcessStartInfo
                {
                    FileName = scriptPath,
                    CreateNoWindow = true,
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                });

                Application.Exit();
            }
            catch (Exception ex)
            {
                progress.Close();
                MessageBox.Show($"Update fehlgeschlagen: {ex.Message}",
                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public class ProgressForm : Form
    {
        public ProgressForm(string message)
        {
            Text = "Clan News Tool";
            Size = new Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ControlBox = false;

            var label = new Label
            {
                Text = message,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(label);
        }
    }
}