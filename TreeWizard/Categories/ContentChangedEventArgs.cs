using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CustomTreeWizard.Categories
{
    public class ContentChangedEventArgs : EventArgs
    {
        public PageItem Content { get; set; }
    }
}
