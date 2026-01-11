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
        private Dictionary<string, DateTime> adminsNearby = new Dictionary<string, DateTime>();
        private HashSet<string> notifiedAdmins = new HashSet<string>();
        private HashSet<string> lastLines = new HashSet<string>();

        private static readonly string AppFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "tmp-admin-checker");
        private static readonly string AdminsPath = Path.Combine(AppFolder, "admins.txt");
        private static readonly string AdminMeetingsPath = Path.Combine(AppFolder, "AdminMeetingsLog.txt");

        private string logPath = "";
        private Thread checkerThread;
        private string adminsFilePath = "";
        private string adminMeetLogPath = "";
        private string lastAdminInfo = "";
        private bool checkerStarted = false;

        private SoundPlayer adminPlayer;
        private bool adminSoundPlaying = false;
        private DateTime lastAdminSeen = DateTime.MinValue;
        private readonly TimeSpan adminPresenceTimeout = TimeSpan.FromSeconds(15);

        private bool notificationSoundEnabled = true;
        private bool notificationDisplayEnabled = true;
        private bool adminSoundEnabled = true;


        public checker()
        {
            InitializeComponent();

            notificationSoundButton.Checked = true;
            notificationDisplayButton.Checked = true;
            AdminSoundButton.Checked = true;

            notificationSoundButton.CheckStateChanged += ToggleButtons_CheckStateChanged;
            notificationDisplayButton.CheckStateChanged += ToggleButtons_CheckStateChanged;
            AdminSoundButton.CheckStateChanged += ToggleButtons_CheckStateChanged;


            adminPlayer = new SoundPlayer(Properties.Resources.sound_admin_nearby);

            codeeloButton1.Click += codeeloButton1_Click;
            codeeloGradientPanel1.Controls.Add(codeeloButton1);

            ShowNotification("Test Notification", "Program started successfully!");
        }

        private void codeeloButton1_Click(object sender, EventArgs e)
        {
            if (checkerStarted) return;
            checkerStarted = true;

            Task.Run(async () =>
            {
                Directory.CreateDirectory(AppFolder);

                await AdminUpdater.UpdateAdminsFile();
                adminsFilePath = AdminsPath;
                adminsById = LoadAdmins(adminsFilePath);
                adminMeetLogPath = AdminMeetingsPath;

                this.Invoke(new Action(() =>
                {
                    if (string.IsNullOrEmpty(logPath) || !File.Exists(logPath))
                    {
                        logPath = DetectSpawningLogPath();
                        if (string.IsNullOrEmpty(logPath))
                        {
                            MessageBox.Show(
                                "Spawning log file was not found automatically.\nMake sure TruckersMP logs exist.",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                            checkerStarted = false;
                            return;
                        }
                    }

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

        private void LogAdminMeeting(string name, string inGameId, string tmpid, string tag, string roles)
        {
            try
            {
                string line =
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | " +
                    $"{name} ({inGameId}) | " +
                    $"TMPID: {tmpid} | " +
                    $"Tag: {tag} | " +
                    $"Roles: {roles}";

                File.AppendAllText(adminMeetLogPath, line + Environment.NewLine);
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

                                    lastAdminInfo = $"{displayName} ({inGameId})";
                                    UpdateLastAdminLabel();

                                    adminsNearby[tmpid] = DateTime.Now;

                                    if (!notifiedAdmins.Contains(tmpid))
                                    {
                                        ShowNotification("Admin nearby!", $"{displayName} ({inGameId})\nRoles: {roles}\nTag: {tag}");
                                        notifiedAdmins.Add(tmpid);
                                        Console.WriteLine($"[ADMIN] {displayName} ({inGameId}) | Roles: {roles} | Tag: {tag}");
                                    }

                                    if (adminSoundEnabled && !adminSoundPlaying)
                                        PlayAdminNearbySound();
                                }
                            }

                            lastLines.Add(line);
                            if (lastLines.Count > 1000)
                                lastLines = lastLines.Skip(lastLines.Count - 500).ToHashSet();
                        }

                        CheckAdminsTimeout();

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

        private void CheckAdminsTimeout()
        {
            var now = DateTime.Now;

            var leftAdmins = adminsNearby
                .Where(kv => now - kv.Value > adminPresenceTimeout)
                .Select(kv => kv.Key)
                .ToList();

            foreach (var tmpid in leftAdmins)
            {
                adminsNearby.Remove(tmpid);
                notifiedAdmins.Remove(tmpid);
            }

            if (adminsNearby.Count == 0 && adminSoundPlaying)
            {
                StopAdminNearbySound();
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
            if (!notificationDisplayEnabled)
                return;

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

                if (notificationSoundEnabled)
                {
                    using (var player = new SoundPlayer(Properties.Resources.notification_sound))
                    {
                        player.Play();
                    }
                }

                Console.WriteLine($"[NOTIFY] {title}: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Notification error: {ex.Message}");
            }
        }

        private void PlayAdminNearbySound()
        {
            lastAdminSeen = DateTime.Now;
            if (adminSoundPlaying || !adminSoundEnabled) return;

            try
            {
                adminSoundPlaying = true;
                adminPlayer.PlayLooping();
                Console.WriteLine("[SOUND] Admin nearby sound started");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Admin sound error: {ex.Message}");
            }
        }

        private void StopAdminNearbySound()
        {
            if (!adminSoundPlaying) return;

            try
            {
                adminPlayer.Stop();
                adminSoundPlaying = false;
                Console.WriteLine("[SOUND] Admin nearby sound stopped");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Admin sound stop error: {ex.Message}");
            }
        }

        private void UpdateLastAdminLabel()
        {
            if (lastAdminLabel.InvokeRequired)
            {
                lastAdminLabel.Invoke(new Action(UpdateLastAdminLabel));
            }
            else
            {
                lastAdminLabel.Text = $"{lastAdminInfo}";
            }
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/GitPolyakoff");
        }

        private void ToggleButtons_CheckStateChanged(object sender, EventArgs e)
        {
            notificationSoundEnabled = notificationSoundButton.Checked;
            notificationDisplayEnabled = notificationDisplayButton.Checked;
            adminSoundEnabled = AdminSoundButton.Checked;
        }

    }

    public static class AdminUpdater
    {
        public static string AdminsFilePath;

        public static async Task UpdateAdminsFile()
        {
            string appFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "tmp-admin-checker");
            Directory.CreateDirectory(appFolder);
            AdminsFilePath = Path.Combine(appFolder, "admins.txt");

            try
            {
                var http = new HttpClient();
                var html = await http.GetStringAsync("https://truckersmp.com/team");

                var parser = new HtmlParser();
                var doc = parser.ParseDocument(html);

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
                                    if (!fullUrl.StartsWith("http"))
                                        fullUrl = "https://truckersmp.com" + fullUrl;

                                    await writer.WriteLineAsync(fullUrl);
                                }
                            }
                        }

                        await writer.WriteLineAsync();
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