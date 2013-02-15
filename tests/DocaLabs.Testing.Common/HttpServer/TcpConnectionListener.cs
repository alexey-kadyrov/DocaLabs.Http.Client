using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace DocaLabs.Testing.Common.HttpServer
{
    public abstract class TcpConnectionListener
    {
        volatile bool _stopping;

        TcpListener TcpListener { get; set; }
        Thread Worker { get; set; }
        ManualResetEvent Started { get; set; }

        public int StoppingCheckTimeout { get; set; }

        protected TcpConnectionListener(IPEndPoint endPoint)
        {
            TcpListener = new TcpListener(endPoint);

            Started = new ManualResetEvent(false);

            Worker = new Thread(Listen)
            {
                IsBackground = true
            };

            StoppingCheckTimeout = 70;
        }

        public void Start()
        {
            Worker.Start();
        }

        public bool Stop(int timeout)
        {
            _stopping = true;

            return Worker.Join(timeout);
        }

        public bool WaitForStartListening(int timeout)
        {
            return Started.WaitOne(timeout);
        }

        void Listen()
        {
            Debug.WriteLine(@"Worker started for end point {0}", TcpListener.LocalEndpoint);

            TcpListener.Start();

            Started.Set();

            while (WaitForClientConnection())
            {
                TcpListener.BeginAcceptTcpClient(x => DoProcessConnection(TcpListener.EndAcceptTcpClient(x)), null);
            }

            TcpListener.Stop();

            Started.Reset();

            _stopping = false;

            Debug.WriteLine(@"Worker completed for end point {0}", TcpListener.LocalEndpoint);
        }

        void DoProcessConnection(TcpClient socket)
        {
            try
            {
                ProcessConnection(socket);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception in worker for end point {0}:\n\r{1}", TcpListener.LocalEndpoint, e);
            }
            finally 
            {
                socket.Close();
            }    
        }

        protected abstract void ProcessConnection(TcpClient socket);

        bool WaitForClientConnection()
        {
            while (!TcpListener.Pending())
            {
                if (_stopping)
                    return false;

                Thread.Sleep(StoppingCheckTimeout);
            }

            return true;
        }
    }
}