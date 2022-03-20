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
    public partial class CreateTournamentForm : Form
    {
        List<TeamModel> availableTeams = GlobalConfig.Connection.GetTeam_All(); //gets all of the teams and puts them in this list of teamModel
        List<TeamModel> selectedTeams = new List<TeamModel>();
        List<PrizeModel> selectedPrizes = new List<PrizeModel>();
        
        public CreateTournamentForm()
        {
            InitializeComponent();
            WireUpLists();

        }
        /// <summary>
        /// updates lists
        /// </summary>
        private void WireUpLists() 
        {
            //wires up the listboxes

            selectTeamDropDown.DataSource = null; //allows to reset list and allow to rebind
            selectTeamDropDown.DataSource = availableTeams;
            selectTeamDropDown.DisplayMember = "TeamName";

            tournamentTeamsListBox.DataSource = null; //allows to reset list and allow to rebind
            tournamentTeamsListBox.DataSource = selectedTeams;
            tournamentTeamsListBox.DisplayMember = "TeamName";


            prizesListBox = null; //allows to reset list and allow to rebind
            prizesListBox.DataSource = selectedPrizes;
            prizesListBox.DisplayMember = "PlaceName";

        }


        private void addTeamButton_Click(object sender, EventArgs e)
        {

            // takes selected team in the drop down and converts it to the TeamModel type
            TeamModel t = (TeamModel)selectTeamDropDown.SelectedItem; //


            //if t is not null then remove it from the availableTeams list and add it to the selectedTeams list 
            if(t != null)
            {
                availableTeams.Remove(t);
                selectedTeams.Add(t);

                WireUpLists();
            }
        }
    }
}
