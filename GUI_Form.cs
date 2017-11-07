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

  
using System.Drawing.Drawing2D;
using System.Collections;
using System.Security.AccessControl;
using System.Security.Principal;
using Ini;
 
 
namespace DSPMediaPlayer
{
    #region enums

    public enum MovieState
    {
        enPlayer_Stop,
        enPlayer_Play,
        enPlayer_Idle,
        enPlayer_Pause,
        enPlayer_Forward,
        enPlayer_Rewind
    };
    public enum ViewMode
    {
        enRegular,
        enOrigFrameSize
    };
    public  enum Processing_Type
    {
        enVideo,
        enAudio,
        enTerminate
    };
    public enum Process_Operation
    {
        enDecoder = 0,
        enEncoder 
    };
    public enum Threads
    {
        enProcessThread,
        enDisplayThread
    };

    public enum pcap
    {
        enAnalyzeStream,
        enProcessSelectedStream
    };
    #endregion enums

    #region structs

    public   struct  Process_To_Display_Queue
    {
        //common members (between all process types)
        public Processing_Type  m_Processing_Type;
        public bool             mb_valid;

        //Video members
        public Bitmap           m_Pic;
        public bool             m_bIsPicValid;
        public UInt32           m_ChannelHandle;
        public int              m_Number;
        public string           m_FrameType;
        public UInt32           m_FrameTimeStamp;
        public int              m_FrameFPS;
        public UInt32           m_Profile_Indication;
        public bool             mb_IsFrameIntra;
        public UInt32           m_NumFramesUntilIntra;
        public bool             mb_Avc_Tsvc_GopStruct;
        public string           m_SharpIndication;
        public string           m_Frame_Resolution;
        public t_ExtraSolutionInfo mExtraInfoObj;

        //audio members
        public string m_RawAudioFileName;
        public int m_sampleRate;
        public int m_bitRate;

        public Process_To_Display_Queue(Processing_Type type, bool Valid, Bitmap bit,bool a_bIsPicValid,
                                        UInt32 ChannelHandle, int aNumber,
                                        string aFrameType, UInt32 aFrameTimeStamp,
                                        bool ab_IsFrameIntra, UInt32 aNumFramesUntilIntra,
                                        bool ab_Avc_Tsvc_GopStruct, string aFrameResolution,
                                        UInt32 aProfileIndication,
                                        t_ExtraSolutionInfo aExtraInfoObj,
                                        string aSharpIndication)
        {
            this.m_Processing_Type      = type;
            this.mb_valid               = Valid;
            this.m_Pic                  = bit;
            this.m_bIsPicValid          = a_bIsPicValid;
            this.m_ChannelHandle        = ChannelHandle;
            this.m_Number               = aNumber;
            this.m_FrameType            = aFrameType;
            this.m_FrameTimeStamp       = aFrameTimeStamp;
            this.m_FrameFPS             = 0;
            this.mb_IsFrameIntra        = ab_IsFrameIntra;
            this.m_NumFramesUntilIntra  = aNumFramesUntilIntra;
            this.mb_Avc_Tsvc_GopStruct  = ab_Avc_Tsvc_GopStruct;
            this.m_Frame_Resolution     = aFrameResolution;
            this.m_Profile_Indication   = aProfileIndication;
            this.mExtraInfoObj          = aExtraInfoObj;
            this.m_RawAudioFileName     = "";
            this.m_SharpIndication      = aSharpIndication;
            m_sampleRate = 0;
            m_bitRate = 0;

        }


        public Process_To_Display_Queue(Processing_Type type, string aRawAudioFileName, int aSampleRate, int abitRate)
        {
            this.m_Processing_Type = type;
            this.mb_valid = false;
            this.m_Pic = null;
            this.m_bIsPicValid = false;
            this.m_ChannelHandle = 0;
            this.m_Number = 0;
            this.m_FrameType = "";
            this.m_FrameTimeStamp = 0;
            this.m_FrameFPS = 0;
            this.mb_IsFrameIntra = false;
            this.m_NumFramesUntilIntra = 0;
            this.mb_Avc_Tsvc_GopStruct = false;
            this.m_Frame_Resolution = "";
            this.m_Profile_Indication = 0;
            this.mExtraInfoObj = new t_ExtraSolutionInfo();
            this.m_SharpIndication = "";
            this.m_RawAudioFileName = aRawAudioFileName;
            m_sampleRate = aSampleRate;
            m_bitRate = abitRate;
        }
    };

    public struct Form_To_Process_Queue
    {

        public string m_FileName;
        public bool   mb_valid;
        public enDspCodec m_CodecType;
        public Form_To_Process_Queue(string FileName, bool Valid, enDspCodec codecType)
        {
            this.m_FileName   = FileName;
            this.mb_valid     = Valid;
            this.m_CodecType = codecType; 
        }
    };
    public struct res
    {
        public int width;
        public int Height;

        public res(int width, int Height)
        {
            this.width  = width;
            this.Height = Height;
        }
    };
    
 

    #endregion structs

   
    public partial class DSPMediaPlayer_Form : Form
    {

        #region globals
        string ApplicationVersion = "2016_Nov_07 - 0.0.0.12";

        private const UInt32 MAX_STRING_LENGTH = 1000;
        private char[] mTempCharBuffer = new char[MAX_STRING_LENGTH];       
      
        public Queue<Process_To_Display_Queue> g_Process_To_Display_Queue = new Queue<Process_To_Display_Queue>();
        public Queue<Form_To_Process_Queue> g_Form_To_Process_Queue = new Queue<Form_To_Process_Queue>();


        public const int Frames_Queue_Buf_Size = 30;
        public Process_To_Display_Queue[] FramesQueue = new Process_To_Display_Queue[Frames_Queue_Buf_Size];
       
        public Boolean ManualFrameTransmittion = false;
        public Thread[] threadsArray = new Thread[2];

        public static ViewMode View_Mode = ViewMode.enRegular;
        public  MovieState Movie_State = MovieState.enPlayer_Idle;

        public static string DSP_MEDIA_PLAYER_WORKING_FOLDER = @"c:\temp\DSPMediaPlayer";
        public static string DSP_MEDIA_PLAYER_VIDEO_WORKING_FOLDER = @"c:\temp\DSPMediaPlayer\video";
        public static string DSP_MEDIA_PLAYER_AUDIO_WORKING_FOLDER = @"c:\temp\DSPMediaPlayer\audio";
        public static string DSP_MEDIA_PLAYER_SHARPFRAMES_WORKING_FOLDER = @"c:\temp\DSPMediaPlayer\SharpFrames";
        public static string DSP_MEDIA_PLAYER_WEBC_WORKING_FOLDER = @"c:\temp\Webc_in";
        public string[] DspMediaPlayerWorkingFolderArray = new string[4]{DSP_MEDIA_PLAYER_VIDEO_WORKING_FOLDER,
                                                                         DSP_MEDIA_PLAYER_AUDIO_WORKING_FOLDER,
                                                                         DSP_MEDIA_PLAYER_SHARPFRAMES_WORKING_FOLDER,
                                                                         DSP_MEDIA_PLAYER_WEBC_WORKING_FOLDER};
        public string CurrSharpFramesFolderName = "babcafe";

        SettingForm settingsForm;
        ThreadsInfo ThreadsInfoForm = new ThreadsInfo();
       
#region Analyaer
        public const int PSNR_Threshold_Default = 600;
        public const int PitchSkipThreshold_Default = 0;
        public const int CB_BackgroundColor_Value_Default = 114;
        public const int CR_BackgroundColor_Value_Default = 80;
        AnalyzerBitMapSettings_Form AnalyzerSettings;
        public int gPSNR_Threshold = PSNR_Threshold_Default;
        public int gPitchSkipThreshold = PitchSkipThreshold_Default;
        public int gCB_BackgroundColor_Value = CB_BackgroundColor_Value_Default;
        public int gCR_BackgroundColor_Value = CR_BackgroundColor_Value_Default;
#endregion
       

        
        public int DefaultFpsPlay = 30;
        public enDspCodec meCALCodecType = enDspCodec.enDspCodecNull;
        public enDspCodec meInputCodecType= enDspCodec.enDspCodecNull;
        public FileStream fsSource;
        public  long filePos = 0;
        public  long filePosOffset = 0;
        public int CurrentFrameCount = -1;
        public int FrameQueueTail = -1;
        public int FrameQueueHead = -1;

        /// <summary>
        /// Getting rid of flickering!
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED  - Handle double buffer while painting the frame      
                return handleParam;
            }
        }
        public string FileName = " ";
        public string VideoWidth = null;
        public string VideoHeight = null;
        public Form_To_Process_Queue g_Last_Queue_Task;

        public enThreadsState_t g_ProcessThreadState = enThreadsState_t.enThreadstate_Not_Initialized;
        public enThreadsState_t g_DisplayThreadState = enThreadsState_t.enThreadstate_Not_Initialized;
        public UInt32 ChannelHandle = 0;

        public struct InfoLabels
        {
            public Label m_CodecType;
            public Label m_Resolution;
            public Label m_FrameNum;
            public Label m_FrameType;
            public Label m_CurrFPS;
            public Label m_FrameTimeStamp;
            public Label m_NumFramesUntilIntra;
            public Label m_Frame_GopStructure;
            public Label m_Profile_Indication;
            public Label m_Sharp_Indication;
            /*
             more displayable labels to be come...
             */

        };
        public InfoLabels InfoLabelsObj = new InfoLabels();

        public string InfoLabelStringPattern = "";

        public static Thread pcapListenerThreadObj;

        public StationsListForm StationsListFormObj;

        public static bool Indication = false;

        public StationStreamResult_t StationStreamResult;

        public static Thread ProgressBarObj;

        public Process_Operation enProcessOp = Process_Operation.enDecoder;

        public Processing_Type meProcessType = Processing_Type.enVideo;


        public static string PcapDir = Directory.GetCurrentDirectory()+"\\PcapDir";
        static string MediaPlayer_INI_LogFile = @"\\rvnx-storage\nbu\dsp\utils\DSPMediaPlayer\Logs\MediaPlayer_Report_" + Environment.UserName + ".ini";
        IniFile IniWriter = new Ini.IniFile(MediaPlayer_INI_LogFile);

        #endregion globals

        public DSPMediaPlayer_Form()
        {
            string pathvar = System.Environment.GetEnvironmentVariable("PATH");
            System.Environment.SetEnvironmentVariable("PATH", pathvar + @";R:\build\dev\libs\intel-ipp\9.0.3.210\windows\ipp\lib\intel64;R:\build\dev\libs\intel-ipp\9.0.3.210\windows\redist\intel64\ipp;R:\build\dev\libs\intel-ipp\9.0.3.210\windows\redist\intel64\compiler");


            InitializeComponent();
            ResizeRedraw = true;//Redraw the form correctly 
            InitializeLabelsArray();//Temp - until we set up something better logically
            InitializeThreads();
            settingsForm = new SettingForm(this);
            AnalyzerSettings = new AnalyzerBitMapSettings_Form(this);

            versionLabel.Text = "Version = " + ApplicationVersion;
            versionLabel.BackColor = System.Drawing.Color.Cyan;

            updateMainFormAccordingToProcessType(Processing_Type.enVideo);


            //default values
            Linear16000.Checked = true;
            extraInfoToolStripMenuItem.Checked = true;
            infoTextToolStripMenuItem.Checked = true;

            InitializeWorkingDirectiries();

        }


        private void InitializeWorkingDirectiries()
        {          
            foreach (string DspMediaFolders in DspMediaPlayerWorkingFolderArray)
            {
                if (!Directory.Exists(DspMediaFolders))
                {
                    Directory.CreateDirectory(DspMediaFolders);
                }
                else
                {
                    //delete it's content - if there are some
                    foreach (string file in Directory.GetFiles(DspMediaFolders))
                    {
                        try
                        {
                            File.Delete(file);
                        }
                        catch
                        {
                        }
                    }
                   
                    foreach (string dir in Directory.GetDirectories(DspMediaFolders))
                    {
                        try
                        {
                            Directory.Delete(dir, true);
                        }
                        catch
                        {
                        }

                    }
                 }
                    
                

            }
           
        }

        private void DSPMediaPlayer_Form_Load(object sender, EventArgs e)
        {
            string[] args = System.Environment.GetCommandLineArgs();
            string filePath = args[0];
            for (int i = 0; i <= args.Length - 1; i++)
            {
                if (args[i].EndsWith(".exe") == false)
                {
                    //If we reached here it means we have "Open with" scenario and we fire the Browse event
                    richTextBox1.Text = args[i];//Path.GetDirectoryName(filePath) +"\\"+ args[i].Trim();
                    Browse_Event(richTextBox1.Text, false);
                    VerifyMultipleResolutionWasHandeled();
                }
            }
        }

        public void InitializeThreads()
        {
            threadsArray[(int)Threads.enProcessThread] = new Thread(() => Processthread.ProcessThreadFunc(this));

            threadsArray[(int)Threads.enDisplayThread] = new Thread(() => Display.DisplayThread(this));
            threadsArray[(int)Threads.enDisplayThread].Priority = ThreadPriority.Highest;

            threadsArray[(int)Threads.enProcessThread].Start();
            threadsArray[(int)Threads.enDisplayThread].Start();

           
        }
        public void InitializeLabelsArray()
        {
            InfoLabelsObj.m_CodecType           = new Label();
            InfoLabelsObj.m_Resolution          = new Label();
            InfoLabelsObj.m_FrameNum            = new Label();
            InfoLabelsObj.m_CurrFPS             = new Label();
            InfoLabelsObj.m_FrameType           = new Label();
            InfoLabelsObj.m_NumFramesUntilIntra = new Label();
            InfoLabelsObj.m_FrameTimeStamp      = new Label();
            InfoLabelsObj.m_Frame_GopStructure  = new Label();
            InfoLabelsObj.m_Profile_Indication  = new Label();
            InfoLabelsObj.m_Sharp_Indication    = new Label();
        }

        public bool IsAllReadyForDestroy()
        {
            bool retVal = (g_DisplayThreadState.Equals(enThreadsState_t.enThreadstate_Pending_For_Destroy)
                             || g_DisplayThreadState.Equals(enThreadsState_t.enThreadstate_Not_Initialized))
                          &&
                            (g_ProcessThreadState.Equals(enThreadsState_t.enThreadstate_Pending_For_Destroy)
                             || g_ProcessThreadState.Equals(enThreadsState_t.enThreadstate_Not_Initialized));

            return retVal;
        }
        public void updateDisplayInfo()
        {
            InfoLabelStringPattern = InfoLabelsObj.m_CodecType.Text.PadRight(20, ' ') 
                                    + "Res: " + InfoLabelsObj.m_Resolution.Text.PadRight(10, ' ') 
                                    +"FrameNum: " + InfoLabelsObj.m_FrameNum.Text.PadRight(10, ' ') 
                                    + "FrameType: " + InfoLabelsObj.m_FrameType.Text.PadRight(10, ' ') 
                                    +"FPS: " + InfoLabelsObj.m_CurrFPS.Text.PadRight(5, ' ') 
                                    + "FUI: " 
                                    + InfoLabelsObj.m_NumFramesUntilIntra.Text.PadRight(10, ' ')
                                    + "TimeStamp: " + InfoLabelsObj.m_FrameTimeStamp.Text.PadRight(15, ' ') 
                                    + "Gop: " + InfoLabelsObj.m_Frame_GopStructure.Text.PadRight(10, ' ')
                                    + "Profile: " + InfoLabelsObj.m_Profile_Indication.Text.PadRight(10, ' ')
                                    + "Sharp: " + InfoLabelsObj.m_Sharp_Indication.Text.PadRight(10, ' ');


         
            if (this.InfoLabel.InvokeRequired)
            {
                this.InfoLabel.Invoke(new MethodInvoker(
                delegate()
                {
                    this.InfoLabel.Text = InfoLabelStringPattern;
                }));
            }
            else
            {
                this.InfoLabel.Text = InfoLabelStringPattern;
            }
           
        }
        public void CleanDataInfo()
        {
            g_Form_To_Process_Queue.Clear();

            InfoLabelsObj.m_CodecType.Text              = "CodecType: ";
            InfoLabelsObj.m_Resolution.Text             = string.Empty;
            InfoLabelsObj.m_FrameNum.Text               = string.Empty;
            InfoLabelsObj.m_FrameType.Text              = string.Empty;
            InfoLabelsObj.m_CurrFPS.Text                = string.Empty;
            InfoLabelsObj.m_NumFramesUntilIntra.Text    = string.Empty;
            InfoLabelsObj.m_FrameTimeStamp.Text         = string.Empty; 
            InfoLabelsObj.m_Frame_GopStructure.Text     =  string.Empty;
            InfoLabelsObj.m_Profile_Indication.Text     = string.Empty;
            InfoLabelsObj.m_Sharp_Indication.Text       = string.Empty;
        }
        #region GUI interface

        private void Stop_Event()
        {
            this.ActiveControl = null;
            if (Movie_State.Equals(MovieState.enPlayer_Stop) || Movie_State.Equals(MovieState.enPlayer_Idle))
            {

            }
            else
            {
                Movie_State = MovieState.enPlayer_Stop;
            }

            
            //Clean screen
            CleanDataInfo();
            updateDisplayInfo();
            ChannelHandle++;

            gTogglePlay_Pause = true;
        }
        private void Stop_button_Click(object sender, EventArgs e)
        {
            Stop_Event();
        }


        private void Pause_Event()
        {
            this.ActiveControl = null;
            if (Movie_State.Equals(MovieState.enPlayer_Idle))
                return;
            if (Movie_State.Equals(MovieState.enPlayer_Play))
                Movie_State = MovieState.enPlayer_Pause;
            gTogglePlay_Pause = true;

        }
        private void Pause_button_Click(object sender, EventArgs e)
        {
            Pause_Event();            
        }
        

        public void setFrameNum(int FrameNum)
        {

            if (this.InfoLabelsObj.m_FrameNum.InvokeRequired)
            {
                this.InfoLabelsObj.m_FrameNum.Invoke(new MethodInvoker(
                delegate()
                {
                    this.InfoLabelsObj.m_FrameNum.Text =  FrameNum.ToString();
                }));
            }
            else
            {
                this.InfoLabelsObj.m_FrameNum.Text =  FrameNum.ToString();
            }
        }
        public void setFrameType(string aFrameType)
        {
            if (InfoLabelsObj.m_FrameType.InvokeRequired)
            {
                InfoLabelsObj.m_FrameType.Invoke(new MethodInvoker(
                delegate()
                {
                    InfoLabelsObj.m_FrameType.Text = aFrameType;
                }));
            }
            else
            {
                InfoLabelsObj.m_FrameType.Text = aFrameType;
            }
        }
        public void setFrameTimeStamp(string aFrameTimeStamp)
        {
            if (InfoLabelsObj.m_FrameTimeStamp.InvokeRequired)
            {
                InfoLabelsObj.m_FrameTimeStamp.Invoke(new MethodInvoker(
                delegate()
                {
                    InfoLabelsObj.m_FrameTimeStamp.Text = aFrameTimeStamp;
                }));
            }
            else
            {
                InfoLabelsObj.m_FrameTimeStamp.Text = aFrameTimeStamp;
            }
        }
        public void SetFrameGopStruct(string aFrameGopStruct)
        {
            if (InfoLabelsObj.m_Frame_GopStructure.InvokeRequired)
            {
                InfoLabelsObj.m_Frame_GopStructure.Invoke(new MethodInvoker(
                delegate()
                {
                    InfoLabelsObj.m_Frame_GopStructure.Text = aFrameGopStruct;
                }));
            }
            else
            {
                InfoLabelsObj.m_Frame_GopStructure.Text = aFrameGopStruct;
            }

        }
        public void SetSharpIndiaction(string aSharpIndiaction)
        {
            if (InfoLabelsObj.m_Sharp_Indication.InvokeRequired)
            {
                InfoLabelsObj.m_Sharp_Indication.Invoke(new MethodInvoker(
                delegate()
                {
                    InfoLabelsObj.m_Sharp_Indication.Text = aSharpIndiaction;
                }));
            }
            else
            {
                InfoLabelsObj.m_Sharp_Indication.Text = aSharpIndiaction;
            }

        }
        
        public void SetProfileIndication(string aProfile_Indication)
        {
            if (InfoLabelsObj.m_Profile_Indication.InvokeRequired)
            {
                InfoLabelsObj.m_Profile_Indication.Invoke(new MethodInvoker(
                delegate()
                {
                    InfoLabelsObj.m_Profile_Indication.Text = aProfile_Indication;
                }));
            }
            else
            {
                InfoLabelsObj.m_Profile_Indication.Text = aProfile_Indication;
            }

        }
        public void setNumFramesUntilIntra(string aNumFramesUntilIntra)
        {
            if (InfoLabelsObj.m_NumFramesUntilIntra.InvokeRequired)
            {
                InfoLabelsObj.m_NumFramesUntilIntra.Invoke(new MethodInvoker(
                delegate()
                {
                    InfoLabelsObj.m_NumFramesUntilIntra.Text = aNumFramesUntilIntra;
                }));
            }
            else
            {
                InfoLabelsObj.m_NumFramesUntilIntra.Text = aNumFramesUntilIntra;
            }
        }
        public void SetFrameResolution(string aFrameResolution)
        {
            if (InfoLabelsObj.m_Resolution.InvokeRequired)
            {
                InfoLabelsObj.m_Resolution.Invoke(new MethodInvoker(
                delegate()
                {
                    InfoLabelsObj.m_Resolution.Text = aFrameResolution;
                }));
            }
            else
            {
               InfoLabelsObj.m_Resolution.Text = aFrameResolution;
            }
        }
        public void setFrameFPS(string aFPS)
        {
            if (InfoLabelsObj.m_CurrFPS.InvokeRequired)
            {
                InfoLabelsObj.m_CurrFPS.Invoke(new MethodInvoker(
                delegate()
                {
                    InfoLabelsObj.m_CurrFPS.Text = aFPS;
                }));
            }
            else
            {
                InfoLabelsObj.m_CurrFPS.Text = aFPS;
            }
        }
        private void Play_Event()
        {
            this.ActiveControl = null; //returns the control to the Form, and consequently enable other controls to interfere 


            if (!Movie_State.Equals(MovieState.enPlayer_Play))
            {
                Movie_State = MovieState.enPlayer_Play;
                //This case: Playing the same movie after pressing stop!
                if (g_Form_To_Process_Queue.Count.Equals(0)
                    &&
                    g_Last_Queue_Task.mb_valid.Equals(true))
                {
                    g_Form_To_Process_Queue.Enqueue(g_Last_Queue_Task);

                }
            }
            else if (g_ProcessThreadState.Equals(enThreadsState_t.enThreadstate_Not_Initialized)
                    &&
                    g_DisplayThreadState.Equals(enThreadsState_t.enThreadstate_Not_Initialized))
            {
                if (g_Last_Queue_Task.mb_valid.Equals(true))
                {
                    CleanDataInfo();
                    g_Form_To_Process_Queue.Enqueue(g_Last_Queue_Task);
                }
            }

            gTogglePlay_Pause = false;
        }
        private void Play_button_Click(object sender, EventArgs e)
        {
            Play_Event();
        }
        


        private void DSPMediaPlayer_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            Environment.Exit(1);
        }

        private void Rewind_Event()
        {
            this.ActiveControl = null;
            if (!Movie_State.Equals(MovieState.enPlayer_Pause) && !Movie_State.Equals(MovieState.enPlayer_Rewind))
            {
                return;
            }
            Movie_State = MovieState.enPlayer_Rewind;
            if (0 == FramesQueue[(FrameQueueHead + Frames_Queue_Buf_Size - 1) % Frames_Queue_Buf_Size].m_Number
                ||
                FramesQueue[(FrameQueueTail + Frames_Queue_Buf_Size - 1) % Frames_Queue_Buf_Size].m_Number == (FramesQueue[FrameQueueHead % Frames_Queue_Buf_Size].m_Number))
            {
                return;
            }
            if (FramesQueue[(FrameQueueTail + Frames_Queue_Buf_Size - 2) % Frames_Queue_Buf_Size].mb_valid)
            {
                FrameQueueTail = (FrameQueueTail + Frames_Queue_Buf_Size - 1) % Frames_Queue_Buf_Size;// FramesQueue[CurrentFrameCount].m_Number;
            }
            
        }
        private void Rewind_button_Click(object sender, EventArgs e)
        {
            Rewind_Event();
        }
        private void Forward_Event()
        {
            this.ActiveControl = null;
            if (Movie_State.Equals(MovieState.enPlayer_Play) || Movie_State.Equals(MovieState.enPlayer_Idle))
            {
                return;
            }
            else if (CurrentFrameCount < 0)
            {
                return;
            }

            Movie_State = MovieState.enPlayer_Forward;

        }
        private void Forward_button_Click(object sender, EventArgs e)
        {
            Forward_Event();
        }

        public void AnalyzerSettingsFormClosing()
        {
            gPSNR_Threshold             = AnalyzerSettings.mPSNR_Threshold;
            gPitchSkipThreshold         = AnalyzerSettings.mPitchSkipThreshold;
            gCB_BackgroundColor_Value   = AnalyzerSettings.m_CB_BackgroundColor_Value;
            gCR_BackgroundColor_Value   = AnalyzerSettings.m_CR_BackgroundColor_Value;
        }


        public void SettingsFormClosing()
        {
            VideoWidth = settingsForm.yuvWidth;
            VideoHeight = settingsForm.yuvHeight;

            if (string.Empty != settingsForm.PlayFPS)
            {
                DefaultFpsPlay = Convert.ToInt32(settingsForm.PlayFPS);

            }
            if (Movie_State.Equals(MovieState.enPlayer_Pause) || Movie_State.Equals(MovieState.enPlayer_Play))
            {
                Stop_Event();

                bool retVal = Browse_Event(FileName, true);
                if (true == retVal)
                {
                    VerifyMultipleResolutionWasHandeled();
                }
            }
        }

        private void view11ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (view11ToolStripMenuItem.Checked.Equals(false))
            {
                view11ToolStripMenuItem.Checked = true;
                View_Mode = ViewMode.enOrigFrameSize;
                view11ToolStripMenuItem.Image = ((System.Drawing.Image)(Properties.Resources.check1));
            }
            else
            {
                view11ToolStripMenuItem.Checked = false;
                View_Mode = ViewMode.enRegular;
                view11ToolStripMenuItem.Image = null;
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // settingsForm.FormClosing += new FormClosingEventHandler(SettingsFormClosing);
            settingsForm.Show();
        }
        #endregion GUI interface


        private bool Browse_Event(string InputFile, bool abSkipParsing)
        {
            res[] ResolutionArr = { new res(176,144),new res(224,120),  /*new res(320,180),  new res(352,576),
                                       new res(640,360),*/ new res(864,480),  new res(848,480),
                                       new res(960,540), new res(1280,720),new res(1920,1080),new res(1920,1088),
                                       new res(704,576),/*new res(512,288),*/new res(352,288)
                                     };
            
            
            filePos = 0;
            FileName = richTextBox1.Text = InputFile;
            
            string FileExtension = Path.GetExtension(InputFile).Replace(".", "").ToLower();
            if (!abSkipParsing)
            {
            if (FileExtension.Equals("yuv"))//Hack
            {
                
                
                using (/*FileStream*/ fsSource = new FileStream(richTextBox1.Text,
               FileMode.Open, FileAccess.Read))
                {
                    int HitResolutionCounter = 0;
                    List<res> NumOfFrames = new List<res>();
                    foreach (res FrameRes in ResolutionArr)
                    {
                        if (fsSource.Length % (FrameRes.width * FrameRes.Height * 1.5) == 0)
                        {
                            HitResolutionCounter++;
                            NumOfFrames.Add(FrameRes);

                        }
                    }



                    if (1 == HitResolutionCounter)
                    {
                        int NumberOfFrames = Convert.ToInt32(fsSource.Length / (NumOfFrames[0].width * NumOfFrames[0].Height * 1.5));

                        settingsForm.YUV_WidthTextBox.Text  = VideoWidth = NumOfFrames[0].width.ToString();
                        settingsForm.YUV_HeightTextBox.Text = VideoHeight = NumOfFrames[0].Height.ToString();

                    }
                    else if (0 == HitResolutionCounter)
                    {

                        MessageBox.Show("Couldn't identify stream resolution.\nPlease choose valid stream resolution manually.", "Warning!", MessageBoxButtons.OK,
                                         MessageBoxIcon.Exclamation,
                                         MessageBoxDefaultButton.Button1);
                        Movie_State = MovieState.enPlayer_Idle;
                        VideoWidth = null;
                        VideoHeight = null;
                        return false;
                    }
                    else if (HitResolutionCounter > 1)
                    {
                        if (settingsForm.ResolutioncomboBox.Items.Count > 0)
                        {
                            settingsForm.ResolutioncomboBox.Items.Clear();
                        }
                        string msg = "Couldn't identify specific stream resolution.\nPlease choose between the following resolution manually:";
                        int counter = 1;
                        foreach (res NumOfres in NumOfFrames)
                        {
                            msg += "\n" + counter + ")" + NumOfres.width.ToString() + "X" + NumOfres.Height.ToString();
                            counter++;
                            settingsForm.ResolutioncomboBox.Items.Insert(counter - 2, NumOfres.width.ToString() + " X " + NumOfres.Height.ToString());
                        }
                        VideoWidth  = null;
                        VideoHeight = null;
                       
                        MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation,
                                        MessageBoxDefaultButton.Button1);

                        
                        settingsForm.ResolutioncomboBox.Visible = true;
                        settingsForm.ShowDialog();
                    
                        if (settingsForm.bResChangeWasCanceled.Equals(true))
                        {
                            settingsForm.bResChangeWasCanceled = false;
                            return false;
                        }
                       
                        //Movie_State = MovieState.enPlayer_Idle;
                    }
                }
            }
            }
            else
            {
                MessageBox.Show("Changing resolution", "Warning!", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation,
                            MessageBoxDefaultButton.Button1);
            }


            UpdateProcessQueue(FileExtension, FileName);
            return true;
        }

        private void Browse_Wrapper()
        {
            errorLogTextBox.Text = "";
            Stop_Event();

            if (false == IsAllReadyForDestroy())
            {
                System.Windows.Forms.MessageBox.Show("PLEASE PRESS STOP BEFORE...");
                return;
            }

            OpenFileDialog openFileDialog1 = new OpenFileDialog();


            openFileDialog1.Filter = "video files (*.dat;*.264;*.263;*.vp8;*.yuv;*.mse;*.webc) | *.dat;*.264;*.263;*.vp8;*.yuv;*.mse;*.webc| audio files (*.g711a;*.g711u;*.g722;*.g729;*.aaclc;*.g7221;*.g7221c;*.siren14;*.opus;*.wav)|*.g711a;*.g711u;*.g722;*.g729;*.aaclc;*.g7221;*.g7221c;*.siren14;*.opus;*.wav| All files (*.*)|*.*";
            openFileDialog1.Multiselect = true;
            openFileDialog1.RestoreDirectory = true;

            bool retVal = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog1.FileName != string.Empty)
                {
                    UserUpdateFile(openFileDialog1.FileName, "DATA_PLAYER");

                    retVal = Browse_Event(openFileDialog1.FileName, false);
                }
            }
            if (true == retVal)
            {
                VerifyMultipleResolutionWasHandeled();
            }
        }
        private void Browse_Button_Click(object sender, EventArgs e)
        {
            Browse_Wrapper();
        }
        public void VerifyMultipleResolutionWasHandeled()
        {
            Movie_State = MovieState.enPlayer_Idle;
            
            if (null == VideoWidth && null == VideoHeight)
            {
                VideoWidth = settingsForm.yuvWidth;
                VideoHeight = settingsForm.yuvHeight;
            }
            //else if (null == Width && null == Height)
            //{
            //    g_Form_To_DecoderProcess_Queue.Dequeue();
            //    return;
            //}
        }
        public void UpdateProcessQueue(string FileExtension, string FileName)
        {
            Form_To_Process_Queue QueueTemp;
            QueueTemp.m_FileName = FileName;
            QueueTemp.mb_valid = true;
            meInputCodecType= GetCodecType(FileExtension, FileName);
            meProcessType = GetProcessType(meInputCodecType);
            if (meProcessType.Equals(Processing_Type.enAudio))
            {
                MessageBox.Show("Audio isn't supported on this version, Please copy locally and try version 0.0.0.11 on \\\\rvnx-storage\\NBU\\DSP\\utils\\DSPMediaPlayer\\Version_0.0.0.11_2016Apr10");
                return;
            }
            switch (enProcessOp)
            {
                case Process_Operation.enEncoder:
                    {
                        //?In case encoding process is done codec type is YUV which is the file extension
                        //CAL codec type is the type that is entered by the user 
                        QueueTemp.m_CodecType = meCALCodecType;
                        break;
                    }
                case Process_Operation.enDecoder:
                default:
                    {
                        //?In case decoding process is done codec type is the file extension (for example 264,263)   
                        QueueTemp.m_CodecType = meCALCodecType = meInputCodecType;
                        break;
                    }
            }

            if (QueueTemp.m_CodecType.Equals(enDspCodec.enDspCodecNull))
            {
                return;
            }
            if (g_Form_To_Process_Queue.Count > 0)
            {
                CleanDataInfo();
            }
            ChannelHandle++;

            g_Form_To_Process_Queue.Enqueue(QueueTemp);
            UpdateThreadsInfo();
            g_Last_Queue_Task = QueueTemp;

        }

        public Processing_Type GetProcessType(enDspCodec aeCodec)
        {
            switch (aeCodec)
            {
                case enDspCodec.enDspCodecNull:
                case enDspCodec.enDspCodecYUV:
                case enDspCodec.enDspCodecH264:
                case enDspCodec.enDspCodecH264TSVC:
                case enDspCodec.enDspCodecWebCYUV:
                case enDspCodec.enDspCodecH263:
                case enDspCodec.enDspCodecH263P:
                case enDspCodec.enDspCodecVP8:
                    {
                        return Processing_Type.enVideo;
                    }
                default:
                    return Processing_Type.enAudio;
            }
        }


        public enDspCodec GetCodecType(string FileExtension, string FileName)
        {
            FileExtension = FileExtension.ToLower();

            string[] StreamType = { "dat", 
                                    "yuv", 
                                    "264", 
                                    "263", 
                                    "263plus", 
                                    "nv12", 
                                    "vp8", 
                                    "g711A", 
                                    "g711U", 
                                    "g722", 
                                    "aaclc", 
                                    "aacld", 
                                    "g7221", 
                                    "g7221c", 
                                    "g729", 
                                    "opus",
                                    "raw",
                                    "wav",
                                    "siren14",
                                    "webc"};
            enDspCodec[] codecs = { enDspCodec.enDspCodecNull, 
                                    enDspCodec.enDspCodecYUV, 
                                    enDspCodec.enDspCodecH264, 
                                    enDspCodec.enDspCodecH263, 
                                    enDspCodec.enDspCodecH263P,
                                    enDspCodec.enDspCodecNull,
                                  enDspCodec.enDspCodecVP8,
                                  enDspCodec.enDspCodecG711A,
                                  enDspCodec.enDspCodecG711U,
                                  enDspCodec.enDspCodecG722,
                                  enDspCodec.enDspCodecAACLC,
                                  enDspCodec.enDspCodecAACLD,
                                  enDspCodec.enDspCodecG7221,
                                  enDspCodec.enDspCodecG7221C,
                                  enDspCodec.enDspCodecG729,
                                  enDspCodec.enDspCodecOpus,
                                  enDspCodec.enDspCodecRaw,
                                  enDspCodec.enDspCodecWav,
                                  enDspCodec.enDspCodecSiren14,
                                  enDspCodec.enDspCodecWebCYUV};


            if ("dat" == FileExtension)
            {
                
                FixPayloadHeader(FileName);
                
                byte[] Buff = new byte[2000];
                using (FileStream fDatSourceFile = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                {
                    int numBytesRead = 0;
                    fDatSourceFile.Read(Buff, numBytesRead, 2);
                    if (numBytesRead < 0)
                    {
                        MessageBox.Show("Can't read from file - too small?", "Error!", MessageBoxButtons.OK,
                                         MessageBoxIcon.Exclamation,
                                         MessageBoxDefaultButton.Button1);

                        fDatSourceFile.Close();
                        return enDspCodec.enDspCodecNull;
                    }

                    Int32 Size = (Buff[0] << 8) | Buff[1];
                    Size -= 2;
                    fDatSourceFile.Seek(numBytesRead, SeekOrigin.Current);
                    numBytesRead = 0;
                    fDatSourceFile.Read(Buff, numBytesRead, Size);
                    var BuffToStr = System.Text.Encoding.Default.GetString(Buff);

                    for (int i = 0; i < StreamType.Length; i++)
                    {
                        if (BuffToStr.ToLower().Contains(StreamType[i].ToLower()))
                        {
                            InfoLabelsObj.m_CodecType.Text = "CodecType: " + StreamType[i];
                            return codecs[i];
                        }
                    }

                }
               
            }
            else
            {
                for (int i = 0; i < StreamType.Length; i++)
                {
                    if (StreamType[i].ToLower().Equals(FileExtension.ToLower()))
                    {
                        InfoLabelsObj.m_CodecType.Text = "CodecType: " + StreamType[i];
                        return codecs[i];
                    }
                }
            }

            UserCodecSelection codecUserSelect = new UserCodecSelection(StreamType);
            codecUserSelect.ShowDialog();

            return codecUserSelect.GetChoosenCodec();


        }
        //This function's purpose is to fix the packet type in the payload header, that will match the real enum we use in our input.
        //Data packet = 0x0, FEC packet = 0x40 
        private void  FixPayloadHeader( string aFileName)
        {
            int index = 0;
            int Mbit = 0;
            int PL = 0;

            byte[] allBytes = File.ReadAllBytes(aFileName);

            index = (allBytes[index] << 8) + allBytes[index + 1];
            bool FirstPacket = true;
            int PacketType = 0;

          

            while (index < allBytes.Length)
            {
                Mbit = ((allBytes[index + 3] & 0x80) == 0x80) ? 1 : 0;
                PL = allBytes[index + 3] & 0x7f;

                if (FirstPacket.Equals(true))
                {
                    FirstPacket = false;
                    PacketType = PL;
                }
                if (PL.Equals(PacketType))
                {
                    allBytes[index + 3] = (byte)((byte)Mbit << 7 | ((byte)0x00));//Data packet
                }
                else
                {
                    allBytes[index + 3] = (byte)((byte)Mbit << 7 | ((byte)0x40));//Fec packet
                }
               
                index += (allBytes[index] << 8) + allBytes[index + 1];               
                index += 2;
            }


           // newFileName =  Path.Combine(Path.GetDirectoryName(aFileName), Path.GetFileNameWithoutExtension(aFileName).Replace("_AP",string.Empty)) + "_AP.dat";

            FileStream output = new FileStream(aFileName, FileMode.Create);

            
            output.Write(allBytes, 0, allBytes.Length);
            output.Close();

           
        }

        private void threadsInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThreadsInfoForm.Show();
        }

        public void UpdateThreadsInfo()
        {

            if (this.ThreadsInfoForm.FrameQueueHead_TextBox.InvokeRequired)
            {
                this.ThreadsInfoForm.FrameQueueHead_TextBox.Invoke(new MethodInvoker(
                delegate()
                {
                    this.ThreadsInfoForm.FrameQueueHead_TextBox.Text = FrameQueueHead.ToString();
                }));
            }
            else
            {
                this.ThreadsInfoForm.FrameQueueTail_TextBox.Text = FrameQueueTail.ToString();
            }

            if (this.ThreadsInfoForm.FrameQueueTail_TextBox.InvokeRequired)
            {
                this.ThreadsInfoForm.FrameQueueTail_TextBox.Invoke(new MethodInvoker(
                delegate()
                {
                    this.ThreadsInfoForm.FrameQueueTail_TextBox.Text = FrameQueueTail.ToString();
                }));
            }
            else
            {
                this.ThreadsInfoForm.DisplayQueue_TextBox.Text = g_Process_To_Display_Queue.ToString();
            }

            if (this.ThreadsInfoForm.DisplayQueue_TextBox.InvokeRequired)
            {
                this.ThreadsInfoForm.DisplayQueue_TextBox.Invoke(new MethodInvoker(
                delegate()
                {
                    this.ThreadsInfoForm.DisplayQueue_TextBox.Text = g_Process_To_Display_Queue.Count.ToString();
                }));
            }
            else
            {
                this.ThreadsInfoForm.DisplayQueue_TextBox.Text = g_Process_To_Display_Queue.Count.ToString();
            }

            if (this.ThreadsInfoForm.TasksQueue_TextBox.InvokeRequired)
            {
                this.ThreadsInfoForm.TasksQueue_TextBox.Invoke(new MethodInvoker(
                delegate()
                {
                    this.ThreadsInfoForm.TasksQueue_TextBox.Text = g_Form_To_Process_Queue.Count.ToString();
                }));
            }
            else
            {
                this.ThreadsInfoForm.TasksQueue_TextBox.Text = g_Form_To_Process_Queue.Count.ToString();
            }
        }
#region Pcap

        private void pcapFileHandlerTool(string aCodecType)
        {
            this.ActiveControl = null;
            if (!Movie_State.Equals(MovieState.enPlayer_Stop) && !Movie_State.Equals(MovieState.enPlayer_Idle))
            {
                MessageBox.Show("Please stop the movie before initiating this process.");
                return;
            }
            
            OpenFileDialog openPcapFileDialog = new OpenFileDialog();
            openPcapFileDialog.Filter = "All files (*.*)|*.*";
            openPcapFileDialog.Multiselect = true;
            openPcapFileDialog.RestoreDirectory = true;


            if (openPcapFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (openPcapFileDialog.FileName != string.Empty)
                {
                    UserUpdateFile(openPcapFileDialog.FileName, "PCAP");
                    
                    if (Directory.Exists(PcapDir))
                    {
                        string[] fileNames = Directory.GetFiles(PcapDir);
                        foreach (string FileName in fileNames)
                        {

                            if (Path.GetExtension(FileName).ToLower().Equals("pcap"))
                            {
                                
                                File.Delete(FileName);
                                
                            }

                        }
                        Directory.Delete(PcapDir, true);
                       
                    }

                    Directory.CreateDirectory(PcapDir);
                    StationsListFormObj = new StationsListForm(this, openPcapFileDialog.FileName, PcapDir, aCodecType);
                    pcapListenerThreadObj = new Thread(() => pcapListenerThread(this, openPcapFileDialog.FileNames.ToList()));
                    pcapListenerThreadObj.Start();
                     
                    
                    
                }

            }
            
        }
        
         public static string BytesToString(long byteCount)
         {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return  "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() +" "+ suf[place];
         }

        public static void Initiate_ProgressBarThread()
        {
            ProgressBarObj = new Thread(new ThreadStart(InProgressBar.Background_WorkThreadFunction));
            ProgressBarObj.SetApartmentState(ApartmentState.STA);
            ProgressBarObj.Start();


        }
        public void updateStationStreamResult(string FileName, string FileExtension)
        {
            StationStreamResult.m_StreamFile = FileName;
            StationStreamResult.m_FileExtension = FileExtension;
            Indication = true;
        }

        //Pcap Process Thread
        public static void pcapListenerThread(DSPMediaPlayer_Form MainForm, List<string> PcapFileDialogFileName)
        {
            StationStreamResult_t stationStreamRes = MainForm.StationStreamResult;

           Thread.CurrentThread.IsBackground = true;

           pcap pcapState = pcap.enAnalyzeStream;
        

           Initiate_ProgressBarThread();

           while(true)
           {
               switch (pcapState)
               {
                   case pcap.enAnalyzeStream:
                       {
                           foreach (string fileName in PcapFileDialogFileName)
                           {

                               PcapFileHandler.PcapFileHandlerProcess(PcapDir, fileName);
                           }
                           if (PcapFileHandler.Traffic_Path_List.Count > 0)
                           {
                               
                               
                               foreach (Single_Traffic_Path station in PcapFileHandler.Traffic_Path_List)
                               {
                                   station.FileSize = new FileInfo(PcapDir+'\\'+station.pcapFileName).Length;
                                   station.FileSizeStr = BytesToString(station.FileSize);
                               }
                               PcapFileHandler.Traffic_Path_List.Sort((x, y) =>
                                                                x.FileSize.CompareTo(y.FileSize));
                              
                               
                               MainForm.StationsListFormObj.StationsListForm_Load(ref  PcapFileHandler.Traffic_Path_List);
                               
                              
                               ProgressBarObj.Abort();
                               MainForm.StationsListFormObj.ShowDialog(); //
                               pcapState = pcap.enProcessSelectedStream;
                           }
                           else
                           {
                               MessageBox.Show("An error occurred during processing");
                               pcapListenerThreadObj.Abort();
                               ProgressBarObj.Abort();
                           }
                          
                           break;
                       }
                 
                   case pcap.enProcessSelectedStream:
                       {
                          
                           if (Indication.Equals(true))
                           {
                               Indication = false;
                               ProgressBarObj.Abort(); //progress bar for ether2MF
                               if (MainForm.richTextBox1.InvokeRequired)
                               {
                                   //Invoke to update main thread from current thread
                                   MainForm.richTextBox1.Invoke(new MethodInvoker(
                                   delegate()
                                   {
                                       MainForm.richTextBox1.Text = stationStreamRes.m_StreamFile;
                                   }));
                               }
                               else
                               {
                                   MainForm.richTextBox1.Text = stationStreamRes.m_StreamFile;
                               }
                               MainForm.FileName = stationStreamRes.m_StreamFile;
                               //MessageBox.Show("Other formats rather that DAT aren't yet supported for display ");//Hack
                               MainForm.UpdateProcessQueue(stationStreamRes.m_FileExtension, stationStreamRes.m_StreamFile);
                            
                               MainForm.Movie_State = MovieState.enPlayer_Idle;
                               
                               pcapListenerThreadObj.Abort();
                              
                           }
                           
                           break;
                       }
                   default:
                       {
                           break;
                       }
               }
               System.Threading.Thread.Sleep(1);
           }
        }

#endregion Pcap

        private void versionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Version is:   " + ApplicationVersion);
        }

        static bool gTogglePlay_Pause = true;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            
            if (keyData == Keys.Up)
            {
                Forward_Event();
                return true;    // indicate that you handled this keystroke
            }
            else if (keyData == Keys.Down)
            {
                Rewind_Event();
                return true;    // indicate that you handled this keystroke
            }
            else if (keyData == Keys.Space && gTogglePlay_Pause.Equals(true))
            {
                Play_Event();               
            }
            else if (keyData == Keys.Space && gTogglePlay_Pause.Equals(false))
            {
                Pause_Event();
            }
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            if (keyData == Keys.O)
            {
                 Browse_Wrapper();
            }
            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }
        public void UserUpdateFile(string aMediaFile,string aFeature)
        {
            try
            {
                if (false == IniWriter.IniWriteValue("GENERAL", "LOAD", System.DateTime.Now.ToString())
                    ||
                    false == IniWriter.IniWriteValue("GENERAL", "VERSION", ApplicationVersion)
                    ||
                    false == IniWriter.IniWriteValue("GENERAL", "FEATURE", aFeature)
                    ||
                    false == IniWriter.IniWriteValue("GENERAL", "MEDIA_FILE", aMediaFile)
                    )
                {
                    MessageBox.Show("Error while trying to update" + IniWriter);
                    return;
                }
               
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void h264ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            h264ToolStripMenuItem.Checked = true;
            h264ToolStripMenuItem.Image = ((System.Drawing.Image)(Properties.Resources.check1));
            pcapFileHandlerTool("H264");
            h264ToolStripMenuItem.Checked = false;
        }

        private void h263ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            h263ToolStripMenuItem.Checked = true;
            h263ToolStripMenuItem.Image = ((System.Drawing.Image)(Properties.Resources.check1));
            pcapFileHandlerTool("H263");
            h263ToolStripMenuItem.Checked = false;
           
        }

        private void vp8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            vp8ToolStripMenuItem.Checked = true;
            vp8ToolStripMenuItem.Image = ((System.Drawing.Image)(Properties.Resources.check1));
            pcapFileHandlerTool("VP8");
            vp8ToolStripMenuItem.Checked = false;
        }

        private void saveCleanFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (saveCleanFrameToolStripMenuItem.Checked.Equals(false))
            {
                saveCleanFrameToolStripMenuItem.Checked = true;
                saveCleanFrameToolStripMenuItem.Image = ((System.Drawing.Image)(Properties.Resources.check1));
            }
            else
            {
                saveCleanFrameToolStripMenuItem.Checked = false;
                saveCleanFrameToolStripMenuItem.Image = null;
            }
        }

        private void saveRawFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveRawFrameToolStripMenuItem.Checked.Equals(false))
            {
                saveRawFrameToolStripMenuItem.Checked = true;
                saveRawFrameToolStripMenuItem.Image = ((System.Drawing.Image)(Properties.Resources.check1));
            }
            else
            {
                saveRawFrameToolStripMenuItem.Checked = false;
                saveRawFrameToolStripMenuItem.Image = null;
            }
        }


        private void showYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (showYToolStripMenuItem.Checked.Equals(false))
            {
                showYToolStripMenuItem.Checked = true;
                showYToolStripMenuItem.Image = ((System.Drawing.Image)(Properties.Resources.check1));
            }
            else
            {
                showYToolStripMenuItem.Checked = false;
                showYToolStripMenuItem.Image = null;
            }
        }

        private void showUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (showUToolStripMenuItem.Checked.Equals(false))
            {
                showUToolStripMenuItem.Checked = true;
                showUToolStripMenuItem.Image = ((System.Drawing.Image)(Properties.Resources.check1));
            }
            else
            {
                showUToolStripMenuItem.Checked = false;
                showUToolStripMenuItem.Image = null;
            }
        }

        private void showVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (showVToolStripMenuItem.Checked.Equals(false))
            {
                showVToolStripMenuItem.Checked = true;
                showVToolStripMenuItem.Image = ((System.Drawing.Image)(Properties.Resources.check1));
            }
            else
            {
                showVToolStripMenuItem.Checked = false;
                showVToolStripMenuItem.Image = null;
            }
        }


        delegate void SetExtraInfoCallback(ref t_ExtraSolutionInfo arExtraInfoObj, enDspCodec aeCodec);
        public void updateExtraInfoTreeView(ref t_ExtraSolutionInfo arExtraInfoObj, enDspCodec aeCodec)
        {
            try
            {
                if (this.ExtraInfoTreeView.InvokeRequired)
                {
                    SetExtraInfoCallback d = new SetExtraInfoCallback(updateExtraInfoTreeViewCB);
                    this.Invoke(d, new object[] { arExtraInfoObj, aeCodec });
                }
                else
                {
                    this.updateExtraInfoTreeViewCB(ref arExtraInfoObj, aeCodec);
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((ex.ToString()));
            }

        }


        public void updateExtraInfoTreeViewCB(ref t_ExtraSolutionInfo arExtraInfoObj, enDspCodec aeCodec)
        {
            ExtraInfoTreeView.Nodes.Clear();

            switch (aeCodec)
            {
                case enDspCodec.enDspCodecH263:
                case enDspCodec.enDspCodecH263P:
                    {

                        ExtraInfoTreeView.Nodes.Add("PTYPE            =" + arExtraInfoObj.mH263ExtraInfo.m_PTYPE.ToString());
                        ExtraInfoTreeView.Nodes.Add("SrcFormat        =" + arExtraInfoObj.mH263ExtraInfo.m_SrcFormat.ToString());
                        ExtraInfoTreeView.Nodes.Add("UFEP             =" + arExtraInfoObj.mH263ExtraInfo.m_UFEP.ToString());
                        ExtraInfoTreeView.Nodes.Add("ExtendedSrcFormat=" + arExtraInfoObj.mH263ExtraInfo.m_ExtendedSrcFormat.ToString("X"));
                        ExtraInfoTreeView.Nodes.Add("UFEP_extra15bits =" + arExtraInfoObj.mH263ExtraInfo.m_UFEP_extra15bits.ToString("X"));
                        ExtraInfoTreeView.Nodes.Add("PictureTypeCode  =" + arExtraInfoObj.mH263ExtraInfo.m_PictureTypeCode.ToString());
                        ExtraInfoTreeView.Nodes.Add("PlusTypeExtra6Bits=" + arExtraInfoObj.mH263ExtraInfo.m_PlusTypeExtra6Bits.ToString("X"));
                        ExtraInfoTreeView.Nodes.Add("CPM_PSBI          =" + arExtraInfoObj.mH263ExtraInfo.m_CPM_PSBI.ToString());
                        ExtraInfoTreeView.Nodes.Add("CPFMT_PAR         =" + arExtraInfoObj.mH263ExtraInfo.m_CPFMT_PAR.ToString());
                        ExtraInfoTreeView.Nodes.Add("CPFMT_PictureWidth=" + arExtraInfoObj.mH263ExtraInfo.m_CPFMT_PictureWidth.ToString());
                        ExtraInfoTreeView.Nodes.Add("CPFMT_PictureHeigth=" + arExtraInfoObj.mH263ExtraInfo.m_CPFMT_PictureHeigth.ToString());
                        ExtraInfoTreeView.Nodes.Add("EPAR              =" + arExtraInfoObj.mH263ExtraInfo.m_EPAR.ToString());



                        break;
                    }


                case enDspCodec.enDspCodecH264:
                case enDspCodec.enDspCodecH264TSVC:
                    {
                        UInt32 width = (arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_pic_width_in_mbs_minus_1 + 1) * 16; 
                        UInt32 heigth = (arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_pic_height_in_map_units_minus_1 + 1) * 16; 
                        /////
                        //SPS
                        TreeNode SpsNode = ExtraInfoTreeView.Nodes.Add("SPS");

                        SpsNode.Nodes.Add("profile_idc           =" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_profile_idc.ToString());
                        SpsNode.Nodes.Add("constraint_set0_flag  =" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_constraint_set0_flag.ToString());
                        SpsNode.Nodes.Add("constraint_set0_flag  =" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_constraint_set0_flag.ToString());
                        SpsNode.Nodes.Add("constraint_set1_flag  =" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_constraint_set1_flag.ToString());
                        SpsNode.Nodes.Add("constraint_set2_flag  =" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_constraint_set2_flag.ToString());
                        SpsNode.Nodes.Add("constraint_set3_flag  =" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_constraint_set3_flag.ToString());
                        SpsNode.Nodes.Add("reserved_zero_4bits   =" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_reserved_zero_4bits.ToString());
                        SpsNode.Nodes.Add("level_idc             =" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_level_idc.ToString());
                        SpsNode.Nodes.Add("seq_parameter_set_id  =" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_seq_parameter_set_id.ToString());
                        SpsNode.Nodes.Add("log2_max_frame_num_minus4=" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_log2_max_frame_num_minus4.ToString());
                        SpsNode.Nodes.Add("m_pic_order_cnt_type  =" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_pic_order_cnt_type.ToString());
                        SpsNode.Nodes.Add("m_log2_max_pic_order_cnt_lsb_minus4=" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_log2_max_pic_order_cnt_lsb_minus4.ToString());
                        SpsNode.Nodes.Add("m_delta_pic_order_always_zero_flag=" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_delta_pic_order_always_zero_flag.ToString());
                        SpsNode.Nodes.Add("m_offset_for_non_ref_pic=" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_offset_for_non_ref_pic.ToString());
                        SpsNode.Nodes.Add("m_offset_for_top_to_bottom_field=" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_offset_for_top_to_bottom_field.ToString());
                        SpsNode.Nodes.Add("m_num_ref_frames=" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_num_ref_frames.ToString());
                        SpsNode.Nodes.Add("m_gaps_in_frame_num_value_allowed_flag=" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_gaps_in_frame_num_value_allowed_flag.ToString());
                        SpsNode.Nodes.Add("m_pic_width_in_mbs_minus_1=" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_pic_width_in_mbs_minus_1.ToString() + "(" + width.ToString() + ")");
                        SpsNode.Nodes.Add("m_pic_height_in_map_units_minus_1=" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_pic_height_in_map_units_minus_1.ToString() + "(" + heigth.ToString() + ")");
                        SpsNode.Nodes.Add("m_frame_mbs_only_flag=" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_frame_mbs_only_flag.ToString());
                        SpsNode.Nodes.Add("m_mb_adaptive_frame_field_flag=" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_mb_adaptive_frame_field_flag.ToString());
                        SpsNode.Nodes.Add("m_direct_8x8_inference_flag=" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_direct_8x8_inference_flag.ToString());
                        SpsNode.Nodes.Add("m_frame_cropping_flag=" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_frame_cropping_flag.ToString());
                        SpsNode.Nodes.Add("m_frame_crop_left_offset=" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_frame_crop_left_offset.ToString());
                        SpsNode.Nodes.Add("m_frame_crop_right_offset=" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_frame_crop_right_offset.ToString());
                        SpsNode.Nodes.Add("m_frame_crop_top_offset=" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_frame_crop_top_offset.ToString());
                        SpsNode.Nodes.Add("m_frame_crop_bottom_offset=" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_frame_crop_bottom_offset.ToString());
                        SpsNode.Nodes.Add("m_vui_prameters_present_flag=" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_vui_prameters_present_flag.ToString());
                        SpsNode.Nodes.Add("m_aspect_ratio_info_present_flag=" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_aspect_ratio_info_present_flag.ToString());
                        SpsNode.Nodes.Add("m_aspect_ratio_idc    =" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_aspect_ratio_idc.ToString());
                        SpsNode.Nodes.Add("m_sar_width           =" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_sar_width.ToString());
                        SpsNode.Nodes.Add("m_sar_height          =" + arExtraInfoObj.mH264ExtraInfo.mSpsObj.m_sar_height.ToString());


                        /////
                        //PPS
                        TreeNode PpsNode = ExtraInfoTreeView.Nodes.Add("PPS");

                        PpsNode.Nodes.Add("m_pic_parameter_set_id =" + arExtraInfoObj.mH264ExtraInfo.mPpsObj.m_pic_parameter_set_id.ToString());
                        PpsNode.Nodes.Add("m_seq_parameter_set_id =" + arExtraInfoObj.mH264ExtraInfo.mPpsObj.m_seq_parameter_set_id.ToString());
                        PpsNode.Nodes.Add("m_entropy_coding_mode_flag=" + arExtraInfoObj.mH264ExtraInfo.mPpsObj.m_entropy_coding_mode_flag.ToString());
                        PpsNode.Nodes.Add("m_bottom_field_pic_order_in_frame_present_flag=" + arExtraInfoObj.mH264ExtraInfo.mPpsObj.m_bottom_field_pic_order_in_frame_present_flag.ToString());
                        PpsNode.Nodes.Add("m_num_slice_groups_minus1=" + arExtraInfoObj.mH264ExtraInfo.mPpsObj.m_num_slice_groups_minus1.ToString());

                        break;
                    }


                case enDspCodec.enDspCodecVP8:
                    {
                        //ExtraInfoTreeView.Nodes.Add("INTRA=" + arExtraInfoObj.mVPXExtraInfo.m_INTRA.ToString());
                        ExtraInfoTreeView.Nodes.Add("VER             =" + arExtraInfoObj.mVPXExtraInfo.m_VER.ToString());
                        ExtraInfoTreeView.Nodes.Add("H               =" + arExtraInfoObj.mVPXExtraInfo.m_H.ToString());
                        ExtraInfoTreeView.Nodes.Add("PacketSize      =" + arExtraInfoObj.mVPXExtraInfo.m_H.ToString());
                        ExtraInfoTreeView.Nodes.Add("SPC             =" + arExtraInfoObj.mVPXExtraInfo.m_SPC.ToString("X"));
                        ExtraInfoTreeView.Nodes.Add("ResW            =" + arExtraInfoObj.mVPXExtraInfo.m_ResW.ToString());
                        ExtraInfoTreeView.Nodes.Add("ResW_scaleFactor=" + arExtraInfoObj.mVPXExtraInfo.m_ResW_scaleFactor.ToString());
                        ExtraInfoTreeView.Nodes.Add("ResH            =" + arExtraInfoObj.mVPXExtraInfo.m_ResH.ToString());
                        ExtraInfoTreeView.Nodes.Add("ResH_scaleFactor=" + arExtraInfoObj.mVPXExtraInfo.m_ResH_scaleFactor.ToString());

                        break;
                    }

                default:
                    break;
            }

            ExtraInfoTreeView.ExpandAll();
            return;
        }
            


        private void codecTypeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }


        private void CodecH264EncodeTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodecH264EncodeTypeToolStripMenuItem_Click(enDspCodec.enDspCodecH264);
        }

        private void CodecH263EncodeTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodecH264EncodeTypeToolStripMenuItem_Click(enDspCodec.enDspCodecH263);
        }
         
        private void CodecH264EncodeTypeToolStripMenuItem_Click(enDspCodec arCodecType)
        {
            OpenFileDialog openFileEncodeDialog = new OpenFileDialog();

            openFileEncodeDialog.Filter = "YUV files (*.yuv)|*.yuv";
            openFileEncodeDialog.Multiselect = true;
            openFileEncodeDialog.RestoreDirectory = true;

            if (openFileEncodeDialog.ShowDialog() == DialogResult.OK)
            {
                if (openFileEncodeDialog.FileName != string.Empty)
                {
                    meInputCodecType = enDspCodec.enDspCodecYUV;
                    meCALCodecType = arCodecType;    
                    enProcessOp = Process_Operation.enEncoder;
                    Browse_Event(openFileEncodeDialog.FileName, false);
                }
            }

        }

        public void LogString(string aNewStr)
        {
            string newLine = DateTime.Now.ToString() + ": " + aNewStr + Environment.NewLine + errorLogTextBox.Text;

            try
            {
                if (this.errorLogTextBox.InvokeRequired)
                {

                    MethodInvoker del = delegate
                    {
                        LogString(aNewStr);
                    };
                    this.Invoke(del);
                    return;
                    
                }
                else
                {
                    this.errorLogTextBox.Text = newLine;
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show((ex.ToString()));	
            }
           
        }

         public unsafe void LogUTF_8_CharArr(char *aNewStr)
        {

            int strLen = 0;
            for (int i = 0; i < 1000; i++)
            {
                if (0 == (char)(((byte*)(aNewStr))[i]))
                {
                    mTempCharBuffer[i] = (char)(((byte*)(aNewStr))[i]);
                    strLen = i;
                    break;
                }

                mTempCharBuffer[i] = (char)(((byte*)(aNewStr))[i]);
            }

            string str = new string(mTempCharBuffer);
            str = str.Substring(0, str.IndexOf('\0'));
            LogString(str);             
        }

         public void HandleAudio(string aWavFile, int aSampleRate, int aBitRate)
         {
             if (this.waveControl_UI.InvokeRequired)
             {

                 MethodInvoker del = delegate
                 {
                     bool retVal = waveControl_UI.HandleAudioWavFile(aWavFile, aSampleRate, aBitRate);
                     if (false == retVal)
                     {
                         LogString("HandleAudioWavFile return an error (probably the wav file is occupied)");
                     }
                 };
                 this.Invoke(del);
                 return;

             }
             else
             {
                 bool retVal = waveControl_UI.HandleAudioWavFile(aWavFile, aSampleRate, aBitRate);
                 if (false == retVal)
                 {
                     LogString("HandleAudioWavFile return an error (probably the wav file is occupied)");
                 }
             }
         }


         public void updateMainFormAccordingToProcessType(Processing_Type aProcesstype)
         {

             switch (aProcesstype)
             {
                 case Processing_Type.enAudio:
                     {
                         updateWaveControlState(true);
                         updateImagePanelState(false);
                         break;
                     }
                 case Processing_Type.enVideo:
                     {
                         updateWaveControlState(false);
                         updateImagePanelState(true);
                         break;
                     }
                 default:
                     break;
             }
         }

        private void updateWaveControlState(bool abVisible)
        {
            if (this.waveControl_UI.InvokeRequired)
            {

                MethodInvoker del = delegate
                {
                    waveControl_UI.Visible = abVisible;
                };
                this.Invoke(del);
                return;

            }
            else
            {
                waveControl_UI.Visible = abVisible;
            }
        }

        private void updateImagePanelState(bool abVisible)
        {
            if (this.imagePanel1.InvokeRequired)
            {

                MethodInvoker del = delegate
                {
                    imagePanel1.Visible = abVisible;
                };
                this.Invoke(del);
                return;

            }
            else
            {
                imagePanel1.Visible = abVisible;
            }
        }

        private void saveImageafterDecoderDisplayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveImageafterDecoderDisplayToolStripMenuItem.Checked.Equals(false))
            {
                saveImageafterDecoderDisplayToolStripMenuItem.Checked = true;
                saveImageafterDecoderDisplayToolStripMenuItem.Image = ((System.Drawing.Image)(Properties.Resources.check1));
            }
            else
            {
                saveImageafterDecoderDisplayToolStripMenuItem.Checked = false;
                saveImageafterDecoderDisplayToolStripMenuItem.Image = null;
            }
        }

        public int getLinearSampleRate()
        {
            if (Linear8000.Checked)
            {
                return 8000;
            }
            else if (Linear16000.Checked)
            {
                return 16000;
            }
            else if (Linear32000.Checked)
            {
                return 32000;
            }
            else if (Linear48000.Checked)
            {
                return 48000;
            }
            else if (Linear64000.Checked)
            {
                return 64000;
            }

            return 16000;
        }

        public void UpdateLinearSampleRateUI(int aSamplerate)
        {
            
            Linear8000.Checked = false;
            Linear16000.Checked = false;
            Linear32000.Checked = false;
            Linear48000.Checked = false;
            Linear64000.Checked = false;

            switch (aSamplerate)
            {
                case 8000:
                    Linear8000.Checked = true;
                    break;
                case 16000:
                    Linear16000.Checked = true;
                    break;
                case 32000:
                    Linear32000.Checked = true;
                    break;
                case 48000:
                    Linear48000.Checked = true;
                    break;
                case 64000:
                    Linear64000.Checked = true;
                    break;
                default:
                    Linear16000.Checked = true;
                    break;
            }
        }
        private void Linear8000_Click(object sender, EventArgs e)
        {
            UpdateLinearSampleRateUI(8000);
        }
        private void Linear16000_Click(object sender, EventArgs e)
        {
            UpdateLinearSampleRateUI(16000);
        }

        private void Linear32000_Click(object sender, EventArgs e)
        {
            UpdateLinearSampleRateUI(32000);
        }

        private void Linear48000_Click(object sender, EventArgs e)
        {
            UpdateLinearSampleRateUI(48000);
        }

        private void Linear64000_Click(object sender, EventArgs e)
        {
            UpdateLinearSampleRateUI(64000);
        }

        private void extraInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            extraInfoToolStripMenuItem.Checked = !extraInfoToolStripMenuItem.Checked;

            ExtraInfoTreeView.Visible = extraInfoToolStripMenuItem.Checked;

        }

        private void infoTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            infoTextToolStripMenuItem.Checked = !infoTextToolStripMenuItem.Checked;

            errorLogTextBox.Visible = infoTextToolStripMenuItem.Checked;
        }
		
		
		 private void enableDisableROIGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (enableDisableROIGridToolStripMenuItem.Checked.Equals(false))
            {
                enableDisableROIGridToolStripMenuItem.Checked = true;
                enableDisableROIGridToolStripMenuItem.Image = ((System.Drawing.Image)(Properties.Resources.check1));
            }
            else
            {
                enableDisableROIGridToolStripMenuItem.Checked = false;
                enableDisableROIGridToolStripMenuItem.Image = null;
            }

        }
		

         private void analyzerBitMapSettingsToolStripMenuItem_Click(object sender, EventArgs e)
         {
             AnalyzerSettings.Show();
         }

         private void openSharpFramesFolderToolStripMenuItem_Click(object sender, EventArgs e)
         {
             if (Directory.Exists(DSP_MEDIA_PLAYER_SHARPFRAMES_WORKING_FOLDER+CurrSharpFramesFolderName))
             {
                 System.Diagnostics.Process.Start(DSP_MEDIA_PLAYER_SHARPFRAMES_WORKING_FOLDER+CurrSharpFramesFolderName);
             }
             else
             {
                 MessageBox.Show("No sharpFrames folders to display.","", MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation,
                                        MessageBoxDefaultButton.Button1);
             }
         }        



    }
}

 