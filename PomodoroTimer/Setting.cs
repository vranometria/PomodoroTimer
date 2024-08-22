using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroTimer
{
    public class Setting
    {
        /// <summary>
        /// 作業時間(分)
        /// </summary>
        public int WorkTime { get; set; } = 25;

        /// <summary>
        /// 休憩時間(分)
        /// </summary>
        public int BreakTime { get; set; } = 5;

        /// <summary>
        /// 通知音の再生時間(秒)
        /// </summary>
        public int VoiceDuration { get; set; } = 3;

        public string VoicePath { get; set; } = "ずんだもん.wav";
    }
}
