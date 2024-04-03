using System;
using System.Collections.Generic;
using ArchiveTransferManagerSample.ViewModels;
using GalaSoft.MvvmLight.CommandWpf;
using Genetec.Sdk;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Genetec.Sdk.Entities;
using ArchiveTransferManagerSample.Services;

namespace ArchiveTransferManagerSample.Controls.TransferGroupControl.ViewModels
{
    /// <summary>
    /// This class allow to Duplicate Archive
    /// Duplicate has no limitation
    /// </summary>
    public class DuplicateArchiveViewModel : ViewModelBase
    {
        private int m_days;
        private QueryService m_archiverQueryService;
        private Engine m_engine;

        private bool m_isEverythingSelected;

        private MainWindow m_ownerWindow;

        private int m_simultaneousTransfers;
        private bool m_backupOrRetrieve;
        private ObservableCollection<SecurityCenterEntityViewModel> m_archiverSources;


        public DuplicateArchiveViewModel()
        {
            IsEverythingSelected = true;
            Days = 1;
            SimultaneousTransfers = 1;
            BackupOrRetrieve = true;

            CamerasSources = new ObservableCollection<SecurityCenterEntityViewModel>();
            AddCameraButtonCommand = new RelayCommand(AddCameraSource);
            RemoveCameraButtonCommand = new RelayCommand(RemoveCameraSource);
            SaveManualDuplicateButtonCommand = new RelayCommand(SaveManualDuplicate);
            ArchiverSources = new ObservableCollection<SecurityCenterEntityViewModel>();
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

        public ObservableCollection<SecurityCenterEntityViewModel> CamerasSources { get; set; }
        public SecurityCenterEntityViewModel SelectedCameraSource { get; set; }
        public SecurityCenterEntityViewModel SelectedArchiverSource { get; set; }

        public ICommand AddCameraButtonCommand { get; set; }
        public ICommand RemoveCameraButtonCommand { get; set; }
        public ICommand SaveManualDuplicateButtonCommand { get; set; }

        public bool BackupOrRetrieve
        {
            get => m_backupOrRetrieve;
            set
            {
                if (value != BackupOrRetrieve)
                {
                    m_backupOrRetrieve = value;
                    CamerasSources?.Clear();
                    OnPropertyChanged();
                }
            }
        }

        public bool IsEverythingSelected
        {
            get => m_isEverythingSelected;
            set => Set(ref m_isEverythingSelected, value);
        }

        public int Days
        {
            get => m_days;
            set => Set(ref m_days, value);
        }

        public int SimultaneousTransfers
        {
            get => m_simultaneousTransfers;
            set => Set(ref m_simultaneousTransfers, value);
        }
        private void SaveManualDuplicate()
        {
            if (SelectedArchiverSource == null) return;

            var duplicate = m_engine.ArchiveTransferManager.CreateManualTricklingTransferHandler();

            duplicate.SetSources(CamerasSources.Select(x => x.EntityGuid));
            duplicate.SetDestination(SelectedArchiverSource.EntityGuid);
            duplicate.SetSimultaneousDownload(SimultaneousTransfers);
            duplicate.StartTransfer();

            CamerasSources.Clear();
        }

        private void RemoveCameraSource()
        {
            if (SelectedCameraSource == null) return;
            if (CamerasSources.Contains(SelectedCameraSource))
            {
                CamerasSources.Remove(SelectedCameraSource);
            }
        }

        public void SetConnectedEngine(Engine engine)
        {
            m_engine = engine;

            m_archiverQueryService = new QueryService(m_engine);
            m_archiverQueryService.OnQueryCompleted += OnOnArchiverQueryCompleted;
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

        private void AddCameraSource()
        {
            List<Guid> filter = new List<Guid>();
            if (!BackupOrRetrieve)
            {
                foreach (var task in m_engine.TransferGroupManager.GetAllBackupTasks())
                    filter.AddRange(task.Cameras);
            }
            filter.AddRange(CamerasSources.Select(x => x.EntityGuid));
            using (var sourceDialog = new AddSourceDialog(m_engine, filter) { Owner = m_ownerWindow })
            {
                if (sourceDialog.ShowDialog() == true)
                {
                    CamerasSources.Add(sourceDialog.SelectedSource);
                }
            }
        }

        private ObservableCollection<SecurityCenterEntityViewModel> SetSource<TEntityType>(EntityType entityType)
            where TEntityType : Entity
        {
            var collection = new ObservableCollection<SecurityCenterEntityViewModel>();

            // Filter on ArchiverRole if it's Archiver type, or get everything otherwise
            var entitiesToFind = typeof(TEntityType) == typeof(ArchiverRole)
                ? m_engine.GetEntities(entityType)
                    .Where(x => x is ArchiverRole)
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