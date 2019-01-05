﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Questionnaire.MvvmBase
{
    public abstract class DialogViewModel : IDialogCloseRequested
    {
        public ICommand OkCommand => new MvvmCommand(p => { OnDialogRequestClose(this, new CloseRequestedEventArgs(true));});
        public ICommand CancelCommand => new MvvmCommand(p => { OnDialogRequestClose(this, new CloseRequestedEventArgs(false));});

        public virtual void OnDialogRequestClose(object sender, CloseRequestedEventArgs args)
        {
            DialogCloseRequested?.Invoke(sender, args);
        }

        public event EventHandler< CloseRequestedEventArgs > DialogCloseRequested;
    }
}
