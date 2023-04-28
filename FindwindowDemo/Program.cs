using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FindwindowDemo
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Spy.Send_Click("萌尘框架", "", "2-1"); // 重启按钮

            //遍历所有
            GetButtonHandlesWithNames(null, "萌尘框架");
            IntPtr hWndMainWindow = FindWindow(null, "萌尘框架");

            Console.ReadLine();
        }

        public static List<Tuple<IntPtr, string>> GetButtonHandlesWithNames(string windowClassName, string windowTitle)
        {
            List<Tuple<IntPtr, string>> buttons = new List<Tuple<IntPtr, string>>();
            IntPtr hwnd = FindWindow(windowClassName, windowTitle);
            IntPtr child = GetWindow(hwnd, GW_CHILD);
            while (child != IntPtr.Zero)
            {
                StringBuilder sb = new StringBuilder(256);
                GetClassName(child, sb, 256);
                if (sb.ToString() == "Button")
                {
                    StringBuilder sbText = new StringBuilder(256);
                    GetWindowText(child, sbText, 256);
                    buttons.Add(new Tuple<IntPtr, string>(child, sbText.ToString()));
                    List<Tuple<IntPtr, string>> Childbuttons = GetChildButtonHandlesWithNames(child, null);
                }
                if (sb.ToString() == "_EL_RgnButton")
                {
                    StringBuilder sbText = new StringBuilder(256);
                    GetWindowText(child, sbText, 256);
                    buttons.Add(new Tuple<IntPtr, string>(child, sbText.ToString()));
                    List<Tuple<IntPtr, string>> Childbuttons = GetChildButtonHandlesWithNames(child, null);
                }
                if (sb.ToString() == "_EL_PicBox")
                {
                    StringBuilder sbText = new StringBuilder(256);
                    GetWindowText(child, sbText, 256);
                    buttons.Add(new Tuple<IntPtr, string>(child, sbText.ToString()));
                    List<Tuple<IntPtr, string>> Childbuttons = GetChildButtonHandlesWithNames(child, null);
                }
                child = GetWindow(child, GW_HWNDNEXT);
            }
            return buttons;
        }

        public static List<Tuple<IntPtr, string>> GetChildButtonHandlesWithNames(IntPtr hwnd, string windowTitle)
        {
            List<Tuple<IntPtr, string>> buttons = new List<Tuple<IntPtr, string>>();
            IntPtr child = GetWindow(hwnd, GW_CHILD);
            while (child != IntPtr.Zero)
            {
                StringBuilder sb = new StringBuilder(256);
                GetClassName(child, sb, 256);
                if (sb.ToString() == "Button")
                {
                    StringBuilder sbText = new StringBuilder(256);
                    GetWindowText(child, sbText, 256);
                    buttons.Add(new Tuple<IntPtr, string>(child, sbText.ToString()));
                }
                if (sb.ToString() == "_EL_RgnButton")
                {
                    StringBuilder sbText = new StringBuilder(256);
                    GetWindowText(child, sbText, 256);
                    buttons.Add(new Tuple<IntPtr, string>(child, sbText.ToString()));
                }
                if (sb.ToString() == "_EL_PicBox")
                {
                    StringBuilder sbText = new StringBuilder(256);
                    GetWindowText(child, sbText, 256);
                    buttons.Add(new Tuple<IntPtr, string>(child, sbText.ToString()));
                }
                child = GetWindow(child, GW_HWNDNEXT);
            }
            return buttons;
        }

        private const int GW_CHILD = 5;
        private const int GW_HWNDNEXT = 2;

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, int uCmd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);

        [DllImport("User32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);

    }
}
