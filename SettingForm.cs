using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DSPMediaPlayer
{
    public partial class SettingForm : Form
    {
        public string yuvWidth { get; set; }
        public string yuvHeight { get; set; }
        public string PlayFPS { get; set; }
        public bool   OK_Cancel { get; set; }
        public bool bResChangeWasCanceled { get; set; }
        public DSPMediaPlayer_Form MainForm;

        public SettingForm(DSPMediaPlayer_Form Main_Form)
        {
            InitializeComponent();
            this.PlayFPSTextBox.SelectionAlignment    = HorizontalAlignment.Center;
            this.YUV_WidthTextBox.SelectionAlignment  = HorizontalAlignment.Center;
            this.YUV_HeightTextBox.SelectionAlignment = HorizontalAlignment.Center;
            OK_Cancel = false;
            bResChangeWasCanceled = false;
            MainForm = Main_Form;
        }

        private void OK_button_Click(object sender, EventArgs e)
        {
            yuvWidth    = YUV_WidthTextBox.Text;
            yuvHeight   = YUV_HeightTextBox.Text;
            PlayFPS     = PlayFPSTextBox.Text;
            OK_Cancel = true;
            //Hiding the window, because closing it makes the window unaccessible.
            this.Hide();
            this.Parent = null;
            MainForm.SettingsFormClosing();
            
        }

        private void Cancel_button_Click(object sender, EventArgs e)
        {
            //Hiding the window, because closing it makes the window unaccessible.
            this.Hide();
            this.Parent = null;
        }

        private void SettingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bResChangeWasCanceled = OK_Cancel.Equals(false)? true:false;
            OK_Cancel = false;
            //Hiding the window, because closing it makes the window unaccessible.
            this.Hide();
            this.Parent = null;
            e.Cancel = true; //hides the form, cancels closing event
        }

        private void ResolutioncomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ResolutioncomboBox.SelectedIndex > -1)
            {
                YUV_WidthTextBox.Text   = ResolutioncomboBox.SelectedItem.ToString().Split(new char[] { 'X',' ' })[0].Trim();
                YUV_HeightTextBox.Text = ResolutioncomboBox.SelectedItem.ToString().Split(new char[] { 'X', ' ' })[3];
            }
        }

       
        
    }
}
