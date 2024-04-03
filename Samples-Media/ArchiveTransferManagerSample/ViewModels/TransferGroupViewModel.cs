using Genetec.Sdk;
using Genetec.Sdk.Entities.Video.ArchiveBackup;

namespace ArchiveTransferManagerSample.ViewModels
{
    /// <summary>
    /// This class represent the transfer group data we need to fill the our list
    /// </summary>
    public class TransferGroupViewModel : SecurityCenterEntityViewModel
    {
        public string m_endTime;
        private float m_progressPercent;
        private string m_startTime;
        private TransferStateStatus m_status;
        public long transferSize;
        private string m_recurrence;
        public TransferGroupType TransferType { get; set; } 

        public float ProgressPercent
        {
            get => m_progressPercent;
            set
            {
                m_progressPercent = value;
                OnPropertyChanged();
            }
        }

        public TransferStateStatus Status
        {
            get => m_status;
            set
            {
                m_status = value;
                OnPropertyChanged();
            }
        }

        public string StartTime
        {
            get => m_startTime;
            set
            {
                m_startTime = value;
                OnPropertyChanged();
            }
        }

        public string EndTime
        {
            get => m_endTime;
            set
            {
                m_endTime = value;
                OnPropertyChanged();
            }
        }

        public long TransferSize
        {
            get => transferSize;
            set
            {
                transferSize = value;
                OnPropertyChanged();
            }
        }

        public string recurrence
        {
            get => m_recurrence;
            set
            {

                m_recurrence = value;
                OnPropertyChanged();
            }
        }
    }
}