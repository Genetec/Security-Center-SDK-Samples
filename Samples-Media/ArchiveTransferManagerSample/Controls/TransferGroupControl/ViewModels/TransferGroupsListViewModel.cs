using ArchiveTransferManagerSample.Services;
using ArchiveTransferManagerSample.ViewModels;
using GalaSoft.MvvmLight.CommandWpf;
using Genetec.Sdk;
using Genetec.Sdk.Entities.Video.ArchiveBackup;
using Genetec.Sdk.Entities.Video.TransferGroups;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using Application = System.Windows.Application;

namespace ArchiveTransferManagerSample.Controls.TransferGroupControl.ViewModels
{
    /// <summary>
    /// Class representing the different transfer that are in the different state of transferring(Pending, active, finished, idle, etc.)
    /// </summary>
    public class TransferGroupsListViewModel : ViewModelBase, IDisposable
    {
        private Engine m_engine;
        private QueryService m_queryService;

        public TransferGroupsListViewModel()
        {
            TransferGroups = new ObservableCollection<TransferGroupViewModel>();
            StartTransferButton = new RelayCommand(StartSelectedTransfer);
            DeleteButton = new RelayCommand(DeleteSelectedTransfer);
        }

        public ObservableCollection<TransferGroupViewModel> TransferGroups { get; set; }
        public TransferGroupViewModel SelectedTransferGroup { get; set; }

        public ICommand StartTransferButton { get; set; }

        public ICommand DeleteButton { get; set; }

        public void Dispose()
        {
            if (m_engine != null)
            {
                m_engine.EntitiesAdded -= Engine_EntitiesAdded;
                m_engine.EntitiesRemoved -= Engine_EntitiesRemoved;
                m_engine.EntitiesInvalidated -= Engine_EntitiesInvalidated;
                m_engine.TransferGroupManager.TransferStateChanged -= TransferGroupManagerOnTransferStateChanged;
                m_engine.Dispose();
            }
        }

        /// <summary>
        ///     Start transfer of the selected transfer and subscribe to the Transfers to get
        ///     <see cref="TransferGroupManagerOnTransferStateChanged" /> event.
        /// </summary>
        private void StartSelectedTransfer()
        {
            if (SelectedTransferGroup == null) return;
            m_engine.ArchiveTransferManager.StartTransfer(SelectedTransferGroup.EntityGuid);
            m_engine.TransferGroupManager.AddTransferGroupsToMonitor(TransferGroups.Select(x => x.EntityGuid));
        }

        /// <summary>
        /// Delete the selected transfer group from the app and the directory
        /// The change to the TransferGroups list is made under the application dispatcher since the GUI is going to change
        /// </summary>
        private void DeleteSelectedTransfer()
        {
            if (SelectedTransferGroup == null) return;
            var entityToDelete = SelectedTransferGroup.EntityGuid;
            m_engine.TransferGroupManager.RemoveTransferGroupsToMonitor(new[] { entityToDelete });
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                new Action(() => { TransferGroups.Remove(SelectedTransferGroup); }));
            m_engine.DeleteEntity(entityToDelete);
        }

        public void SetConnectedEngine(Engine engine)
        {
            if (m_engine == null)
            {
                m_engine = engine;
                m_engine.EntitiesAdded += Engine_EntitiesAdded;
                m_engine.EntitiesRemoved += Engine_EntitiesRemoved;
                m_engine.EntitiesInvalidated += Engine_EntitiesInvalidated;
                m_engine.TransferGroupManager.TransferStateChanged += TransferGroupManagerOnTransferStateChanged;

                m_queryService = new QueryService(m_engine);
                m_queryService.AddEntitiesToCache(new[]
                    {EntityType.TransferGroup, EntityType.Camera, EntityType.Role, EntityType.Agent});
            }
        }

        /// <summary>
        /// Event triggered when a transfer state change
        /// (Status change, progress change, bit-rate change, etc)
        /// The change to the TransferGroups list is made under the application dispatcher since the GUI is going to change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TransferGroupManagerOnTransferStateChanged(object sender, TransferStateChangedEventArgs e)
        {
            var state = e.ModifiedTransferStates.FirstOrDefault();
            var transferGroupItem = TransferGroups.FirstOrDefault(x => x.EntityGuid == state.Id);
            var index = TransferGroups.IndexOf(transferGroupItem);

            var item = state?.Items.FirstOrDefault();
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                if (item != null)
                {
                    transferGroupItem.ProgressPercent = item.Progression;
                    transferGroupItem.Status = item.Status;
                    transferGroupItem.StartTime = item.LastStart.ToString();
                    transferGroupItem.EndTime = item.LastEnd.ToString();
                    transferGroupItem.TransferSize = item.TransferedDataSize;
                }

                TransferGroups[index] = transferGroupItem;
            }));
        }

        private void Engine_EntitiesInvalidated(object sender, EntitiesInvalidatedEventArgs e)
        {
        }

        /// <summary>
        /// Event triggered when an entity is removed from the directory.
        /// The entity is filtered to only remove Transfer group entity from the TransferGroups list
        /// The change to the TransferGroups list is made under the application dispatcher since the GUI is going to change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Engine_EntitiesRemoved(object sender, EntitiesRemovedEventArgs e)
        {
            foreach (var entityUpdateInfo in e.Entities.Where(x =>
                x.EntityType == EntityType.TransferGroup && TransferGroups.Any(y => y.EntityGuid == x.EntityGuid)))
                MainWindow.ExecuteOnUIThread(() =>
                {
                    var transferGroup = TransferGroups.FirstOrDefault(x => x.EntityGuid == entityUpdateInfo.EntityGuid);
                    TransferGroups.Remove(transferGroup);
                });
        }

        /// <summary>
        /// Event triggered when a new entity is added in the directory
        /// The entity is filtered to only add transfer group item into the TransferGroups list
        /// Important, this event doesn't trigger if you do a restore or a manual backup service, since both are "Service" and not Entities
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Engine_EntitiesAdded(object sender, EntitiesAddedEventArgs e)
        {
            foreach (var entityUpdateInfo in e.Entities.Where(x =>
                x.EntityType == EntityType.TransferGroup && TransferGroups.All(y => y.EntityGuid != x.EntityGuid)))
            {

                var transferGroupEntity = m_engine.GetEntity(entityUpdateInfo.EntityGuid) as TransferGroup;
                var stuff = transferGroupEntity.GetCameras();

                MainWindow.ExecuteOnUIThread(() =>
                {
                    TransferGroups.Add(new TransferGroupViewModel
                    {
                        EntityGuid = transferGroupEntity.Guid,
                        EntityName = transferGroupEntity.Name,
                        EntityIcon = transferGroupEntity.GetIcon(true),
                        TransferType = transferGroupEntity.TransferGroupType,
                        Status = TransferStateStatus.Idle,
                        ProgressPercent = 0.0f,
                        StartTime = "Pending",
                        EndTime = "Never"
                    });
                });
            }
        }
    }
}