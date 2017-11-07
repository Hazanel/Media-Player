using System.Windows;
namespace DSPMediaPlayer
{
    partial class SettingForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ResolutioncomboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.YUV_HeightTextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.YUV_WidthTextBox = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.PlayFPSTextBox = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.OK_button = new System.Windows.Forms.Button();
            this.Cancel_button = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ResolutioncomboBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.YUV_HeightTextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.YUV_WidthTextBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(303, 130);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "YUV settings";
            // 
            // ResolutioncomboBox
            // 
            this.ResolutioncomboBox.FormattingEnabled = true;
            this.ResolutioncomboBox.Location = new System.Drawing.Point(203, 36);
            this.ResolutioncomboBox.Name = "ResolutioncomboBox";
            this.ResolutioncomboBox.Size = new System.Drawing.Size(94, 21);
            this.ResolutioncomboBox.TabIndex = 14;
            this.ResolutioncomboBox.Visible = false;
            this.ResolutioncomboBox.SelectedIndexChanged += new System.EventHandler(this.ResolutioncomboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "YUV Height:";
            // 
            // YUV_HeightTextBox
            // 
            this.YUV_HeightTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YUV_HeightTextBox.Location = new System.Drawing.Point(98, 86);
            this.YUV_HeightTextBox.Multiline = false;
            this.YUV_HeightTextBox.Name = "YUV_HeightTextBox";
            this.YUV_HeightTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.YUV_HeightTextBox.Size = new System.Drawing.Size(78, 26);
            this.YUV_HeightTextBox.TabIndex = 13;
            this.YUV_HeightTextBox.Text = "720";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "YUV width:";
            // 
            // YUV_WidthTextBox
            // 
            this.YUV_WidthTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YUV_WidthTextBox.Location = new System.Drawing.Point(98, 36);
            this.YUV_WidthTextBox.Multiline = false;
            this.YUV_WidthTextBox.Name = "YUV_WidthTextBox";
            this.YUV_WidthTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.YUV_WidthTextBox.Size = new System.Drawing.Size(78, 26);
            this.YUV_WidthTextBox.TabIndex = 11;
            this.YUV_WidthTextBox.Text = "1280";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.PlayFPSTextBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(12, 173);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(303, 62);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RAW streams";
            // 
            // PlayFPSTextBox
            // 
            this.PlayFPSTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayFPSTextBox.Location = new System.Drawing.Point(98, 18);
            this.PlayFPSTextBox.Multiline = false;
            this.PlayFPSTextBox.Name = "PlayFPSTextBox";
            this.PlayFPSTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.PlayFPSTextBox.Size = new System.Drawing.Size(78, 26);
            this.PlayFPSTextBox.TabIndex = 14;
            this.PlayFPSTextBox.Text = "30";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Playback FPS:";
            // 
            // OK_button
            // 
            this.OK_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OK_button.Location = new System.Drawing.Point(21, 264);
            this.OK_button.Name = "OK_button";
            this.OK_button.Size = new System.Drawing.Size(75, 23);
            this.OK_button.TabIndex = 2;
            this.OK_button.Text = "OK";
            this.OK_button.UseVisualStyleBackColor = true;
            this.OK_button.Click += new System.EventHandler(this.OK_button_Click);
            // 
            // Cancel_button
            // 
            this.Cancel_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancel_button.Location = new System.Drawing.Point(122, 264);
            this.Cancel_button.Name = "Cancel_button";
            this.Cancel_button.Size = new System.Drawing.Size(75, 23);
            this.Cancel_button.TabIndex = 3;
            this.Cancel_button.Text = "Cancel";
            this.Cancel_button.UseVisualStyleBackColor = true;
            this.Cancel_button.Click += new System.EventHandler(this.Cancel_button_Click);
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 299);
            this.Controls.Add(this.Cancel_button);
            this.Controls.Add(this.OK_button);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "SettingForm";
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.RichTextBox YUV_HeightTextBox;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.RichTextBox YUV_WidthTextBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox PlayFPSTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button OK_button;
        private System.Windows.Forms.Button Cancel_button;
        public System.Windows.Forms.ComboBox ResolutioncomboBox;
    }
}