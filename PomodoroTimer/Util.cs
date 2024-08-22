using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;

namespace PomodoroTimer
{
    public static class Util
    {
        public static Setting ReadJsonFile(string path)
        {
            string s = File.ReadAllText(path);
            Setting? setting = JsonSerializer.Deserialize<Setting>(s);
            if (setting == null) { throw new Exception(); }
            return setting;
        }

        internal static void WriteJsonFile(string sETTING_FILE, Setting setting)
        {
            string s = JsonSerializer.Serialize(setting);
            File.WriteAllText(sETTING_FILE, s);
        }
    }
}
