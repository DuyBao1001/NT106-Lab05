using Bai5;
using System;
using System.Net;
using System.Windows.Forms;

namespace Bai5
{
    internal static class Program
    {

        [STAThread]
        static void Main()
        {

            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}