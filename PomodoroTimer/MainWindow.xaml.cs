using System.IO;
using System.Media;
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
using System.Windows.Threading;

namespace PomodoroTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string SETTING_FILE = "setting.json";

        private DispatcherTimer WorkingTimer = new DispatcherTimer();

        private DispatcherTimer BreakTimer = new DispatcherTimer();

        private DispatcherTimer NotifyTimer = new();

        private Setting Setting;

        /// <summary>
        /// 現在作業時間(秒)
        /// </summary>
        private int WorkTime = 0;

        private SoundPlayer SoundPlayer = new SoundPlayer("ずんだもん.wav");

        public MainWindow()
        {
            InitializeComponent();

            Setting = LoadSetting();
        }

        private static Setting LoadSetting()
        {
            if (File.Exists(SETTING_FILE))
            {
                return Util.ReadJsonFile(SETTING_FILE);
            }
            else
            {
                return new Setting();
            }

        }

        /// <summary>
        /// 作業開始
        /// </summary>
        private void StartWorking() 
        {
            BreakTimer.Stop();
            WorkingTimer.Start();
        }

        /// <summary>
        /// 休憩開始
        /// </summary>
        private void StartBreak()
        {
            WorkingTimer.Stop();
            BreakTimer.Start();
        }

        /// <summary>
        /// 通知開始
        /// </summary>
        private void StartNotify()
        {
            WorkTime = 0;
            TimeLabel.Content = "CHANGE!";
            TimeLabel.Background = Brushes.Red;
            WorkButton.IsEnabled = false;
            BreakButton.IsEnabled = false;
            SoundPlayer.Play();
            NotifyTimer.Start();
        }

        /// <summary>
        /// 時間表示
        /// </summary>
        /// <param name="sec"></param>
        private void ShowTime(int sec)
        {
            int minutes = sec / 60;
            int seconds = sec % 60;
            string min = minutes == 0 ? "" : minutes.ToString() + " min ";
            TimeLabel.Content = $"{min}{seconds} sec";
        }

        /// <summary>
        /// 作業中状態を初期化
        /// </summary>
        private void InitWorkState() 
        {
            WorkingTimer.Stop();
            WorkButton.IsEnabled = true;
            WorkButton.Background = Brushes.White;
        }

        /// <summary>
        /// 休憩中状態を初期化
        /// </summary>
        private void InitBreakState()
        {
            BreakTimer.Stop();
            BreakButton.IsEnabled = true;
            BreakButton.Background = Brushes.White;
        }   

        /// <summary>
        /// 通知状態を初期化
        /// </summary>
        private void InitNotifyState()
        {
            NotifyTimer.Stop();
            SoundPlayer.Stop();
            TimeLabel.Background = Brushes.White;
        }

        /// <summary>
        /// リセット
        /// </summary>
        private void Reset() 
        {
            WorkTime = 0;
            ShowTime(Setting.WorkTime);
            InitWorkState();
            InitBreakState();
            InitNotifyState();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WorkingTimer.Interval = new TimeSpan(0, 0, 1);
            WorkingTimer.Tick += WorkingTimer_Tick;

            BreakTimer.Interval = new TimeSpan(0, 0, 1);
            BreakTimer.Tick += BreakTimer_Tick;

            NotifyTimer.Interval = new TimeSpan(0, 0, Setting.VoiceDuration);
            NotifyTimer.Tick += NotifyTimer_Tick;
        }

        private void WorkButton_Click(object sender, RoutedEventArgs e)
        {
            if (WorkingTimer.IsEnabled)
            {
                WorkingTimer.Stop();
                WorkButton.Background = Brushes.White;
                BreakButton.IsEnabled = true;
            }
            else { 
                WorkButton.Background = Brushes.Orange;
                StartWorking();
                BreakButton.IsEnabled = false;
            }
        }

        private void BreakButton_Click(object sender, RoutedEventArgs e)
        {
            if (BreakTimer.IsEnabled)
            {
                BreakTimer.Stop();
                BreakButton.Background = Brushes.White;
                WorkButton.IsEnabled = true;
            }
            else
            {
                BreakButton.Background = Brushes.Orange;
                StartBreak();
                WorkButton.IsEnabled = false;
            }
        }


        private void WorkingTimer_Tick(object? sender, EventArgs e)
        {
            WorkTime++;
            ShowTime(Setting.WorkTime * 60 - WorkTime);
            if (WorkTime >= Setting.WorkTime * 60)
            {
                InitWorkState();
                StartNotify();
            }
        }


        private void BreakTimer_Tick(object? sender, EventArgs e)
        {
            WorkTime++;
            ShowTime(Setting.BreakTime * 60 - WorkTime);
            if (WorkTime >= Setting.BreakTime * 60)
            {
                InitBreakState();
                StartNotify();
            }
        }


        private void NotifyTimer_Tick(object? sender, EventArgs e)
        {
            SoundPlayer.Play();
            Activate();
        }

        private void TimeLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (NotifyTimer.IsEnabled)
            {
                Reset();
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void OpenSettingWindowMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow window = new(Setting);
            if (window.ShowDialog() == true)
            {
                Setting = window.Setting;
                Util.WriteJsonFile(SETTING_FILE, Setting);
                NotifyTimer.Interval = new TimeSpan(0, 0, Setting.VoiceDuration);
            }
        }
    }
}