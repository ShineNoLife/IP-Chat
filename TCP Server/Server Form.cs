using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace TCP_Server
{
    public partial class ServerForm : Form
    {
        private static readonly string clientLogFile = @"D:\Coding Tools\Client Logs.txt";
        private static readonly List<string> clientData = new List<string>();
        static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static readonly List<Socket> clientSockets = new List<Socket>();
        private static List<string> clientNames = new List<string>();
        static int port = 8000;
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
                infoTextBox.Text += $"Server setup complete.";
                portTextBox.Text = port.ToString();
                portTextBox.ReadOnly = true;
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
            clientNames.Add($"Client { clientSockets.IndexOf(socket) }");
            SendAllSockets(clientSockets, $"{ DateTime.Now.ToString("HH:mm").ToLower() }: ***{ clientNames[clientSockets.IndexOf(socket)] } Joined The Chat***");

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
                    serverTextBox.Text += $"{ DateTime.Now.ToString("HH:mm").ToLower() }: ***{ clientNames[clientSockets.IndexOf(current)] } Left The Chat***";
                    serverTextBox.AppendText(Environment.NewLine);
                });

                foreach (Socket client in clientSockets)
                {
                    if (client != current)
                    {
                        byte[] data = Encoding.ASCII.GetBytes($"{ DateTime.Now.ToString("HH:mm").ToLower() }: ***{ clientNames[clientSockets.IndexOf(current)] } Left The Chat***");
                        client.Send(data);
                    }
                }

                clientSockets[clientSockets.IndexOf(current)] = null;
                current.Close();
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf);

            if (text.ToLower() == "/gettime") // Client requested time
            {
                byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString());
                current.Send(data);
            }
            else if (text.Length > 6 && text.Substring(0, 5).ToLower() == "/name")
            {
                if (text.Substring(6) != "")
                {
                    SendAllSockets(clientSockets, $"{ clientNames[clientSockets.IndexOf(current)] } Changed name to { text.Substring(6) }");
                    clientNames[clientSockets.IndexOf(current)] = text.Substring(6);
                }
            }
            else if (text.ToLower() == "/exit") // Client wants to exit
            {
                SendAllSockets(clientSockets, $"{ DateTime.Now.ToString("HH:mm").ToLower() }: ***{ clientNames[clientSockets.IndexOf(current)] } Left The Chat***");
                //Shutdown before closing
                current.Shutdown(SocketShutdown.Both);
                current.Close();
                clientSockets[clientSockets.IndexOf(current)] = null;
                return;
            }
            else
            {
                SendAllSockets(clientSockets, $"{ DateTime.Now.ToString("HH:mm").ToLower() }: +>{ clientNames[clientSockets.IndexOf(current)] }: " + text);
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
                if(client != null)
                    client.Send(data);
            }
        }

        private void portTextBox_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(portTextBox.Text) != 0)
                port = Convert.ToInt32(portTextBox.Text);
            else
                port = 8000;
        }

    }
}
