using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TelefonRehberi
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AccessDB accdb = new AccessDB("db.accdb");
            accdb.AddNewTable("telefonlar", "Telefonlar");
            accdb.AddNewTable("baglantilar", "Baglantilar");

            Application.Run(new AnaForm(accdb));
        }
    }
}
