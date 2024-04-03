using System;
using System.Windows.Controls;
using ArchiveTransferManagerSample.Services;
using Genetec.Sdk;

namespace ArchiveTransferManagerSample
{
    /// <summary>
    ///     Interaction logic for ManualTransferGroupConstrol.xaml
    /// </summary>
    public partial class ManualTransferGroupControl : UserControl
    {
        private Engine m_engine;
        private QueryService m_queryService;

        public ManualTransferGroupControl()
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