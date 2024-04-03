using System;
using System.Windows;
using Genetec.Sdk;
using Genetec.Sdk.Entities;

namespace VideoViewer
{
    public partial class AddBookmark : Window
    {
        private IEngine m_sdkEngine;
        private Guid m_targetCamera;
        
        public AddBookmark(IEngine sdkEngine, Guid camera)
        {
            InitializeComponent();
            m_sdkEngine = sdkEngine;
            m_targetCamera = camera;
        }

        private void ButtonAddBookmark_OnClick(object sender, RoutedEventArgs e)
        { 
            m_sdkEngine.ActionManager.AddCameraBookmark(m_targetCamera, DateTime.UtcNow.AddHours(-1), textboxBookmark.Text);
        }
    }
}