﻿namespace Qujck.MarkdownEditor
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.SplitContainer1 = new System.Windows.Forms.SplitContainer();
            this.RefreshTimer = new System.Windows.Forms.Timer(this.components);
            this.TextView = new Qujck.MarkdownEditor.ScrollSyncTextBox();
            this.RenderedView = new Qujck.MarkdownEditor.ScrollSyncWebBrowser();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).BeginInit();
            this.SplitContainer1.Panel1.SuspendLayout();
            this.SplitContainer1.Panel2.SuspendLayout();
            this.SplitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SplitContainer1
            // 
            this.SplitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SplitContainer1.IsSplitterFixed = true;
            this.SplitContainer1.Location = new System.Drawing.Point(1, 0);
            this.SplitContainer1.Name = "SplitContainer1";
            // 
            // SplitContainer1.Panel1
            // 
            this.SplitContainer1.Panel1.Controls.Add(this.TextView);
            // 
            // SplitContainer1.Panel2
            // 
            this.SplitContainer1.Panel2.Controls.Add(this.RenderedView);
            this.SplitContainer1.Size = new System.Drawing.Size(934, 695);
            this.SplitContainer1.SplitterDistance = 464;
            this.SplitContainer1.TabIndex = 2;
            // 
            // RefreshTimer
            // 
            this.RefreshTimer.Interval = 10;
            this.RefreshTimer.Tick += new System.EventHandler(this.RefreshTimer_Tick);
            // 
            // TextView
            // 
            this.TextView.AutoWordSelection = true;
            this.TextView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextView.Buddy = this.RenderedView;
            this.TextView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextView.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextView.Location = new System.Drawing.Point(0, 0);
            this.TextView.Name = "TextView";
            this.TextView.Size = new System.Drawing.Size(464, 695);
            this.TextView.TabIndex = 3;
            this.TextView.Text = "";
            this.TextView.TextChanged += new System.EventHandler(this.TextView_TextChanged);
            // 
            // RenderedView
            // 
            this.RenderedView.AllowWebBrowserDrop = false;
            this.RenderedView.Buddy = this.TextView;
            this.RenderedView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenderedView.Location = new System.Drawing.Point(0, 0);
            this.RenderedView.MinimumSize = new System.Drawing.Size(20, 20);
            this.RenderedView.Name = "RenderedView";
            this.RenderedView.ScriptErrorsSuppressed = true;
            this.RenderedView.Size = new System.Drawing.Size(466, 695);
            this.RenderedView.TabIndex = 3;
            this.RenderedView.WebBrowserShortcutsEnabled = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 695);
            this.Controls.Add(this.SplitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Qujck Markdown Editor";
            this.SplitContainer1.Panel1.ResumeLayout(false);
            this.SplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).EndInit();
            this.SplitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer SplitContainer1;
        private ScrollSyncWebBrowser RenderedView;
        private ScrollSyncTextBox TextView;
        private System.Windows.Forms.Timer RefreshTimer;
    }
}

