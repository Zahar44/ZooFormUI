using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZooFormUI.Database;

namespace ZooFormUI
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //DateTime start = DateTime.Now;
            Thread conn = new Thread(new ThreadStart(Connection));
            conn.Start();
            //Connection();
            //TimeSpan end = DateTime.Now - start;
            //MessageBox.Show(end.TotalSeconds.ToString());
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainMenu());
        }
        static void Connection()
        {
            using (ZooDbContext db = new ZooDbContext())
            {
            }
        }
    }
}
