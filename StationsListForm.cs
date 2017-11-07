using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using SharpPcap;
using SharpPcap.LibPcap;

namespace DSPMediaPlayer
{
    
    public struct StationStreamResult_t
    {
        public string m_StreamFile;
        public string m_FileExtension;

    }
  
    
    public partial class StationsListForm : Form
    {
      

        [DllImport("ether2mf.dll")]
        static extern int Ether2MF(string PcapFile, string OutFile, string CodecType);
        private DSPMediaPlayer_Form MainForm;
        private string OriginalPcapFile;
        private static string PcapDir;
        private static string CodecType;

        public StationsListForm(DSPMediaPlayer_Form Main_Form, string PcapFileDialogFileName, string aPcapDir, string aCodectype)
        {
            InitializeComponent();
            MainForm = Main_Form;
            PcapDir = aPcapDir;
            CodecType = aCodectype;
            this.OnItemSelected += new StationsListForm.OnItemSelectedDelegate(StationsListForm_OnItemSelected);
            this.OnCancel += new StationsListForm.OnCancelDelegate(StationsListForm_OnCancel);
            OriginalPcapFile = PcapFileDialogFileName;
            //StationsList.DrawMode = DrawMode.OwnerDrawVariable;
            //StationsList.DrawItem += new DrawItemEventHandler(listBox_DrawItem);
        }


        /*void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {


            ListBox list = (ListBox)sender;
            if (e.Index > -1)
            {
                object item = list.Items[e.Index];
                e.DrawBackground();
                e.DrawFocusRectangle();
                Brush brush = new SolidBrush(e.ForeColor);
                SizeF size = e.Graphics.MeasureString(item.ToString(), e.Font);
                e.Graphics.DrawString(item.ToString(), e.Font, brush, e.Bounds.Left + (e.Bounds.Width / 2 - size.Width / 2), e.Bounds.Top + (e.Bounds.Height / 2 - size.Height / 2));
            }
        }*/
       
       
        public void StationsListForm_Load(ref List<Single_Traffic_Path> Traffic_Path_List)
        {
            var str = String.Format("IP:                    PORT: ==> IP:                    PORT:               Size:");
            StationsList.Items.Add(str);

            foreach (Single_Traffic_Path station in Traffic_Path_List)
            {
               
                str = String.Format("{0,-20} | {1,-7} | {2,-20} | {3,-7} | {4,15}",
                    station.packetSourceAdress,
                    station.PortSourceAddress,
                    station.packetDestAdress, station.PortDestAddress,
                    station.FileSizeStr);

                StationsList.Items.Add(str);
                station.FileWriter.Close();
  
            }
            Traffic_Path_List.Clear();
        }


        public delegate void OnItemSelectedDelegate(int itemIndex);
        public event OnItemSelectedDelegate OnItemSelected;

        public delegate void OnCancelDelegate();
        public event OnCancelDelegate OnCancel;

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            OnCancel();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (StationsList.SelectedItem != null)
            {
                OnItemSelected(StationsList.SelectedIndex);
            }
        }

        private void deviceList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (StationsList.SelectedItem != null)
            {
                OnItemSelected(StationsList.SelectedIndex);
            }
        }

        void StationsListForm_OnCancel()
        {
            StationsList.Items.Clear();
            this.Hide();
            string[] fileNames = Directory.GetFiles(PcapDir);

            foreach (string FileName in fileNames)
            {

                if (Path.GetExtension(FileName).Replace(".", "").ToLower().Equals("pcap"))
                {
                    try
                    {
                      
                      
                        File.Delete(FileName);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Caught exception when deleting file" + e.ToString());
                        return;
                    }
                    
                }

            }
            Directory.Delete(PcapDir, true);
            
        }

        public void StationsListForm_OnItemSelected(int itemIndex)
        {
            if (itemIndex.Equals(0)) return;
            // close the Stations list form
            this.Hide();
            DSPMediaPlayer_Form.Initiate_ProgressBarThread();                                                                                                 //0-3- 4 first string cells that consist the file name
            string PcapFileName = PcapDir+'\\'+string.Join("_", this.StationsList.SelectedItem.ToString().Split(new char[] { ' ', '|' }, StringSplitOptions.RemoveEmptyEntries), 0, 4) + ".pcap";

          
            Thread.CurrentThread.IsBackground = true;
            //Thread Ether2MFThread = new Thread(() => Ether2MF(PcapFileName, PcapFileName + ".out"));
            //Ether2MFThread.Start();
            //Ether2MFThread.Join();
            Ether2MF(PcapFileName, PcapFileName + ".out", CodecType);

            string[] fileNames = Directory.GetFiles(PcapDir);

            foreach (string FileName in fileNames)
            {
                if (FileName.Equals(OriginalPcapFile))
                {
                    continue;
                }
                
                if (FileName.Contains(PcapFileName + ".out"))
                {
                    if (/*!Path.GetExtension(FileName).Replace(".", "").ToLower().Equals("txt")
                    //    &&
                    //    !Path.GetExtension(FileName).Replace(".", "").ToLower().Equals("raw")
                    //    &&
                    //    !Path.GetExtension(FileName).Replace(".", "").ToLower().Equals("rtp")
                    //    &&*/
                        Path.GetExtension(FileName).Replace(".", "").ToLower().Equals("dat"))
                    {

                        MainForm.updateStationStreamResult(FileName, Path.GetExtension(FileName).Replace(".", "").ToLower());
                    }
                    else
                    {
                        if (!IsFileLocked(FileName))//Be on the safe side - might encounter file attributes condition, so it the worst case we won't delete file which is locked by other thread
                        {
                            File.SetAttributes(FileName, FileAttributes.Normal);
                            File.Delete(FileName);
                        }
                    }

                }
                else
                {
                    if (Path.GetExtension(FileName).Replace(".", "").ToLower().Equals("pcap"))
                    {
                        if (!IsFileLocked(FileName))//Be on the safe side - might encounter file attributes condition, so it the worst case we won't delete file which is locked by other thread
                        {
                            File.SetAttributes(FileName, FileAttributes.Normal);
                            File.Delete(FileName);
                        }
                    }
                }
            }
           
        }
        public bool IsFileLocked(string filePath)
        {
            try
            {
                using (File.Open(filePath, FileMode.Open)) { }
            }
            catch (IOException e)
            {
                var errorCode = Marshal.GetHRForException(e) & ((1 << 16) - 1);

                return errorCode == 32 || errorCode == 33;
            }

            return false;
        }
    }
}
