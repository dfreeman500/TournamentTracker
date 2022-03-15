﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrackerUI
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
            TrackerLibrary.GlobalConfig.InitializeConnections(TrackerLibrary.DatabaseType.TextFile);
            Application.Run(new CreatePrizeForm()); // sets the first form to open up to

            //Application.Run(new TournamentDashboardForm()); // sets the first form to open up to
        }
    }
}
