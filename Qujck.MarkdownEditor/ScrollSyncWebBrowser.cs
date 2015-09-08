using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qujck.MarkdownEditor
{
    public class ScrollSyncWebBrowser : WebBrowser, ISyncScroll
    {
        public ISyncScroll Buddy { get; set; }

        public bool IsScrolling
        {
            get;
            private set;
        }

        public void Load(string html)
        {
            this.Url = new Uri("about:blank");
            this.Document.Write(html);
            this.Document.Window.AttachEventHandler("onscroll", OnScrollEventHandler);
        }

        public void OnScrollEventHandler(object sender, EventArgs e)
        {
            // Trap WM_VSCROLL message and pass to buddy
            if (!this.IsScrolling &&
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

        private double ScrollBarTopToPercentage()
        {
            return (this.Document.GetElementsByTagName("HTML")[0].ScrollTop * 100) /
                    this.Document.GetElementsByTagName("HTML")[0].ScrollRectangle.Height;
        }

        private int ScrollBarTopFromPercentage(double percentage)
        {
            return (int)((percentage / 100) * this.Document.GetElementsByTagName("HTML")[0].ScrollRectangle.Height);   
        }

        public void Scroll(double percentage)
        {
            if (!this.IsScrolling)
            {
                this.IsScrolling = true;
                int top = this.ScrollBarTopFromPercentage(percentage);
                this.Document.GetElementsByTagName("HTML")[0].ScrollTop = top;
                Task.Factory.StartNew(() => this.IsScrolling = false);
            }
        }
    }
}
