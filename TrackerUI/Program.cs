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
            TrackerLibrary.GlobalConfig.InitializeConnections(TrackerLibrary.DatabaseType.Sql);
            //Application.Run(new CreateTeamForm()); // sets the first form to open up to
            //Application.Run(new CreateTournamentForm());
            Application.Run(new TournamentDashboardForm()); // sets the first form to open up to
        }
    }
}


//TODO 23:32:06
// Add Delete Tournament Option to Tournament Dashboard
