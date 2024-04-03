using System;
using System.Windows.Controls;
using ArchiveTransferManagerSample.Services;
using Genetec.Sdk;

namespace ArchiveTransferManagerSample
{
    /// <summary>
    ///     Interaction logic for DuplicateArchiveControl.xaml
    /// </summary>
    public partial class DuplicateArchiveControl : UserControl
    {
        private Engine m_engine;
        private QueryService m_queryService;

        public DuplicateArchiveControl()
        {
            InitializeComponent();
        }

        public void Initialize(Engine engine)
        {
            m_engine = engine ?? throw new ArgumentNullException(nameof(engine));

            m_queryService = new QueryService(engine);
        }
    }
}