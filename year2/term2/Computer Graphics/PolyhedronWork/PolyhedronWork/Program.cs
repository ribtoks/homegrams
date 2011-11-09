using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;

namespace PolyhedronWork
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
#if RELEASE
            catch (FormatException e)
            {
                MessageBox.Show("Values of point coordinates must be <int> or <double>!");
            }
            catch (DataException e2)
            {
                MessageBox.Show("DataError!");
            }

            catch
            {
                MessageBox.Show("Global Error!");
            }
#endif
            finally
            {

            }
        }
    }
}
