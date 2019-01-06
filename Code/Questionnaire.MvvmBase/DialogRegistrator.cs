using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Questionnaire.MvvmBase
{
    public class DialogRegistrator : IDialogRegistrator
    {
        private readonly Dictionary<Type, Type> _repository;
        private readonly Window _owner;



        public DialogRegistrator( Window owner )
        {
            _owner = owner ?? throw new ArgumentNullException();

            _repository = new Dictionary<Type, Type>();
        }



        public void Register< TViewModel, TView >() 
            where TViewModel : IDialogCloseRequested
            where      TView : IDialog
        {
            _repository[ typeof( TViewModel ) ] = typeof( TView );
        }


        public IDialog GetView< TViewModel >( TViewModel viewModel )
            where TViewModel : IDialogCloseRequested
        {
            if ( _repository.TryGetValue( typeof( TViewModel ), out var viewType ) ) {

                IDialog view = ( IDialog )Activator.CreateInstance( viewType );

                view.DataContext = viewModel;
                view.Owner = _owner;


                EventHandler< CloseRequestedEventArgs > onCloseRequestEventHandler = null;

                onCloseRequestEventHandler = ( sender, args ) =>
                {
                    viewModel.DialogCloseRequested -= onCloseRequestEventHandler;
                    view.DialogResult = args.DialogResult;
                };

                viewModel.DialogCloseRequested += onCloseRequestEventHandler;

                return view;
            }

            return null;
        }
    }
}
