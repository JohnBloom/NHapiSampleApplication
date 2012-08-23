using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHapiSampleApplication.Tcp
{
    public class NotifyEventArgs : EventArgs
    {
        public string Message { get; set; }
        public NotifyType Type { get; set; }

        public NotifyEventArgs(string message, NotifyType type)
        {
            Type = type;
            Message = message;
        }
    }
}
