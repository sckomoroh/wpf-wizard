using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using WizardTest.WizardContainer.ViewModel;

namespace WizardTest.WizardContainer.View
{
    /// <summary>
    /// Interaction logic for WizardContainer.xaml
    /// </summary>
    public partial class WizardContainer : UserControl
    {
        public static DependencyProperty WizardPagesProperty = DependencyProperty.Register(
            "WizardPages",
            typeof (ObservableCollection<object>),
            typeof (WizardContainer),
            new FrameworkPropertyMetadata(null, OnValueChanged));

        public WizardContainer()
        {
            SetValue(WizardPagesProperty, new ObservableCollection<object>());

            WizardPages = new ObservableCollection<object>();

            InitializeComponent();
        }

        public ContainerViewModel ViewModel
        {
            get { return DataContext as ContainerViewModel; }
        }

        public ObservableCollection<object> WizardPages
        {
            get
            {
                return GetValue(WizardPagesProperty) as ObservableCollection<object>;
            }
            set
            {
                SetValue(WizardPagesProperty, value);
            }
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as WizardContainer;
            if (control == null)
            {
                return;
            }

            control.WizardPages = (ObservableCollection<object>)e.NewValue;
            if (control.WizardPages == null)
            {
                return;
            }

            if (e.OldValue != null)
            {
                (e.OldValue as ObservableCollection<object>).CollectionChanged -= control.ListSourceOnCollectionChanged;
            }

            control.WizardPages.CollectionChanged += control.ListSourceOnCollectionChanged;
        }

        private void ListSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
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
    }
}
