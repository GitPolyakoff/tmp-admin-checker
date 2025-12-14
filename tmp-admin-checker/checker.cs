using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;
using System.Net.Http;
using AngleSharp.Html.Parser;
using System.Diagnostics;

namespace tmp_admin_checker
{
    public partial class checker : Form
    {
        private Dictionary<string, List<string>> adminsById = new Dictionary<string, List<string>>();
        private HashSet<string> lastLines = new HashSet<string>();
        private string logPath = "";
        private Thread checkerThread;
        private string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TMPCheckerPaths.txt");
        private string adminsFilePath = "";
        private string adminMeetLogPath = "";
        private bool checkerStarted = false;

        public checker()
        {
            InitializeComponent();
            Start.Click += Start_Click;
            this.Controls.Add(Start);

            ShowNotification("Test Notification", "Program started successfully!");
        }

        private void Start_Click(object sender, EventArgs e)
        {
            if (checkerStarted) return;
            checkerStarted = true;

            Task.Run(async () =>
            {
                await AdminUpdater.UpdateAdminsFile();
                adminsFilePath = AdminUpdater.AdminsFilePath;
                adminsById = LoadAdmins(adminsFilePath);
                adminMeetLogPath = Path.Combine(
                    Path.GetDirectoryName(adminsFilePath),
                    "AdminMeetingsLog.txt"
                );

                this.Invoke(new Action(() =>
                {
                    if (File.Exists(savePath))
                    {
                        var lines = File.ReadAllLines(savePath);
                        if (lines.Length >= 2)
                        {
                            logPath = lines[1];
                        }
                    }

                    if (string.IsNullOrEmpty(logPath) || !File.Exists(logPath))
                    {
                        logPath = DetectSpawningLogPath();

                        if (string.IsNullOrEmpty(logPath))
                        {
                            MessageBox.Show(
                                "Spawning log file was not found automatically.\n" +
                                "Make sure TruckersMP logs exist.",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                            checkerStarted = false;
                            return;
                        }
                    }

                    File.WriteAllLines(savePath, new string[] { adminsFilePath, logPath });
                    MessageBox.Show($"Admins and log paths saved.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    checkerThread = new Thread(new ThreadStart(CheckLogs));
                    checkerThread.IsBackground = true;
                    checkerThread.Start();

                    MessageBox.Show("Checker started!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));
            });
        }

        private Dictionary<string, List<string>> LoadAdmins(string path)
        {
            var result = new Dictionary<string, List<string>>();
            string currentRole = null;

            var allowedRoles = new HashSet<string>
    {
        "Game Moderation Manager",
        "Game Moderation Leader",
        "Game Moderation Trainer",
        "Game Moderator",
        "Report Moderator",
        "Game Moderation Trainee"
    };

            foreach (var line in File.ReadAllLines(path))
            {
                var trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed))
                {
                    currentRole = null;
                    continue;
                }

                if (trimmed.StartsWith("https://truckersmp.com/user/"))
                {
                    if (currentRole != null && allowedRoles.Contains(currentRole))
                    {
                        var idMatch = Regex.Match(trimmed, @"\/user\/(\d+)");
                        if (!idMatch.Success) continue;

                        string tmpid = idMatch.Groups[1].Value;

                        if (!result.ContainsKey(tmpid))
                            result[tmpid] = new List<string>();

                        if (!result[tmpid].Contains(currentRole))
                            result[tmpid].Add(currentRole);
                    }
                }
                else
                {
                    currentRole = trimmed;
                }
            }

            Console.WriteLine($"Loaded {result.Count} moderators from allowed roles.");
            return result;
        }

        private void LogAdminMeeting(
            string name,
            string inGameId,
            string tmpid,
            string tag,
            string roles)
        {
            try
            {
                string line =
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | " +
                    $"{name} ({inGameId}) | " +
                    $"TMPID: {tmpid} | " +
                    $"Tag: {tag} | " +
                    $"Roles: {roles}";

                File.AppendAllText(
                    adminMeetLogPath,
                    line + Environment.NewLine
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Log write error: {ex.Message}");
            }
        }


        private void CheckLogs()
        {
            try
            {
                using (var fs = new FileStream(logPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var sr = new StreamReader(fs))
                {
                    string line;
                    while (true)
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (lastLines.Contains(line)) continue;

                            var logMatch = Regex.Match(line, @"\((.+?)\((\d+)\)\s*-\s*TMPID:\s*(\d+)\s*-\s*SteamID64:\s*(\d+)\s*-\s*Tag:\s*(.*?)\)");
                            if (logMatch.Success)
                            {
                                string displayName = logMatch.Groups[1].Value.Trim();
                                string inGameId = logMatch.Groups[2].Value.Trim();
                                string tmpid = logMatch.Groups[3].Value.Trim();
                                string tag = logMatch.Groups[5].Value.Trim();

                                if (adminsById.ContainsKey(tmpid))
                                {
                                    var roles = string.Join(", ", adminsById[tmpid]);
                                    LogAdminMeeting(displayName, inGameId, tmpid, tag, roles);
                                    ShowNotification("Admin nearby!", $"{displayName} ({inGameId})\nRoles: {roles}\nTag: {tag}");
                                    Console.WriteLine($"[ADMIN] {displayName} ({inGameId}) | Roles: {roles} | Tag: {tag}");
                                }
                            }

                            lastLines.Add(line);

                            if (lastLines.Count > 1000)
                                lastLines = lastLines.Skip(lastLines.Count - 500).ToHashSet();
                        }

                        Thread.Sleep(500);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CheckLogs: {ex.Message}");
                Thread.Sleep(2000);
                CheckLogs();
            }
        }

        private string DetectSpawningLogPath()
        {
            Console.WriteLine("🔍 Searching for spawning log automatically...");

            var possiblePaths = new[]
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ETS2MP", "logs"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ATS2MP", "logs"),
                $@"C:\Users\{Environment.UserName}\Documents\ETS2MP\logs",
                $@"C:\Users\{Environment.UserName}\Documents\ATS2MP\logs",
            };

            foreach (var logDir in possiblePaths)
            {
                if (!Directory.Exists(logDir))
                    continue;

                var today = DateTime.Now;
                var todayFile = $"log_spawning_{today:yyyy.MM.dd}_log.txt";
                var todayPath = Path.Combine(logDir, todayFile);

                if (File.Exists(todayPath))
                {
                    Console.WriteLine($"✅ Spawning log found: {todayPath}");
                    return todayPath;
                }

                try
                {
                    var files = Directory.GetFiles(logDir, "log_spawning_*_log.txt")
                        .OrderByDescending(File.GetLastWriteTime)
                        .ToArray();

                    if (files.Length > 0)
                    {
                        Console.WriteLine($"📄 Latest spawning log found: {files[0]}");
                        return files[0];
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Log scan error: {ex.Message}");
                }
            }

            Console.WriteLine("❌ Spawning log not found automatically.");
            return null;
        }

        private void ShowNotification(string title, string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ShowNotification(title, message)));
                return;
            }

            try
            {
                var popupNotifier = new PopupNotifier
                {
                    TitleText = title,
                    ContentText = message,
                    IsRightToLeft = false,
                    Delay = 5000,
                    TitleColor = System.Drawing.Color.White,
                    ContentColor = System.Drawing.Color.WhiteSmoke,
                    BodyColor = System.Drawing.Color.FromArgb(45, 55, 72),
                    BorderColor = System.Drawing.Color.FromArgb(100, 149, 237),
                    TitleFont = new System.Drawing.Font("Segoe UI Semibold", 12, System.Drawing.FontStyle.Bold),
                    ContentFont = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Regular),
                    GradientPower = 50,
                    HeaderHeight = 30,
                    ContentPadding = new System.Windows.Forms.Padding(15),
                    Image = null
                };

                popupNotifier.Size = new System.Drawing.Size(400, 180);

                popupNotifier.Popup();

                SystemSounds.Hand.Play();
                //SystemSounds.Asterisk.Play();
                Console.WriteLine($"[NOTIFY] {title}: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Notification error: {ex.Message}");
            }
        }

        private void githubLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/GitPolyakoff");
        }

        private void steamLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://steamcommunity.com/profiles/76561199147759312/");
        }

        private void discordLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://discord.com/users/913793634376241192");
        }
    }


    public static class AdminUpdater
    {
        public static readonly string AdminsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "admins.txt");

        public static async Task UpdateAdminsFile()
        {
            try
            {
                var http = new HttpClient();
                var html = await http.GetStringAsync("https://truckersmp.com/team");

                var parser = new HtmlParser();
                var doc = parser.ParseDocument(html);

                //all role blocks
                var roleSections = doc.QuerySelectorAll("div.headline-center");

                if (roleSections.Length == 0)
                {
                    Console.WriteLine("Warning: no role sections found on site!");
                    return;
                }

                using (var writer = new StreamWriter(AdminsFilePath, false))
                {
                    foreach (var roleSection in roleSections)
                    {
                        var roleName = roleSection.QuerySelector("h2")?.TextContent.Trim();
                        if (string.IsNullOrEmpty(roleName)) continue;

                        await writer.WriteLineAsync(roleName);

                        //admin cards(sibling - .row.team-v4)
                        var adminRow = roleSection.NextElementSibling;
                        if (adminRow != null && adminRow.ClassList.Contains("row") && adminRow.ClassList.Contains("team-v4"))
                        {
                            var adminCards = adminRow.QuerySelectorAll("div.t-card");
                            foreach (var card in adminCards)
                            {
                                var link = card.QuerySelector("span.break-all > a");
                                if (link != null)
                                {
                                    string fullUrl = link.GetAttribute("href");
                                    //if the link is not complete
                                    if (!fullUrl.StartsWith("http"))
                                        fullUrl = "https://truckersmp.com" + fullUrl;

                                    await writer.WriteLineAsync(fullUrl);
                                }
                            }
                        }

                        await writer.WriteLineAsync();//empty line between roles
                    }
                }

                Console.WriteLine("Admins file updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Admin update error: {ex.Message}");
            }
        }
    }
}