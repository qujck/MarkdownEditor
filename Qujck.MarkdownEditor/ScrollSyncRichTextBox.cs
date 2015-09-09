using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Qujck.MarkdownEditor
{
    public sealed class ScrollSyncRichTextBox : RichTextBox, ISyncScroll
    {
        public ScrollSyncRichTextBox()
        {
            this.InitializeComponent();
        }

        public ISyncScroll Buddy { get; set; }

        public bool IsScrolling { get; private set; }

        /* http://stackoverflow.com/q/3823188 - http://stackoverflow.com/users/319611/lesderid
         * http://stackoverflow.com/a/8149232 - http://stackoverflow.com/users/1049308/john-willemse */
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if ((m.Msg == WM_VSCROLL || m.Msg == WM_MOUSEWHEEL) && 
                !this.IsScrolling && 
                this.Buddy != null)
            {
                this.ScrollBuddy();
                if (m.Msg == WM_MOUSEWHEEL)
                {
                    this.SyncSmoothScrollTimer.Enabled = true;
                }
            }
        }

        public void Scroll(double percentage)
        {
            if (!this.IsScrolling)
            {
                this.IsScrolling = true;
                int top = this.ScrollBarTopFromPercentage(percentage);
                var message = Message.Create(this.Handle, WM_VSCROLL, (IntPtr)(SB_THUMBPOSITION + 0x10000 * top), (IntPtr)0);
                this.WndProc(ref message);
                Task.Factory.StartNew(() => this.IsScrolling = false);
            }
        }

        private void ScrollBuddy()
        {
            this.IsScrolling = true;
            double percentage = this.ScrollBarTopToPercentage();
            this.Buddy.Scroll(percentage);
            this.IsScrolling = false;
        }

        private double ScrollBarTopToPercentage()
        {
            SCROLLINFO info = this.GetScrollInfo();

            return (info.nPos * 100.0) / (info.nMax * 1.0);
        }

        private int ScrollBarTopFromPercentage(double percentage)
        {
            SCROLLINFO info = this.GetScrollInfo();

            return (int)((percentage / 100) * info.nMax);
        }

        private SCROLLINFO GetScrollInfo()
        {
            SCROLLINFO info = new SCROLLINFO();
            info.cbSize = Marshal.SizeOf(info);
            info.fMask = (int)ScrollInfoMask.SIF_ALL;
            GetScrollInfo(this.Handle, (int)ScrollBarDirection.SB_VERT, ref info);

            return info;
        }

        private void SyncSmoothScrollTimer_Tick(object sender, EventArgs e)
        {
            this.SyncSmoothScrollTimer.Enabled = false;
            this.HandleSmoothScrolling();
        }

        private void HandleSmoothScrolling()
        {
            SCROLLINFO info = this.GetScrollInfo();
            int tag = Convert.ToInt32(this.SyncSmoothScrollTimer.Tag);
            if (tag < info.nPos - 1 || tag > info.nPos + 1)
            {
                this.SyncSmoothScrollTimer.Tag = info.nPos;
                this.ScrollBuddy();
                this.smoothScrollSyncCheckCounter = 0;
                this.SyncSmoothScrollTimer.Enabled = true;
            }
            else
            {
                this.SyncSmoothScrollTimer.Enabled =
                    smoothScrollSyncCheckCounter++ < MAXSMOOTHSCROLLCHECKCOUNTER;
            }
        }

        private int smoothScrollSyncCheckCounter = 0;
        private const int MAXSMOOTHSCROLLCHECKCOUNTER = 10;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SyncSmoothScrollTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // SyncSmoothScrollTimer
            // 
            this.SyncSmoothScrollTimer.Interval = 10;
            this.SyncSmoothScrollTimer.Tick += new System.EventHandler(this.SyncSmoothScrollTimer_Tick);
            this.ResumeLayout(false);

        }

        private const int SB_THUMBPOSITION = 4;
        private const int WM_VSCROLL = 0x115;
        private Timer SyncSmoothScrollTimer;
        private System.ComponentModel.IContainer components;
        private const int WM_MOUSEWHEEL = 0x20a;

        [DllImport("user32.dll")]
        private static extern bool GetScrollInfo(IntPtr hwnd, int fnBar, ref SCROLLINFO ScrollInfo);

        private struct SCROLLINFO
        {
            public int cbSize;
            public int fMask;
            public int nMin;
            public int nMax;
            public int nPage;
            public int nPos;
            public int nTrackPos;

            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        private enum ScrollBarDirection
        {
            SB_HORZ = 0,
            SB_VERT = 1,
            SB_CTL = 2,
            SB_BOTH = 3
        }

        private enum ScrollInfoMask
        {
            SIF_RANGE = 0x1,
            SIF_PAGE = 0x2,
            SIF_POS = 0x4,
            SIF_DISABLENOSCROLL = 0x8,
            SIF_TRACKPOS = 0x16,
            SIF_ALL = SIF_RANGE + SIF_PAGE + SIF_POS + SIF_TRACKPOS
        }
    }
}
