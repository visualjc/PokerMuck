﻿namespace PokerMuck
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabHandsPage = new System.Windows.Forms.TabPage();
            this.playerHandsContainer = new PokerMuck.ControlListContainer();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tabConfigurationPage = new System.Windows.Forms.TabPage();
            this.txtUserId = new System.Windows.Forms.TextBox();
            this.lblUserId = new System.Windows.Forms.Label();
            this.btnChangeHandHistory = new System.Windows.Forms.Button();
            this.txtHandHistoryDirectory = new System.Windows.Forms.TextBox();
            this.tabAboutPage = new System.Windows.Forms.TabPage();
            this.pictureEagle = new System.Windows.Forms.PictureBox();
            this.tabControl.SuspendLayout();
            this.tabHandsPage.SuspendLayout();
            this.tabConfigurationPage.SuspendLayout();
            this.tabAboutPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEagle)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabHandsPage);
            this.tabControl.Controls.Add(this.tabConfigurationPage);
            this.tabControl.Controls.Add(this.tabAboutPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(193, 293);
            this.tabControl.TabIndex = 3;
            // 
            // tabHandsPage
            // 
            this.tabHandsPage.Controls.Add(this.playerHandsContainer);
            this.tabHandsPage.Controls.Add(this.lblStatus);
            this.tabHandsPage.Location = new System.Drawing.Point(4, 22);
            this.tabHandsPage.Name = "tabHandsPage";
            this.tabHandsPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabHandsPage.Size = new System.Drawing.Size(185, 267);
            this.tabHandsPage.TabIndex = 0;
            this.tabHandsPage.Text = "Hands";
            this.tabHandsPage.UseVisualStyleBackColor = true;
            // 
            // playerHandsContainer
            // 
            this.playerHandsContainer.BackColor = System.Drawing.Color.Transparent;
            this.playerHandsContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playerHandsContainer.Location = new System.Drawing.Point(3, 3);
            this.playerHandsContainer.Name = "playerHandsContainer";
            this.playerHandsContainer.Size = new System.Drawing.Size(179, 224);
            this.playerHandsContainer.TabIndex = 4;
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(3, 227);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(179, 37);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Status Label";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabConfigurationPage
            // 
            this.tabConfigurationPage.Controls.Add(this.txtUserId);
            this.tabConfigurationPage.Controls.Add(this.lblUserId);
            this.tabConfigurationPage.Controls.Add(this.btnChangeHandHistory);
            this.tabConfigurationPage.Controls.Add(this.txtHandHistoryDirectory);
            this.tabConfigurationPage.Location = new System.Drawing.Point(4, 22);
            this.tabConfigurationPage.Name = "tabConfigurationPage";
            this.tabConfigurationPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabConfigurationPage.Size = new System.Drawing.Size(185, 267);
            this.tabConfigurationPage.TabIndex = 1;
            this.tabConfigurationPage.Text = "Configuration";
            this.tabConfigurationPage.UseVisualStyleBackColor = true;
            // 
            // txtUserId
            // 
            this.txtUserId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUserId.Location = new System.Drawing.Point(82, 8);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new System.Drawing.Size(95, 20);
            this.txtUserId.TabIndex = 4;
            this.txtUserId.TextChanged += new System.EventHandler(this.txtUserId_TextChanged);
            // 
            // lblUserId
            // 
            this.lblUserId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUserId.AutoSize = true;
            this.lblUserId.Location = new System.Drawing.Point(8, 10);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Size = new System.Drawing.Size(71, 13);
            this.lblUserId.TabIndex = 3;
            this.lblUserId.Text = "Your User ID:";
            // 
            // btnChangeHandHistory
            // 
            this.btnChangeHandHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangeHandHistory.Location = new System.Drawing.Point(8, 78);
            this.btnChangeHandHistory.Name = "btnChangeHandHistory";
            this.btnChangeHandHistory.Size = new System.Drawing.Size(169, 23);
            this.btnChangeHandHistory.TabIndex = 2;
            this.btnChangeHandHistory.Text = "Change Hand History Directory";
            this.btnChangeHandHistory.UseVisualStyleBackColor = true;
            this.btnChangeHandHistory.Click += new System.EventHandler(this.btnChangeHandHistory_Click);
            // 
            // txtHandHistoryDirectory
            // 
            this.txtHandHistoryDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHandHistoryDirectory.Location = new System.Drawing.Point(8, 52);
            this.txtHandHistoryDirectory.Name = "txtHandHistoryDirectory";
            this.txtHandHistoryDirectory.ReadOnly = true;
            this.txtHandHistoryDirectory.Size = new System.Drawing.Size(169, 20);
            this.txtHandHistoryDirectory.TabIndex = 1;
            // 
            // tabAboutPage
            // 
            this.tabAboutPage.Controls.Add(this.pictureEagle);
            this.tabAboutPage.Location = new System.Drawing.Point(4, 22);
            this.tabAboutPage.Name = "tabAboutPage";
            this.tabAboutPage.Size = new System.Drawing.Size(185, 267);
            this.tabAboutPage.TabIndex = 2;
            this.tabAboutPage.Text = "About";
            this.tabAboutPage.UseVisualStyleBackColor = true;
            // 
            // pictureEagle
            // 
            this.pictureEagle.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureEagle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureEagle.Image = global::PokerMuck.Properties.Resources.Eagle;
            this.pictureEagle.Location = new System.Drawing.Point(41, 69);
            this.pictureEagle.Name = "pictureEagle";
            this.pictureEagle.Size = new System.Drawing.Size(106, 89);
            this.pictureEagle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureEagle.TabIndex = 0;
            this.pictureEagle.TabStop = false;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(193, 293);
            this.Controls.Add(this.tabControl);
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PokerMuck";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.ResizeEnd += new System.EventHandler(this.FrmMain_ResizeEnd);
            this.LocationChanged += new System.EventHandler(this.FrmMain_LocationChanged);
            this.tabControl.ResumeLayout(false);
            this.tabHandsPage.ResumeLayout(false);
            this.tabConfigurationPage.ResumeLayout(false);
            this.tabConfigurationPage.PerformLayout();
            this.tabAboutPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEagle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabHandsPage;
        private ControlListContainer playerHandsContainer;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TabPage tabConfigurationPage;
        private System.Windows.Forms.Button btnChangeHandHistory;
        private System.Windows.Forms.TextBox txtHandHistoryDirectory;
        private System.Windows.Forms.TabPage tabAboutPage;
        private System.Windows.Forms.PictureBox pictureEagle;
        private System.Windows.Forms.Label lblUserId;
        private System.Windows.Forms.TextBox txtUserId;


    }
}

