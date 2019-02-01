using System;
using Questionnaire.Data.Models;
using Questionnaire.MvvmBase;

namespace Questionnaire.DesktopClient.ViewModels.EntityViewModel
{
    public class FirmViewModel : ViewModel
    {
        private readonly Firm _firm;
        private readonly IMainViewModel _mainViewModel;

        public FirmViewModel ( Firm firm, IMainViewModel mainViewModel )
        {
            _firm = firm ?? throw new ArgumentNullException( nameof( firm ), @"Firm cannot be null.");
            _mainViewModel = mainViewModel ?? throw new ArgumentNullException( nameof( mainViewModel ), @"IMainViewModel cannot be null.");
        }

        public Firm Firm => _firm;

        public int Id => _firm.Id;
        public string Name => $"{_firm.Name} {GetEmployeeCount()}";
        public int CityId => _firm.CityId;

        public void UpdateName ()
        {
            OnPropertyChanged( nameof( Name ) );
        }

        private string GetEmployeeCount ()
        {
            var nextEmployeeNum = _mainViewModel.Context.GetNextNumOfTested( _firm.Id );

            return nextEmployeeNum <= 1
                        ? ""
                        : $"({(nextEmployeeNum - 1).ToString()})";
        }


    }
}
