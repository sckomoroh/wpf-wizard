using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WizardTest.WizardContainer.Common;
using WizardTest.WizardContainer.WizardPage;

namespace WizardTest.WizardContainer.ViewModel
{
    public class ContainerViewModel : INotifyPropertyChanged
    {
        private int _statusBarWidth = 180;
        private bool _statusBarVisible = true;
        private UserControl _currentPage;

        public event PropertyChangedEventHandler PropertyChanged;

        public ContainerViewModel()
        {
            WizardPages = new ObservableCollection<UserControl>();

            NextCommand = new SimpleCommand(NextCommandHandler, CanNavigateNext);
            PreviousCommand = new SimpleCommand(PreviousCommandHandler, CanNavigatePrevious);
            CancelCommand = new SimpleCommand(CancelCommandHandler);

            PageNames = new ObservableCollection<string>();

            FinishVisible = Visibility.Collapsed;
            NextVisible = Visibility.Visible;
        }

        public ICommand NextCommand { get; set; }

        public ICommand PreviousCommand { get; set; }
        
        public ICommand CancelCommand { get; set; }

        public ICommand FinishCommand { get; set; }

        public ObservableCollection<string> PageNames { get; set; }

        public IList<UserControl> WizardPages { get; set; }

        public UserControl CurrentPageControl 
        {
            get { return _currentPage; }

            set
            {
                if (_currentPage != null)
                {
                    GetPageViewModel(_currentPage).ExecutionStateChanged -= OnExecutionStateChanged;
                }

                if (_currentPage != value && value != null)
                {
                    _currentPage = value;
                    GetPageViewModel(_currentPage).ExecutionStateChanged += OnExecutionStateChanged;
                    OnPropertyChanged("CurrentPageControl");
                    OnPropertyChanged("CurrentPageIndex");

                    (PreviousCommand as SimpleCommand).RaiseExecuteStateChanged();
                    (NextCommand as SimpleCommand).RaiseExecuteStateChanged();
                }
            }
        }

        public Visibility FinishVisible { get; private set; }

        public Visibility NextVisible { get; private set; }

        public int CurrentPageIndex
        {
            get { return WizardPages.IndexOf(CurrentPageControl); }
        }

        public int SelectedPageIndex { get; set; }

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

        public void AddWizardPage(UserControl page)
        {
            WizardPages.Add(page);
            PageNames.Add(GetPageViewModel(page).PageName);
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void NextCommandHandler()
        {
            var pageIndex = WizardPages.IndexOf(CurrentPageControl);
            if (pageIndex < WizardPages.Count - 1)
            {
                pageIndex++;
                CurrentPageControl = WizardPages[pageIndex];

                GetPageViewModel(CurrentPageControl).Rollback();
                SelectedPageIndex = pageIndex;
                OnPropertyChanged("SelectedPageIndex");
            }

            pageIndex = WizardPages.IndexOf(CurrentPageControl);
            if (pageIndex == WizardPages.Count - 1)
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

            var pageIndex = WizardPages.IndexOf(CurrentPageControl);
            pageIndex--;
            CurrentPageControl = WizardPages[pageIndex];

            SelectedPageIndex = pageIndex;
            OnPropertyChanged("SelectedPageIndex");

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

            if (WizardPages.Count > 0 && WizardPages.IndexOf(CurrentPageControl) > 0)
            {
                return true;
            }

            return false;
        }

        private bool CanNavigateNext(object o)
        {
            if (CurrentPageControl != null)
            {
                return GetPageViewModel(CurrentPageControl).ExecutionState == ExecutionState.AllowNext;
            }

            return false;
        }

        private void OnExecutionStateChanged(ExecutionState state)
        {
            (NextCommand as SimpleCommand).RaiseExecuteStateChanged();
        }

        private IPageViewModel GetPageViewModel(UserControl control)
        {
            return control.DataContext as IPageViewModel;
        }
    }
}
