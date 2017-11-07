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

   
    
    
    public partial class AnalyzerBitMapSettings_Form : Form
    {
        public DSPMediaPlayer_Form MainForm;
        string[] PitchSkipArr = { "0", "1", "2" };
        public int mPSNR_Threshold { get; set; }
        public int mPitchSkipThreshold { get; set; }
        public int m_CB_BackgroundColor_Value { get; set; }
        public int m_CR_BackgroundColor_Value { get; set; }
     
        public AnalyzerBitMapSettings_Form(DSPMediaPlayer_Form Main_Form)
        {
            InitializeComponent();
            MainForm = Main_Form;
            this.Analyzer_PSNR_TextBox.SelectionAlignment = HorizontalAlignment.Center;
            this.CB_BackgroundColor_TextBox.SelectionAlignment = HorizontalAlignment.Center;
            this.CR_BackgroundColor_TextBox.SelectionAlignment = HorizontalAlignment.Center;

            this.Analyzer_PSNR_TextBox.Text = MainForm.gPSNR_Threshold.ToString();
            mPitchSkipThreshold = MainForm.gPitchSkipThreshold;
            Analyzer_PitchSkip_ComboBox.SelectedItem = mPitchSkipThreshold.ToString();
            CB_BackgroundColor_TextBox.Text = MainForm.gCB_BackgroundColor_Value.ToString();
            CR_BackgroundColor_TextBox.Text = MainForm.gCR_BackgroundColor_Value.ToString();

            foreach (string PitchSkip in PitchSkipArr)
            {
                this.Analyzer_PitchSkip_ComboBox.Items.Add(PitchSkip);
            }
           
        }

        private void OK_button_Click(object sender, EventArgs e)
        {

            mPSNR_Threshold = Convert.ToInt32(Analyzer_PSNR_TextBox.Text);
            if (mPSNR_Threshold < 100)
            {
                mPSNR_Threshold =100;
                Analyzer_PSNR_TextBox.Text = mPSNR_Threshold.ToString();
            }
            m_CB_BackgroundColor_Value = Convert.ToInt32(CB_BackgroundColor_TextBox.Text);
            if (m_CB_BackgroundColor_Value > 255 || m_CB_BackgroundColor_Value < 0)
            {
                m_CB_BackgroundColor_Value = mPSNR_Threshold > 255 ? 255 : 0;
                CB_BackgroundColor_TextBox.Text = m_CB_BackgroundColor_Value.ToString();
            }
            m_CR_BackgroundColor_Value = Convert.ToInt32(CR_BackgroundColor_TextBox.Text);
            if (m_CR_BackgroundColor_Value > 255 || m_CR_BackgroundColor_Value < 0)
            {
                m_CR_BackgroundColor_Value = m_CR_BackgroundColor_Value > 255 ? 255 : 0;
                CR_BackgroundColor_TextBox.Text = m_CR_BackgroundColor_Value.ToString();
            }
            //Hiding the window, because closing it makes the window unaccessible.
            this.Hide();
            this.Parent = null;
            MainForm.AnalyzerSettingsFormClosing();
        }

        private void Analyzer_PitchSkip_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Analyzer_PitchSkip_ComboBox.SelectedIndex > -1)
            {
                mPitchSkipThreshold = Convert.ToInt32(Analyzer_PitchSkip_ComboBox.SelectedItem.ToString());
            }

        }

        private void Cancel_button_Click(object sender, EventArgs e)
        {
            //Hiding the window, because closing it makes the window unaccessible.
            this.Hide();
            this.Parent = null;
        }

        private void AnalyzerBitMapSettings_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Hiding the window, because closing it makes the window unaccessible.
            this.Hide();
            this.Parent = null;
            e.Cancel = true; //hides the form, cancels closing event
        }

       
    }
}
