using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace VisualMapObject
{
    #region Classes

    /// <summary>
    /// This class is a quick implementations of an observable collection that supports range adding and removing.
    /// </summary>
    /// <typeparam name="T">The type of the elements thats is holded in the collection.</typeparam>
    public class RangeObservableCollection<T> : ObservableCollection<T>
    {
        #region Constructors

        public RangeObservableCollection() { }

        public RangeObservableCollection(IEnumerable<T> items) : base(items) { }

        public RangeObservableCollection(List<T> items) : base(items) { }

        public RangeObservableCollection(ObservableCollection<T> items) : base(items) { }

        #endregion

        #region Public Methods

        public void AddRange(params T[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            CheckReentrancy();

            foreach (var item in items)
            {
                Items.Add(item);
            }

            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Items[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Replace,//Sadly the add event can't be used
                items,
                Enumerable.Empty<T>()
            ));
        }

        public bool RemoveRange(params T[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            CheckReentrancy();

            foreach (var item in items)
            {
                Items.Remove(item);
            }

            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Items[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Replace,//Sadly the remove event can't be used
                Enumerable.Empty<T>(),
                items
            ));
            return true;
        }

        #endregion
    }

    #endregion
}

