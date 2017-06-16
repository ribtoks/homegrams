using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EDiary
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(((Exception)e.ExceptionObject).Message, "Critical error occured!!!",
                MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (MessageBox.Show("Try to continue work?", "Error consequence",
                MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                Application.Exit();
        }
    }
}
