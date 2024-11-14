using ContextualAction;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Workspace;
using Genetec.Sdk.Workspace.ContextualAction;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace ContextualActionSample
{
    internal class PopupMessageContextualAction : Genetec.Sdk.Workspace.ContextualAction.ContextualAction
    {
        public override Guid Id => Guid.Parse("02c619f7-b44c-41e3-a8a2-3379b7159e52");

        public PopupMessageContextualAction()
        {
            Name = "Popup Message";
            Icon = Resources.Message.ToBitmapSource();
        }

        public override bool CanExecute(ContextualActionContext context)
        {
            if (context is EntityBrowserContextualActionContext || context is SystemStatusContextualActionContext)
                return true;

            return false;
        }

        public override bool Execute(ContextualActionContext context)
        {
            string caption;
            Entity selectedEntity;

            if (context is EntityBrowserContextualActionContext ebContext)
            {
                caption = "Hello there Entity Browser user!!!";
                selectedEntity = Workspace.Sdk.GetEntity(ebContext.SelectedEntities.FirstOrDefault());
            }
            else if (context is SystemStatusContextualActionContext ssContext)
            {
                caption = "Hello there System Status user!!!";
                selectedEntity = Workspace.Sdk.GetEntity(ssContext.SelectedEntities.FirstOrDefault());
            }
            else
                return false;

            if (selectedEntity == null)
                MessageBox.Show("Please select an entity", caption);
            else
                MessageBox.Show($"My name is {selectedEntity.Name}.\r\nI'm a {selectedEntity.EntityType}.\r\nMy GUID is {selectedEntity.Guid}.\r\nNice to meet you 😊", caption);

            return true;
        }
    }

    public static class Extentions
    {
        public static BitmapSource ToBitmapSource(this Bitmap source)
        {
            BitmapSource bitmapSource;
            var bitmap = source.GetHbitmap(Color.FromArgb(0, 0, 0, 0));

            try
            {
                bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                    bitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Win32Exception)
            {
                bitmapSource = null;
            }

            return bitmapSource;
        }
    }
}
