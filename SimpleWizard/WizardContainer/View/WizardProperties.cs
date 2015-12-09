using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace WizardTest.WizardContainer.View
{
    /// <summary>
    /// Contains the set of depended properties for the WizardContainer
    /// </summary>
    public partial class WizardContainer
    {
        public static DependencyProperty WizardPagesProperty = DependencyProperty.Register(
            "WizardPages",
            typeof (ObservableCollection<object>),
            typeof (WizardContainer),
            new FrameworkPropertyMetadata(null, OnWizardPagesValueChanged));

        public static DependencyProperty StatusBarVisibleProperty = DependencyProperty.Register(
            "StatusBarVisible",
            typeof (bool?),
            typeof (WizardContainer),
            new FrameworkPropertyMetadata(true, OnStatusBarVisibleValueChanged));

        public static DependencyProperty StatusBarWidthProperty = DependencyProperty.Register(
            "StatusBarWidth",
            typeof (int?),
            typeof (WizardContainer),
            new FrameworkPropertyMetadata(null, OnStatusBarWidthValueChanged));

        public ObservableCollection<object> WizardPages
        {
            get { return GetValue(WizardPagesProperty) as ObservableCollection<object>; }
            set { SetValue(WizardPagesProperty, value); }
        }

        public bool? StatusBarVisible
        {
            get { return GetValue(StatusBarVisibleProperty) as bool?; }
            set { SetValue(StatusBarVisibleProperty, value); }
        }

        public int? StatusBarWidth
        {
            get { return GetValue(StatusBarWidthProperty) as int?; }
            set { SetValue(StatusBarWidthProperty, value); }
        }

        private static void OnWizardPagesValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WizardContainer;
            if (control == null)
            {
                return;
            }

            control.WizardPages = (ObservableCollection<object>) e.NewValue;
            if (control.WizardPages == null)
            {
                return;
            }

            var oldValue = e.OldValue as ObservableCollection<object>;
            if (oldValue != null)
            {
                oldValue.CollectionChanged -= control.WizardPagesOnCollectionChanged;
            }

            control.WizardPages.CollectionChanged += control.WizardPagesOnCollectionChanged;
        }

        private void WizardPagesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (notifyCollectionChangedEventArgs.NewItems != null)
            {
                foreach (var newItem in notifyCollectionChangedEventArgs.NewItems)
                {
                    var page = newItem as UserControl;
                    ViewModel.AddWizardPage(page);

                    if (ViewModel.CurrentPageControl == null)
                    {
                        ViewModel.CurrentPageControl = page;
                    }
                }
            }
        }

        private static void OnStatusBarVisibleValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WizardContainer;
            if (control == null)
            {
                return;
            }

            control.ViewModel.IsStatusBarVisible = (bool)e.NewValue;
        }

        private static void OnStatusBarWidthValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WizardContainer;
            if (control == null)
            {
                return;
            }

            control.ViewModel.StatusBarWidth = (int)e.NewValue;
        }
    }
}
