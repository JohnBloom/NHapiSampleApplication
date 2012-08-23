using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using NHapiSampleApplication.nHapi;

namespace NHapiSampleApplication.ViewModels
{
    public class MessageInspectViewModel : Screen
    {
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                if (value == _Title)
                {
                    return;
                }
                _Title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }

        public MessageInspectViewModel()
        {
            Title = "Nothing Selected";
        }

        public void Init(string message)
        {
            var parser = new A01Parser();
            
            parser.Parse(message);

            if (parser.Message != null)
            {
                Title = parser.Message.PID.GetPatientName().First().GivenName.ToString();
            }
        }
    }
}
