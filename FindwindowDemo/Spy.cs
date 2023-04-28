using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace FindwindowDemo
{
    internal class Spy
    {
        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hwnd);
        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern int FindWindowEx(IntPtr hwnd1, IntPtr hwnd2, string lpsz1, string lpsz2);
        [DllImport("user32.dll")]
        public static extern int PostMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern void SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, int lParam);

        //====

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, int uCmd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        const int BM_CLICK = 0xF5;
        const int MOUSEEVENTF_LEFTDOWN = 0x2;
        const int MOUSEEVENTF_LEFTUP = 0x4;
        const int WM_LBUTTONDOWN = 0x0201;
        const int WM_LBUTTONUP = 0x0202;
        const int MK_LBUTTON = 0x1;
        const int WM_MY = 0x10001;

        private const int GW_CHILD = 5;


        /// <summary>
        /// 模拟点击按钮 ，请配合Spy++ 使用。
        /// </summary>
        /// <param name="AppName">窗口标题名称</param>
        /// <param name="ClassName">窗口或按钮类名，如Button</param>
        /// <param name="BtnDeep">使用Spy++ 查找 BtnDeep 深度 如 1-1， 2-1， 3-1-3 等</param>
        /// <returns></returns>
        public static bool Send_Click(string AppName, string ClassName, String BtnDeep)
        {
            int App = 0;
            int Swin = 0;
            string AppHex = "";
            //----------------------------------------寻找主窗口
            if (ClassName != "" && AppName != "")
                App = FindWindow(ClassName, AppName);
            else if (ClassName == "")
                App = FindWindow(null, AppName);
            else if (AppName == "")
                App = FindWindow(ClassName, null);
            else
                return false;
            //----------------------------------------寻找子窗口
            if (App > 0)
            {
                SetForegroundWindow((IntPtr)App);
                Swin = FindWindowEx((IntPtr)App, IntPtr.Zero, null, null);
                Regex reg = new Regex(@"\d+");
                MatchCollection mch = reg.Matches(BtnDeep);
                Swin = App;
                if (mch.Count > 0)
                {
                    for (int deep = 0; deep < mch.Count; deep++)
                    {
                        int inx = Convert.ToInt32(mch[deep].ToString());
                        App = Swin;
                        Swin = 0;

                        for (int Finx = 0; Finx < inx; Finx++)
                        {
                            Swin = FindWindowEx((IntPtr)App, (IntPtr)Swin, null, null);

                            ////获取控件类名
                            //IntPtr child = (IntPtr)Swin;
                            //StringBuilder sb = new StringBuilder(256);
                            //GetClassName(child, sb, 256);
                            ////获取控件名称
                            //StringBuilder sbText = new StringBuilder(256);
                            //GetWindowText(child, sbText, 256);
                        }
                    }
                    App = Swin;
                    if (App > 0)
                    {
                        IntPtr ptr = (IntPtr)App;
                        SetForegroundWindow((IntPtr)App);
                        //SendMessage((IntPtr)App, BM_CLICK, (IntPtr)0, 0);
                        //SendMessage((IntPtr)App, BM_CLICK, (IntPtr)0, 0);
                        PostMessage((IntPtr)App, WM_LBUTTONDOWN, MK_LBUTTON, WM_MY);
                        Thread.Sleep(300);
                        PostMessage((IntPtr)App, WM_LBUTTONUP, MK_LBUTTON, WM_MY);
                        //-----------------------------------------------------------
                        AppHex = Convert.ToString(App, 16).ToUpper();
                        //MessageBox.Show(AppHex);
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
