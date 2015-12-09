using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizardTest.WizardContainer.WizardPage;

namespace WizardTest.WizardPages.ViewModel
{
    public class WellcomeViewModel : IPageViewModel
    {
        public event ExecutionStateChanged ExecutionStateChanged;
        
        public string PageName 
        {
            get { return "Wellcome!!!"; }
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
