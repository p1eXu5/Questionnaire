using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.MvvmBase
{
    public interface IDialogRegistrator
    {
        void Register< TViewModel, TView > () 
            where TViewModel : IDialogCloseRequested
            where TView : IDialog;

        IDialog GetView< TViewModel > ( TViewModel viewModel )
            where TViewModel : IDialogCloseRequested;
    }
}
