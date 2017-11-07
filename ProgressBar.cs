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
    public partial class InProgressBar : Form
    {
        public static InProgressBar ProgressBarDialog;
       

        public InProgressBar()
        {
            InitializeComponent();
            progressBar1.Style = ProgressBarStyle.Marquee;
            this.Location = new Point(600, 500);
            
            //progressBar1.PerformStep();
        }

        public static void Background_WorkThreadFunction()
        {
            try
            {
                ProgressBarDialog = new InProgressBar();
                ProgressBarDialog.ShowDialog(); // 
            }
            catch
            {


            }
        }
    }
}
