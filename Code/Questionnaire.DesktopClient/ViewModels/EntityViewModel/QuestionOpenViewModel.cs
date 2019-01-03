using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.Models;
using Questionnaire.MvvmBase;

namespace Questionnaire.DesktopClient.ViewModels.EntityViewModel
{
    public class QuestionOpenViewModel : ViewModel
    {
        private readonly QuestionOpen _question;

        public QuestionOpenViewModel ( QuestionOpen question )
        {
            _question = question ?? throw new ArgumentNullException( nameof( question ), @"Question cannot be null." );
        }
    }
}
