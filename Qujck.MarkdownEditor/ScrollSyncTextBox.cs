using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Qujck.MarkdownEditor
{
    public sealed class ScrollSyncTextBox : TextBox, ISyncScroll
    {
        public ISyncScroll Buddy { get; set; }

        public bool IsScrolling { get; private set; }

        /* http://stackoverflow.com/q/3823188 - http://stackoverflow.com/users/319611/lesderid
         * http://stackoverflow.com/a/3823319 - http://stackoverflow.com/users/17034/hans-passant */
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if ((m.Msg == 0x115 || m.Msg == 0x20a) && 
                !this.IsScrolling && 
                !this.Buddy.IsScrolling &&
                this.Buddy != null && 
                this.Buddy.IsHandleCreated)
            {
                this.IsScrolling = true;
                double percentage = this.ScrollBarTopToPercentage();
                this.Buddy.Scroll(percentage);
                this.IsScrolling = false;
            }
        }

        public void Scroll(double percentage)
        {
            if (!this.IsScrolling)
            {
                this.IsScrolling = true;
                int top = this.ScrollBarTopFromPercentage(percentage);
                SetScrollPos(this.Handle, (int)ScrollBarDirection.SB_VERT, top, false);
                SendMessage(this.Handle, WM_VSCROLL, SB_THUMBPOSITION + 0x10000 * top, 0);
                Task.Factory.StartNew(() => this.IsScrolling = false);
            }
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

        [DllImport("user32.dll")]
        private static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

        [DllImport("user32.dll")]
        private static extern bool SendMessage(IntPtr hWnd, int nBar, int wParam, int lParam);

        private const int SB_THUMBPOSITION = 4;
        private const int WM_VSCROLL = 0x115;

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
