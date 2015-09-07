﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Qujck.MarkdownEditor.Infrastructure;
using Qujck.MarkdownEditor.Queries;
using Qujck.MarkdownEditor.Commands;

namespace Qujck.MarkdownEditor
{
    public sealed partial class MainForm : Form, IDisposable
    {
        private readonly CompositionRoot resolver;

        public MainForm()
        {
            InitializeComponent();
            this.resolver = new CompositionRoot();
            this.RenderedView.Url = new Uri("about:blank");

            var md = ResourceHelpers.ReadResource("Qujck.MarkdownEditor.test.md");
            TextView.Text = md;
            TextView.Select(0, 0);
        }

        private void TextView_TextChanged(object sender, EventArgs e)
        {
            var writeDocumentHandler = this.resolver.Resolve<ICommandHandler<Command.WriteDocument>>();

            writeDocumentHandler.Run(RenderedView, TextView.Text);
        }
    }
}
