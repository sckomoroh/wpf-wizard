using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CustomTreeWizard.Categories
{
    public class PageItem : UserControl
    {
        public static DependencyProperty PageNameProperty = DependencyProperty.Register(
            "PageName",
            typeof(string),
            typeof(PageItem));

        public static DependencyProperty IsActiveProperty = DependencyProperty.Register(
            "IsActive",
            typeof (bool?),
            typeof(PageItem));

        public EventHandler<ContentChangedEventArgs> ContentChanged;

        public string PageName
        {
            get { return GetValue(PageNameProperty) as string; }
            set { SetValue(PageNameProperty, value); }
        }

        public bool? IsActive
        {
            get { return GetValue(IsActiveProperty) as bool?; }
            set { SetValue(IsActiveProperty, value); }
        }

        public CategoryItem ParentPageItem { get; internal set; }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.Property.Name == "Content")
            {
                if (ContentChanged != null)
                {
                    var newValue = args.NewValue as UIElement;
                    ContentChanged(this, new ContentChangedEventArgs { Content = this });
                }
            }
        }
    }
}
