using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizardTest.WizardContainer.WizardPage;

namespace WizardTest.WizardPages.ViewModel
{
    public class FinishViewModel : IPageViewModel
    {
        public event ExecutionStateChanged ExecutionStateChanged;

        public string PageName
        {
            get { return "FINISH!"; }
        }

        public ExecutionState ExecutionState
        {
            get
            {
                return ExecutionState.AllowNext;
            }
        }
        
        public void StopPageExecution()
        {
        }

        public void PrepareNext()
        {
        }

        public void Rollback()
        {
        }
    }
}
