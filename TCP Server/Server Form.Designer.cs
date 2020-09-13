namespace TCP_Server
{
    partial class serverForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serverTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.connectedCheckBox = new System.Windows.Forms.CheckBox();
            this.infoTextBox = new System.Windows.Forms.TextBox();
            this.startedCheckBox = new System.Windows.Forms.CheckBox();
            this.serverRecieveMessage = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // serverTextBox
            // 
            this.serverTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serverTextBox.Location = new System.Drawing.Point(13, 12);
            this.serverTextBox.Multiline = true;
            this.serverTextBox.Name = "serverTextBox";
            this.serverTextBox.ReadOnly = true;
            this.serverTextBox.Size = new System.Drawing.Size(633, 377);
            this.serverTextBox.TabIndex = 0;
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startButton.Location = new System.Drawing.Point(645, 396);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(142, 42);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.stopButton.Location = new System.Drawing.Point(484, 396);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(142, 42);
            this.stopButton.TabIndex = 2;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // connectedCheckBox
            // 
            this.connectedCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connectedCheckBox.AutoSize = true;
            this.connectedCheckBox.Location = new System.Drawing.Point(400, 421);
            this.connectedCheckBox.Name = "connectedCheckBox";
            this.connectedCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.connectedCheckBox.Size = new System.Drawing.Size(78, 17);
            this.connectedCheckBox.TabIndex = 3;
            this.connectedCheckBox.Text = "Connected";
            this.connectedCheckBox.UseVisualStyleBackColor = true;
            // 
            // infoTextBox
            // 
            this.infoTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoTextBox.Location = new System.Drawing.Point(652, 12);
            this.infoTextBox.Multiline = true;
            this.infoTextBox.Name = "infoTextBox";
            this.infoTextBox.ReadOnly = true;
            this.infoTextBox.Size = new System.Drawing.Size(136, 377);
            this.infoTextBox.TabIndex = 5;
            // 
            // startedCheckBox
            // 
            this.startedCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.startedCheckBox.AutoSize = true;
            this.startedCheckBox.Location = new System.Drawing.Point(418, 398);
            this.startedCheckBox.Name = "startedCheckBox";
            this.startedCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.startedCheckBox.Size = new System.Drawing.Size(60, 17);
            this.startedCheckBox.TabIndex = 4;
            this.startedCheckBox.Text = "Started";
            this.startedCheckBox.UseVisualStyleBackColor = true;
            // 
            // serverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.infoTextBox);
            this.Controls.Add(this.startedCheckBox);
            this.Controls.Add(this.connectedCheckBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.serverTextBox);
            this.Name = "serverForm";
            this.Text = "Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.CheckBox connectedCheckBox;
        private System.Windows.Forms.TextBox infoTextBox;
        private System.Windows.Forms.CheckBox startedCheckBox;
        private System.ComponentModel.BackgroundWorker serverRecieveMessage;
        public System.Windows.Forms.TextBox serverTextBox;
    }
}

