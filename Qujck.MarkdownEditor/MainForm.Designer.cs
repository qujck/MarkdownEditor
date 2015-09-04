namespace Qujck.MarkdownEditor
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
            this.SplitContainer1 = new System.Windows.Forms.SplitContainer();
            this.TextView = new System.Windows.Forms.TextBox();
            this.RenderedView = new System.Windows.Forms.WebBrowser();
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
            this.SplitContainer1.Size = new System.Drawing.Size(933, 695);
            this.SplitContainer1.SplitterDistance = 437;
            this.SplitContainer1.TabIndex = 2;
            // 
            // TextView
            // 
            this.TextView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextView.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextView.Location = new System.Drawing.Point(0, 0);
            this.TextView.Multiline = true;
            this.TextView.Name = "TextView";
            this.TextView.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextView.Size = new System.Drawing.Size(434, 692);
            this.TextView.TabIndex = 3;
            this.TextView.TextChanged += new System.EventHandler(this.TextView_TextChanged);
            // 
            // RenderedView
            // 
            this.RenderedView.AllowWebBrowserDrop = false;
            this.RenderedView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RenderedView.IsWebBrowserContextMenuEnabled = false;
            this.RenderedView.Location = new System.Drawing.Point(3, 0);
            this.RenderedView.MinimumSize = new System.Drawing.Size(20, 20);
            this.RenderedView.Name = "RenderedView";
            this.RenderedView.ScriptErrorsSuppressed = true;
            this.RenderedView.Size = new System.Drawing.Size(486, 692);
            this.RenderedView.TabIndex = 3;
            this.RenderedView.WebBrowserShortcutsEnabled = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 695);
            this.Controls.Add(this.SplitContainer1);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.SplitContainer1.Panel1.ResumeLayout(false);
            this.SplitContainer1.Panel1.PerformLayout();
            this.SplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).EndInit();
            this.SplitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer SplitContainer1;
        private System.Windows.Forms.WebBrowser RenderedView;
        private System.Windows.Forms.TextBox TextView;
    }
}

