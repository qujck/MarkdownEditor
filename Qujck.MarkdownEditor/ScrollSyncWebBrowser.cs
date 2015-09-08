using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qujck.MarkdownEditor
{
    public sealed class ScrollSyncWebBrowser : WebBrowser, ISyncScroll
    {
        public ISyncScroll Buddy { get; set; }

        public bool IsScrolling { get; private set; }

        public void Load(string html)
        {
            this.Url = new Uri("about:blank");
            this.Document.Write(html);
            /* http://stackoverflow.com/q/11115043 - http://stackoverflow.com/users/1356321/pomster
             * http://stackoverflow.com/a/12893075 - http://stackoverflow.com/users/331982/nick-w */
            this.Document.Window.AttachEventHandler("onscroll", OnScrollEventHandler);
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

        private void OnScrollEventHandler(object sender, EventArgs e)
        {
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
            int top = this.Document.GetElementsByTagName("HTML")[0].ScrollTop;
            int height = this.Document.GetElementsByTagName("HTML")[0].ScrollRectangle.Height;

            return (top * 100.0) / (height * 1.0);
        }

        private int ScrollBarTopFromPercentage(double percentage)
        {
            int height = this.Document.GetElementsByTagName("HTML")[0].ScrollRectangle.Height;

            return (int)((percentage / 100) * height);
        }
    }
}
