using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class TournamentDashboardForm : Form
    {
        List<TournamentModel> tournaments = GlobalConfig.Connection.GetTournament_All(); 
        public TournamentDashboardForm()
        {
            InitializeComponent();
            WireUpLists();
        }

        private void WireUpLists()
        {
            loadExistingTournamentDropDown.DataSource = tournaments;
            loadExistingTournamentDropDown.DisplayMember = "TournamentName";
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            CreateTournamentForm frm = new CreateTournamentForm(); //creates an instance of the CreateTournamentForm 
            frm.Show(); // shows that new instance
        }

        private void loadTournamentButton_Click(object sender, EventArgs e)
        {
            //Takes whatever is selected in loadExistingTournamentDropdown on the dashboard and casts it to 
            // a TournamentModel. Stores it in variable tm and then gets passed into viewerform.
            TournamentModel tm = (TournamentModel)loadExistingTournamentDropDown.SelectedItem; 
            TournamentViewerForm frm = new TournamentViewerForm(tm);
            frm.Show();
        }
    }
}
