using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.Coverages;
using Genetec.Sdk.EventsArgs;
using Genetec.Sdk.Queries;
using System;
using System.Data;
using System.Windows.Forms;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace Schedules
{
    #region Classes

    public partial class Schedules : Form
    {
        #region Constants

        private readonly Engine m_sdkEngine = new Engine();

        #endregion

        #region Constructors

        public Schedules()
        {
            InitializeComponent();
            m_sdkEngine.LoginManager.LoggedOn += OnEngineLoggedOn;
            m_sdkEngine.LoginManager.LogonFailed += OnEngineLogonFailed;
            m_sdkEngine.LoginManager.RequestDirectoryCertificateValidation += OnEngineDirectoryCertificateValidation;
        }

        #endregion

        #region Event Handlers

        private void OnButton21OfJulyClick(object sender, EventArgs e)
        {
            // Creates a schedule that covers the entire day on the 21st of july every year.
            m_sdkEngine.TransactionManager.CreateTransaction();
            var ordinalDailyRange = (Schedule)m_sdkEngine.CreateEntity("Twenty-first Of July", EntityType.Schedule);
            var coverage = (OrdinalCoverage)CoverageFactory.Instance.Create(CoverageType.Ordinal);
            var range = new RangeCoverage();
            range.Add(CoverageOffset.CurrentDay, new DailyCoverageItem(new SdkTime(0, 0, 0), new SdkTime(24, 0, 0)));
            var secondWeekendDay = new OrdinalCoverageByDayOfMonthItem(21, Months.July, range);
            coverage.Add(secondWeekendDay);
            try
            {
                ordinalDailyRange.Coverage = coverage;
                m_sdkEngine.TransactionManager.CommitTransaction(false);
            }
            catch (Exception exception)
            {
                m_sdkEngine.TransactionManager.RollbackTransaction();
                m_status.Text = exception.Source;
            }
        }

        private void OnButtonAttachAccessRuleClick(object sender, EventArgs e)
        {
            //Attaches the schedule Daily Range to all access rules starting with the text specified
            var listSchedules = GetEntities("Daily Range", EntityType.Schedule);
            if (listSchedules != null)
            {
                if (listSchedules.Data.Rows.Count > 0)
                {
                    var schedule = (Schedule)m_sdkEngine.GetEntity((Guid)listSchedules.Data.Rows[0][0]);
                    if (schedule != null)
                    {
                        var listAccessRules = GetEntities(m_accessRuleName.Text, EntityType.AccessRule);
                        if (listAccessRules != null)
                        {
                            m_sdkEngine.TransactionManager.CreateTransaction();
                            try
                            {
                                foreach (DataRow row in listAccessRules.Data.Rows)
                                {
                                    var accessRule = (AccessRule)m_sdkEngine.GetEntity((Guid)row[0]);
                                    if (accessRule != null)
                                    {
                                        accessRule.Schedule = schedule;
                                    }
                                }
                                m_sdkEngine.TransactionManager.CommitTransaction(false);
                            }
                            catch (Exception exception)
                            {
                                m_sdkEngine.TransactionManager.RollbackTransaction();
                                m_status.Text = exception.Source;
                            }
                        }
                    }
                }
            }
        }

        private void OnButtonAttachCoverageClick(object sender, EventArgs e)
        {
            //Attaches the schedule Daily Range to all alarms starting with the text specified
            var listSchedules = GetEntities("Daily Range", EntityType.Schedule);
            if (listSchedules != null)
            {
                if (listSchedules.Data.Rows.Count > 0)
                {
                    var schedule = (Schedule)m_sdkEngine.GetEntity((Guid)listSchedules.Data.Rows[0][0]);
                    if (schedule != null)
                    {
                        var listAlarms = GetEntities(m_alarmName.Text, EntityType.Alarm);
                        if (listAlarms != null)
                        {
                            m_sdkEngine.TransactionManager.CreateTransaction();
                            try
                            {
                                foreach (DataRow row in listAlarms.Data.Rows)
                                {
                                    var alarm = (Alarm)m_sdkEngine.GetEntity((Guid)row[0]);
                                    if (alarm != null)
                                    {
                                        alarm.Schedule = schedule;
                                    }
                                }
                                m_sdkEngine.TransactionManager.CommitTransaction(false);
                            }
                            catch (Exception exception)
                            {
                                m_sdkEngine.TransactionManager.RollbackTransaction();
                                m_status.Text = exception.Source;
                            }
                        }
                    }
                }
            }
        }

        private void OnButtonDailyRangeClick(object sender, EventArgs e)
        {
            // Creates a schedule that covers every day, from 2pm to 3pm.
            m_sdkEngine.TransactionManager.CreateTransaction();

            var dailyRange = (Schedule)m_sdkEngine.CreateEntity("Daily Range", EntityType.Schedule);
            var coverage = (DailyCoverage)CoverageFactory.Instance.Create(CoverageType.Daily);
            var dailyCoverage = new DailyCoverageItem(new SdkTime(14, 0, 0), new SdkTime(15, 0, 0));
            coverage.Add(dailyCoverage);

            try
            {
                dailyRange.Coverage = coverage;
                m_sdkEngine.TransactionManager.CommitTransaction(false);
            }
            catch (Exception exception)
            {
                m_status.Text = exception.Source;
                m_sdkEngine.TransactionManager.RollbackTransaction();
            }
        }

        private void OnButtonDailyTwilightClick(object sender, EventArgs e)
        {
            // Creates a schedule that covers every day during daylight.
            m_sdkEngine.TransactionManager.CreateTransaction();
            var dailyTwilight = (Schedule)m_sdkEngine.CreateEntity("Daily Twilight", EntityType.Schedule);
            var twilight = new TwilightCoverage(TimeSpan.Zero, TimeSpan.Zero, TimeCoverage.Daytime);
            try
            {
                dailyTwilight.Coverage = twilight;
                m_sdkEngine.TransactionManager.CommitTransaction(false);
            }
            catch (Exception exception)
            {
                m_sdkEngine.TransactionManager.RollbackTransaction();
                m_status.Text = exception.Source;
            }
        }

        private void OnButtonDeleteCoverageClick(object sender, EventArgs e)
        {
            //Deletes all schedules starting with the text specified
            var listSchedules = GetEntities(m_scheduleName.Text, EntityType.Schedule);
            if (listSchedules != null)
            {
                m_sdkEngine.TransactionManager.CreateTransaction();
                try
                {
                    foreach (DataRow row in listSchedules.Data.Rows)
                    {
                        m_sdkEngine.DeleteEntity((Guid)row[0]);
                    }
                    m_sdkEngine.TransactionManager.CommitTransaction(false);
                }
                catch (Exception exception)
                {
                    m_sdkEngine.TransactionManager.RollbackTransaction();
                    m_status.Text = exception.Source;
                }
            }
        }

        private void OnButtonLastDayOfMonthClick(object sender, EventArgs e)
        {
            // Creates a schedule that covers from noon to 3pm on the last day of every month.
            m_sdkEngine.TransactionManager.CreateTransaction();
            var ordinalDailyRange = (Schedule)m_sdkEngine.CreateEntity("Last Day Of Month", EntityType.Schedule);
            var coverage = (OrdinalCoverage)CoverageFactory.Instance.Create(CoverageType.Ordinal);
            var range = new RangeCoverage();
            range.Add(CoverageOffset.CurrentDay, new DailyCoverageItem(new SdkTime(12, 0, 0), new SdkTime(15, 0, 0)));
            var lastOfTheMonth = new OrdinalCoverageByDayOfWeekItem(DayOrdinal.Last, DaySelection.Day, Months.EveryMonth, range);
            coverage.Add(lastOfTheMonth);
            try
            {
                ordinalDailyRange.Coverage = coverage;
                m_sdkEngine.TransactionManager.CommitTransaction(false);
            }
            catch (Exception exception)
            {
                m_sdkEngine.TransactionManager.RollbackTransaction();
                m_status.Text = exception.Source;
            }
        }

        private void OnButtonLoginClick(object sender, EventArgs e)
        {
            m_sdkEngine.LoginManager.BeginLogOn(m_server.Text, m_usernameInput.Text, m_passwordInput.Text);
        }

        private void OnButtonModifyClick(object sender, EventArgs e)
        {
            // Modifies an existing ordinal schedule starting with the text entered 
            //and adds daytime coverage on the second weekend day of the month of july every year.
            var listSchedules = GetEntities(m_scheduleName.Text, EntityType.Schedule);
            if (listSchedules != null)
            {
                m_sdkEngine.TransactionManager.CreateTransaction();
                try
                {
                    foreach (DataRow row in listSchedules.Data.Rows)
                    {
                        var schedule = (Schedule)m_sdkEngine.GetEntity((Guid)row[0]);
                        if (schedule != null)
                        {
                            var coverage = (OrdinalCoverage)schedule.Coverage;
                            if (coverage != null)
                            {
                                var twilight = new TwilightCoverage(new TimeSpan(0, -50, 0), TimeSpan.Zero, TimeCoverage.Daytime);
                                var item = new OrdinalCoverageByDayOfWeekItem(DayOrdinal.Second, DaySelection.WeekendDay, Months.July, twilight);
                                coverage.Add(item);
                            }
                            schedule.Coverage = coverage;
                        }
                    }
                    m_sdkEngine.TransactionManager.CommitTransaction(false);
                }
                catch (Exception exception)
                {
                    m_sdkEngine.TransactionManager.RollbackTransaction();
                    m_status.Text = exception.Source;
                }
            }
        }

        private void OnButtonSecondNonWeekdayOfMonthClick(object sender, EventArgs e)
        {
            // Creates a schedule that covers the entire day on the second non weekday of the month.
            m_sdkEngine.TransactionManager.CreateTransaction();
            var ordinalDailyRange = (Schedule)m_sdkEngine.CreateEntity("Second Non Weekday Of Month", EntityType.Schedule);
            var coverage = (OrdinalCoverage)CoverageFactory.Instance.Create(CoverageType.Ordinal);
            var range = new RangeCoverage();
            range.Add(CoverageOffset.CurrentDay, new DailyCoverageItem(new SdkTime(0, 0, 0), new SdkTime(24, 0, 0)));
            var secondNonWeekdayOfMonth = new OrdinalCoverageByDayOfWeekItem(DayOrdinal.Second, DaySelection.WeekendDay, Months.EveryMonth, range);
            coverage.Add(secondNonWeekdayOfMonth);
            try
            {
                ordinalDailyRange.Coverage = coverage;
                m_sdkEngine.TransactionManager.CommitTransaction(false);
            }
            catch (Exception exception)
            {
                m_sdkEngine.TransactionManager.RollbackTransaction();
                m_status.Text = exception.Source;
            }
        }

        private void OnButtonSpecificRangeClick(object sender, EventArgs e)
        {
            // Creates a schedule that covers from 8am to midnight on December 31 2015.
            m_sdkEngine.TransactionManager.CreateTransaction();
            var specificRange = (Schedule)m_sdkEngine.CreateEntity("New Year's eve range", EntityType.Schedule);
            var specificCoverage = (SpecificCoverage)CoverageFactory.Instance.Create(CoverageType.Specific);
            var coverage = new RangeCoverage();
            coverage.Add(CoverageOffset.DayBefore, new DailyCoverageItem(new SdkTime(8, 0, 0), new SdkTime(24, 0, 0)));
            var specific = new SpecificCoverageItemUnit(new SdkDate(2016, 1, 1), coverage);
            specificCoverage.Add(specific);
            try
            {

                specificRange.Coverage = specificCoverage;
                m_sdkEngine.TransactionManager.CommitTransaction(false);
            }
            catch (Exception exception)
            {
                m_sdkEngine.TransactionManager.RollbackTransaction();
                m_status.Text = exception.Source;
            }
        }

        private void OnButtonSpecificTwlilightClick(object sender, EventArgs e)
        {
            // Creates a schedule that covers the nighttime of the first of January 2016.
            m_sdkEngine.TransactionManager.CreateTransaction();
            var specificTwilight = (Schedule)m_sdkEngine.CreateEntity("New Year's twilight", EntityType.Schedule);
            var specificCoverage = (SpecificCoverage)CoverageFactory.Instance.Create(CoverageType.Specific);

            var twilight = new TwilightCoverage(TimeSpan.Zero, new TimeSpan(0, 1, 0), TimeCoverage.Nighttime);
            var specificItem = new SpecificCoverageItemUnit(new SdkDate(2016, 1, 1), twilight);
            specificCoverage.Add(specificItem);
            try
            {

                specificTwilight.Coverage = specificCoverage;
                m_sdkEngine.TransactionManager.CommitTransaction(false);
            }
            catch (Exception exception)
            {
                m_sdkEngine.TransactionManager.RollbackTransaction();
                m_status.Text = exception.Source;
            }

        }

        private void OnButtonWeeklyRangeClick(object sender, EventArgs e)
        {
            // Creates a schedule that covers from 10am to 4pm on Sundays and from 8am to 1pm on Wednesdays.
            m_sdkEngine.TransactionManager.CreateTransaction();
            var weeklyRange = (Schedule)m_sdkEngine.CreateEntity("Weekly Range", EntityType.Schedule);
            var weeklyCoverage = (WeeklyCoverage)CoverageFactory.Instance.Create(CoverageType.Weekly);

            var rangeSunday = new WeeklyCoverageItem(DayOfWeek.Sunday, new SdkTime(10, 0, 0), new SdkTime(16, 0, 0));
            var rangeWednesday = new WeeklyCoverageItem(DayOfWeek.Wednesday, new SdkTime(8, 0, 0), new SdkTime(13, 0, 0));
            weeklyCoverage.Add(rangeSunday);
            weeklyCoverage.Add(rangeWednesday);
            try
            {
                weeklyRange.Coverage = weeklyCoverage;
                m_sdkEngine.TransactionManager.CommitTransaction(false);
            }
            catch (Exception exception)
            {
                m_sdkEngine.TransactionManager.RollbackTransaction();
                m_status.Text = exception.Source;
            }

        }

        private void OnEngineDirectoryCertificateValidation(object sender, DirectoryCertificateValidationEventArgs e)
        {
            DialogResult result = MessageBox.Show("The identity of the Directory server cannot be verified. \n" +
                                                  "The certificate is not from a trusted certifying authority. \n" +
                                                  "Do you trust this server?", "Secure Communication", MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                e.AcceptDirectory = true;
            }
        }

        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            m_status.Text = "Logged on";
            m_login.Enabled = false;
            m_dailyRange.Enabled = true;
            m_dailyTwilight.Enabled = true;
            m_specificTwilight.Enabled = true;
            m_weeklyRange.Enabled = true;
            m_specificRange.Enabled = true;
            m_lastDayOfMonth.Enabled = true;
            m_secondNonWeekdayOfMonth.Enabled = true;
            m_21stOfJuly.Enabled = true;
            m_modifyOrdinalSchedule.Enabled = true;
            m_scheduleName.Enabled = true;
            m_deleteSchedule.Enabled = true;
            m_attachAlarmSchedule.Enabled = true;
            m_alarmName.Enabled = true;
            m_attachAccessRuleSchedule.Enabled = true;
            m_accessRuleName.Enabled = true;
            m_server.Enabled = false;
            m_passwordInput.Enabled = false;
            m_usernameInput.Enabled = false;
        }

        private void OnEngineLogonFailed(object sender, LogonFailedEventArgs e)
        {
            m_status.Text = "Logon failed";
        }

        #endregion

        #region Private Methods

        private QueryCompletedEventArgs GetEntities(string name, EntityType eType)
        {
            var query = m_sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
            if (query != null)
            {
                query.Name = name;
                query.NameSearchMode = StringSearchMode.StartsWith;
                query.EntityTypeFilter.Add(eType);
                query.DownloadAllRelatedData = true;
                query.StrictResults = true;
                return query.Query();
            }
            return null;
        }

        #endregion
    }

    #endregion
}

