﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Qujck.MarkdownEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var wnd = new MainWindow();
            if (e.Args.Count() > 0)
            {
                this.Properties["OpenFile"] = e.Args[0];
            }
            wnd.Show();
        }
    }
}
