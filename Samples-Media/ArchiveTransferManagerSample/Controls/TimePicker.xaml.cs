using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace ArchiveTransferManagerSample.Controls
{
    /// <summary>
    ///     Interaction logic for TimePicker.xaml
    /// </summary>
    public partial class TimePicker : UserControl, INotifyPropertyChanged
    {
        private const int MaxMinuteSecond = 59;
        private const int MaxHour = 23;
        private const int MinTime = 0;
        private const int MinDate = 1;

        // Using a DependencyProperty as the backing store for TimeValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimeValueProperty =
            DependencyProperty.Register("TimeValue", typeof(DateTime), typeof(TimePicker),
                new PropertyMetadata(DateTime.Now, PropertyChangedCallback));

        private int m_day;

        private int m_hour;
        private int m_minute;

        private int m_month;

        private int m_second;

        private DateTime m_timeAndDate;

        private int m_year;

        public TimePicker()
        {
            DataContext = this;
            InitializeComponent();

            m_timeAndDate = DateTime.Now;
            Year = m_timeAndDate.Year;
            Month = m_timeAndDate.Month;
            Day = m_timeAndDate.Day;
            Hour = m_timeAndDate.Hour;
            Minute = m_timeAndDate.Minute;
            Second = m_timeAndDate.Second;
        }

        public DateTime TimeValue
        {
            get => (DateTime) GetValue(TimeValueProperty);
            set
            {
                if (value != TimeValue)
                    SetValue(TimeValueProperty, value);
            }
        }


        public DateTime TimeAndDate
        {
            get => m_timeAndDate;
            set
            {
                if (TimeAndDate != value)
                {
                    //Change the Date but not the time. This field is modified by the DatePicker Button
                    m_timeAndDate = new DateTime(value.Year, value.Month, value.Day, Hour, Minute, Second);
                    OnPropertyChanged(nameof(TimeAndDate));

                    Year = m_timeAndDate.Year;
                    Month = m_timeAndDate.Month;
                    Day = m_timeAndDate.Day;
                    TimeValue = m_timeAndDate;
                }
            }
        }

        public int Year
        {
            get => m_year;
            set
            {
                if (m_year != value)
                {
                    //this update the TextBox
                    m_year = value.Clamp(1, DateTime.Now.Year);
                    OnPropertyChanged(nameof(Year));
                    //We manually set the time, because the datePicker Time is always zero
                    TimeAndDate = new DateTime(Year, TimeAndDate.Month, TimeAndDate.Day, TimeAndDate.Hour,
                        TimeAndDate.Minute, TimeAndDate.Minute);
                }
            }
        }

        public int Month
        {
            get => m_month;
            set
            {
                if (m_month != value)
                {
                    //this update the TextBox
                    m_month = value.Clamp(MinDate, 12);
                    OnPropertyChanged(nameof(Month));
                    //We manually set the time, because the datePicker Time is always zero
                    TimeAndDate = new DateTime(TimeAndDate.Year, Month, TimeAndDate.Day, TimeAndDate.Hour,
                        TimeAndDate.Minute, TimeAndDate.Minute);
                }
            }
        }

        public int Day
        {
            get => m_day;
            set
            {
                if (m_day != value)
                {
                    //this update the TextBox
                    m_day = value.Clamp(MinDate, DateTime.DaysInMonth(Year, Month));
                    OnPropertyChanged(nameof(Day));
                    //We manually set the time, because the datePicker Time is always zero
                    TimeAndDate = new DateTime(TimeAndDate.Year, TimeAndDate.Month, Day, TimeAndDate.Hour,
                        TimeAndDate.Minute, TimeAndDate.Minute);
                }
            }
        }

        public int Hour
        {
            get => m_hour;
            set
            {
                if (m_hour != value)
                {
                    //this update the TextBox
                    m_hour = value.Clamp(MinTime, MaxHour);
                    OnPropertyChanged(nameof(Hour));
                    //We manually set the time, because the datePicker Time is always zero
                    TimeAndDate = new DateTime(TimeAndDate.Year, TimeAndDate.Month, TimeAndDate.Day, Hour,
                        TimeAndDate.Minute, TimeAndDate.Minute);
                }
            }
        }

        public int Minute
        {
            get => m_minute;
            set
            {
                if (m_minute != value)
                {
                    //this update the TextBox
                    m_minute = value.Clamp(MinTime, MaxMinuteSecond);
                    OnPropertyChanged(nameof(Minute));
                    //We manually set the time, because the datePicker Time is always zero
                    TimeAndDate = new DateTime(TimeAndDate.Year, TimeAndDate.Month, TimeAndDate.Day, TimeAndDate.Hour,
                        Minute, TimeAndDate.Minute);
                }
            }
        }

        public int Second
        {
            get => m_second;
            set
            {
                if (m_second != value)
                {
                    //this update the TextBox
                    m_second = value.Clamp(MinTime, MaxMinuteSecond);
                    OnPropertyChanged(nameof(Second));
                    //We manually set the time, because the datePicker Time is always zero
                    TimeAndDate = new DateTime(TimeAndDate.Year, TimeAndDate.Month, TimeAndDate.Day, TimeAndDate.Hour,
                        TimeAndDate.Minute, Minute);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var timeClass = d as TimePicker;
            timeClass?.OnChange((DateTime) e.NewValue);
        }

        public void OnChange(DateTime value)
        {
            TimeAndDate = value;
            //We manually set the time
            Hour = value.Hour;
            Minute = value.Minute;
            Second = value.Second;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class extension
    {
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            if (val.CompareTo(max) > 0) return max;
            return val;
        }
    }
}