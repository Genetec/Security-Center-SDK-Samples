using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;
using Timer = System.Timers.Timer;

namespace AudioTransmitterSample
{
    public delegate void OnSendDataDel(byte[] data, int offset, int datasize);

    public class PcmAudioGenerator
    {
        #region Constants

        private readonly Timer m_timer;

        private readonly object m_internalLock = new object();

        private readonly int DefaultPayloadSize = 1920;

        #endregion

        #region Fields

        private OnSendDataDel m_proc;

        private int m_i;

        private bool m_isRunning;

        #endregion

        #region Properties

        public int SamplingRate { get { return 8000; } set { } }

        public int BitsPerSample { get { return 16; } set { } }

        //bitrate = (8000*16) = 128'000 bit per second
        //byterate = 128'000 / 8 = 16000 bytes per second
        //byterate = 16000 / 1000 = 16 bytes per millisecond
        //wait time (in ms) = PayloadSize (in Byte) / 16 (byte/ms)
        private int WaitTimeInMs { get { return GeneratedPayloadSize / 16;} }

        public int GeneratedPayloadSize { get; set; }

        #endregion

        #region Constructors

        public PcmAudioGenerator(OnSendDataDel proc)
        {
            GeneratedPayloadSize = DefaultPayloadSize;
            m_proc = proc;
            m_timer = new Timer {AutoReset = false};
            m_timer.Elapsed += OnTimerElapsed;
        }

        #endregion

        #region Destructors and Dispose Methods

        public void Dispose()
        {
            m_timer.Dispose();
        }

        #endregion

        #region Event Handlers

        private void OnTimerElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            lock (m_internalLock)
            {
                if (!m_isRunning)
                {
                    return;
                }

                ProcessFrame();
            }
        }

        #endregion

        #region Public Methods

        public void Start()
        {
            lock (m_internalLock)
            {
                m_isRunning = true;

                ProcessFrame();
            }
        }

        public void Stop()
        {
            lock (m_internalLock)
            {
                m_isRunning = false;
            }
        }

        #endregion

        #region Private Methods

        private int Formula(int t)
        {
            return (t >> 6 | t | t >> (t >> 16)) * 10 + ((t >> 11) & 7);
        }

        private void ProcessFrame()
        {
            byte[] data = new byte[GeneratedPayloadSize];


            for (int i = 0; i < GeneratedPayloadSize; i++, m_i++)
            {
                data[i] = (byte)Formula(m_i);
            }

            if (m_proc != null)
            {
                m_proc(data, 0, GeneratedPayloadSize);
            }

            if (m_timer != null)
            {
                m_timer.Interval = WaitTimeInMs-4;
                m_timer.Start();
            }
        }

        #endregion
    }
}
