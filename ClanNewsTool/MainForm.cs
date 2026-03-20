using System.Drawing;

namespace ClanNewsTool
{
    public partial class MainForm : Form
    {
        private ApiService? _api;
        private ClanInfo? _clan;

        private Panel _loginPanel = new();
        private TextBox _txtApiKey = new();
        private Button _btnLogin = new();
        private Label _lblLoginStatus = new();

        private Panel _mainPanel = new();
        private TabControl _tabs = new();
        private Label _lblClanInfo = new();

        public MainForm()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Font;
            Text = "Clan News Tool – Wolffiles.eu";
            MinimumSize = new Size(1200, 1000);
            Size = new Size(1200, 800);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;

            // App Icon aus eingebetteter Ressource
            try
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                using var stream = assembly.GetManifestResourceStream("ClanNewsTool.Resources.wolffiles.ico");
                if (stream != null)
                    this.Icon = new Icon(stream);
            }
            catch { }

            BuildLoginPanel();
            BuildMainPanel();

            _mainPanel.Visible = false;
            _loginPanel.BringToFront();
            _ = CheckUpdateOnStartup();
        }

        private async Task CheckUpdateOnStartup()
        {
            await UpdateService.CheckAndUpdateAsync(silent: true);
        }

        // ─── SETTINGS ────────────────────────────────────────────────────────
        private static string SettingsPath =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "ClanNewsTool",
                "settings.json"
            );

        private static string LoadApiKey()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    var json = File.ReadAllText(SettingsPath);
                    var doc = System.Text.Json.JsonDocument.Parse(json);
                    return doc.RootElement.GetProperty("ApiKey").GetString() ?? "";
                }
            }
            catch { }
            return "";
        }

        private static void SaveApiKey(string key)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);
                var json = System.Text.Json.JsonSerializer.Serialize(new { ApiKey = key });
                File.WriteAllText(SettingsPath, json);
            }
            catch { }
        }

        // ─── LOGIN PANEL ────────────────────────────────────────────────────
        private void BuildLoginPanel()
        {
            _loginPanel.Dock = DockStyle.Fill;
            Controls.Add(_loginPanel);

            var outer = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 9,
                ColumnCount = 3,
            };
            outer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));
            outer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80));
            outer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));
            outer.RowStyles.Add(new RowStyle(SizeType.Percent, 5));
            outer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            outer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            outer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            outer.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            outer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            outer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            outer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            outer.RowStyles.Add(new RowStyle(SizeType.Percent, 70));

            var logoPic = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                Height = 120,
                Margin = new Padding(40, 0, 40, 8)
            };
            Task.Run(() =>
            {
                try
                {
                    var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                    var resourceName = "ClanNewsTool.Resources.wolffiles_logo.png";
                    using var stream = assembly.GetManifestResourceStream(resourceName);
                    if (stream != null)
                    {
                        var img = Image.FromStream(stream);
                        logoPic.Invoke(() => logoPic.Image = img);
                    }
                }
                catch { }
            });

            var lblTitle = new Label
            {
                Text = "Clan News Tool",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 80, 40),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Height = 60,
                Margin = new Padding(0, 4, 0, 2)
            };

            var lblSub = new Label
            {
                Text = "Wolffiles.eu – Clan Portal",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopCenter,
                AutoSize = false,
                Height = 35,
                Margin = new Padding(0, 25, 0, 4)
            };

            var lblKey = new Label
            {
                Text = "API Key:",
                Font = new Font("Segoe UI", 10),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomLeft,
                AutoSize = false,
                Height = 35,
                Margin = new Padding(2, 0, 0, 2)
            };

            _txtApiKey.Dock = DockStyle.Fill;
            _txtApiKey.Font = new Font("Segoe UI", 11);
            _txtApiKey.PasswordChar = '●';
            _txtApiKey.PlaceholderText = "API Key eingeben...";
            _txtApiKey.Text = LoadApiKey();
            _txtApiKey.Margin = new Padding(0, 0, 0, 8);
            _txtApiKey.MinimumSize = new Size(0, 34);
            _txtApiKey.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) BtnLogin_Click(s, e); };

            var btnWrapper = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 70,
                Padding = new Padding(0, 8, 0, 8)
            };
            _btnLogin.Dock = DockStyle.Fill;
            _btnLogin.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            _btnLogin.BackColor = Color.FromArgb(220, 80, 40);
            _btnLogin.ForeColor = Color.White;
            _btnLogin.FlatStyle = FlatStyle.Flat;
            _btnLogin.FlatAppearance.BorderSize = 0;
            _btnLogin.Text = "Einloggen";
            _btnLogin.Cursor = Cursors.Hand;
            _btnLogin.Click += BtnLogin_Click;
            btnWrapper.Controls.Add(_btnLogin);

            var bottomPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1
            };
            bottomPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            bottomPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

            _lblLoginStatus.Dock = DockStyle.Fill;
            _lblLoginStatus.Font = new Font("Segoe UI", 9);
            _lblLoginStatus.ForeColor = Color.Red;
            _lblLoginStatus.TextAlign = ContentAlignment.MiddleCenter;

            var lblVersion = new Label
            {
                Text = $"Version {UpdateService.CurrentVersion}",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomCenter
            };

            bottomPanel.Controls.Add(_lblLoginStatus, 0, 0);
            bottomPanel.Controls.Add(lblVersion, 0, 1);

            outer.Controls.Add(logoPic, 1, 1);
            outer.Controls.Add(lblTitle, 1, 2);
            outer.Controls.Add(lblSub, 1, 3);
            outer.Controls.Add(new Label(), 1, 4);
            outer.Controls.Add(lblKey, 1, 5);
            outer.Controls.Add(_txtApiKey, 1, 6);
            outer.Controls.Add(btnWrapper, 1, 7);
            outer.Controls.Add(bottomPanel, 1, 8);

            _loginPanel.Controls.Add(outer);
        }

        private async void BtnLogin_Click(object? sender, EventArgs e)
        {
            var key = _txtApiKey.Text.Trim();
            if (string.IsNullOrEmpty(key))
            {
                _lblLoginStatus.Text = "Bitte API Key eingeben!";
                return;
            }

            _btnLogin.Enabled = false;
            _lblLoginStatus.ForeColor = Color.Gray;
            _lblLoginStatus.Text = "Verbinde...";

            try
            {
                _api = new ApiService(key);
                _clan = await _api.GetMeAsync();

                if (_clan == null)
                {
                    _lblLoginStatus.ForeColor = Color.Red;
                    _lblLoginStatus.Text = "Ungültiger API Key!";
                    _api = null;
                }
                else
                {
                    SaveApiKey(key);
                    _lblClanInfo.Text = $"[{_clan.Tag}] {_clan.Name}";
                    _loginPanel.Visible = false;
                    _mainPanel.Visible = true;
                    _mainPanel.BringToFront();
                }
            }
            catch
            {
                _lblLoginStatus.ForeColor = Color.Red;
                _lblLoginStatus.Text = "Verbindung fehlgeschlagen!";
                _api = null;
            }
            finally
            {
                _btnLogin.Enabled = true;
            }
        }

        // ─── MAIN PANEL ─────────────────────────────────────────────────────
        private void BuildMainPanel()
        {
            _mainPanel.Dock = DockStyle.Fill;
            Controls.Add(_mainPanel);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1
            };
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            var header = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(220, 80, 40),
                Padding = new Padding(12, 8, 12, 8),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            _lblClanInfo.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            _lblClanInfo.ForeColor = Color.White;
            _lblClanInfo.AutoSize = true;
            _lblClanInfo.TextAlign = ContentAlignment.MiddleLeft;
            _lblClanInfo.Location = new Point(12, 10);

            var btnLogout = new Button
            {
                Text = "Ausloggen",
                AutoSize = true,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnLogout.FlatAppearance.BorderColor = Color.White;
            btnLogout.Click += (s, e) => {
                SaveApiKey("");
                _mainPanel.Visible = false;
                _loginPanel.Visible = true;
                _loginPanel.BringToFront();
            };

            var btnUpdate = new Button
            {
                Text = "🔄 Update",
                AutoSize = true,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnUpdate.FlatAppearance.BorderColor = Color.White;
            btnUpdate.Click += async (s, e) =>
                await UpdateService.CheckAndUpdateAsync(silent: false);

            header.Resize += (s, e) =>
            {
                btnLogout.Location = new Point(header.Width - btnLogout.Width - 12, 8);
                btnUpdate.Location = new Point(header.Width - btnLogout.Width - btnUpdate.Width - 20, 8);
                _lblClanInfo.Location = new Point(12, (header.Height - _lblClanInfo.Height) / 2);
            };

            header.Controls.Add(_lblClanInfo);
            header.Controls.Add(btnUpdate);
            header.Controls.Add(btnLogout);

            _tabs.Dock = DockStyle.Fill;
            _tabs.Font = new Font("Segoe UI", 10);
            _tabs.Padding = new Point(12, 6);
            _tabs.TabPages.Add(BuildNewsTab());
            _tabs.TabPages.Add(BuildEventTab());
            _tabs.TabPages.Add(BuildMatchTab());
            _tabs.TabPages.Add(BuildRecruitmentTab());

            layout.Controls.Add(header, 0, 0);
            layout.Controls.Add(_tabs, 0, 1);
            _mainPanel.Controls.Add(layout);
        }

        // ─── TABS ────────────────────────────────────────────────────────────
        private TabPage BuildNewsTab()
        {
            var tab = new TabPage("📰 News") { Padding = new Padding(0) };
            tab.Controls.Add(BuildFormLayout(new[]
            {
                ("Titel *", false),
                ("Kurzbeschreibung (optional)", false),
                ("Inhalt *", true),
            }, "📰 News posten", (fields, btn, lbl) =>
            {
                btn.Click += async (s, e) =>
                {
                    if (!Validate(fields[0], fields[2], lbl)) return;
                    await PostAsync(btn, lbl, () => _api!.PostNewsAsync(new NewsPost
                    {
                        Title = fields[0].Text.Trim(),
                        Excerpt = fields[1].Text.Trim(),
                        Content = fields[2].Text.Trim()
                    }));
                };
            }));
            return tab;
        }

        private TabPage BuildEventTab()
        {
            var tab = new TabPage("📅 Event") { Padding = new Padding(0) };
            var layout = MakeTabLayout();

            var txtTitle = AddRow(layout, "Titel *", false);
            var dtpDate = AddDateRow(layout, "Datum & Uhrzeit *", true);
            var txtLocation = AddRow(layout, "Ort / Server (optional)", false);
            var txtContent = AddRow(layout, "Beschreibung *", true);

            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowCount++;
            layout.Controls.Add(new Panel { Dock = DockStyle.Fill }, 0, layout.RowCount - 1);

            var (btnPost, lblStatus) = AddButtonRow(layout, "📅 Event posten");

            btnPost.Click += async (s, e) =>
            {
                if (!Validate(txtTitle, txtContent, lblStatus)) return;
                await PostAsync(btnPost, lblStatus, () => _api!.PostEventAsync(new EventPost
                {
                    Title = txtTitle.Text.Trim(),
                    Content = txtContent.Text.Trim(),
                    EventDate = dtpDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                    EventLocation = txtLocation.Text.Trim()
                }));
            };

            tab.Controls.Add(layout);
            return tab;
        }

        private TabPage BuildMatchTab()
        {
            var tab = new TabPage("⚔️ Match") { Padding = new Padding(0) };
            var layout = MakeTabLayout();

            var txtTitle = AddRow(layout, "Titel *", false);
            var txtOpponent = AddRow(layout, "Gegner-Clan *", false);
            var txtResult = AddRow(layout, "Ergebnis (z.B. 2:1)", false);
            var txtMap = AddRow(layout, "Map (optional)", false);
            var dtpDate = AddDateRow(layout, "Match-Datum", false);
            var txtContent = AddRow(layout, "Match-Bericht *", true);

            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowCount++;
            layout.Controls.Add(new Panel { Dock = DockStyle.Fill }, 0, layout.RowCount - 1);

            var (btnPost, lblStatus) = AddButtonRow(layout, "⚔️ Match posten");

            btnPost.Click += async (s, e) =>
            {
                if (!Validate(txtTitle, txtContent, lblStatus)) return;
                if (string.IsNullOrWhiteSpace(txtOpponent.Text))
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Bitte Gegner-Clan eingeben!";
                    return;
                }
                await PostAsync(btnPost, lblStatus, () => _api!.PostMatchAsync(new MatchPost
                {
                    Title = txtTitle.Text.Trim(),
                    Content = txtContent.Text.Trim(),
                    MatchOpponent = txtOpponent.Text.Trim(),
                    MatchResult = txtResult.Text.Trim(),
                    MatchMap = txtMap.Text.Trim(),
                    EventDate = dtpDate.Value.ToString("yyyy-MM-dd")
                }));
            };

            tab.Controls.Add(layout);
            return tab;
        }

        private TabPage BuildRecruitmentTab()
        {
            var tab = new TabPage("🎮 Rekrutierung") { Padding = new Padding(0) };
            tab.Controls.Add(BuildFormLayout(new[]
            {
                ("Titel *", false),
                ("Kurzbeschreibung (optional)", false),
                ("Anforderungen & Details *", true),
            }, "🎮 Rekrutierung posten", (fields, btn, lbl) =>
            {
                btn.Click += async (s, e) =>
                {
                    if (!Validate(fields[0], fields[2], lbl)) return;
                    await PostAsync(btn, lbl, () => _api!.PostRecruitmentAsync(new RecruitmentPost
                    {
                        Title = fields[0].Text.Trim(),
                        Excerpt = fields[1].Text.Trim(),
                        Content = fields[2].Text.Trim()
                    }));
                };
            }));
            return tab;
        }

        // ─── LAYOUT HELPERS ──────────────────────────────────────────────────
        private TableLayoutPanel MakeTabLayout()
        {
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 0,
                Padding = new Padding(12, 10, 12, 5),
                AutoScroll = false
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            return layout;
        }

        private TableLayoutPanel BuildFormLayout(
            (string label, bool multiline)[] fields,
            string buttonText,
            Action<TextBox[], Button, Label> wireUp)
        {
            var layout = MakeTabLayout();
            var inputs = new TextBox[fields.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                var (label, multiline) = fields[i];
                if (multiline)
                {
                    layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                    layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                    layout.RowCount += 2;
                    var lbl = MakeLabel(label);
                    var txt = MakeInput(true);
                    layout.Controls.Add(lbl, 0, layout.RowCount - 2);
                    layout.Controls.Add(txt, 0, layout.RowCount - 1);
                    inputs[i] = txt;
                }
                else
                {
                    inputs[i] = AddRow(layout, label, false);
                }
            }

            var (btn, lblStatus) = AddButtonRow(layout, buttonText);
            wireUp(inputs, btn, lblStatus);
            return layout;
        }

        private TextBox AddRow(TableLayoutPanel layout, string label, bool multiline)
        {
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(multiline
                ? new RowStyle(SizeType.Percent, 100)
                : new RowStyle(SizeType.AutoSize));
            layout.RowCount += 2;

            var lbl = MakeLabel(label);
            var txt = MakeInput(multiline);
            layout.Controls.Add(lbl, 0, layout.RowCount - 2);
            layout.Controls.Add(txt, 0, layout.RowCount - 1);
            return txt;
        }

        private DateTimePicker AddDateRow(TableLayoutPanel layout, string label, bool withTime)
        {
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowCount += 2;

            var lbl = MakeLabel(label);
            var dtp = MakeDatePicker(withTime);
            layout.Controls.Add(lbl, 0, layout.RowCount - 2);
            layout.Controls.Add(dtp, 0, layout.RowCount - 1);
            return dtp;
        }

        private (Button btn, Label status) AddButtonRow(TableLayoutPanel layout, string text)
        {
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowCount++;

            var rowPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                AutoSize = true,
                Margin = new Padding(0, 8, 0, 4)
            };
            rowPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            rowPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            rowPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            var btn = new Button
            {
                Text = text,
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(220, 80, 40),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Padding = new Padding(16, 8, 16, 8),
                Margin = new Padding(0, 0, 12, 0)
            };
            btn.FlatAppearance.BorderSize = 0;

            var status = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false,
                MinimumSize = new Size(0, 38)
            };

            rowPanel.Controls.Add(btn, 0, 0);
            rowPanel.Controls.Add(status, 1, 0);
            layout.Controls.Add(rowPanel, 0, layout.RowCount - 1);
            return (btn, status);
        }

        private Label MakeLabel(string text)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopLeft,
                AutoSize = false,
                MinimumSize = new Size(0, 32),
                Margin = new Padding(0, 10, 0, 8),
                Padding = new Padding(0, 0, 4, 0)
            };
        }

        private TextBox MakeInput(bool multiline)
        {
            var txt = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Multiline = multiline,
                ScrollBars = multiline ? ScrollBars.Vertical : ScrollBars.None,
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 2)
            };
            if (!multiline)
                txt.MinimumSize = new Size(0, 34);
            else
                txt.MinimumSize = new Size(0, 100);
            return txt;
        }

        private DateTimePicker MakeDatePicker(bool withTime)
        {
            return new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = withTime ? "dd.MM.yyyy HH:mm" : "dd.MM.yyyy",
                MinimumSize = new Size(0, 34),
                Margin = new Padding(0, 0, 0, 2)
            };
        }

        // ─── VALIDATION & POST ───────────────────────────────────────────────
        private bool Validate(TextBox title, TextBox content, Label status)
        {
            if (string.IsNullOrWhiteSpace(title.Text) || string.IsNullOrWhiteSpace(content.Text))
            {
                status.ForeColor = Color.Red;
                status.Text = "Bitte alle Pflichtfelder (*) ausfüllen!";
                return false;
            }
            return true;
        }

        private async Task PostAsync<T>(Button btn, Label status, Func<Task<T?>> action) where T : ApiResponse
        {
            btn.Enabled = false;
            status.ForeColor = Color.Gray;
            status.Text = "Wird gesendet...";
            try
            {
                var result = await action();
                if (result?.Success == true)
                {
                    status.ForeColor = Color.Green;
                    status.Text = "✅ Erfolgreich – wartet auf Freischaltung!";
                }
                else
                {
                    status.ForeColor = Color.Red;
                    status.Text = $"❌ Fehler: {result?.Error ?? "Unbekannter Fehler"}";
                }
            }
            catch (Exception ex)
            {
                status.ForeColor = Color.Red;
                status.Text = $"❌ Verbindungsfehler: {ex.Message}";
            }
            finally
            {
                btn.Enabled = true;
            }
        }
    }
}