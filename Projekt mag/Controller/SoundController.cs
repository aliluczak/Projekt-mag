using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Audio.OpenAL;
using OpenTK.Audio;
using NAudio.Wave;
using System.Media;
using System.Threading;


namespace Projekt_mag.Controller
{
    class SoundController
    {
        int _source;
        int _buffer;
        const string _fileName = "szum biały.wav";

        public SoundController()
        {
           // _source = AL.GenSource();
            //_buffer = AL.GenBuffer();
           
 //           SetParameters();
        }

        public void LoadAndPlay()
        {

            using (AudioContext audioContext = new AudioContext())
            {
                _source = AL.GenSource();
                _buffer = AL.GenBuffer();
                SetParameters();
                KeyValuePair<byte[], int> audio = LoadSound();

                AL.BufferData(_buffer, ALFormat.Mono16, audio.Key, audio.Key.Length* sizeof(byte), audio.Value);
                
                AL.Source(_source, ALSourcei.Buffer, _buffer);

                SetPosition(0, 20, 2);
                SetVolume(0.3f);
                var initialState = AL.GetSourceState(_source);
                AL.SourcePlay(_source);
                var currentSourceState = ALSourceState.Playing;
                while (!currentSourceState.Equals(ALSourceState.Stopped))
                {
                    Thread.Sleep(100);
                    //MoveSource(0, -1, 0);
                    currentSourceState = AL.GetSourceState(_source);
                }

                AL.DeleteBuffer(_buffer);
                AL.DeleteSource(_source);
            }
        }

        public void SetVolume(float volume)
        {
            AL.Source(_source, ALSourcef.Gain, volume);
        }

        public void SetPosition(float x, float y, float z)
        {
            AL.Source(_source, ALSource3f.Position, x, y, z);
        }

        public void SetVelocity(float x, float y, float z)
        {
            AL.Source(_source, ALSource3f.Velocity, x, y, z);
        }

        private void MoveSource(float x, float y, float z)
        {
            OpenTK.Vector3 currentPosition;
            AL.GetSource(_source, ALSource3f.Position, out currentPosition);
            AL.Source(_source, ALSource3f.Position, currentPosition.X + x, currentPosition.Y + y, currentPosition.Z + z);
        }

        #region Helpers
        private KeyValuePair<byte[], int> LoadSound()
        {
          //  var waveOut = new WaveOutEvent();
            WaveFileReader reader = new WaveFileReader(_fileName);
            byte[] buffer = new byte[reader.Length];
            int audioDataHandler = reader.Read(buffer, 0, buffer.Length);
            //waveOut.Init(reader);
            //waveOut.Play();
            KeyValuePair<byte[], int> audiodata = new KeyValuePair<byte[], int>(buffer, reader.WaveFormat.SampleRate);
            reader.Dispose();
            //waveOut.Dispose();
            return audiodata;
        }

        private void SetParameters()
        {
            AL.Listener(ALListener3f.Position, 0, 0, 0);
            AL.Listener(ALListener3f.Velocity, 0, 0, 0);
            AL.Source(_source, ALSource3f.Position, 0, 30, 0);
            AL.Source(_source, ALSource3f.Velocity, 0, 0, 0);
            AL.Source(_source, ALSource3f.Direction, 0, -1, 0);
        }

        #endregion
    }
}
