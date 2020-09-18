using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TCP_Server
{
    public partial class ServerForm : Form
    {
        private static readonly string clientLogFile = @"D:\Coding Tools\Client Logs.txt";
        private static readonly List<string> clientData = new List<string>();
        static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static readonly List<Socket> clientSockets = new List<Socket>();
        static readonly int port = 8000;
        static readonly int bufferSize = 2048;
        static byte[] buffer = new byte[bufferSize];
        public ServerForm()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            SetupServer();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            CloseAllSockets();
        }

        private void SetupServer()
        {
            if (!startedCheckBox.Checked)
            {
                infoTextBox.Text += "Setting up server...";
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                serverSocket.Listen(0);
                serverSocket.BeginAccept(AcceptCallback, null);
                infoTextBox.AppendText(Environment.NewLine);
                infoTextBox.Text += "Server setup complete";
                startedCheckBox.Checked = true;
            }
        }

        private void CloseAllSockets()
        {
            foreach (Socket socket in clientSockets)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }

            serverSocket.Close();
            this.Close();
        }

        private void AcceptCallback(IAsyncResult AR)
        {
            Socket socket;

            try
            {
                socket = serverSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException)
            {
                return;
            }

            clientSockets.Add(socket);
            SendAllSockets(clientSockets, $"***Client { clientSockets.IndexOf(socket) } Joined The Chat***");

            socket.BeginReceive(buffer, 0, bufferSize, SocketFlags.None, ReceiveCallback, socket);
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        private void ReceiveCallback(IAsyncResult AR)
        {
            Socket current = (Socket)AR.AsyncState;
            int received;

            try
            {
                received = current.EndReceive(AR);
            }
            catch (Exception ex)
            {
                serverTextBox.Invoke((Action)delegate
                {
                    serverTextBox.Text += $"***Client { clientSockets.IndexOf(current) } Left The Chat***";
                    serverTextBox.AppendText(Environment.NewLine);
                });

                foreach (Socket client in clientSockets)
                {
                    if (client != current)
                    {
                        byte[] data = Encoding.ASCII.GetBytes($"***Client { clientSockets.IndexOf(current) } Left The Chat***");
                        client.Send(data);
                    }
                }

                clientSockets.Remove(current);
                current.Close();
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf);

            if (text.ToLower() == "get time") // Client requested time
            {
                byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString());
                current.Send(data);
            }
            else if (text.ToLower() == "/exit") // Client wants to exit
            {
                SendAllSockets(clientSockets, $"***Client { clientSockets.IndexOf(current) } Left The Chat***");
                //Shutdown before closing
                current.Shutdown(SocketShutdown.Both);
                current.Close();
                clientSockets.Remove(current);
                return;
            }
            else
            {
                clientData.Add($"+>Client { clientSockets.IndexOf(current) }: " + text);
                SendAllSockets(clientSockets, $"+>Client { clientSockets.IndexOf(current) }: " + text);
            }          
            File.WriteAllLines(clientLogFile, clientData);

            current.BeginReceive(buffer, 0, bufferSize, SocketFlags.None, ReceiveCallback, current);
        }

        private void SendAllSockets(List<Socket> clientSockets, string text)
        {
            clientData.Add(text);
            File.WriteAllLines(clientLogFile, clientData);
            serverTextBox.Invoke((Action)delegate
            {
                serverTextBox.Text += text;
                serverTextBox.AppendText(Environment.NewLine);
            });

            foreach (Socket client in clientSockets)
            {
                byte[] data = Encoding.ASCII.GetBytes(text);
                client.Send(data);
            }
        }
    }
}
