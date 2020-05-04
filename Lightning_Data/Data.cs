using Console = Colorful.Console;
using System.Drawing;

namespace Lightning_Data
{
    public class Data
    {
        public static string PWD { get; set; }
        public static string OLDPWD { get; set; }

        public static int SUCCESS_EXIT = 0;
        public static int FAILED_EXIT = -1;
        public static int UNKNOWN_EXIT = 1;

        public static Color ERROR_COLOR = Color.FromArgb(0xFF0000);
        public static Color PATH_COLOR = Color.FromArgb(0x00e500);
        public static Color DOLLAR_COLOR = Color.FromArgb(0xe5e500);
        public static Color INPUT_COLOR = Color.FromArgb(0x00cccc);
    }
}
