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
using System.Windows.Shapes;

namespace NUIBreak
{
    /// <summary>
    /// Interaction logic for IntervalWindow.xaml
    /// </summary>
    public partial class IntervalWindow : Window
    {
        public IntervalWindow()
        {
            InitializeComponent();

            IntervalValue.Text = Properties.Settings.Default.Interval.ToString();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            int newInterval;
            if (!int.TryParse(IntervalValue.Text, out newInterval) || newInterval > 1000 || newInterval < 1 )
            {
                Error.Text = "Please enter a value between 1 and 1000.";
                Error.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                Properties.Settings.Default.Interval = newInterval;
                Properties.Settings.Default.Save();
                this.Close();
            }
        }
    }
}
