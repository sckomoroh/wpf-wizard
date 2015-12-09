using System.Collections.Generic;
using System.Windows.Controls;
using WizardTest.WizardContainer.WizardPage;

namespace WizardTest.WizardContainer.ViewModel
{
    public class TreeViewWizardItem
    {
        public TreeViewWizardItem(UserControl control, TreeViewWizardItem parent)
        {
            PageControl = control;
            PageViewModel = control.DataContext as IPageViewModel;
            Parent = parent;

            Children = new List<TreeViewWizardItem>();
        }

        public string PageName
        {
            get { return PageViewModel.PageName; }
            
        }

        public TreeViewWizardItem Parent { get; private set; }

        public IPageViewModel PageViewModel { get; private set; }

        public UserControl PageControl { get; private set; }

        public IList<TreeViewWizardItem> Children { get; private set; }
    }
}
