using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xbehave;
using ICSharpCode.AvalonEdit;

namespace Qujck.MarkdownEditor.Specs.Behaviours
{
    public class AvalonEditRepeatBulletBehaviourTests
    {
        [Scenario]
        public void TextEditor_WhenEnteringABulletedList_PrefixesTheNextLineWithTheBullet(
            TestStack.White.Application app,
            TestStack.White.UIItems.WindowItems.Window window)
        {
            string exe = typeof(App).Assembly.Location;

            "Given I have MarkdownEditor open"
                .f(() =>
                {
                    var info = new ProcessStartInfo
                    {
                        FileName = exe,
                        WindowStyle = ProcessWindowStyle.Hidden
                    };
                    app = TestStack.White.Application.Launch(info);
                    window = app.GetWindows()[0];
                });

            "When I have started a list of items"
                .f(() => 
                {
                    //var criteria = TestStack.White.UIItems.Finders
                    //    .SearchCriteria
                    //    .ByAutomationId("name")
                    //    .AndByClassName(typeof(DocumentView).FullName)
                    //    .AndIndex(0);

                    //var textEditor = (DocumentView)window.Get(criteria);
                });

            "And finally I close the window"
                .f(() => app.Close());
        }
    }
}
