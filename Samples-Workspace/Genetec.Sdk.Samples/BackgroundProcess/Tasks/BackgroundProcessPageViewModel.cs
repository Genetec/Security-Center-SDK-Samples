﻿// Copyright (C) 2022 by Genetec, Inc. All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.

namespace BackgroundProcess
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Genetec.Sdk;
    using Genetec.Sdk.Workspace.Services;
    using Prism.Commands;
    using Prism.Mvvm;

    public class BackgroundProcessPageViewModel : BindableBase
    {
        private IBackgroundProcessNotification m_selectedProcessNotification;

        public BackgroundProcessPageViewModel(IBackgroundProcessNotificationService service)
        {
            service.CollectionChanged += (o, args) =>
            {
                foreach (IBackgroundProcessNotification removed in Notifications.Where(n => args.RemovedItems.Contains(n.Id)).ToList())
                {
                    Notifications.Remove(removed);
                }

                foreach (Guid added in args.AddedItems)
                {
                    Notifications.Add(service.GetNotification(added));
                }
            };

            service.ProcessStatusChanged += (o, args) => Update(args.Id);

            Notify = new DelegateCommand<string>(message => service.Notify(message));

            AddProcess = new DelegateCommand<string>(name => service.AddProcess(name, null));

            UpdateProgress = new DelegateCommand<decimal?>(process =>
            {
                service.UpdateProgress(SelectedNotification.Id, decimal.ToDouble(process.Value));
                Update(SelectedNotification.Id);
            }, progress => SelectedNotification != null).ObservesProperty(() => SelectedNotification);

            UpdateMessage = new DelegateCommand<string>(message =>
            {
                service.UpdateProgress(SelectedNotification.Id, message);
                Update(SelectedNotification.Id);
            }, message => SelectedNotification != null).ObservesProperty(() => SelectedNotification);

            EndProcess = new DelegateCommand<BackgroundProcessResult?>(result => service.EndProcess(SelectedNotification.Id, result.Value), result => SelectedNotification != null).ObservesProperty(() => SelectedNotification);

            ClearProcess = new DelegateCommand(() => service.ClearProcess(SelectedNotification.Id), () => SelectedNotification != null).ObservesProperty(() => SelectedNotification);

            ClearCompletedProcesses = new DelegateCommand(() => service.ClearCompletedProcesses());

            void Update(Guid id)
            {
                IBackgroundProcessNotification actual = Notifications.FirstOrDefault(n => n.Id == id);
                if (actual != null)
                {
                    Notifications.Remove(actual);
                }

                Notifications.Add(service.GetNotification(id));
            }
        }

        public DelegateCommand<string> AddProcess { get; }

        public DelegateCommand ClearCompletedProcesses { get; }

        public DelegateCommand ClearProcess { get; }

        public DelegateCommand<BackgroundProcessResult?> EndProcess { get; }

        public DelegateCommand<string> Notify { get; }

        public DelegateCommand<string> UpdateMessage { get; }

        public DelegateCommand<decimal?> UpdateProgress { get; }

        public ObservableCollection<IBackgroundProcessNotification> Notifications { get; } = new ObservableCollection<IBackgroundProcessNotification>();

        public IBackgroundProcessNotification SelectedNotification
        {
            get => m_selectedProcessNotification;
            set => SetProperty(ref m_selectedProcessNotification, value);
        }
    }
}
