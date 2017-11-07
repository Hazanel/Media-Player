namespace DSPMediaPlayer.Audio
{
    partial class WaveControl_UI
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
            this.StartButton = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.waveControl1 = new AudioUtils.WaveControl();
            this.PauseButton = new System.Windows.Forms.Button();
            this.AudioInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(777, 3);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 1;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // StopButton
            // 
            this.StopButton.Location = new System.Drawing.Point(777, 32);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(75, 23);
            this.StopButton.TabIndex = 2;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // waveControl1
            // 
            this.waveControl1.BackColor = System.Drawing.Color.SkyBlue;
            this.waveControl1.Filename = null;
            this.waveControl1.Location = new System.Drawing.Point(3, 0);
            this.waveControl1.Name = "waveControl1";
            this.waveControl1.Size = new System.Drawing.Size(783, 349);
            this.waveControl1.TabIndex = 0;
            // 
            // PauseButton
            // 
            this.PauseButton.Location = new System.Drawing.Point(777, 61);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(75, 23);
            this.PauseButton.TabIndex = 3;
            this.PauseButton.Text = "pause";
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PauseButton.Click += new System.EventHandler(this.PauseButton_Click);
            // 
            // AudioInfo
            // 
            this.AudioInfo.AutoSize = true;
            this.AudioInfo.Location = new System.Drawing.Point(19, 355);
            this.AudioInfo.Name = "AudioInfo";
            this.AudioInfo.Size = new System.Drawing.Size(0, 13);
            this.AudioInfo.TabIndex = 4;
            // 
            // WaveControl_UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.AudioInfo);
            this.Controls.Add(this.PauseButton);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.waveControl1);
            this.Name = "WaveControl_UI";
            this.Size = new System.Drawing.Size(859, 377);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AudioUtils.WaveControl waveControl1;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Button PauseButton;
        private System.Windows.Forms.Label AudioInfo;
    }
}