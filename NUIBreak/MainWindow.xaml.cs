using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Win32;
using System.Windows.Threading;

namespace NUIBreak
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        RegistryKey StartupRegKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        string RegKeyName = "NUIBreak";

        DispatcherTimer IntervalTimer = new DispatcherTimer();
        double IntervalCountdown;
        bool tooltipVisible = false;

        StretchDetector stretchDetector;

        public MainWindow()
        {
            InitializeComponent();

            stretchDetector = new StretchDetector();

            //Catch Interval changing
            Properties.Settings.Default.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Default_PropertyChanged);

            //Start timer
            IntervalTimer.Tick += new EventHandler(IntervalTimer_Tick);
            IntervalTimer.Interval = new TimeSpan(0, 0, 1);
            StartTimer();
        }

        #region Interval/Timer

        void StartTimer()
        {
            IntervalCountdown = Properties.Settings.Default.Interval * 60;
            //IntervalCountdown = 5; //Useful for debugging
            IntervalTimer.Start();
        }

        void IntervalTimer_Tick(object sender, EventArgs e)
        {
            if (IntervalCountdown < 0)
            {
                ShowOverlay();
            }

            if (tooltipVisible)
            {
                UpdateTooltipTimer();
            }
            IntervalCountdown--;
        }

        void ShowOverlay()
        {
            IntervalTimer.Stop();
            if (Properties.Settings.Default.Enabled == true)
            {
                OverlayWindow OverlayWindow = new OverlayWindow(stretchDetector);
                OverlayWindow.Closed += new EventHandler(OverlayWindow_Closed);
                OverlayWindow.Show();
            }
        }

        void OverlayWindow_Closed(object sender, EventArgs e)
        {
            GC.Collect();
            StartTimer();
        }

        void Default_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //Interval was changed, restart the timer
            if (e.PropertyName == "Interval")
            {
                StartTimer();
            }
        }

        #endregion

        #region Tray Icon Event Handlers

        private void EnabledItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.Enabled)
            {
                EnabledItem.IsChecked = true;
            }
        }

        private void EnabledItem_Click(object sender, RoutedEventArgs e)
        {
            if (EnabledItem.IsChecked)
            {
                Properties.Settings.Default.Enabled = true;
                StartTimer();
            }
            else
            {
                Properties.Settings.Default.Enabled = false;
                IntervalTimer.Stop();
            }
        }

        private void RunOnStartupItem_Click(object sender, RoutedEventArgs e)
        {
            if (RunOnStartupItem.IsChecked)
            {
                StartupRegKey.SetValue(RegKeyName, System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
            else
            {
                StartupRegKey.DeleteValue(RegKeyName, false);
            }
        }

        private void IntervalItem_Click(object sender, RoutedEventArgs e)
        {
            IntervalTimer.Stop();
            IntervalWindow IntervalWindow = new IntervalWindow();
            IntervalWindow.Closed += new EventHandler(IntervalWindow_Closed);
            IntervalWindow.Show();
        }

        void IntervalWindow_Closed(object sender, EventArgs e)
        {
            StartTimer();
        }

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        void TrayIcon_PreviewTrayToolTipOpen(object sender, RoutedEventArgs e)
        {
            tooltipVisible = true;
            UpdateTooltipTimer();
        }

        void TrayIcon_PreviewTrayToolTipClose(object sender, RoutedEventArgs e)
        {
            tooltipVisible = false;
        }

        private void UpdateTooltipTimer()
        {
            CountdownTooltip.Text = TimeSpan.FromSeconds(IntervalCountdown).ToString();
        }
        #endregion 
    }
}
