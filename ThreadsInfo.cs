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
    public partial class ThreadsInfo : Form
    {

        public string FrameQueueHead { get; set; }
        public string FrameQueueTail { get; set; }
        public string DisplayQueue { get; set; }
        public string TasksQueue { get; set; }
        public bool OK_Cancel { get; set; }

        public ThreadsInfo()
        {
            InitializeComponent();
            this.FrameQueueHead_TextBox.SelectionAlignment  = HorizontalAlignment.Center;
            this.FrameQueueTail_TextBox.SelectionAlignment  = HorizontalAlignment.Center;
            this.DisplayQueue_TextBox.SelectionAlignment    = HorizontalAlignment.Center;
            this.TasksQueue_TextBox.SelectionAlignment      = HorizontalAlignment.Center;
            OK_Cancel = false;
        }

        private void OK_button_Click(object sender, EventArgs e)
        {
            //Hiding the window, because closing it makes the window unaccessible.
            this.Hide();
            this.Parent = null;
            
        }

        private void ThreadsInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Hiding the window, because closing it makes the window unaccessible.
            this.Hide();
            this.Parent = null;
            e.Cancel = true; //hides the form, cancels closing event
        }

       
    }
}
