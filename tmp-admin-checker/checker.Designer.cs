namespace tmp_admin_checker
{
    partial class checker
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.Start = new System.Windows.Forms.Button();
            this.githubLink = new System.Windows.Forms.LinkLabel();
            this.steamLink = new System.Windows.Forms.LinkLabel();
            this.discordLink = new System.Windows.Forms.LinkLabel();
            this.github = new System.Windows.Forms.Label();
            this.steam = new System.Windows.Forms.Label();
            this.discord = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Start
            // 
            this.Start.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Start.BackColor = System.Drawing.Color.Gray;
            this.Start.Font = new System.Drawing.Font("Comic Sans MS", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Start.Location = new System.Drawing.Point(12, 12);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(256, 92);
            this.Start.TabIndex = 0;
            this.Start.Text = "Start Checker";
            this.Start.UseVisualStyleBackColor = false;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // githubLink
            // 
            this.githubLink.AutoSize = true;
            this.githubLink.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.githubLink.Location = new System.Drawing.Point(111, 0);
            this.githubLink.Name = "githubLink";
            this.githubLink.Size = new System.Drawing.Size(105, 23);
            this.githubLink.TabIndex = 1;
            this.githubLink.TabStop = true;
            this.githubLink.Text = "GitPolyakoff";
            this.githubLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.githubLink_LinkClicked);
            // 
            // steamLink
            // 
            this.steamLink.AutoSize = true;
            this.steamLink.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.steamLink.Location = new System.Drawing.Point(111, 35);
            this.steamLink.Name = "steamLink";
            this.steamLink.Size = new System.Drawing.Size(82, 23);
            this.steamLink.TabIndex = 2;
            this.steamLink.TabStop = true;
            this.steamLink.Text = "Polyakoff";
            this.steamLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.steamLink_LinkClicked);
            // 
            // discordLink
            // 
            this.discordLink.AutoSize = true;
            this.discordLink.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.discordLink.Location = new System.Drawing.Point(111, 70);
            this.discordLink.Name = "discordLink";
            this.discordLink.Size = new System.Drawing.Size(82, 20);
            this.discordLink.TabIndex = 3;
            this.discordLink.TabStop = true;
            this.discordLink.Text = "polyakoff";
            this.discordLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.discordLink_LinkClicked);
            // 
            // github
            // 
            this.github.AutoSize = true;
            this.github.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.github.Location = new System.Drawing.Point(3, 0);
            this.github.Name = "github";
            this.github.Size = new System.Drawing.Size(98, 23);
            this.github.TabIndex = 4;
            this.github.Text = "my GitHub:";
            // 
            // steam
            // 
            this.steam.AutoSize = true;
            this.steam.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.steam.Location = new System.Drawing.Point(3, 35);
            this.steam.Name = "steam";
            this.steam.Size = new System.Drawing.Size(94, 23);
            this.steam.TabIndex = 5;
            this.steam.Text = "my Steam:";
            // 
            // discord
            // 
            this.discord.AutoSize = true;
            this.discord.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.discord.Location = new System.Drawing.Point(3, 70);
            this.discord.Name = "discord";
            this.discord.Size = new System.Drawing.Size(102, 20);
            this.discord.TabIndex = 6;
            this.discord.Text = "my Discord:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.github, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.discordLink, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.discord, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.steamLink, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.steam, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.githubLink, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 301);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(256, 90);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // checker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(280, 403);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.Start);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "checker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TruckersMP Admin Checker by Polyakoff";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.LinkLabel githubLink;
        private System.Windows.Forms.LinkLabel steamLink;
        private System.Windows.Forms.LinkLabel discordLink;
        private System.Windows.Forms.Label github;
        private System.Windows.Forms.Label steam;
        private System.Windows.Forms.Label discord;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

