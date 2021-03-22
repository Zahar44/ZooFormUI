using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZooFormUI.Database;

// V0.5

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
            _ = Connection();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainMenu());
        }
        static async Task Connection()
        {
            await Task.Run(() =>
            {
                ZooDbContext db = new ZooDbContext();
                db.SaveChanges();
            });
            ZooDbContext.Connected = true;
        }
    }
}
