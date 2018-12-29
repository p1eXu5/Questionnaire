using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.MvvmBase;

namespace Questionnaire.DesktopClient.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private readonly IQuestionRepository _questionRepository;

        public MainViewModel ( IQuestionRepository questionRepository )
        {
            _questionRepository = questionRepository ?? throw new ArgumentNullException( nameof( questionRepository ), @"IQuestionRepository cannot be null." );
        }

        public bool HasQuestions => _questionRepository.HasQuestions;


    }
}
