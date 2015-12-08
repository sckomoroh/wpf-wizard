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
    public partial class WizardContainer
    {
        public WizardContainer()
        {
            WizardPages = new ObservableCollection<object>();

            InitializeComponent();
        }

        public ContainerViewModel ViewModel
        {
            get { return DataContext as ContainerViewModel; }
        }
    }
}
