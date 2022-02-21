using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TCPChat.Client
{
    public class MainViewModel
    {
        public string IP { get; set; }
        public int Port { get; set; }
        public string Nick { get; set; }
        public string Chat { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        private TcpClient? _client;
        private StreamReader? _reader;
        private StreamWriter? _writer;

        public MainViewModel()
        {
            IP = Dns.GetHostAddresses(Dns.GetHostName()).ToString() ?? "127.0.0.1";
            Port = 5050;
            Nick = "Nick";
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (_client?.Connected == true)
                    { 
                    var line = _reader?.ReadLine();
                        if (line != null)
                        { 
                            Chat += line + "\n";
                        }
                        else
                        {
                            _client.Close();
                            Chat = "Connected error.";
                        }
                    }
                    Task.Delay(10);
                }
            });
        }

        public Task ConnectCommand
        {
            get
            {
                return Task.Run(() =>
                {
                    try
                    {
                        _client = new TcpClient();
                        _client.Connect(IP, Port);
                        _reader = new StreamReader(_client.GetStream());
                        _writer = new StreamWriter(_client.GetStream());
                        _writer.AutoFlush = true;

                        _writer.WriteLine($"Login: {Nick}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
            }
        }

        public Task SendCommand
        {
            get
            {
                return Task.Run(() =>
                {
                    _writer?.WriteLine($"{Nick}: {Message}");
                });
            }
        }
    }
}
