using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CustomTreeWizard.Categories;
using CustomTreeWizard.Common;
using CustomTreeWizard.Pages;

namespace CustomTreeWizard.WizardContainer.ViewModel
{
    public class ContainerViewModel : INotifyPropertyChanged
    {
        private int _statusBarWidth = 180;
        private bool _statusBarVisible = true;

        private PageItem _currentPageControl;

        public event PropertyChangedEventHandler PropertyChanged;

        public ContainerViewModel()
        {
            WizardPages = new ObservableCollection<PageItem>();

            NextCommand = new SimpleCommand(NextCommandHandler, CanNavigateNext);
            PreviousCommand = new SimpleCommand(PreviousCommandHandler, CanNavigatePrevious);
            CancelCommand = new SimpleCommand(CancelCommandHandler);

            FinishVisible = Visibility.Collapsed;
            NextVisible = Visibility.Visible;
        }

        public ICommand NextCommand { get; set; }

        public ICommand PreviousCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public ICommand FinishCommand { get; set; }

        public PageItem CurrentPageControl
        {
            get
            {
                return _currentPageControl;
            }

            set
            {
                if (_currentPageControl != null)
                {
                    GetPageViewModel(_currentPageControl).ExecutionStateChanged -= OnExecutionStateChanged;
                }

                if (_currentPageControl != value && value != null)
                {
                    _currentPageControl = value;
                    _currentPageControl.IsActive = true;
                    GetPageViewModel(_currentPageControl).ExecutionStateChanged += OnExecutionStateChanged;
                    OnPropertyChanged("CurrentPageControl");
                    OnPropertyChanged("CurrentPageIndex");

                    (PreviousCommand as SimpleCommand).RaiseExecuteStateChanged();
                    (NextCommand as SimpleCommand).RaiseExecuteStateChanged();
                }
            }
        }

        public bool IsStatusBarVisible
        {
            get { return _statusBarVisible; }

            set
            {
                if (_statusBarVisible != value)
                {
                    _statusBarVisible = value;
                    OnPropertyChanged("IsStatusBarVisible");
                }
            }
        }

        public int StatusBarWidth
        {
            get { return _statusBarVisible ? _statusBarWidth : 0; }

            set
            {
                if (_statusBarWidth != value)
                {
                    _statusBarWidth = value;
                    OnPropertyChanged("StatusBarWidth");
                }
            }
        }

        public ObservableCollection<PageItem> WizardPages { get; set; }

        public Visibility FinishVisible { get; private set; }

        public Visibility NextVisible { get; private set; }

        public int SelectedPageIndex { get; set; }

        public void OnItemContentChanged(object sender, ContentChangedEventArgs contentChangedEventArgs)
        {
            if (CurrentPageControl == null && contentChangedEventArgs.Content != null)
            {
                CurrentPageControl = contentChangedEventArgs.Content;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void NextCommandHandler()
        {
            var page = FindNextPage(CurrentPageControl);
            if (page != null)
            {
                GetPageViewModel(page).Rollback();
                CurrentPageControl = page;
            }

            if (IsLastPage(page))
            {
                FinishVisible = Visibility.Visible;
                NextVisible = Visibility.Collapsed;

                OnPropertyChanged("FinishVisible");
                OnPropertyChanged("NextVisible");
            }
        }

        private void PreviousCommandHandler()
        {
            GetPageViewModel(CurrentPageControl).Rollback();

            CurrentPageControl = GetPreviousPage(CurrentPageControl);

            if (FinishVisible == Visibility.Visible)
            {
                FinishVisible = Visibility.Collapsed;
                NextVisible = Visibility.Visible;

                OnPropertyChanged("FinishVisible");
                OnPropertyChanged("NextVisible");
            }
        }

        private void CancelCommandHandler()
        {
            if (CurrentPageControl != null)
            {
                GetPageViewModel(CurrentPageControl).StopPageExecution();
            }
        }

        private bool CanNavigatePrevious(object o)
        {
            if (CurrentPageControl != null && GetPageViewModel(CurrentPageControl).ExecutionState == ExecutionState.DisableAll)
            {
                return false;
            }

            var firstCategory = WizardPages.FirstOrDefault() as CategoryItem;
            if (firstCategory == null)
            {
                return false;
            }

            var firstPage = firstCategory.WizardCategoriesPages.FirstOrDefault();
            if (firstPage == null || firstPage.Equals(CurrentPageControl))
            {
                return false;
            }

            return true;
        }

        private bool CanNavigateNext(object o)
        {
            if (CurrentPageControl != null)
            {
                return GetPageViewModel(CurrentPageControl).ExecutionState == ExecutionState.AllowNext;
            }

            return false;
        }

        private IPageViewModel GetPageViewModel(PageItem control)
        {
            var viewModel = control.DataContext as IPageViewModel;
            if (viewModel == null)
            {
                viewModel = (control.Content as FrameworkElement).DataContext as IPageViewModel;
            }

            return viewModel;
        }

        private void OnExecutionStateChanged(ExecutionState state)
        {
            (NextCommand as SimpleCommand).RaiseExecuteStateChanged();
        }

        private PageItem FindNextPage(PageItem page)
        {
            var index = page.ParentPageItem.WizardCategoriesPages.IndexOf(page);
            if (index == -1)
            {
                return null;
            }

            if (index < page.ParentPageItem.WizardCategoriesPages.Count - 1)
            {
                return page.ParentPageItem.WizardCategoriesPages[index + 1];
            }

            var parentIndex = WizardPages.IndexOf(page.ParentPageItem);

            return (WizardPages[parentIndex+1] as CategoryItem).WizardCategoriesPages.First();
        }

        private bool IsLastPage(PageItem page)
        {
            var lastPage = (WizardPages.Last() as CategoryItem).WizardCategoriesPages.Last();

            return lastPage.Equals(page);
        }

        private PageItem GetPreviousPage(PageItem page)
        {
            var index = page.ParentPageItem.WizardCategoriesPages.IndexOf(page);
            if (index == -1)
            {
                return null;
            }

            if (index > 0)
            {
                return page.ParentPageItem.WizardCategoriesPages[index - 1];
            }

            var parentIndex = WizardPages.IndexOf(page.ParentPageItem);

            return (WizardPages[parentIndex - 1] as CategoryItem).WizardCategoriesPages.Last();
        }
    }
}
