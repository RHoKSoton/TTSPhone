using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio;
using NAudio.Wave;
using System.IO;

namespace TTSPhone
{
    public class Audio
    {
        WaveIn waveIn = new WaveIn();
        WaveOut waveOut = new WaveOut();
        public WaveFileWriter writer;
        public MemoryStream ms;
        bool accessing = false;
        public int SampleRate = 8000;
        public int Channels = 1;
        private byte[] SampledData = new byte[1024];

        public Audio()
        {
            waveIn.DeviceNumber = 0;
            waveIn.WaveFormat = new WaveFormat(SampleRate,Channels);
            waveIn.DataAvailable += DataReceived;
        }

        public void DataReceived(object sender, WaveInEventArgs e)
        {
            for ( int index = 0; index < e.BytesRecorded; index+=2)
            {
                short sample = (short)((e.Buffer[index]<<8)|(e.Buffer[index]));
                float sample32 = sample/ 32786f;
                byte[] floatBytes = BitConverter.GetBytes(sample32);
                int currentLength = SampledData.Length -1;
                for (int i = 0; i<floatBytes.Length;i++)
                {
                    SampledData[currentLength+i] = floatBytes[i];
                }
            }
        }

        public void StartWrite()
        {
            ms = new MemoryStream();
            writer = new WaveFileWriter(ms,new WaveFormat(8000,1));
        }

        public void StartPlaying()
        {
            waveOut.Play();
        }

        public void StartRecording()
        {
            waveIn.StartRecording();
        }

        public void StopRecording()
        {
            waveIn.StopRecording();
        }

        public byte[] GetData()
        {
            byte[] returnData = SampledData;
            SampledData = new byte[1024];
            return returnData;
        }
    }
}
