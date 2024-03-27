using System;
using Genetec.Sdk;
using SdkHelpers.Common;

// ==========================================================================
// Copyright (C) 2017 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace CustomEntitySample
{
    /// <summary>
    /// This program demonstrate how to create custom entity types (to make available in the product)
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            SdkAssemblyLoader.Start();

            var mainApp = new MainApp();

            Console.Read();

            SdkAssemblyLoader.Stop();
        }
    }

    public class MainApp
    {
        public static Engine Engine { get; private set; }

        public MainApp()
        {
            Engine = new Engine();

            Console.WriteLine("Logging on...");
            Engine.LoginManager.LoggedOn += OnEngineLoggedOn;

            Engine.LoginManager.LogonStatusChanged += (sender, eventArgs) => Console.WriteLine("Logon status changed: "+ eventArgs.Status);
            Engine.LoginManager.LogonFailed += (sender, eventArgs) => Console.WriteLine("Logon failed: "+ eventArgs.SdkException);

            Engine.LoginManager.LogOn("localhost", "admin", string.Empty);
        }

        private static void OnEngineLoggedOn(object sender, LoggedOnEventArgs loggedOnEventArgs)
        {
            Console.WriteLine("Logged on... creating types");

            var creator = new EntityTypeCreator(Engine);

            creator.CreateTypes();

            Console.WriteLine("Done.");
        }
    }
}
