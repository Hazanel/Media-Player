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

using NAudio;
using NAudio.Wave;

namespace DSPMediaPlayer
{
    class Display
    {
        private static Display DisplayObj;
        
        private static DSPMediaPlayer_Form MainForm;
        private static long StartFrameDisplayTime;

        private static MovieState Movie_State;
        //private static ViewMode View_Mode;

        public Display(DSPMediaPlayer_Form Main_Form)
        {
           
            MainForm = Main_Form;
            MainForm.g_DisplayThreadState = enThreadsState_t.enThreadstate_In_Progress;
           
           
        }
        public Display(){ }

        public static void DisplayThread(DSPMediaPlayer_Form Main_Form)
        {
           

            while (true)
            {

                 switch(Main_Form.g_DisplayThreadState)
                {
                    case (enThreadsState_t.enThreadstate_Pending_For_Destroy):
                        {
                            if(true == Main_Form.IsAllReadyForDestroy())
                            {                              
                                if (null != DisplayObj)
                                {  
                                        MainForm.g_Process_To_Display_Queue.Clear();
                                        MainForm.CurrentFrameCount = -1;
                                        MainForm.FrameQueueTail = -1;
                                        MainForm.FrameQueueHead = -1;
                                        Array.Clear(MainForm.FramesQueue, 0, MainForm.FramesQueue.Length);                                        
                                    
                                    DisplayObj = null;
                                }

                                MainForm.g_DisplayThreadState = enThreadsState_t.enThreadstate_Not_Initialized;
                            }

                            break;
                        }

                    case (enThreadsState_t.enThreadstate_In_Progress):
                        {
                            DisplayInProgress(Main_Form);

                            break;
                        }

                    case (enThreadsState_t.enThreadstate_Not_Initialized):
                        {
                            if (Main_Form.g_Process_To_Display_Queue.Count > 0)
                            {
                                DisplayObj = new Display(Main_Form);
                                StartFrameDisplayTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                                
                                Main_Form.g_DisplayThreadState = enThreadsState_t.enThreadstate_In_Progress;
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

        public static void DisplayInProgress(DSPMediaPlayer_Form Main_Form)
        {
            MovieState movieState = Main_Form.Movie_State;

            if (    0 == Main_Form.g_Process_To_Display_Queue.Count
                &&  !movieState.Equals(MovieState.enPlayer_Rewind))
            {
                return;
            }

            //we have a task to display


            Movie_State = MainForm.Movie_State;//Overcome compiler warning

            if (    (   Movie_State.Equals(MovieState.enPlayer_Play) 
                     || Movie_State.Equals(MovieState.enPlayer_Forward)
                     || Movie_State.Equals(MovieState.enPlayer_Stop))
                &&  (   (-1 < MainForm.FrameQueueHead) 
                     && (-1 < MainForm.FrameQueueTail))
                )
            {


                if (    Math.Abs(MainForm.FrameQueueHead - MainForm.FrameQueueTail) < MainForm.FramesQueue.Length - 1
                    &&  MainForm.g_Process_To_Display_Queue.Count > 0//Check Queue ain't empty
                    )
                {
                    int FPS = (MainForm.FrameQueueTail == MainForm.FrameQueueHead) ? MainForm.g_Process_To_Display_Queue.Peek().m_FrameFPS : MainForm.FramesQueue[MainForm.FrameQueueTail].m_FrameFPS;
                    if (0 == FPS)
                    {
                        FPS = MainForm.DefaultFpsPlay;
                    }
                    long CurrTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    if (CurrTime >= StartFrameDisplayTime + (long)((1000) / FPS))
                    {
                        StartFrameDisplayTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                        if (MainForm.FrameQueueTail == MainForm.FrameQueueHead)
                        {
                            MainForm.FrameQueueTail = MainForm.FrameQueueHead = (MainForm.FrameQueueHead + 1) % MainForm.FramesQueue.Length;

                            //check if we have there alreay an object, if tru ==> delete it (currently delete only the bitmap
                            if (    (true == MainForm.FramesQueue[MainForm.FrameQueueHead].mb_valid)
                                && (null != MainForm.FramesQueue[MainForm.FrameQueueHead].m_Pic))
                            {
                                MainForm.FramesQueue[MainForm.FrameQueueHead].m_Pic.Dispose();  
                            }

                            MainForm.FramesQueue[MainForm.FrameQueueHead] = MainForm.g_Process_To_Display_Queue.Dequeue();//), true, ++MainForm.CurrentFrameCount);

                        }
                        else//After rewind - display the frames we already have in the queue
                        {
                            MainForm.FrameQueueTail = (MainForm.FrameQueueTail + 1) % MainForm.FramesQueue.Length;
                        }

                    }
                
                }

            }
            else if (   (-1 == MainForm.FrameQueueHead) // First frame
                     && (-1 == MainForm.FrameQueueTail)
                     && MainForm.g_Process_To_Display_Queue.Count > 0)//Check Queue ain't empty 
            {
                MainForm.FrameQueueTail = MainForm.FrameQueueHead = (MainForm.FrameQueueHead + 1) % MainForm.FramesQueue.Length;
                MainForm.FramesQueue[MainForm.FrameQueueHead] = MainForm.g_Process_To_Display_Queue.Dequeue();
            }
            else if (0 < MainForm.g_Process_To_Display_Queue.Count)
            {
                MainForm.FramesQueue[MainForm.FrameQueueHead] = MainForm.g_Process_To_Display_Queue.Dequeue();
            }


            switch (MainForm.FramesQueue[MainForm.FrameQueueHead].m_Processing_Type)
            {
                case Processing_Type.enVideo:
                    {
                        if (false == Movie_State.Equals(MovieState.enPlayer_Stop))
                        {
                            HandleVideoJob();
                        }
                        else
                        {
                            SetPicture(null);
                        }
                        break;
                    }
                case Processing_Type.enTerminate:
                    {
                        MainForm.g_DisplayThreadState = enThreadsState_t.enThreadstate_Pending_For_Destroy;
                        break;
                    }
                case Processing_Type.enAudio:
                    {
                        HandleAudioJob();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
                 
        }

        public static void HandleAudioJob()
        {
            MainForm.HandleAudio(MainForm.FramesQueue[MainForm.FrameQueueHead].m_RawAudioFileName, 
                                 MainForm.FramesQueue[MainForm.FrameQueueHead].m_sampleRate, 
                                 MainForm.FramesQueue[MainForm.FrameQueueHead].m_bitRate);
        }


        public static void HandleVideoJob()
        {
            
            if (MainForm.FramesQueue[MainForm.FrameQueueTail].m_ChannelHandle.Equals(MainForm.ChannelHandle))
            {
                if (true == MainForm.FramesQueue[MainForm.FrameQueueTail].m_bIsPicValid)
                {
                    SetPicture(MainForm.FramesQueue[MainForm.FrameQueueTail].m_Pic);
                }                

                MainForm.setFrameNum(MainForm.FramesQueue[MainForm.FrameQueueTail].m_Number);
                if (!MainForm.FramesQueue[MainForm.FrameQueueTail].m_FrameType.Equals(MainForm.InfoLabelsObj.m_FrameType))
                {
                    MainForm.setFrameType(MainForm.FramesQueue[MainForm.FrameQueueTail].m_FrameType);
                }
                MainForm.FramesQueue[MainForm.FrameQueueTail].m_FrameFPS = GetFrameRate();
                SetDisplayInfo();
                MainForm.updateDisplayInfo();

                if (true == MainForm.FramesQueue[MainForm.FrameQueueTail].mb_IsFrameIntra)
                {
                    MainForm.updateExtraInfoTreeView(ref MainForm.FramesQueue[MainForm.FrameQueueTail].mExtraInfoObj, MainForm.meInputCodecType);
                }
                
            }
            
        }
         
                   
        public static void SetDisplayInfo()
        {
            MainForm.SetFrameResolution(MainForm.FramesQueue[MainForm.FrameQueueTail].m_Frame_Resolution);
            MainForm.setFrameType(MainForm.FramesQueue[MainForm.FrameQueueTail].m_FrameType);
            MainForm.setNumFramesUntilIntra(MainForm.FramesQueue[MainForm.FrameQueueTail].m_NumFramesUntilIntra.ToString());
            MainForm.setFrameTimeStamp(MainForm.FramesQueue[MainForm.FrameQueueTail].m_FrameTimeStamp.ToString());
            MainForm.SetFrameGopStruct(MainForm.FramesQueue[MainForm.FrameQueueTail].mb_Avc_Tsvc_GopStruct.Equals(true) ? "TSVC" : "AVC");
            MainForm.setFrameFPS(MainForm.FramesQueue[MainForm.FrameQueueTail].m_FrameFPS.ToString());
            MainForm.SetSharpIndiaction(MainForm.FramesQueue[MainForm.FrameQueueTail].m_SharpIndication);
            if (100 == MainForm.FramesQueue[MainForm.FrameQueueTail].m_Profile_Indication)
            {
                MainForm.SetProfileIndication("High");
            }
            else if (66 == MainForm.FramesQueue[MainForm.FrameQueueTail].m_Profile_Indication)
            {
                MainForm.SetProfileIndication("Base");
            }
            else if (77 == MainForm.FramesQueue[MainForm.FrameQueueTail].m_Profile_Indication)
            {
                MainForm.SetProfileIndication("Main");
            }
            else
            {
                MainForm.SetProfileIndication(string.Empty);
            }
        }

        private static int GetFrameRate()
        {
            int Count = 0;
            if (MainForm.FrameQueueTail < 0 || MainForm.FrameQueueHead < 0) return MainForm.DefaultFpsPlay;
            int J = MainForm.FrameQueueHead;

            for (; ; )
            {
                if (J == (MainForm.FrameQueueHead + 1) % MainForm.FramesQueue.Length)
                {
                    break;
                }

                J = (J + MainForm.FramesQueue.Length - 1) % MainForm.FramesQueue.Length;

                ++Count;
                if (MainForm.FramesQueue[J].mb_valid)
                {
                    if (TimeDiff(MainForm.FramesQueue[J].m_FrameTimeStamp, MainForm.FramesQueue[MainForm.FrameQueueHead].m_FrameTimeStamp) / 90 > 1000) break;
                }
                else
                {
                    J = (J + MainForm.FramesQueue.Length + 1) % MainForm.FramesQueue.Length;
                    --Count;
                    break;
                }
            }

            if (TimeDiff(MainForm.FramesQueue[J].m_FrameTimeStamp, MainForm.FramesQueue[MainForm.FrameQueueHead].m_FrameTimeStamp) == 0) return 0;
            double FR = ((double)Count) * 1000 * 90 / TimeDiff(MainForm.FramesQueue[J].m_FrameTimeStamp, MainForm.FramesQueue[MainForm.FrameQueueHead].m_FrameTimeStamp);
            return (int)Math.Round(FR, System.MidpointRounding.ToEven);

        }

        private static long TimeDiff(UInt32 StartTime, UInt32 EndTime)
        {
            return ((EndTime >= StartTime) ? (EndTime - StartTime) : (0xFFFFFFFF - (StartTime - EndTime)));
        }

        public static void  SetPicture(Bitmap img)
        {

            if (true == MainForm.saveImageafterDecoderDisplayToolStripMenuItem.Checked)
            {
                string outFileName = Path.Combine(Directory.GetCurrentDirectory(), "forDisplay.bmp");
                img.Save(outFileName, System.Drawing.Imaging.ImageFormat.Bmp);
            }

            if (MainForm.imagePanel1.InvokeRequired)
            {
                MethodInvoker del = delegate
                {
                    MainForm.imagePanel1.Image = img;
 
                   };
                MainForm.Invoke(del);
                return;
            }
            else
            {
                MainForm.imagePanel1.Image = img;
 
            }            
        } 
    }
   
}
