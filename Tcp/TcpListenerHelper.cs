using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NHapiSampleApplication.Tcp
{
    public class TcpListenerHelper
    {
        public event EventHandler<NotifyEventArgs> Notify;
        private string _IPAddress;
        private int _Port;
        private BackgroundWorker _Worker;
        private TcpListener _Server;

        public void ListenOnThread(string ipAddress, int port)
        {
            _IPAddress = ipAddress;
            _Port = port;

            _Worker = new BackgroundWorker();
            _Worker.DoWork += new DoWorkEventHandler(DoWork);
            _Worker.RunWorkerAsync();
            _Worker.Disposed += new EventHandler(Worker_Disposed);
        }

        void Worker_Disposed(object sender, EventArgs e)
        {
            _Server.Stop();
            OnNotify("Service Stopped", NotifyType.Message);
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            Listen();
        }

        private void Listen()
        {
            try
            {
                IPAddress ip = null;
                IPAddress.TryParse(_IPAddress, out ip);

                var ep = new IPEndPoint(ip, _Port);

                _Server = new TcpListener(ep);

                _Server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                
                // Enter the listening loop.
                while (true)
                {
                    OnNotify("Waiting for a connection... ", NotifyType.Message);

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = _Server.AcceptTcpClient();
                    OnNotify("Connected!", NotifyType.Message);

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;
                    var buffer = new TcpBuffer();
                    
                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        buffer.Add(bytes, i);
                        var messages = buffer.GetMessages(start: (char)11, stop: (char)28);
                        foreach (var message in messages)
                        {
                            OnNotify(message, NotifyType.Received);

                            string returnMessage = "Received Ack";
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(returnMessage);
                            
                            // Send back a response.
                            stream.Write(msg, 0, msg.Length);
                            OnNotify(String.Format("{0}", returnMessage), NotifyType.Sent);
                        }
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (Exception e)
            {
                OnNotify(e.Message, NotifyType.Error);
            }
        }

        public void Stop()
        {
            _Worker.Dispose();
        }

        public void OnNotify(string message, NotifyType type)
        {
            if (Notify != null)
            {
                Notify(this, new NotifyEventArgs(message, type));
            }
        }
    }
}
