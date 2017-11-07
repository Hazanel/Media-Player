using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NAudio;
using NAudio.Wave;
using System.Threading;

namespace DSPMediaPlayer.Audio
{
    public delegate void WaveControlCB();

    public enum WaveControlState
    {
        enWaveControlState_Idle,
        enWaveControlState_Playing,
        enWaveControlState_StopReq

    };


    public partial class WaveControl_UI : UserControl
    {
        private IWavePlayer player;
        private AudioFileReader audio;
        private WaveControlState mWaveControlState = WaveControlState.enWaveControlState_Idle;
        private Thread mPlayerThread;
        private string mWavFileName = "";
        private int mSamplerate;

        private WaveControlCB CBFunc;

        public WaveControl_UI()
        {
            InitializeComponent();

            CBFunc += WaveControlCBFunc;
        }

        public bool HandleAudioWavFile(string aRawFile, int aSampleRate, int aBitRate)
        {
            mSamplerate = aSampleRate;

            mWavFileName = aRawFile + ".wav";

            try
            {
                if (!aRawFile.ToLower().EndsWith(".wav"))
                {
                    NAudio.Wave.WaveFileWriter ws = new NAudio.Wave.WaveFileWriter(mWavFileName, NAudio.Wave.WaveFormat.CreateCustomFormat(NAudio.Wave.WaveFormatEncoding.Pcm, aSampleRate, 1, aBitRate / 8, 1, 16));
                    byte[] fileData = System.IO.File.ReadAllBytes(aRawFile);
                    ws.Write(fileData, 0, fileData.Length);
                    ws.Close();
                    ws.Dispose();
                }
                else
                {
                    mWavFileName = aRawFile;
                }

                waveControl1.Filename = mWavFileName;
                waveControl1.Read();
                waveControl1.CBFunc = CBFunc;

                updateAudioInfo();
				
				//play the all window
                PlayWav(0,100);

            }
            catch (System.Exception ex)
            {
                return false;
            }

            return true;
        }


        private void PlayWav(double aStartLocationInPercentage, double aEndLocationInPercentage)
        {
         	
			//wait until the playing is stoped   
            while (WaveControlState.enWaveControlState_Idle != mWaveControlState)
            {
                mWaveControlState = WaveControlState.enWaveControlState_StopReq;
                System.Threading.Thread.Sleep(100);

                if (!mPlayerThread.IsAlive)
                {
                    mWaveControlState = WaveControlState.enWaveControlState_Idle;
                }
            }

            audio = new AudioFileReader(mWavFileName);

            //seek the correct location
			//NOTE: for the case we are not in full window (0, 100)
            if (!(    (0 == aStartLocationInPercentage)
                  &&  (100 == aEndLocationInPercentage)))
            {
                double seekLocation = audio.Length * aStartLocationInPercentage / 100;
                //verify that the seek number is even (as each sample is 2 bytes)
                int seekLocationFixed = (int)seekLocation;
                if (0 != (seekLocationFixed % 2))
                {
                    seekLocationFixed++;
                }

                double seekLength = (double)(audio.Length * aEndLocationInPercentage / 100) - seekLocation;
                int seekLengthFixed = (int)seekLength;
				//verify that the seek number is even (as each sample is 2 bytes)
                if (0 != (seekLengthFixed % 2))
                {
                    seekLengthFixed++;
                }

                audio.Seek(seekLocationFixed, System.IO.SeekOrigin.Begin);
                //not supported
                //audio.SetLength(seekLengthFixed);
            }


            player = new WaveOut(WaveCallbackInfo.FunctionCallback());
            player.Init(audio);


            mWaveControlState = WaveControlState.enWaveControlState_Playing;

            mPlayerThread = new Thread(() => PlayerthreadFunc());
            mPlayerThread.Start();

        }

        private void PlayerthreadFunc()
        {
            player.Play();

            while (     (player.PlaybackState == PlaybackState.Playing)
                            && (WaveControlState.enWaveControlState_Playing == mWaveControlState)
                   ||   (player.PlaybackState == PlaybackState.Paused))
            {
                System.Threading.Thread.Sleep(100);
            }

            player.Stop();
            mWaveControlState = WaveControlState.enWaveControlState_Idle;
            
        }


        private void StopButton_Click(object sender, EventArgs e)
        {
            mWaveControlState = WaveControlState.enWaveControlState_StopReq;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {

            int startPercentage = 0;
            int endPercentage = 0;
			
			//getting the start\end location from teh wave control 
            waveControl1.GetXCoordinatesinPercentage(ref startPercentage, ref endPercentage);

			//play the audio
            PlayWav(startPercentage, endPercentage);
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            if ("pause" == PauseButton.Text.ToLower())
            {
                player.Pause();
                PauseButton.Text = "continue";
            }
            else
            {
                player.Play();
                PauseButton.Text = "pause";
            }

        }


        public void WaveControlCBFunc()
        {
            if (this.AudioInfo.InvokeRequired)
            {
                this.AudioInfo.Invoke(new MethodInvoker(
                delegate()
                {
                    updateAudioInfo();
                }));
            }
            else
            {
                updateAudioInfo();
            }
        }


        private void updateAudioInfo()
        {
            int numSamples = 0;
            double duration = 0;
            double currentCurser = 0;
            double userSelectionDurationInMs = 0;

            waveControl1.GetInfo(ref numSamples, ref currentCurser, ref userSelectionDurationInMs);

           duration = (double)numSamples / (double)mSamplerate;

            AudioInfo.Text = "Duration(sec)= " + duration.ToString();
            AudioInfo.Text += "    ";
            AudioInfo.Text += "Samples= " + numSamples.ToString();
            AudioInfo.Text += "    ";
            AudioInfo.Text += "Curser(sec)= " + (currentCurser / mSamplerate).ToString("00.00");
            AudioInfo.Text += "    ";
            AudioInfo.Text += "selection(sec)= " + (userSelectionDurationInMs / mSamplerate).ToString("00.00"); 
            AudioInfo.Refresh();
        }
    }
}
