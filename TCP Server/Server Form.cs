using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TCP_Server
{
    public partial class serverForm : Form
    {
        static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        byte[] buffer = new byte[2048];
        public serverForm()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (!startedCheckBox.Checked)
            {
                try
                {
                    //Open the server
                    serverSocket.Bind(new IPEndPoint(IPAddress.Any, 9000));
                    serverSocket.Listen(100);
                    startedCheckBox.Checked = true;

                    Socket clientSocket = serverSocket.Accept(); //Get client
                    infoTextBox.Text = "Client connected";
                    connectedCheckBox.Checked = true;

                    //Turn info got from client into string and put in server's textbox
                    buffer = new byte[clientSocket.SendBufferSize];
                    int bytesRead = clientSocket.Receive(buffer);
                    if (bytesRead <= 0)
                    {
                        throw new SocketException();
                    }
                    byte[] formatted = new byte[bytesRead];
                    for (int i = 0; i < bytesRead; i++)
                    {
                        formatted[i] = buffer[i];
                    }

                    string strData = Encoding.ASCII.GetString(formatted);
                    serverTextBox.Text = strData;
                }
                catch (Exception ex)
                {
                    connectedCheckBox.Checked = false;
                    infoTextBox.Text = "";
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (startedCheckBox.Checked)
            {
                //Close server application
                serverSocket.Close();
                startedCheckBox.Checked = false;
                connectedCheckBox.Checked = false;
                this.Close();
            }

        }

    }
}
