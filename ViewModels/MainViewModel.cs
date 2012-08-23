using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.Windows;
using NHapiSampleApplication.Tcp;
using NHapiSampleApplication.ViewModels;

namespace NHapiSampleApplication.ViewModels
{
    public class MainViewModel : Conductor<Screen>.Collection.OneActive
    {
        private TcpListenerHelper _Listener;

        public MessageInspectViewModel MessageInspect { get; set; }

        private BindableCollection<string> _ReceivedMessages;
        public BindableCollection<string> ReceivedMessages
        {
            get { return _ReceivedMessages; }
            set
            {
                if (Equals(value, _ReceivedMessages))
                {
                    return;
                }
                _ReceivedMessages = value;
                NotifyOfPropertyChange(() => ReceivedMessages);
            }
        }

        private BindableCollection<string> _Messages;
        public BindableCollection<string> Messages
        {
            get { return _Messages; }
            set
            {
                if (Equals(value, _Messages))
                {
                    return;
                }
                _Messages = value;
                NotifyOfPropertyChange(() => Messages);
            }
        }

        private BindableCollection<string> _SentMessages;
        public BindableCollection<string> SentMessages
        {
            get { return _SentMessages; }
            set
            {
                if (Equals(value, _SentMessages))
                {
                    return;
                }
                _SentMessages = value;
                NotifyOfPropertyChange(() => SentMessages);
            }
        }

        private string _SelectedReceivedMessage;
        public string SelectedReceivedMessage
        {
            get { return _SelectedReceivedMessage; }
            set
            {
                if (value == _SelectedReceivedMessage)
                {
                    return;
                }

                MessageInspect.Init(value);
                _SelectedReceivedMessage = value;
                NotifyOfPropertyChange(() => SelectedReceivedMessage);
            }
        }

        private string _IPAddress;
        public string IPAddress
        {
            get { return _IPAddress; }
            set
            {
                _IPAddress = value;
                NotifyOfPropertyChange(()=> IPAddress);
            }
        }

        private int? _Port;
        public int? Port
        {
            get { return _Port; }
            set
            {
                _Port = value;
                NotifyOfPropertyChange(() => Port);
            }
        }

        public MainViewModel()
        {
            ReceivedMessages = new BindableCollection<string>();
            SentMessages = new BindableCollection<string>();
            Messages = new BindableCollection<string>();
            MessageInspect = new MessageInspectViewModel();

            _Listener = new TcpListenerHelper();
            _Listener.Notify += new EventHandler<NotifyEventArgs>(Listener_Notify);
        }

        void Listener_Notify(object sender, NotifyEventArgs e)
        {
            if (e.Type == NotifyType.Received )
            {
                ReceivedMessages.Add(e.Message);    
            }
            else if (e.Type == NotifyType.Sent)
            {
                SentMessages.Add(e.Message);
            }
            else
            {
                Messages.Add(e.Message);
            }
            
        }

        public void Listen()
        {
            if (IPAddress == null)
            {
                MessageBox.Show("You must have an Ip address");
                return;
            }

            if (Port == null)
            {
                MessageBox.Show("You must have a port number");
                return;
            }

            _Listener.ListenOnThread(IPAddress, Port.Value);
        }

        public void Stop()
        {
            _Listener.Stop();
        }
    }
}
