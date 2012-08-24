using System;
using NHapiSampleApplication.Tcp;

namespace NHapiSampleApplication.Models
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
