﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Qujck.MarkdownEditor.Tests.Integration
{
    internal class StubPresentationSource : PresentationSource
    {
        protected override CompositionTarget GetCompositionTargetCore()
        {
            return null;
        }

        public override Visual RootVisual { get; set; }

        public override bool IsDisposed { get { return false; } }
    }
}
