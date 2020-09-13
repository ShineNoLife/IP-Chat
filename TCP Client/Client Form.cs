﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCP_Client
{
    public partial class clientForm : Form
    {
        private static Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        string ip = "127.0.0.1"; //Default ip
        private const int port = 8000;
        public clientForm()
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
            ReceiveResponse();
        }

        private void ConnectToServer()
        {
            int attempts = 0;

            while (!clientSocket.Connected)
            {
                try
                {
                    attempts++;
                    infoTextBox.Text += $"Connection attempt: { attempts }";
                    IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                    clientSocket.Connect(serverEndPoint);
                }
                catch (SocketException)
                {
                    infoTextBox.Clear();
                }
            }

            infoTextBox.Clear();
            infoTextBox.Text += "Connected";
        }

        private static void Exit()
        {
            SendString("exit"); // Tell the server we are exiting
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            Environment.Exit(0);
        }

        private void SendRequest()
        {
            infoTextBox.AppendText(Environment.NewLine);
            string request = clientTextbox.Text;
            SendString(request);

            if (request.ToLower() == "exit")
            {
                Exit();
            }
        }

        private static void SendString(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }

        private void ReceiveResponse()
        {
            var buffer = new byte[2048];
            int received = clientSocket.Receive(buffer, SocketFlags.None);
            if (received == 0) return;
            byte[] data = new byte[received];
            Array.Copy(buffer, data, received);
            string text = Encoding.ASCII.GetString(data);
            infoTextBox.Text += "+> Server: ";
            infoTextBox.AppendText(Environment.NewLine);
            infoTextBox.Text += text;
        }
    }
}
