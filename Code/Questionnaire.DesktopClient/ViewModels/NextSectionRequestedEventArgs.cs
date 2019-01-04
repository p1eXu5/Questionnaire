using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.Models;

namespace Questionnaire.DesktopClient.ViewModels
{
    public class NextSectionRequestedEventArgs : EventArgs
    {
        public NextSectionRequestedEventArgs ( IEnumerable< dynamic > answers )
        {
            Answers = answers ?? throw new ArgumentNullException( nameof( answers ), @"Answers cannot be null." );
        }

        public IEnumerable< dynamic > Answers { get; set; }
    }
}
