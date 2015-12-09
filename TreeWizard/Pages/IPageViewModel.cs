using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomTreeWizard.Pages
{
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
