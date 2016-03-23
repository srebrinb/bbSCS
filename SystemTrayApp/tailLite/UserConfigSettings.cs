using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Html5WebSCSTrayApp
{
    class UserConfigSettings
    {
        public static bool isTailFormTop()
        {
            Properties.Settings setting = new Properties.Settings();
            return setting.TailFormTop;
        }
        public static bool ToggleTailFormTop()
        {
            Properties.Settings.Default.TailFormTop = !Properties.Settings.Default.TailFormTop;
            Properties.Settings.Default.Save();
            return isTailFormTop();
        }
    }
}
