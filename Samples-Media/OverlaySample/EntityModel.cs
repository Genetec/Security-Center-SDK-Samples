using System.Collections.Generic;
using System.Windows.Media;
using Genetec.Sdk.Media.Overlay;
using System;
using System.Windows;

namespace OverlaySample
{
    public class EntityModel : DependencyObject
    {
        public static readonly DependencyProperty EntityNameProperty =
            DependencyProperty.Register("EntityName", typeof(string), typeof(EntityModel), new PropertyMetadata(""));

        public string EntityName
        {
            get { return (string)GetValue(EntityNameProperty); }
            set { SetValue(EntityNameProperty, value); }
        }

        public Guid Id { get; set; }
    }

    public class CameraModel : EntityModel
    {
        public static readonly DependencyProperty CurrentIconProperty =
            DependencyProperty.Register("CurrentIcon", typeof(ImageSource), typeof(CameraModel), new PropertyMetadata(null));

        public ImageSource CurrentIcon
        {
            get { return (ImageSource)GetValue(CurrentIconProperty); }
            set { SetValue(CurrentIconProperty, value); }
        }
    }

    public class MetadataStreamModel : EntityModel
    {
        public static readonly DependencyProperty IsEditingProperty =
            DependencyProperty.Register("IsEditing", typeof(bool), typeof(MetadataStreamModel), new PropertyMetadata(false));

        public static readonly DependencyProperty IsViewingProperty =
            DependencyProperty.Register("IsViewing", typeof(bool), typeof(MetadataStreamModel), new PropertyMetadata(false));

        public static readonly DependencyProperty IsShowingTimeProperty =
            DependencyProperty.Register("IsShowingTime", typeof (bool), typeof (MetadataStreamModel), new PropertyMetadata(false));

        public bool IsViewing
        {
            get { return (bool)GetValue(IsViewingProperty); }
            set { SetValue(IsViewingProperty, value); }
        }

        public bool IsEditing
        {
            get { return (bool)GetValue(IsEditingProperty); }
            set { SetValue(IsEditingProperty, value); }
        }

        public bool IsShowingTime
        {
            get { return (bool) GetValue(IsShowingTimeProperty); }
            set { SetValue(IsShowingTimeProperty, value); }
        }

        public Overlay Overlay { get; set; }
        public Queue<Layer> EditingLayers { get; set; }

        public Layer HourLayer { get; set; }
        public Layer MinuteLayer { get; set; }
        public Layer SecondLayer { get; set; }

        public MetadataStreamModel()
        {
            EditingLayers = new Queue<Layer>();
        }
    }
}
