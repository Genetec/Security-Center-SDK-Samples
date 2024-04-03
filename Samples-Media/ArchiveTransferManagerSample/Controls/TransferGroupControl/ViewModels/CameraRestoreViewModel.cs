using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ArchiveTransferManagerSample.Services;
using ArchiveTransferManagerSample.ViewModels;
using GalaSoft.MvvmLight.CommandWpf;
using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.Roles;
using Genetec.Sdk.Workflows.ArchiveTransferManagers.ManualTransfers.Services;
using Microsoft.Win32;

namespace ArchiveTransferManagerSample.Controls.TransferGroupControl.ViewModels
{
    /// <summary>
    /// View model to the camera restore tab
    /// Allow to restore archive from backup 
    /// </summary>
    public class CameraRestoreViewModel : ViewModelBase
    {
        private QueryService m_agentQueryService;
        private QueryService m_archiverQueryService;
        private Engine m_engine;
        private MainWindow m_ownerWindow;
        private DateTime m_startTimeValue;
        private DateTime m_endTimeValue;
        private string m_labelFileName;
        private ObservableCollection<SecurityCenterEntityViewModel> m_archiverSources;
        private ObservableCollection<SecurityCenterEntityViewModel> m_agentSources;

        public CameraRestoreViewModel()
        {
            SaveButtonCommand = new RelayCommand(SaveRestoreTask);
            OpenFileCommand = new RelayCommand(OpenFileTask);
            AgentSources = new ObservableCollection<SecurityCenterEntityViewModel>();
            ArchiverSources = new ObservableCollection<SecurityCenterEntityViewModel>();
            StartTimeValue = DateTime.Now;
            EndTimeValue = DateTime.Now;
        }

        public ObservableCollection<SecurityCenterEntityViewModel> ArchiverSources
        {
            get => m_archiverSources;
            set
            {
                m_archiverSources = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SecurityCenterEntityViewModel> AgentSources
        {
            get => m_agentSources;
            set
            {
                m_agentSources = value;
                OnPropertyChanged();
            }
        }

        public SecurityCenterEntityViewModel SelectedArchiverSource { get; set; }
        public SecurityCenterEntityViewModel SelectedAgentSource { get; set; }
        /// <summary>
        /// The name of the file chosen to retrieve
        /// </summary>
        public string LabelFileName
        {
            get => m_labelFileName;
            set
            {
                m_labelFileName = value;
                OnPropertyChanged();
            }
        }

        private string FileName { get; set; }

        /// <summary>
        /// Get or Set the start time of the archive you want
        /// </summary>
        public DateTime StartTimeValue
        {
            get => m_startTimeValue;
            set
            {
                m_startTimeValue = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Get or Set the End time of the archive you want
        /// </summary>
        public DateTime EndTimeValue
        {
            get => m_endTimeValue;
            set
            {
                m_endTimeValue = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveButtonCommand { get; set; }
        public ICommand OpenFileCommand { get; set; }

        private void SaveRestoreTask()
        {
            if (LabelFileName == string.Empty) return;
            if (SelectedArchiverSource == null) return;
            if (SelectedAgentSource == null) return;

            var task = m_engine.ArchiveTransferManager.CreateManualRestoreService()
                .SetArchivesToRestore(new[]
                {
                    new ManualRestoreTransferService.RestoreData
                    {
                        FilePath = FileName,
                        StartTime = StartTimeValue,
                        EndTime = EndTimeValue
                    }
                })
                .SetArchiverAgent(SelectedAgentSource.EntityGuid)
                .SetDestination(SelectedArchiverSource.EntityGuid);
           
            task.StartTransfer();
        }
        /// <summary>
        /// Select the Backup File to restore to the archiver
        /// </summary>
        private void OpenFileTask()
        {
            var sourceDialog = new OpenFileDialog();
            if (sourceDialog.ShowDialog() != true) return;

            LabelFileName = sourceDialog.SafeFileName;
            FileName = sourceDialog.FileName;
        }

        public void SetConnectedEngine(Engine engine)
        {
            m_engine = engine;

            m_agentQueryService = new QueryService(m_engine);
            m_archiverQueryService = new QueryService(m_engine);

            m_agentQueryService.OnQueryCompleted += OnOnAgentQueryCompleted;
            m_archiverQueryService.OnQueryCompleted += OnOnArchiverQueryCompleted;

            m_agentQueryService.AddEntitiesToCache(new[] { EntityType.Agent });
            m_archiverQueryService.AddEntitiesToCache(new[] { EntityType.Role });
        }

        public void SetOwnerWindow(MainWindow owner)
        {
            m_ownerWindow = owner;
        }

        private void OnOnArchiverQueryCompleted(object sender, EventArgs e)
        {
            MainWindow.ExecuteOnUIThread(() =>
            {
                ArchiverSources = SetSource<ArchiverRole>(EntityType.Role);
            });
        }

        private void OnOnAgentQueryCompleted(object sender, EventArgs e)
        {
            MainWindow.ExecuteOnUIThread(() =>
            {
                AgentSources = SetSource<Agent>(EntityType.Agent);
            });
        }

        private ObservableCollection<SecurityCenterEntityViewModel> SetSource<TEntityType>(EntityType entityType)
            where TEntityType : Entity
        {
            var collection = new ObservableCollection<SecurityCenterEntityViewModel>();

            // Filter on ArchiverRole if it's Archiver type, or get everything otherwise
            var entitiesToFind = typeof(TEntityType) == typeof(ArchiverRole)
                ? m_engine.GetEntities(entityType)
                    .Where(x => x is ArchiverRole || x is Agent y && y.AgentType == AgentType.Archiver)
                    .Cast<TEntityType>()
                : m_engine.GetEntities(entityType).Where(x => true).Cast<TEntityType>();

            foreach (var entityFound in entitiesToFind)
            {
                var sc = new SecurityCenterEntityViewModel
                {
                    EntityGuid = entityFound.Guid,
                    EntityName = entityFound.Name,
                    EntityIcon = entityFound.GetIcon(true)
                };
                if (!collection.Contains(sc))
                    collection.Add(sc);
            }
            return collection;
        }
    }
}