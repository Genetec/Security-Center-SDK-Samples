// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace ModuleSample.Pages.Controls
{
    public class CustomItem : DependencyObject
    {

        #region Public Fields

        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register
            ("Name", typeof(string), typeof(CustomItem),
            new PropertyMetadata(string.Empty));

        #endregion Public Fields

        #region Public Properties

        public string Name
        {
            get => (string)GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        #endregion Public Properties

    }

    /// <summary>
    /// Interaction logic for ListBoxes.xaml
    /// </summary>
    public partial class ListBoxes
    {

        #region Public Fields

        public static readonly DependencyProperty RandomItemsProperty =
            DependencyProperty.Register
            ("RandomItems", typeof(ObservableCollection<CustomItem>), typeof(ListBoxes),
            new PropertyMetadata(null));

        #endregion Public Fields

        #region Private Fields

        private char m_lastchar = 'a';

        private string m_word = string.Empty;

        #endregion Private Fields

        #region Public Properties

        public ObservableCollection<CustomItem> RandomItems
        {
            get => (ObservableCollection<CustomItem>)GetValue(RandomItemsProperty);
            set => SetValue(RandomItemsProperty, value);
        }

        #endregion Public Properties

        #region Public Constructors

        public ListBoxes()
        {
            RandomItems = new ObservableCollection<CustomItem>();
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void OnHorizontalAddRandom(object sender, RoutedEventArgs e)
        {
            m_word += m_lastchar++;
            for (var i = 0; i < 100; i++)
            {
                RandomItems.Add(i % 10 == 0
                    ? new CustomItem {Name = "Potatoes" + i}
                    : new CustomItem {Name = "Beer" + i});
            }
        }

        private void OnRemoveRandom(object sender, RoutedEventArgs e)
        {
            if (RandomItems.Count > 0)
            {
                RandomItems.RemoveAt(0);
            }
        }

        #endregion Private Methods

    }
}