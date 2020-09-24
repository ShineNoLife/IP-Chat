using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace TCP_Client
{
    public partial class ClientForm : Form
    {
        private static Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        string ip = "127.0.0.1"; //Default ip
        private int port = 8000;
        static readonly int bufferSize = 2048;
        static byte[] buffer = new byte[bufferSize];
        public ClientForm()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            ConnectToServer();
        }

        private void IpTextbox_TextChanged(object sender, EventArgs e)
        {
            ip = ipTextbox.Text;
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            SendRequest();
            clientTextbox.Clear();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            if (clientSocket.Connected)
                Exit();
            else
            {
                clientSocket.Close();
                Environment.Exit(0);
            }
        }

        private void ConnectToServer()
        {
            int attempts = 0;

            while (!clientSocket.Connected)
            {
                attemptConnect:
                try
                {
                    attempts++;
                    infoTextBox.Text += $"Connection attempt: { attempts }";
                    IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                    clientSocket.Connect(serverEndPoint);
                }
                catch (SocketException ex)
                {
                    DialogResult answer = MessageBox.Show(ex.ToString() + "Do you want to try again ?", "Error", MessageBoxButtons.YesNo);
                    if (answer == DialogResult.Yes)
                        goto attemptConnect;
                    else
                        return;
                }
            }

            infoTextBox.Clear();
            portTextBox.ReadOnly = true;
            ipTextbox.ReadOnly = true;
            infoTextBox.Text += "Connected";
            infoTextBox.AppendText(Environment.NewLine);
            portTextBox.Text = port.ToString();
            ipTextbox.Text = ip.ToString();
            clientSocket.BeginReceive(buffer, 0, bufferSize, SocketFlags.None, ReceiveCallBack, clientSocket);
        }

        private static void Exit()
        {
            SendString("/exit"); // Tell the server we are exiting
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            Environment.Exit(0);
        }

        private void SendRequest()
        {
            string request = clientTextbox.Text;
            SendString(request);

            if (request.ToLower() == "/exit")
            {
                Exit();
            }
        }

        private static void SendString(string text)
        {
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(text);
                clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error occured while attempting to send message", MessageBoxButtons.OK);
            }
        }

        private void ReceiveCallBack(IAsyncResult AR)
        {
            Socket client = (Socket)AR.AsyncState;
            int received = 0;
            try
            {
                received = client.EndReceive(AR);
            }
            catch
            {
                return;
            }

            byte[] data = new byte[received];
            Array.Copy(buffer, data, received);
            string text = Encoding.ASCII.GetString(data);

            clientTextbox.Invoke((Action)delegate
            {
                infoTextBox.Text += text;
                infoTextBox.AppendText(Environment.NewLine);
            });

            if(client.Connected)
                client.BeginReceive(buffer, 0, bufferSize, SocketFlags.None,  ReceiveCallBack, client);
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
