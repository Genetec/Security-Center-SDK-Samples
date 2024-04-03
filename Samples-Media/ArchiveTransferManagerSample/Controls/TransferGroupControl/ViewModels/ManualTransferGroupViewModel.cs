using System;
using System.Collections.Generic;
using ArchiveTransferManagerSample.ViewModels;
using GalaSoft.MvvmLight.CommandWpf;
using Genetec.Sdk;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Genetec.Sdk.Entities;

namespace ArchiveTransferManagerSample.Controls.TransferGroupControl.ViewModels
{
    /// <summary>
    /// This class allow to Backup Archive or Retrieve Archive 
    /// Backup has no limitation
    /// Retrieve has the limitation that selected camera source must be edge trickling compatible (Check your camera configuration)
    /// </summary>
    public class ManualTransferGroupViewModel : ViewModelBase
    {
        private int m_days;
        private Engine m_engine;

        private bool m_isEverythingSelected;

        private string m_name;
        private MainWindow m_ownerWindow;

        private int m_simultaneousTransfers;
        private bool m_backupOrRetrieve;


        public ManualTransferGroupViewModel()
        {
            Name = "Transfer Group";
            IsEverythingSelected = true;
            Days = 1;
            SimultaneousTransfers = 1;
            BackupOrRetrieve = true;

            CamerasSources = new ObservableCollection<SecurityCenterEntityViewModel>();
            AddCameraButtonCommand = new RelayCommand(AddCameraSource);
            RemoveCameraButtonCommand = new RelayCommand(RemoveCameraSource);
            SaveManualTransferButtonCommand = new RelayCommand(SaveManualTransfer);
        }

        public ObservableCollection<SecurityCenterEntityViewModel> CamerasSources { get; set; }
        public SecurityCenterEntityViewModel SelectedCameraSource { get; set; }

        public ICommand AddCameraButtonCommand { get; set; }
        public ICommand RemoveCameraButtonCommand { get; set; }
        public ICommand SaveManualTransferButtonCommand { get; set; }

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

        public string Name
        {
            get => m_name;
            set => Set(ref m_name, value);
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
        private void SaveManualTransfer()
        {
            if (BackupOrRetrieve)
            {
                m_engine.ArchiveTransferManager.CreateBackupBuilder()
                       .SetName(Name)
                       .SetSources(CamerasSources.Select(x => x.EntityGuid))
                       .SetManual()
                       .SetSimultaneousDownload(SimultaneousTransfers)
                       .Build();
            }
            else
            {
                m_engine.ArchiveTransferManager.CreateRetrieveBuilder()
                    .SetName(Name)
                    .SetSources(CamerasSources.Select(x => x.EntityGuid))
                   .SetManual()
                   .SetSimultaneousDownload(SimultaneousTransfers)
                   .Build();
            }
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
        }

        public void SetOwnerWindow(MainWindow owner)
        {
            m_ownerWindow = owner;
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
    }
}