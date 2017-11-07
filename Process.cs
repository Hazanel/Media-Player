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



namespace DSPMediaPlayer
{
  

    public unsafe class Processthread
    {
        private static Processthread ProcessThread;
        private const int MAX_PROCESS_THREAD_FLOW_LENGTH = 2;
        private const int MAX_NUM_OF_PLANES = 3;
        private  static Node_Interface [] mProcessthreadFlow;
        private static DSPMediaPlayer_Form MainForm;
        private static MovieState Movie_State;
        private static NodeInArgs nodeInArgs;
        private static NodeOutArgs nodeOutArgs;

        public Processthread(DSPMediaPlayer_Form Main_Form)
        {
            MainForm = Main_Form;
            mProcessthreadFlow = new Node_Interface[MAX_PROCESS_THREAD_FLOW_LENGTH];           
            nodeInArgs = new NodeInArgs();
            nodeOutArgs = new NodeOutArgs();
            nodeOutArgs.mCALOutFrame.mFrame.YCbCrBuff = new byte*[MAX_NUM_OF_PLANES];
 
            nodeInArgs.mCommonArgs.mFileName = MainForm.FileName;
            nodeInArgs.mCommonArgs.mCodectype = MainForm.meCALCodecType;
            nodeInArgs.mRTPInArgs.mSaveOutFile = 0;

            //update form according to processtype:
            //Audio - display audio wave form
            //video - display image display
            MainForm.updateMainFormAccordingToProcessType(MainForm.meProcessType);

            switch (MainForm.meProcessType)
            {
                case Processing_Type.enVideo:
                    {
                        mProcessthreadFlow[0] = new RTPModuleobjAPI(MainForm, nodeInArgs.mCommonArgs.mFileName,
                                                                MainForm.meInputCodecType);
                        mProcessthreadFlow[1] = new CALobjAPI(MainForm, nodeInArgs.mCommonArgs);
                        break;
                    }
                case Processing_Type.enAudio:
                    {
                        mProcessthreadFlow[0] = new AudioCodecsWrapperDLL(MainForm, 
                                                                          nodeInArgs.mCommonArgs.mFileName,
                                                                          MainForm.meInputCodecType);
                        break;
                    }
                default:
                    {
                        System.Windows.Forms.MessageBox.Show("BAD PROCESS TYPE.");
                    }
                    break;

            }      
           
        }       


        public static void ProcessThreadFunc(DSPMediaPlayer_Form Main_Form)
        {
            

            while (true)
            {


                switch(Main_Form.g_ProcessThreadState)
                {
                    case (enThreadsState_t.enThreadstate_Pending_For_Destroy):
                        {

                            if(true == Main_Form.IsAllReadyForDestroy())
                            {
                                if (null != ProcessThread)
                                {
                                    ProcessThreadFuncDestroy();
                                    ProcessThread = null;
                                }

                                MainForm.g_ProcessThreadState = enThreadsState_t.enThreadstate_Not_Initialized;
                            }

                            break;
                        }

                    case (enThreadsState_t.enThreadstate_In_Progress):
                        {
                            ProcessInProgress(Main_Form);

                            break;
                        }

                    case (enThreadsState_t.enThreadstate_Not_Initialized):
                        {
                            if (Main_Form.g_Form_To_Process_Queue.Count > 0)
                            {
                                ProcessThread = new Processthread(Main_Form);

                                Main_Form.g_ProcessThreadState = enThreadsState_t.enThreadstate_In_Progress;
                            }
                            break;
                        }
                    default:
                        {
                            //error
                            break;
                        }

                }

                Main_Form.UpdateThreadsInfo();
                System.Threading.Thread.Sleep(1);

            }

        }

        public static void ProcessInProgress(DSPMediaPlayer_Form Main_Form)
        {
            enProcessNodeRetValType_t eDecProcessRetVal = enProcessNodeRetValType_t.enProcessNodeRetCodeSuccess;

            //check if we need to end the process
            if (0 == Main_Form.g_Form_To_Process_Queue.Count)
            {
                Process_To_Display_Queue terminateTask = new Process_To_Display_Queue();

                terminateTask.m_Processing_Type = Processing_Type.enTerminate;

                MainForm.g_Process_To_Display_Queue.Enqueue(terminateTask);

                Main_Form.g_ProcessThreadState = enThreadsState_t.enThreadstate_Pending_For_Destroy;

                return;
            }


            //we have a task in the queue

            Movie_State = MainForm.Movie_State;//Overcome compiler warning
            if (MainForm.g_Form_To_Process_Queue.Count <= MainForm.FramesQueue.Length)//prevent override the cyclic buffer between process thread and display thread
            {
                try
                {
                    bool bFirstTime = Movie_State.Equals(MovieState.enPlayer_Idle)
                                       && (-1 == MainForm.FrameQueueHead)
                                       && (-1 == MainForm.FrameQueueTail);


                    if (    bFirstTime.Equals(true)
                        ||  Movie_State.Equals(MovieState.enPlayer_Play)
                        ||  Movie_State.Equals(MovieState.enPlayer_Forward)
                        )
                    {
                        eDecProcessRetVal = RunProcessFlow();
                    }
                    
                    //in case we are on video (NOT audio), we wish to present first frame and wait for user action
                    if (    (true == bFirstTime)
                        && (Processing_Type.enVideo == MainForm.meProcessType ))
                    {
                        if (eDecProcessRetVal.Equals(enProcessNodeRetValType_t.enProcessNodeRetCodeSuccess))
                        {
                            MainForm.Movie_State = MovieState.enPlayer_Pause;
                        }
                    }
                    
                    if (   eDecProcessRetVal.Equals(enProcessNodeRetValType_t.enProcessNodeRetRTPSolutionErr)
                        || eDecProcessRetVal.Equals(enProcessNodeRetValType_t.enProcessNodeRetAudioFinishPlaying))
                    {
                       Main_Form.g_Form_To_Process_Queue.Clear();
                    }
					
					//in case of forward, after process teh job, we'll return to pause
                    if (Movie_State.Equals(MovieState.enPlayer_Forward))
                        MainForm.Movie_State = MovieState.enPlayer_Pause;
                }
                catch (FileNotFoundException ioEx)
                {
                    MessageBox.Show(ioEx.Message);
                }
            }

        }

  

        public static enProcessNodeRetValType_t RunProcessFlow()
        {
            enProcessNodeRetValType_t eDecProcessRetVal = enProcessNodeRetValType_t.enProcessNodeRetCodeSuccess;
            bool bInsertJobToQueue = true;
            nodeInArgs.mRTPInArgs.mSaveOutFile      = MainForm.saveCleanFrameToolStripMenuItem.Checked ? 1 : 0;
			nodeInArgs.mCALInArgs.mbSaveRawOutFile  = MainForm.saveRawFrameToolStripMenuItem.Checked ? 1 : 0;
            nodeInArgs.mCALInArgs.mbAnalyzerActive = MainForm.enableDisableROIGridToolStripMenuItem.Checked ? 1 : 0;


            for (int i = 0; i < MAX_PROCESS_THREAD_FLOW_LENGTH; i++)
            {
                if (null == mProcessthreadFlow[i])
                {
                    break;
                }

               eDecProcessRetVal = mProcessthreadFlow[i].Node_Process(ref nodeInArgs, ref nodeOutArgs);
               if (!eDecProcessRetVal.Equals(enProcessNodeRetValType_t.enProcessNodeRetCodeSuccess))
               {
                   bInsertJobToQueue = false;
                   break;
               }
            }
            //Output the solution
            if (bInsertJobToQueue.Equals(true))
            {                
                MainForm.g_Process_To_Display_Queue.Enqueue(nodeOutArgs.m_TaskInQueue);                
            }
            return eDecProcessRetVal;
        }

        public static enProcessNodeRetValType_t ProcessThreadFuncDestroy()
        {
            for (int i = 0; i < MAX_PROCESS_THREAD_FLOW_LENGTH; i++)
            {
                if (null != mProcessthreadFlow[i])
                {
                    mProcessthreadFlow[i].Node_Destroy();
                }
                 mProcessthreadFlow[i] = null;
            }

            return enProcessNodeRetValType_t.enProcessNodeRetCodeSuccess;
        }
    }

    public unsafe class RTPModuleobjAPI : Node_Interface
    { 
        [DllImport("RTPModule.dll")]
        static extern int RTPModuleObjAPICreate(string aFilename, int aeCodecType,int aResWidth, int aResHeight, int** apRTPObj);
        [DllImport("RTPModule.dll")]
        static extern bool RTPModuleObjAPIProcess(void* apRTPObj, void** apRTPSolution, int abSaveOutputFile, void *apExtraObj);
        [DllImport("RTPModule.dll")]
        static extern bool RTPModuleObjAPIReleaseSolution(void* apRTPObj, void* apRTPSolution);
        [DllImport("RTPModule.dll")]
        static extern bool RTPModuleObjAPIDestroy(void* apRTPObj);
        [DllImport("RTPModule.dll")]
        static extern bool RTPModuleObjAPIGetLastErrStr(char** apCharArr);

        int* mpRTPObj;
        enDspCodec mCodecType;
        string mFileName;
        private DSPMediaPlayer_Form MainForm;
        private unsafe char* mLastErrStr = null;
        

        public RTPModuleobjAPI(DSPMediaPlayer_Form Main_Form,string aInFileName, enDspCodec aCodecType)
        {
            MainForm = Main_Form;
            mFileName = aInFileName;
            mCodecType = aCodecType;
            Node_Create();

        }

        public override void Node_Create()
        {
            fixed (int** pRTPObj = &mpRTPObj)
            {

                RTPModuleObjAPICreate(mFileName, (int)mCodecType, Convert.ToInt32(MainForm.VideoWidth), Convert.ToInt32(MainForm.VideoHeight), pRTPObj);
                MainForm.g_ProcessThreadState = enThreadsState_t.enThreadstate_In_Progress;
            }

            return;
        }

        public override void Node_Destroy()
        {
            RTPModuleObjAPIDestroy(mpRTPObj);
            return;
        }

        public override enProcessNodeRetValType_t Node_Process(ref NodeInArgs arNodeInArgs,
                                                               ref NodeOutArgs arNodeOutArgs)
        {            
            bool bRetVal = true;
            fixed (void** pRTPSolution = &arNodeInArgs.mRTPInArgs.mRTPSolution)
            {
                fixed (t_ExtraSolutionInfo *pExtraInfoObj = &arNodeOutArgs.mExtraInfoObj)
                {
                    bRetVal = RTPModuleObjAPIProcess(mpRTPObj, pRTPSolution, arNodeInArgs.mRTPInArgs.mSaveOutFile, (void*)pExtraInfoObj);
                }               

            }
            arNodeInArgs.mRTPInArgs.mRTPReleaseSolutionCB = ReleaseRTPSolution;

            if (false == bRetVal)
            {
                fixed (char** charArrPtr = &mLastErrStr)
                {
                    RTPModuleObjAPIGetLastErrStr(charArrPtr);

                    MainForm.LogUTF_8_CharArr(*charArrPtr);
                }               
            }

            return (bRetVal == true) ? enProcessNodeRetValType_t.enProcessNodeRetCodeSuccess : enProcessNodeRetValType_t.enProcessNodeRetRTPSolutionErr;

        }

        public void ReleaseRTPSolution(ref NodeInArgs arNodeInArgs,
                                       ref NodeOutArgs arNodeOutArgs)
        {
            RTPModuleObjAPIReleaseSolution(mpRTPObj, arNodeInArgs.mRTPInArgs.mRTPSolution);
        }

    }




    public unsafe class CALobjAPI : Node_Interface
    {
        [DllImport("CAL_DLL.dll")]
        static extern bool CalObjAPICreate(int aCodecStandard, void** apCALObj, int aProcessOp, string apEncodedFileName);
        [DllImport("CAL_DLL.dll")]
        static extern bool CalObjAPIDestroy(void* apCALObj);

        [DllImport("CAL_DLL.dll")]
        static extern bool CalObjAPIProcess(void* apCALObj, void* apRTPSolution,ref CALInArgs CAL_InArgs, ref CAL_OutArgs_t CALStructInt);

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct CAL_OutArgs_t
        {
            
            public UInt32 m_FrameWidth;
            public UInt32 m_FrameHeight;
            public UInt32 m_FrameFPS;
            public UInt32 m_Profile_Idc;
            public bool   mb_IsFrameIntra;
            public UInt32 m_NumFramesUntilIntra;
            public UInt32 m_FrameTimeStamp;
            public bool mb_Avc_Tsvc_GopStruct;
            public UInt32 m_SharpIndication;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPArray, SizeConst = 3)]
            public IntPtr[] Mp_PlanData;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPArray, SizeConst = 3)]
            public IntPtr[] Mp_Org_PlanData;

            public CAL_OutArgs_t( int NumOfPlans)
            {
                this.Mp_PlanData            = new IntPtr[NumOfPlans];
                this.Mp_Org_PlanData        = new IntPtr[NumOfPlans];
                this.m_FrameWidth           = 0;
                this.m_FrameHeight          = 0;
                this.m_FrameFPS             = 0;
                this.mb_IsFrameIntra        = false;

                this.m_NumFramesUntilIntra  = 0;
                this.m_FrameTimeStamp       = 0;
                this.mb_Avc_Tsvc_GopStruct  = false;
                this.m_Profile_Idc          = 0;
                this.m_SharpIndication      = 0;
            }
        };
            
        private DSPMediaPlayer_Form MainForm;
        private int NumOfPlans = 3;
        void* mpCALObj;
        enDspCodec mCodecType;
        string mFileName;

        CAL_OutArgs_t CAL_OutArgsObj;

        public CALobjAPI(DSPMediaPlayer_Form Main_Form, CommonInArgs aCommonArgs)
        {
            MainForm = Main_Form;
            mCodecType = aCommonArgs.mCodectype;
            mFileName = aCommonArgs.mFileName;

            CAL_OutArgsObj = new CAL_OutArgs_t(NumOfPlans);

            MainForm.CurrSharpFramesFolderName =   "\\SharpFrames_"+DateTime.Now.ToString("MM.dd.yy")+"_"+DateTime.Now.ToString("h_mm_ss")+"\\";
            Node_Create();
        }

        public override void Node_Create()
        {
            string EncodedFileName = "";
            string EncodedFileNameExtension = "";

            if (MainForm.enProcessOp.Equals(Process_Operation.enEncoder))
            {
                switch(mCodecType)
                {
                    case enDspCodec.enDspCodecVP8:
                        {
                            EncodedFileNameExtension = ".vp8";
                            break;
                        }
                    case enDspCodec.enDspCodecH264:
                        {
                            EncodedFileNameExtension = ".264";
                            break;
                        }
                    case enDspCodec.enDspCodecH263:
                    case enDspCodec.enDspCodecH263P:
                        {
                            EncodedFileNameExtension = ".263";
                            break;
                        }
	                default:
                        break;
                }
               
                EncodedFileName = Path.Combine(Path.GetDirectoryName(MainForm.FileName), Path.GetFileNameWithoutExtension(MainForm.FileName) + EncodedFileNameExtension);
            }
            fixed (void** pCALObj = &mpCALObj)
            {
                bool retVal = CalObjAPICreate((int)mCodecType, pCALObj, (int)MainForm.enProcessOp, EncodedFileName);
            }
            
            return;
        }

        public override void Node_Destroy()
        {
            CalObjAPIDestroy(mpCALObj);
            return;
        }

        public override enProcessNodeRetValType_t Node_Process(ref NodeInArgs arNodeInArgs,
                                                                        ref NodeOutArgs arNodeOutArgs)
        {
          Processing_Type type;
          bool            valid;
          Bitmap          bit;
          UInt32          channelHandle;
          int             number;
          string          frameType;
          UInt32          frameTimeStamp;
          bool            isFrameIntra;
          UInt32          numFramesUntilIntra;
          bool            avc_Tsvc_GopStruct;
          string          frameResolution;
          UInt32          Profile_Idc;
          bool            bIsBitmapValid = true;



            bool bRetVal;
            arNodeInArgs.mCALInArgs.mCodecType = mCodecType;
            arNodeInArgs.mCALInArgs.m_Analyzer_IN_Params.mPSNR_Threshold            = MainForm.gPSNR_Threshold ;  
            arNodeInArgs.mCALInArgs.m_Analyzer_IN_Params.mPitchSkipThreshold        = MainForm.gPitchSkipThreshold;
            arNodeInArgs.mCALInArgs.m_Analyzer_IN_Params.m_CB_BackgroundColor_Value = MainForm.gCB_BackgroundColor_Value;
            arNodeInArgs.mCALInArgs.m_Analyzer_IN_Params.m_CR_BackgroundColor_Value = MainForm.gCR_BackgroundColor_Value; 
       

            bRetVal = CalObjAPIProcess(mpCALObj, arNodeInArgs.mRTPInArgs.mRTPSolution,
                                        ref arNodeInArgs.mCALInArgs,ref CAL_OutArgsObj);
            arNodeInArgs.mRTPInArgs.mRTPReleaseSolutionCB(ref arNodeInArgs, ref arNodeOutArgs);


            if (true == bRetVal)
            {


                arNodeOutArgs.mCALOutFrame.mFrame.YCbCrBuff[(int)enPlaneNum_t.eY_Plane] = (byte*)CAL_OutArgsObj.Mp_PlanData[(int)enPlaneNum_t.eY_Plane];
                arNodeOutArgs.mCALOutFrame.mFrame.YCbCrBuff[(int)enPlaneNum_t.eCb_Plane] = (byte*)CAL_OutArgsObj.Mp_PlanData[(int)enPlaneNum_t.eCb_Plane];
                arNodeOutArgs.mCALOutFrame.mFrame.YCbCrBuff[(int)enPlaneNum_t.eCr_Plane] = (byte*)CAL_OutArgsObj.Mp_PlanData[(int)enPlaneNum_t.eCr_Plane];
                arNodeOutArgs.mCALOutFrame.mFrame.FrameWidth = (int)CAL_OutArgsObj.m_FrameWidth;
                arNodeOutArgs.mCALOutFrame.mFrame.FrameHeight = (int)CAL_OutArgsObj.m_FrameHeight;

                if (Process_Operation.enEncoder == MainForm.enProcessOp)
                {

                    Bitmap bitMap = ConvertYUV2RGB(ref arNodeOutArgs.mCALOutFrame.mFrame);
                    if (null == bitMap)
                        return enProcessNodeRetValType_t.enRetCodeError;

                    type = Processing_Type.enVideo;
                    valid = true;
                    bit = bitMap;
                    bIsBitmapValid = true;
                    channelHandle = MainForm.ChannelHandle;
                    number = ++MainForm.CurrentFrameCount;
                    frameType = "";
                    frameTimeStamp = 0;
                    isFrameIntra = false;
                    numFramesUntilIntra = 0;
                    avc_Tsvc_GopStruct = false;
                    frameResolution = "";
                    Profile_Idc = 0;

                }
                else
                {

                    arNodeOutArgs.mCALOutFrame.mFrame.FrameFPS              = (int)CAL_OutArgsObj.m_FrameFPS;
                    arNodeOutArgs.mCALOutFrame.mFrame.FrameTimeStamp        = CAL_OutArgsObj.m_FrameTimeStamp;
                    arNodeOutArgs.mCALOutFrame.mFrame.m_Profile_Idc         = CAL_OutArgsObj.m_Profile_Idc;
                    arNodeOutArgs.mCALOutFrame.mFrame.m_NumFramesUntilIntra = CAL_OutArgsObj.m_NumFramesUntilIntra;
                    arNodeOutArgs.mCALOutFrame.mFrame.mb_IsFrameIntra       = CAL_OutArgsObj.mb_IsFrameIntra;
                    arNodeOutArgs.mCALOutFrame.mFrame.mb_Avc_Tsvc_GopStruct = CAL_OutArgsObj.mb_Avc_Tsvc_GopStruct;
                    if (1 == arNodeInArgs.mCALInArgs.mbAnalyzerActive)
                    {
                        arNodeOutArgs.mCALOutFrame.mFrame.m_SharpIndication = CAL_OutArgsObj.m_SharpIndication > 0 ? (CAL_OutArgsObj.m_SharpIndication > 1 ? "Sharper" : "Sharp") : "No_Sharp";
                    }
                    else
                    {
                        arNodeOutArgs.mCALOutFrame.mFrame.m_SharpIndication = "No_Sharp";
                    }
                    

                    Bitmap bitMap = ConvertYUV2RGB(ref  arNodeOutArgs.mCALOutFrame.mFrame);
                    if (null == bitMap)
                        return enProcessNodeRetValType_t.enRetCodeError;

                    if (true == MainForm.saveImageafterDecoderDisplayToolStripMenuItem.Checked)
                    {
                        string outFileName = Path.Combine(Directory.GetCurrentDirectory(), "afterDecoding.bmp");
                        bitMap.Save(outFileName, System.Drawing.Imaging.ImageFormat.Bmp);
                    }
                    else if((1 == arNodeInArgs.mCALInArgs.mbAnalyzerActive)
                            && 
                            (arNodeOutArgs.mCALOutFrame.mFrame.m_SharpIndication.Equals("Sharp")))
                    {
                        if (!Directory.Exists(DSPMediaPlayer_Form.DSP_MEDIA_PLAYER_SHARPFRAMES_WORKING_FOLDER + MainForm.CurrSharpFramesFolderName))
                        {
                            Directory.CreateDirectory(DSPMediaPlayer_Form.DSP_MEDIA_PLAYER_SHARPFRAMES_WORKING_FOLDER + MainForm.CurrSharpFramesFolderName);
                        }
                        string outFileName = Path.Combine(DSPMediaPlayer_Form.DSP_MEDIA_PLAYER_SHARPFRAMES_WORKING_FOLDER + MainForm.CurrSharpFramesFolderName, "FrmaeNum_" + (MainForm.CurrentFrameCount + 1).ToString() + "_SharpFrame.bmp");

                        arNodeOutArgs.mCALOutFrame.mFrame.YCbCrBuff[(int)enPlaneNum_t.eY_Plane] = (byte*)CAL_OutArgsObj.Mp_Org_PlanData[(int)enPlaneNum_t.eY_Plane];
                        arNodeOutArgs.mCALOutFrame.mFrame.YCbCrBuff[(int)enPlaneNum_t.eCb_Plane] = (byte*)CAL_OutArgsObj.Mp_Org_PlanData[(int)enPlaneNum_t.eCb_Plane];
                        arNodeOutArgs.mCALOutFrame.mFrame.YCbCrBuff[(int)enPlaneNum_t.eCr_Plane] = (byte*)CAL_OutArgsObj.Mp_Org_PlanData[(int)enPlaneNum_t.eCr_Plane];

                        ConvertYUV2RGB(ref  arNodeOutArgs.mCALOutFrame.mFrame).Save(outFileName, System.Drawing.Imaging.ImageFormat.Bmp);
                    }
                    
                    
                    string Resolution = arNodeOutArgs.mCALOutFrame.mFrame.FrameWidth.ToString() + "X" + arNodeOutArgs.mCALOutFrame.mFrame.FrameHeight.ToString();

                    type = Processing_Type.enVideo;
                    valid = true;
                    bit = bitMap;
                    bIsBitmapValid = true;
                    channelHandle = MainForm.ChannelHandle;
                    number = ++MainForm.CurrentFrameCount;
                    frameType = (CAL_OutArgsObj.mb_IsFrameIntra) ? "INTRA" : "P Frame";
                    frameTimeStamp = arNodeOutArgs.mCALOutFrame.mFrame.FrameTimeStamp;
                    isFrameIntra = CAL_OutArgsObj.mb_IsFrameIntra;
                    numFramesUntilIntra = CAL_OutArgsObj.m_NumFramesUntilIntra;
                    avc_Tsvc_GopStruct = CAL_OutArgsObj.mb_Avc_Tsvc_GopStruct;
                    frameResolution = Resolution;
                    Profile_Idc = arNodeOutArgs.mCALOutFrame.mFrame.m_Profile_Idc;

                }
            }
            else
            {
                type = Processing_Type.enVideo;
                valid = true;
                bit = null;
                bIsBitmapValid = false;
                channelHandle = MainForm.ChannelHandle;
                number = ++MainForm.CurrentFrameCount;
                frameType = (CAL_OutArgsObj.mb_IsFrameIntra) ? "INTRA" : "P Frame";
                frameTimeStamp = arNodeOutArgs.mCALOutFrame.mFrame.FrameTimeStamp;
                isFrameIntra = CAL_OutArgsObj.mb_IsFrameIntra;
                numFramesUntilIntra = CAL_OutArgsObj.m_NumFramesUntilIntra;
                avc_Tsvc_GopStruct = false;
                frameResolution = "";
                Profile_Idc = 0;

                if (true == CAL_OutArgsObj.mb_IsFrameIntra)
                {
                    MainForm.LogString("Error while decoding frame " + number.ToString());
                }                

            }


            arNodeOutArgs.m_TaskInQueue = new Process_To_Display_Queue(type, valid, bit, bIsBitmapValid,
                                                               channelHandle, number,
                                                               frameType,
                                                               frameTimeStamp,
                                                               isFrameIntra, numFramesUntilIntra,
                                                               avc_Tsvc_GopStruct, frameResolution,
                                                               arNodeOutArgs.mCALOutFrame.mFrame.m_Profile_Idc,
                                                               arNodeOutArgs.mExtraInfoObj,
                                                               arNodeOutArgs.mCALOutFrame.mFrame.m_SharpIndication);


           
            return enProcessNodeRetValType_t.enProcessNodeRetCodeSuccess;
        }

       
       
        private static byte clamp(float input)
        {
            if (input < 0) input = 0;
            if (input > 255) input = 255;
            return (byte)Math.Abs(input);
        }

        private void yuvtorgb(char y, char u, char v, ref char r, ref char g, ref char b)
        {



            r = (char)clamp((1192 * ((int)y - 16) + 1634 * ((int)u - 128)) >> 10);
            g = (char)clamp((1192 * ((int)y - 16) - 833 * ((int)v - 128) - 400 * ((int)u - 128)) >> 10);
            b = (char)clamp((1192 * ((int)y - 16) + 2066 * ((int)v - 128)) >> 10);

        }

        private Bitmap ConvertYUV2RGB(/*int w, int h,*/ ref Frame arFrame)
        {
           
            
            IntPtr OutBuffer;
            char y, u, v, r = ' ', g = ' ', b = ' ';

            int FrameWidth = arFrame.FrameWidth;
            int FrameHeight = arFrame.FrameHeight;

            //byte[] imageData = new byte[w * h * ch]; //you image data here
            Bitmap bitmap = new Bitmap(FrameWidth, FrameHeight, PixelFormat.Format32bppRgb);

            BitmapData bmData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            OutBuffer = bmData.Scan0;

            unsafe
            {

                byte*[] YCbcr = new byte*[3];

                    YCbcr[0] = arFrame.YCbCrBuff[(int)enPlaneNum_t.eY_Plane];
                    YCbcr[1] = arFrame.YCbCrBuff[(int)enPlaneNum_t.eCb_Plane];
                    YCbcr[2] = arFrame.YCbCrBuff[(int)enPlaneNum_t.eCr_Plane];

                int y_sum, u_sum, v_sum;
                int i,j, m,n;
                y_sum = u_sum = v_sum = 0;

                if (MainForm.showUToolStripMenuItem.Checked || MainForm.showVToolStripMenuItem.Checked)
                {
                    for (j = 0; j < FrameHeight / 8; j++)
		            {
			            for (i = 0; i < FrameWidth / 8; i++)
			            {
				            y_sum = u_sum = v_sum = 0;

				            for (m = 0; m < 4; m++)
				            {
					            for (n = 0; n < 4; n++)
					            {
						            u_sum +=  YCbcr[1][(j*4+m)*FrameWidth/2+ (i*4 + n)];
                                    v_sum += YCbcr[2][(j * 4 + m) * FrameWidth / 2 + (i * 4 + n)];
					            }
				            }
				            u_sum /= 16;
				            v_sum /= 16;

				            if (MainForm.showUToolStripMenuItem.Checked)//cb is checked
                            {
                                y_sum = u_sum;
                            }
				            else//MainForm.showVToolStripMenuItem.Checked - cr is checked
                            {
					            y_sum = v_sum;
                            }
				            //y_sum = u_sum*0.5 + v_sum*0.5;

				            for (m = 0; m < 8; m++)
				            {
					            for (n = 0; n < 8; n++)
					            {
                                    YCbcr[0][(j * 8 + m) * FrameWidth + (i * 8 + n)] = (byte)(char)y_sum;
					            }
				            }
			            }
		            }
                }
                else if(MainForm.showYToolStripMenuItem.Checked)
                {

                    for (j = 0; j < FrameHeight / 8; j++)
                    {
                        for (i = 0; i < FrameWidth / 8; i++)
                        {
                            y_sum = u_sum = v_sum = 0;

                            for (m = 0; m < 4; m++)
                            {
                                for (n = 0; n < 4; n++)
                                {
                                    u_sum += YCbcr[1][(j * 4 + m) * FrameWidth / 2 + (i * 4 + n)];
                                    v_sum += YCbcr[2][(j * 4 + m) * FrameWidth / 2 + (i * 4 + n)];
                                }
                            }
                            u_sum /= 16;
                            v_sum /= 16;

                            //y_sum = u_sum*0.5 + v_sum*0.5;
                            //y_sum = min(u_sum, v_sum);
                            y_sum = (((u_sum) > (v_sum)) ? (u_sum) : (v_sum));

                            for (m = 0; m < 8; m++)
                            {
                                for (n = 0; n < 8; n++)
                                {
                                    YCbcr[0][(j * 8 + m) * FrameWidth + (i * 8 + n)] = (byte)(char)y_sum;
                                }
                            }
                        }
                    }
                }

                if (MainForm.showYToolStripMenuItem.Checked || MainForm.showUToolStripMenuItem.Checked || MainForm.showVToolStripMenuItem.Checked)
                {
                    for (j = 0; j < FrameHeight / 2; j++)
                    {
                        for (i = 0; i < FrameWidth / 2; i++)
                        {
                            YCbcr[1][(j) * FrameWidth / 2 + (i)] = 128;
                            YCbcr[2][(j) * FrameWidth / 2 + (i)] = 128;
                        }
                    }
                }


                //Actual conversion
                for (int col = 0; col < FrameHeight / 2; col++)
                {
                    for (int row = 0; row < FrameWidth / 2; row++)
                    {



                        u = (char)YCbcr[1][col * FrameWidth / 2 + row];
                        v = (char)YCbcr[2][col * FrameWidth / 2 + row];
                        y = (char)YCbcr[0][col * 2 * FrameWidth + row * 2];
                        yuvtorgb(y, u, v, ref r, ref g, ref b);


                        int* BmpBytes = (int*)OutBuffer;
                        BmpBytes[col * 2 * FrameWidth + row * 2] = (r | (g) << 8 | (b) << 16);

                        y = (char)YCbcr[0][col * 2 * FrameWidth + row * 2 + 1];
                        yuvtorgb(y, u, v, ref r, ref g, ref b);

                        BmpBytes[col * 2 * FrameWidth + row * 2 + 1] = (r | (g) << 8 | (b) << 16);


                        y = (char)YCbcr[0][(col * 2 + 1) * FrameWidth + row * 2];
                        yuvtorgb(y, u, v, ref r, ref g, ref b);

                        BmpBytes[(col * 2 + 1) * FrameWidth + row * 2] = (r | (g) << 8 | (b) << 16);

                        y = (char)YCbcr[0][(col * 2 + 1) * FrameWidth + row * 2 + 1];
                        yuvtorgb(y, u, v, ref r, ref g, ref b);

                        BmpBytes[(col * 2 + 1) * FrameWidth + row * 2 + 1] = (r | (g) << 8 | (b) << 16);


                    }

                }


            }

            bitmap.UnlockBits(bmData);
            return bitmap;
        }

        #region YUV processing - comment
               

        #endregion YUV processing - comment
    }


    public unsafe class AudioCodecsWrapperDLL : Node_Interface
    {
        public enum enTestCodec_t
        {
            eEncoder = 0,
            eDecoder = 1
        }

        public enum enTestInFileFormat_t
        {
            enRaw,
            enDat
        }


        public enum enSupportedAudioSwCodecs
        {
            eSupportedAudioSwCodecBegin = -1,
            eSupportedAudioSwCodecFirst = 0,
            eSupportedAudioSwCodecG711A = eSupportedAudioSwCodecFirst,
            eSupportedAudioSwCodecG711U = 1,
            eSupportedAudioSwCodecG7221 = 2,
            eSupportedAudioSwCodecG722 = 3,
            eSupportedAudioSwCodecG7221C = 4,
            eSupportedAudioSwCodecG729I = 5,
            eSupportedAudioSwCodecAACLD = 6,
            eSupportedAudioSwCodecAACLC = 7,
            eSupportedAudioSwCodecOPUS = 8,
            eSupportedAudioSwCodecRaw = 9,
            eSupportedAudioSwCodecSiren14 = 10,
            eSupportedAudioSwCodecLast
        }

        [DllImport("AudioCodecsWrapperDLL.dll")]
        static extern int AudioCodecsWrapperAPICreate();
        [DllImport("AudioCodecsWrapperDLL.dll")]
        static extern bool AudioCodecsWrapperAPIDestroy();
        [DllImport("AudioCodecsWrapperDLL.dll")]
        static extern bool AudioCodecsWrapperAPIProcess(enTestCodec_t               aeCommandType,
                                                        enTestInFileFormat_t        aeStreamFormat,
                                                        string                      apInFileName,
                                                        string                      apOutFileName,
                                                        enSupportedAudioSwCodecs    aeCodec,
                                                        int                         aSamplingRate,
                                                        int                         aBitRate);



        enDspCodec mCodecType;
        string mFileName;
        string mOutFileName;
        private DSPMediaPlayer_Form MainForm;
        private int mSampleRate;
        private int mBitRate;
        private bool bProcessDone = false;

        public AudioCodecsWrapperDLL(DSPMediaPlayer_Form Main_Form, string aInFileName, enDspCodec aCodecType)
        {
            MainForm = Main_Form;
            mFileName = aInFileName;

            string currentTime = DateTime.Now.ToString().Replace(' ', '_').Replace(':', '_').Replace('-', '_');
            string uniqueFileName = Path.GetFileName(aInFileName) + "." + currentTime + ".processed"; 

            //mOutFileName = mFileName + ".processed";
            mOutFileName = Path.Combine(DSPMediaPlayer_Form.DSP_MEDIA_PLAYER_AUDIO_WORKING_FOLDER, uniqueFileName);
            mCodecType = aCodecType;
            Node_Create();
        }

        public override void Node_Create()
        {
            AudioCodecsWrapperAPICreate();
            return;
        }

        public override void Node_Destroy()
        {
            AudioCodecsWrapperAPIDestroy();
            return;
        }

        public override enProcessNodeRetValType_t Node_Process(ref NodeInArgs arNodeInArgs,
                                                               ref NodeOutArgs arNodeOutArgs)
        {
            if (true == bProcessDone)
            {
                return enProcessNodeRetValType_t.enProcessNodeRetAudioFinishPlaying;
            }
            else
            {
                bProcessDone = true;
            }

            //perform audio processing for codec different from WAV                    
            if (enDspCodec.enDspCodecWav != mCodecType)
            {
                AudioCodecsWrapperAPIProcess(enTestCodec_t.eDecoder, GetFileFormat(mFileName), mFileName, mOutFileName, GetCodec(mCodecType), mSampleRate, mBitRate);
            }
            else
            {
                mOutFileName = mFileName;
            }        

            arNodeOutArgs.m_TaskInQueue = new Process_To_Display_Queue(Processing_Type.enAudio, mOutFileName, mSampleRate, mBitRate);
            
            return enProcessNodeRetValType_t.enProcessNodeRetCodeSuccess;
        }


        private enTestInFileFormat_t GetFileFormat(string aFilename)
        {
            if (aFilename.ToLower().EndsWith("dat"))
            {
                return enTestInFileFormat_t.enDat;
            }
            else
            {
                return enTestInFileFormat_t.enRaw;
            }
        }

        private enSupportedAudioSwCodecs GetCodec(enDspCodec aCodecType)
        {
            switch (aCodecType)
            {
                case enDspCodec.enDspCodecRaw:
                    {
                        mSampleRate = MainForm.getLinearSampleRate();
                        mBitRate = 64000;
                        return enSupportedAudioSwCodecs.eSupportedAudioSwCodecRaw;
                    }  
                case enDspCodec.enDspCodecG7221C:
                    {
                        mSampleRate = 32000;
                        mBitRate = 48000;
                        return enSupportedAudioSwCodecs.eSupportedAudioSwCodecG7221C;
                    }                    
                case enDspCodec.enDspCodecAACLC:
                    {
                        mSampleRate = 32000;
                        mBitRate = 96000;
                        return enSupportedAudioSwCodecs.eSupportedAudioSwCodecAACLC;
                    }                    
                case enDspCodec.enDspCodecAACLD:
                    {
                        mSampleRate = 32000;
                        mBitRate = 96000;
                        return enSupportedAudioSwCodecs.eSupportedAudioSwCodecAACLD;
                    } 
                case enDspCodec.enDspCodecG7221:
                    {
                        mSampleRate = 16000;
                        mBitRate = 24000;
                        return enSupportedAudioSwCodecs.eSupportedAudioSwCodecG7221;
                    } 
                case enDspCodec.enDspCodecG711U:
                    {
                        mSampleRate = 8000;
                        mBitRate = 64000;
                        return enSupportedAudioSwCodecs.eSupportedAudioSwCodecG711U;
                    } 
                case enDspCodec.enDspCodecG711A:
                    {
                        mSampleRate = 8000;
                        mBitRate = 64000;
                        return enSupportedAudioSwCodecs.eSupportedAudioSwCodecG711A;
                    } 
                case enDspCodec.enDspCodecG722:
                    {
                        mSampleRate = 16000;
                        mBitRate = 64000;
                        return enSupportedAudioSwCodecs.eSupportedAudioSwCodecG722;
                    } 
                case enDspCodec.enDspCodecOpus:
                    {
                        mSampleRate = 48000;
                        mBitRate = 96000;
                        return enSupportedAudioSwCodecs.eSupportedAudioSwCodecOPUS;
                    } 
                case enDspCodec.enDspCodecG729:
                    {
                        mSampleRate = 8000;
                        mBitRate = 1000;
                        return enSupportedAudioSwCodecs.eSupportedAudioSwCodecG729I;
                    }
                case enDspCodec.enDspCodecSiren14:
                    {
                        mSampleRate = 32000;
                        mBitRate = 48000;
                        return enSupportedAudioSwCodecs.eSupportedAudioSwCodecSiren14;
                    }  
                default:
                    return enSupportedAudioSwCodecs.eSupportedAudioSwCodecBegin;
            }
        } 
    }
}
