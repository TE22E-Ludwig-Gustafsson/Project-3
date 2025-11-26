using System;
using System.Windows.Forms;
using WinFormsApp.Forms;

namespace WinFormsApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Starta MainForm
            Application.Run(new MainForm());
        }
    }
}
