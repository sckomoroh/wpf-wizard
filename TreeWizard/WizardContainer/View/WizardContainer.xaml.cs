using System.Collections.ObjectModel;
using System.Diagnostics;
using CustomTreeWizard.Categories;
using CustomTreeWizard.WizardContainer.ViewModel;

namespace CustomTreeWizard.WizardContainer.View
{
    /// <summary>
    /// Interaction logic for WizardContainer.xaml
    /// </summary>
    public partial class WizardContainer
    {
        public WizardContainer()
        {
            WizardCategoriesPages = new ObservableCollection<object>();

            InitializeComponent();

            Debug.WriteLine("[DEBUG] Init completed.");
        }

        public ContainerViewModel ViewModel
        {
            get
            {
                return DataContext as ContainerViewModel;
            }
        }
    }
}
