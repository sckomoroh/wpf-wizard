using System;
using System.Security.Cryptography.X509Certificates;

namespace WizardTest.WizardContainer.WizardPage
{
    public enum ExecutionState
    {
        DisableAll,
        DisableNext,
        AllowNext
    }

    public delegate void ExecutionStateChanged(ExecutionState state);

    public interface IPageViewModel
    {
        event ExecutionStateChanged ExecutionStateChanged;

        string PageName { get; }

        ExecutionState ExecutionState { get; }

        void StopPageExecution();

        void PrepareNext();

        void Rollback();
    }
}
