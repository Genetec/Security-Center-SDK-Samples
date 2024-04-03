using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ArchiveTransferManagerSample.Services;
using ArchiveTransferManagerSample.ViewModels;
using Genetec.Sdk;
using Genetec.Sdk.Entities;

namespace ArchiveTransferManagerSample.Controls
{
    /// <summary>
    /// Interaction logic for AddSourceDialog.xaml
    /// </summary>
    public partial class AddSourceDialog : Window, IDisposable
    {
        private Engine m_engine;
        private QueryService m_queryService;

        public ObservableCollection<SecurityCenterEntityViewModel> PotentialSources { get; set; }
        public SecurityCenterEntityViewModel SelectedSource { get; set; }
        public List<Guid> filterOutValues { get; set; }

        public AddSourceDialog(Engine engine, List<Guid> filter = null)
        {
            m_engine = engine;
            m_queryService = new QueryService(m_engine);
            m_queryService.OnQueryCompleted += QueryService_OnQueryCompleted;

            PotentialSources = new ObservableCollection<SecurityCenterEntityViewModel>();
            LoadEntities();

            InitializeComponent();
            filterOutValues = filter ?? new List<Guid>();
            DataContext = this;
        }

        private void LoadEntities()
        {
            m_queryService.AddEntitiesToCache(new List<EntityType> { EntityType.Camera, EntityType.Role });
        }

        private void QueryService_OnQueryCompleted(object sender, EventArgs e)
        {
            var cameras = m_engine.GetEntities(EntityType.Camera).Cast<Camera>();
            var archivers = m_engine.GetEntities(EntityType.Role).Where(x => x is ArchiverRole).Cast<ArchiverRole>();

            MainWindow.ExecuteOnUIThread(() =>
            {
                foreach (Camera camera in cameras)
                {
                    if (!filterOutValues.Contains(camera.Guid) && PotentialSources.All(x => x.EntityGuid != camera.Guid))
                        PotentialSources.Add(new SecurityCenterEntityViewModel
                        {
                            EntityGuid = camera.Guid,
                            EntityName = camera.Name,
                            EntityIcon = camera.GetIcon(true)
                        });
                }
                foreach (ArchiverRole archiverRole in archivers)
                {
                    if (!filterOutValues.Contains(archiverRole.Guid) && PotentialSources.All(x => x.EntityGuid != archiverRole.Guid))
                        PotentialSources.Add(new SecurityCenterEntityViewModel
                        {
                            EntityGuid = archiverRole.Guid,
                            EntityName = archiverRole.Name,
                            EntityIcon = archiverRole.GetIcon(true)
                        });
                }
            });
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public void Dispose()
        {
            m_queryService.OnQueryCompleted -= QueryService_OnQueryCompleted;
        }
    }
}
