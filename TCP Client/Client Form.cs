using System;
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
        static Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        string ip = "127.0.0.1"; //Default ip
        public clientForm()
        {
            InitializeComponent();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
        attempConnect:
            IPEndPoint userEndPoint = new IPEndPoint(IPAddress.Parse(ip), 9000);
            try
            {
                clientSocket.Connect(userEndPoint);              
            }
            catch(Exception ex)
            {
                //Give option to try again at defined ip location
                DialogResult error = MessageBox.Show(ex.ToString() + "\r\nDo you want to try again ?", "Error", MessageBoxButtons.YesNo);
                if(error == DialogResult.Yes)
                {
                    goto attempConnect;
                }
            }
        }

        private void ipTextbox_TextChanged(object sender, EventArgs e)
        {
            ip = ipTextbox.Text;
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            if (clientSocket.Connected)
            {
                try
                {
                    //Send current text inside client's textbox to the server's textbox
                    string text = clientTextbox.Text;
                    byte[] data = Encoding.ASCII.GetBytes(text);

                    clientSocket.Send(data);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK);
                }
            }
        }
    }
}
