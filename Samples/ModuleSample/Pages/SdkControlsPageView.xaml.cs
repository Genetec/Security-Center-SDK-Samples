// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using ModuleSample.Controls;
using ModuleSample.Pages.Controls;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ModuleSample.Pages
{

    public partial class SdkControlsPageView
    {

        #region Public Fields

        public static readonly DependencyProperty CurrentCultureProperty =
                    DependencyProperty.Register
                    ("CurrentCulture", typeof(CultureInfo), typeof(SdkControlsPageView),
                        new UIPropertyMetadata(new CultureInfo("en"), OnPropertyCurrentCultureChanged));

        public static readonly DependencyProperty PagesProperty =
                    DependencyProperty.Register
                    ("Pages", typeof(ObservableCollection<ControlPage>), typeof(SdkControlsPageView),
                        new UIPropertyMetadata(new ObservableCollection<ControlPage>()));

        #endregion Public Fields

        #region Private Fields

        private readonly ObservableCollection<CultureInfo> m_cultures = new ObservableCollection<CultureInfo>();

        #endregion Private Fields

        #region Public Properties

        public ReadOnlyObservableCollection<CultureInfo> Cultures => new ReadOnlyObservableCollection<CultureInfo>(m_cultures);

        public CultureInfo CurrentCulture
        {
            get => (CultureInfo)GetValue(CurrentCultureProperty);
            set => SetValue(CurrentCultureProperty, value);
        }

        public ObservableCollection<ControlPage> Pages
        {
            get => (ObservableCollection<ControlPage>)GetValue(PagesProperty);
            set => SetValue(PagesProperty, value);
        }

        #endregion Public Properties

        #region Public Constructors

        public SdkControlsPageView()
        {
            LoadCultures();

            InitializeComponent();

            Loaded += OnViewLoaded;
        }

        #endregion Public Constructors

        #region Private Methods

        private static void OnPropertyCurrentCultureChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is SdkControlsPageView instance)
            {
                instance.OnCurrentCulturedChanged();
            }
        }

        private void AddPage(UserControl control, string iconUri)
        {
            var page = new ControlPage { Control = control };
            if (!string.IsNullOrEmpty(iconUri))
                page.Icon = new BitmapImage(new Uri("/ModuleSample;component/Resources/" + iconUri + ".png", UriKind.Relative));
            if (control.Tag != null)
                page.Title = control.Tag.ToString();
            Pages.Add(page);
        }

        private void AddPageDefault(UserControl control, string iconUri)
        {
            AddPage(control, iconUri);
            m_list.SelectedItem = Pages[Pages.Count - 1];
        }

        private void LoadCultures()
        {
            m_cultures.Clear();

            foreach (var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                m_cultures.Add(culture);
            }
        }

        private void OnCurrentCulturedChanged()
        {
            FlowDirection = CurrentCulture.TextInfo.IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

            Thread.CurrentThread.CurrentCulture = CurrentCulture;
            Thread.CurrentThread.CurrentUICulture = CurrentCulture;
        }

        private void OnListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (m_list.SelectedItem is ControlPage page)
            {
                m_contentContainer.Child = page.Control;
            }
        }

        private void OnViewLoaded(object sender, RoutedEventArgs e)
        {
            Pages.Clear();
            AddPageDefault(new All(), string.Empty);
            AddPage(new Buttons(), string.Empty);
            AddPage(new DatePickers(), string.Empty);
            AddPage(new Expanders(), string.Empty);
            AddPage(new Groups(), string.Empty);
            AddPage(new Inputs(), string.Empty);
            AddPage(new Labels(), string.Empty);
            AddPage(new ListBoxes(), string.Empty);
            AddPage(new ScrollViewers(), string.Empty);
            AddPage(new Tabs(), string.Empty);
            AddPage(new Charts(), string.Empty);
        }

        #endregion Private Methods
    }

}