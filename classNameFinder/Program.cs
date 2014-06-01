using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using System.IO;

namespace classNameFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            //ウィンドウを列挙する
            EnumWindows(new EnumWindowsDelegate(EnumWindowCallBack), IntPtr.Zero);

            Console.ReadLine();
        }

        public delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool EnumWindows(EnumWindowsDelegate lpEnumFunc,
            IntPtr lparam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd,
            StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetClassName(IntPtr hWnd,
            StringBuilder lpClassName, int nMaxCount);

        private static bool EnumWindowCallBack(IntPtr hWnd, IntPtr lparam)
        {
            Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
            StreamWriter writer =
              new StreamWriter("output.txt", true, sjisEnc);



            //ウィンドウのタイトルの長さを取得する
            int textLen = GetWindowTextLength(hWnd);

            //ウィンドウのタイトルを取得する
            StringBuilder tsb = new StringBuilder(textLen + 1);
            GetWindowText(hWnd, tsb, tsb.Capacity);

            //ウィンドウのクラス名を取得する
            StringBuilder csb = new StringBuilder(256);
            GetClassName(hWnd, csb, csb.Capacity);

            //結果を表示する
            Console.WriteLine("クラス名:" + csb.ToString());
            Console.WriteLine("タイトル:" + tsb.ToString());
            Console.WriteLine("--------------------------------------------");

            writer.WriteLine("クラス名:" + csb.ToString());
            writer.WriteLine("タイトル:" + tsb.ToString());
            writer.WriteLine("--------------------------------------------");

            writer.Close();

            //すべてのウィンドウを列挙する
            return true;
        }
    }
}
