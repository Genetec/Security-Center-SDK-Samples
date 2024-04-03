using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Genetec.Sdk.Entities.Maps;
using Genetec.Sdk.Workspace;
using Genetec.Sdk.Workspace.Maps;
using Genetec.Sdk.Workspace.Services;

namespace ACEClient.Map
{
    /// <inheritdoc />
    /// <summary>
    /// Interaction logic for ACEMapObjectView.xaml
    /// </summary>
    public partial class CustomCameraMapObjectView : IMapObjectView
    {
        private readonly Workspace m_workspace;
        private readonly CustomEntityMapObject m_mapObject;
        
        //You MUST hide the existing MapObject property and replace it with this one
        public new MapObject MapObject { get { return m_mapObject; }}

        public CustomCameraMapObjectView()
        {
            InitializeComponent();

            MinWidth = 16;
            MinHeight = 16;
            MaxWidth = 128;
            MaxHeight = 128;
        }

        public CustomCameraMapObjectView(Workspace workspace, CustomEntityMapObject mapObject)
            : this()
        {
            // You MUST store a reference to the mapObject, and return that in the MapObject property
            m_mapObject = mapObject;
            m_workspace = workspace;
                       
            RelativeSizeMode = RelativeSizeMode.Auto;
            RelativeWidth = 192;
            RelativeHeight = 192;
        }

        protected override void OnClick(RoutedEventArgs e)
        {
            IsSelected = !IsSelected;
            e.Handled = true;
        }

        protected override void OnIsSelectedChanged()
        {
            base.OnIsSelectedChanged();

            if (IsSelected)
            {
                var service = m_workspace.Services.Get<IContentBuilderService>();
                if(service == null) return;
                var contentGroup = service.Build(MapObject.LinkedEntity);
                if (contentGroup != null)
                {
                    DisplayTile(contentGroup);
                }
            }
            else
            {
                HideTile();
            }
        }
    }
}
