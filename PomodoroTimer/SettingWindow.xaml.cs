using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PomodoroTimer
{
    /// <summary>
    /// SettingWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingWindow : Window
    {
        private static readonly Regex NumberOnlyRegex = new ("[^0-9.-]+");

        public Setting Setting { get; set; } = new Setting();

        public SettingWindow()
        {
            InitializeComponent();
            this.DataContext = Setting;
        }

        public SettingWindow(Setting setting):this()
        {
            Setting.VoiceDuration = setting.VoiceDuration;
            Setting.WorkTime = setting.WorkTime;
            Setting.BreakTime = setting.BreakTime;
        }


        private void VoiceDurationTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = NumberOnlyRegex.IsMatch(e.Text);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
