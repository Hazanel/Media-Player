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
    public partial class UserCodecSelection : Form
    {
        string mChoosenCodec = "";

        public UserCodecSelection(string[] aComboBoxOptions)
        {
            InitializeComponent();

            foreach (string codec in aComboBoxOptions)
                CodecSelectionComboBox.Items.Add(codec);

        }

        private void OK_button_Click(object sender, EventArgs e)
        {
            mChoosenCodec = CodecSelectionComboBox.Text;
            this.Hide();
        }

        public enDspCodec GetChoosenCodec()
        {

            switch (mChoosenCodec.ToLower())
            {
                case "yuv":
                    return enDspCodec.enDspCodecYUV;
                case "264":
                    return enDspCodec.enDspCodecH264;
                case "263":
                    return enDspCodec.enDspCodecH263;
                case "263plus":
                    return enDspCodec.enDspCodecH263P;
                case "vp8":
                    return enDspCodec.enDspCodecVP8;
                case "g711a":
                    return enDspCodec.enDspCodecG711A;
                case "g711u":
                    return enDspCodec.enDspCodecG711U;
                case "g722":
                    return enDspCodec.enDspCodecG722;
                case "aaclc":
                    return enDspCodec.enDspCodecAACLC;
                case "aacld":
                    return enDspCodec.enDspCodecAACLD;
                case "g7221":
                    return enDspCodec.enDspCodecG7221;
                case "g7221c":
                    return enDspCodec.enDspCodecG7221C;
                case "g729":
                    return enDspCodec.enDspCodecG729;
                case "opus":
                    return enDspCodec.enDspCodecOpus;
                case "raw":
                    return enDspCodec.enDspCodecRaw;
                case "siren14":
                    return enDspCodec.enDspCodecSiren14;
                default:
                    return enDspCodec.enDspCodecNull;

            }           
        }
       
    }
}
