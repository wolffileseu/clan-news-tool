# 🐺 ClanNewsTool – Wolffiles.eu

[![Build and Release](https://github.com/wolffileseu/clan-news-tool/actions/workflows/release.yml/badge.svg)](https://github.com/wolffileseu/clan-news-tool/actions/workflows/release.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Platform: Windows](https://img.shields.io/badge/Platform-Windows-blue.svg)](https://github.com/wolffileseu/clan-news-tool/releases/latest)
[![Download](https://img.shields.io/github/v/release/wolffileseu/clan-news-tool?label=Download&color=orange)](https://github.com/wolffileseu/clan-news-tool/releases/latest)

A free Windows desktop tool for clan leaders to publish news, events, match results and recruitment posts directly on [Wolffiles.eu](https://wolffiles.eu) – the ET/RtCW community platform.

---

## 📥 Download

👉 **[Download latest version](https://github.com/wolffileseu/clan-news-tool/releases/latest)**

No installation required – just download and run `ClanNewsTool.exe`.

> **Requirements:** Windows 10 / 11 (64-bit)

---

## ✨ Features

### 📰 News & Announcements
Post clan news and announcements directly to Wolffiles.eu. Include a title, short description and full content. Posts are reviewed before going live.

### 📅 Events
Create event posts with date, time and server/location details. Events are displayed with a special badge on the news page and homepage.

### ⚔️ Match Reports
Post match results including opponent clan, score, map and a detailed match report. Match results are highlighted with a dedicated badge.

### 🎮 Recruitment
Create recruitment posts to find new clan members. Include requirements, details and a short teaser description.

### 🔄 Auto-Update
The tool automatically checks for new versions on startup. When a new version is available, it downloads and installs itself – no manual download needed.

### 🌍 Multilanguage
The tool automatically detects your system language and supports:
- 🇩🇪 Deutsch
- 🇬🇧 English
- 🇫🇷 Français
- 🇳🇱 Nederlands
- 🇵🇱 Polski
- 🇹🇷 Türkçe

### 🔐 API Key Authentication
No account required. Clan leaders receive a personal API key from the Wolffiles.eu admin team. The key is securely stored on your device.

### 🖥️ DPI Aware
The tool is fully DPI-aware and scales correctly on all screen resolutions and scaling settings (100% – 300%+).

---

## 🚀 How to get started

1. **Request an API Key** – Contact us on [Discord](https://discord.gg/wolffiles) or via the [Wolffiles.eu contact form](https://wolffiles.eu/contact)
2. **Download the tool** – [Latest Release](https://github.com/wolffileseu/clan-news-tool/releases/latest)
3. **Run `ClanNewsTool.exe`** – Enter your API key and log in
4. **Post your content** – Select a tab, fill in the form and submit

> Posts are briefly reviewed by the Wolffiles.eu team before going live.

---

## 🔒 Code Signing Policy

Free code signing provided by [SignPath.io](https://about.signpath.io), certificate by [SignPath Foundation](https://signpath.org).

### Team roles

| Role | Members |
|------|---------|
| Committers & Reviewers | [Owners](https://github.com/orgs/wolffileseu/people?query=role%3Aowner) |
| Approvers | [Owners](https://github.com/orgs/wolffileseu/people?query=role%3Aowner) |

### Privacy Policy

This program will not transfer any information to other networked systems unless specifically requested by the user or the person installing or operating it.

The tool communicates exclusively with the Wolffiles.eu API (`wolffiles.eu/api/v1/clan/*`) to:
- Validate the API key
- Submit posts for review
- Check for software updates

No personal data is collected or stored beyond the API key (stored locally in `%AppData%\ClanNewsTool\settings.json`).

---

## 🗑️ Uninstallation

No installer is used. To uninstall:
1. Delete `ClanNewsTool.exe`
2. Optionally delete `%AppData%\ClanNewsTool\` to remove saved settings

---

## 🛠️ Built With

- [.NET 10](https://dotnet.microsoft.com/) – Windows Forms
- [C#](https://learn.microsoft.com/en-us/dotnet/csharp/) – Application logic
- [GitHub Actions](https://github.com/features/actions) – Automated builds & releases

---

## 🏗️ Build from Source

```bash
git clone https://github.com/wolffileseu/clan-news-tool.git
cd clan-news-tool
dotnet publish ClanNewsTool/ClanNewsTool.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish
```

---

## ✅ Code Signing

This project is applying for free code signing through [SignPath Foundation](https://signpath.org).

Free code signing provided by [SignPath.io](https://about.signpath.io), certificate by [SignPath Foundation](https://signpath.org).

---

## 📄 License

This project is licensed under the **MIT License** – see the [LICENSE](LICENSE) file for details.

---

## 🌐 Links

- 🌍 [Wolffiles.eu](https://wolffiles.eu)
- 💬 [Discord](https://discord.gg/wolffiles)
- 📰 [News](https://wolffiles.eu/news)
- 🐙 [GitHub Organization](https://github.com/wolffileseu)

---

*ClanNewsTool is an open source project by [Wolffiles.eu](https://wolffiles.eu) – your ET/RtCW community platform.*
