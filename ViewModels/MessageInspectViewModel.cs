using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using NHapiSampleApplication.nHapi;
using NHapiSampleApplication.Models;

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

        private Patient _Patient;
        public Patient Patient
        {
            get { return _Patient; }
            set
            {
                if (Equals(value, _Patient))
                {
                    return;
                }

                _Patient = value;
                NotifyOfPropertyChange(() => Patient);
            }
        }

        public MessageInspectViewModel()
        {
            Title = "Nothing Selected";
        }

        public void Init(string message)
        {
            var parser = new A01Parser();
            
            var adtMessage = parser.Parse(message);

            if (adtMessage != null)
            {
                Patient = parser.Convert(adtMessage);    
            }
        }
    }
}
